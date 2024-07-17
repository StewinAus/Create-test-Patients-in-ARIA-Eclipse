using Newtonsoft.Json;
using services.varian.com.AriaWebConnect.Link;
using System;
using System.Configuration;
using System.Net.Http;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VMSType = services.varian.com.AriaWebConnect.Common;
using static System.Console;
using EnterDets;
using services.varian.com.AWV.WebService;
using System.Linq;

namespace StartupPage
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var enterDetails = new EnterDetails();
            enterDetails.ShowDialog();
        }
    }
}
