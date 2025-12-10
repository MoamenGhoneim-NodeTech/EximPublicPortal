using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXIM.Common.Lib.Models
{
    public class ItemWithSub
    {
        public object Item { get; set; }
        public object SubItem { get; set; }

        public ItemWithSub(object _item, object _subItem)
        {
            Item = _item;
            SubItem = _subItem;
        }
    }
}
