using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using WebKit;
using WebKit.DOM;

namespace Nimbus
{
    class BrowserInterop
    {

        public bool TestMethod(string obj1, string obj2)
        {
            MessageBox.Show(obj1.ToString());
            MessageBox.Show(obj2.ToString());
            return true;
        }

        public static void RunScript(WebKitBrowser browser, String function, params object[] args)
        {
            Element scriptElm = browser.Document.CreateElement("script");
            StringBuilder sb = new StringBuilder();
            sb.Append(function + "(");
            foreach (object o in args)
            {
                sb.Append("'" + o.ToString() + "', ");
            }
            sb.Remove(sb.Length - 2, 2);
            sb.Append(")");
            Console.WriteLine("Writing: {0}", sb.ToString());
            scriptElm.AppendChild(browser.Document.CreateTextNode(sb.ToString()));

            NodeList headElm = browser.Document.GetElementsByTagName("head");
            headElm[0].AppendChild(scriptElm);

        }

    }
}
