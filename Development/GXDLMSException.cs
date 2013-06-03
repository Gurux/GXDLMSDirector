using System;
using System.Collections.Generic;
using System.Text;

namespace GXDLMSDirector
{
    public class GXDLMSException : Exception
    {
        static private string GetDescription(int errCode)
        {            
            string str = null;
            switch (errCode)
            {
                case 1: //Access Error : Device reports a hardware fault
                    str = GXDLMSDirector.Properties.Resources.HardwareFaultTxt;
                    break;
                case 2: //Access Error : Device reports a temporary failure
                    str = GXDLMSDirector.Properties.Resources.TemporaryFailureTxt;
                    break;
                case 3: // Access Error : Device reports Read-Write denied
                    str = GXDLMSDirector.Properties.Resources.ReadWriteDeniedTxt;
                    break;
                case 4: // Access Error : Device reports a undefined object
                    str = GXDLMSDirector.Properties.Resources.UndefinedObjectTxt;
                    break;
                case 5: // Access Error : Device reports a inconsistent Class or Object
                    str = GXDLMSDirector.Properties.Resources.InconsistentClassTxt;
                    break;
                case 6: // Access Error : Device reports a unavailable object
                    str = GXDLMSDirector.Properties.Resources.UnavailableObjectTxt;
                    break;
                case 7: // Access Error : Device reports a unmatched type
                    str = GXDLMSDirector.Properties.Resources.UnmatchedTypeTxt;
                    break;
                case 8: // Access Error : Device reports scope of access violated
                    str = GXDLMSDirector.Properties.Resources.AccessViolatedTxt;
                    break;
                default:
                    str = GXDLMSDirector.Properties.Resources.UnknownErrorTxt;
                    break;
            }
            return str;
        }
        public GXDLMSException(int errCode) : base(GetDescription(errCode))
        {
            ErrorCode = errCode;            
        }

        /// <summary>
        /// Returns occurred error code.
        /// </summary>
        public int ErrorCode
        {
            get;
            internal set;
        }
    }
}
