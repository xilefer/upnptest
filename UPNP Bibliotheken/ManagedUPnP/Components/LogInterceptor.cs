using System.ComponentModel;

namespace ManagedUPnP.Components
{
    /// <summary>
    /// Encapsulates a windows forms component which
    /// can capture the ManagedUPnP internal log information
    /// and raise the events using thread safe GUI methods.
    /// </summary>
    public class LogInterceptor : Component
    {
        #region Protected Constants

        /// <summary>
        /// The default enabled value.
        /// </summary>
        protected const bool mbDefEnabled = false;

        #endregion

        #region Protected Locals

        /// <summary>
        /// True if the log interceptor is enabled.
        /// </summary>
        protected bool mbEnabled = mbDefEnabled;

        /// <summary>
        /// The locking object used to ensure thread safety.
        /// </summary>
        protected object moLockObject = new object();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when new lines have been posted to the log.
        /// </summary>
        public event LogLinesEventHandler LogLines;

        #endregion

        #region Initialisation

        /// <summary>
        /// Creates a new LogInterceptor.
        /// </summary>
        public LogInterceptor()
        {
        }

        #endregion

        #region Event Callers

        /// <summary>
        /// Calls the LogLines event handler with GUI thread safety.
        /// </summary>
        /// <param name="a">The event arguments.</param>
        protected virtual void OnLogLines(LogLinesEventArgs a)
        {
            if (LogLines != null)
                // Call with GUI thread safety
                LogLines.GetInvocationList().InvokeEventGUIThreadSafe(this, a);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Raised when new lines are sent from the log.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="a">The event arguments.</param>
        private void Logging_LogLines(object sender, LogLinesEventArgs a)
        {
            OnLogLines(new LogLinesEventArgs(a.Lines, a.Indent));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the enabled status of this
        /// log interceptor.
        /// </summary>
        /// <remarks>
        /// Setting this value to true will also ensure that the
        /// global ManagedUPnP.Logging.Enabled property is also true,
        /// settings this value to false does not change the 
        /// ManagedUPnP.Logging.Enabled value in any way.
        /// </remarks>
        [DefaultValue(mbDefEnabled)]
        public bool Enabled
        {
            get
            {
                return mbEnabled;
            }
            set
            {
                // Lock to ensure thread safety of enabled property
                lock(moLockObject)
                {
                    // If enabled property has changed
                    if (mbEnabled != value)
                    {
                        // Set it
                        mbEnabled = value;

                        // If enabling
                        if (mbEnabled)
                        {
                            // Ensure global log is enabled
                            if (!Logging.Enabled) Logging.Enabled = true;

                            // Add event handler
                            Logging.LogLines += new LogLinesEventHandler(Logging_LogLines);
                        }
                        else
                            // Remove event handler
                            Logging.LogLines -= new LogLinesEventHandler(Logging_LogLines);
                    }
                }
            }
        }

        #endregion

        #region Finalisation

        /// <summary>
        /// Disposes of the LogInterceptor.
        /// </summary>
        /// <param name="disposeManaged">True to dispose managed resources.</param>
        protected override void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
                Enabled = false;

            base.Dispose(disposeManaged);
        }

        #endregion
    }
}
