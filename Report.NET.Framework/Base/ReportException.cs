using System;

namespace Root.Reports
{
    /// <summary>Report Exception</summary>
    public class ReportException : SystemException
    {
        //------------------------------------------------------------------------------------------05.07.2004
        /// <summary>Creates a report exception.</summary>
        /// <param name="sMsg">Error message</param>
        internal ReportException(String sMsg) : base(sMsg)
        {
        }

        //------------------------------------------------------------------------------------------05.07.2004
        /// <summary>Creates a report exception.</summary>
        /// <param name="sMsg">Error message</param>
        /// <param name="exception_Inner">Inner exception</param>
        internal ReportException(String sMsg, Exception exception_Inner) : base(sMsg, exception_Inner)
        {
        }
    }
}
