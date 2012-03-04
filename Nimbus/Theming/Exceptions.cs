using System;
using System.Collections.Generic;
using System.Text;

namespace Nimbus.Theming
{
    class ThemingError : Exception
    {
        public string Error;
        public object Sender;

        public ThemingError(object sender, string error)
        {
            this.Sender = sender;
            this.Error = error;

        }
    }
}
