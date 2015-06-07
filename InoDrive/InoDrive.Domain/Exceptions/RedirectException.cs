using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoDrive.Domain.Exceptions
{
    [Serializable]
    public class RedirectException : Exception
    {
        public RedirectException(string message)
            : base(message)
        {

        }
        public Boolean NeedRedirect
        {
            get { return true; }
        }
    }
}
