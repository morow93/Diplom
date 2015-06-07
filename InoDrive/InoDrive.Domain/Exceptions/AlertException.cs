using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Exceptions
{
    [Serializable]
    public class AlertException : Exception
    {
        public AlertException(string message)
            : base(message)
        {

        }
        public Boolean NeedAlert
        {
            get { return true; }
        }
    }
}
