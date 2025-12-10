using System;
using System.Collections.Generic;

namespace Exim.Portal.WebParts
{
    public partial class LabelMessage : System.Web.UI.UserControl
    {

        #region Constants

        /// <summary>
        ///     The messages key.
        /// </summary>
        protected const string MessagesKey = "Messages";

        /// <summary>
        /// The clear message default
        /// </summary>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>15/09/2015</created>
        private const bool ClearMessageDefault = true;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the messages.
        /// </summary>
        /// <value>
        ///     The messages.
        /// </value>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>16/08/2016</created>
        private List<MessageItem> Messages
        {
            get
            {
                var messages = this.ViewState[MessagesKey] as List<MessageItem>;
                if (messages == null)
                {
                    messages = new List<MessageItem>();
                    this.ViewState[MessagesKey] = messages;
                }

                return messages;
            }

            set
            {
                this.ViewState[MessagesKey] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Clears the messages.
        /// </summary>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>16/08/2016</created>
        public void ClearMessages()
        {
            this.Messages = new List<MessageItem>();
            this.rptMessages.DataSource = this.Messages;
            this.rptMessages.DataBind();
        }

        /// <summary>
        /// The show error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearMessages">if set to <c>true</c> [clear messages].</param>
        public void ShowError(string message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("ErrorMessageClass").ToString(), clearMessages);
        }

        public void ShowErrors(List<string> messages, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(messages, GetLocalResourceObject("ErrorMessageClass").ToString(), clearMessages);
        }

        /// <summary>
        /// The show info.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearMessages">if set to <c>true</c> [clear messages].</param>
        public void ShowInfo(string message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("InfoMessageClass").ToString(), clearMessages);
        }

        public void ShowInfos(List<string> message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("InfoMessageClass").ToString(), clearMessages);
        }

        /// <summary>
        /// Shows the success.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearMessages">if set to <c>true</c> [clear messages].</param>
        /// <author>
        /// Basem Moqbel (bmoqbel@sure.com.sa)
        /// </author>
        /// <created>16/08/2016</created>
        public void ShowSuccess(string message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("SuccessMessageClass").ToString(), clearMessages);
        }

        public void ShowSuccesses(List<string> message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("SuccessMessageClass").ToString(), clearMessages);
        }

        /// <summary>
        /// The show warn.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearMessages">if set to <c>true</c> [clear messages].</param>
        public void ShowWarn(string message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message,  GetLocalResourceObject("WarrningMessageClass").ToString(), clearMessages);
        }

        public void ShowWarns(List<string> message, bool clearMessages = ClearMessageDefault)
        {
            this.ShowMessage(message, GetLocalResourceObject("WarrningMessageClass").ToString(), clearMessages);
        }


        /// <summary>
        /// Shows the unexpected error.
        /// </summary>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>16/08/2016</created>
        public void ShowUnexpectedError(bool clearMessages = ClearMessageDefault)
        {
			this.ShowError(GetLocalResourceObject("UnexpectedErrorMessage").ToString());
        }

        /// <summary>
        /// Shows the invalid parameters error.
        /// </summary>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>16/08/2016</created>
        public void ShowInvalidParametersError()
        {
			this.ShowError(GetLocalResourceObject("DataNotFoundMessage").ToString());
		}

        public void ShowErrorDuringSaving()
        {
            this.ShowError(GetLocalResourceObject("SavingErrorMessage").ToString());
        }
        #endregion

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
        /// <created>16/08/2016</created>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The show message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="divCss">The div CSS.</param>
        /// <param name="imageCss">The image CSS.</param>
        /// <param name="clearMessages">if set to <c>true</c> [clear messages].</param>
        private void ShowMessage(string message, string divCss, bool clearMessages = false)
        {
            if (clearMessages)
            {
                this.Messages = new List<MessageItem>();
            }

            this.Messages.Add(new MessageItem(message, divCss, ""));

            this.ViewState[MessagesKey] = this.Messages;
            this.rptMessages.DataSource = this.Messages;
            this.rptMessages.DataBind();
        }

        private void ShowMessage(List<string> messages, string divCss, bool clearMessages = false)
        {
            if (clearMessages)
            {
                this.Messages = new List<MessageItem>();
            }

            foreach (string msg in messages)
                this.Messages.Add(new MessageItem(msg, divCss, ""));

            this.ViewState[MessagesKey] = this.Messages;
            this.rptMessages.DataSource = this.Messages;
            this.rptMessages.DataBind();
        }

        #endregion

        /// <summary>
        ///     The message item.
        /// </summary>
        [Serializable]
        protected class MessageItem
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="MessageItem" /> class.
            /// </summary>
            /// <param name="message">The message.</param>
            /// <param name="divCss">The div CSS.</param>
            /// <param name="imageCss">The image CSS.</param>
            public MessageItem(string message, string divCss, string imageCss)
            {
                this.Message = message;
                this.DivCss = divCss;
                this.ImageCss = imageCss;
            }

            #endregion

            #region Public Properties

            /// <summary>
            ///     Gets the message.
            /// </summary>
            /// <value>
            ///     The message.
            /// </value>
            /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
            /// <created>16/08/2016</created>
            public string Message { get; private set; }

            /// <summary>
            /// Gets the message div CSS.
            /// </summary>
            /// <value>
            /// The div CSS.
            /// </value>
            /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
            /// <created>16/08/2016</created>
            public string DivCss { get; private set; }

            /// <summary>
            /// Gets the image CSS.
            /// </summary>
            /// <value>
            /// The image CSS.
            /// </value>
            /// <author>Basem Moqbel (bmoqbel@sure.com.sa)</author>
            /// <created>16/08/2016</created>
            public string ImageCss { get; private set; }

            #endregion
        }
    }
}