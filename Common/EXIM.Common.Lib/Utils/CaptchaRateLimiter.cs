using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace EXIM.Common.Lib.Utils
{
    public static class CaptchaRateLimiter
    {
        private static readonly ConcurrentDictionary<string, RequestWindow> GenerationHits =
            new ConcurrentDictionary<string, RequestWindow>();

        private static readonly ConcurrentDictionary<string, RequestWindow> ValidationHits =
            new ConcurrentDictionary<string, RequestWindow>();

        // Image (re)generation: generous, since clicking "refresh" is legitimate user behavior
        // (including a frustrated user who mistyped a few times). Mainly guards against
        // scripted image-scraping / flooding, not normal human refresh clicking.
        private const int GenerationLimit = 30;
        private static readonly TimeSpan GenerationWindow = TimeSpan.FromMinutes(1);

        // Validation (form submit) attempts: tight. This is what actually protects against
        // brute-forcing the captcha code itself.
        private const int ValidationLimit = 8;
        private static readonly TimeSpan ValidationWindow = TimeSpan.FromMinutes(3);

        /// <summary>Call when serving a new captcha image. Returns false if this IP should be throttled.</summary>
        public static bool AllowGeneration(string clientIp)
        {
            return AllowAndRecord(GenerationHits, clientIp, GenerationLimit, GenerationWindow);
        }

        /// <summary>Call on every server-side validation attempt (form postback). Returns false if this IP should be throttled.</summary>
        public static bool AllowValidationAttempt(string clientIp)
        {
            return AllowAndRecord(ValidationHits, clientIp, ValidationLimit, ValidationWindow);
        }

        private static bool AllowAndRecord(
            ConcurrentDictionary<string, RequestWindow> store, string clientIp, int limit, TimeSpan window)
        {
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = "unknown";
            }

            DateTime now = DateTime.UtcNow;

            RequestWindow entry = store.AddOrUpdate(
                clientIp,
                _ => new RequestWindow { WindowStart = now, Count = 1 },
                (_, existing) =>
                {
                    lock (existing)
                    {
                        if (now - existing.WindowStart > window)
                        {
                            existing.WindowStart = now;
                            existing.Count = 1;
                        }
                        else
                        {
                            existing.Count++;
                        }
                        return existing;
                    }
                });

            OccasionallyPrune(store, window);

            lock (entry)
            {
                return entry.Count <= limit;
            }
        }

        // Cheap opportunistic cleanup so the dictionary doesn't grow unbounded over the app's
        // lifetime. Runs roughly every ~500 calls rather than on a timer, to avoid adding a
        // background thread / dependency for something this low-stakes.
        private static int _pruneCounter;

        private static void OccasionallyPrune(ConcurrentDictionary<string, RequestWindow> store, TimeSpan window)
        {
            if (Interlocked.Increment(ref _pruneCounter) % 500 != 0)
            {
                return;
            }

            DateTime cutoff = DateTime.UtcNow - window - TimeSpan.FromMinutes(3);
            foreach (var kvp in store)
            {
                if (kvp.Value.WindowStart < cutoff)
                {
                    store.TryRemove(kvp.Key, out _);
                }
            }
        }

        private class RequestWindow
        {
            public DateTime WindowStart;
            public int Count;
        }
    }
}
