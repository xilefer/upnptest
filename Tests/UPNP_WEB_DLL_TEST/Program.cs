using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedUPnP;

namespace UPNP_WEB_DLL_TEST
{
    class Program
    {
        CUPNP oCupnp = new CUPNP("");
        static void Main(string[] args)
        {
            Console.WriteLine("Suche starten (y/n)");
            string sIn = Console.ReadLine();
            while (sIn.Length != 1 && (sIn[0] != 'y' || sIn[0] != 'n'))
            {
                sIn = Console.ReadLine();
            }
            if (sIn[0] == 'y')
            {
                CUPNP oCupnp = new CUPNP("");
                DoDeviceSearch(oCupnp);
            }
            else
            {
                Console.WriteLine("Programm wird beendet!");
                System.Threading.Thread.Sleep(1500);
            }
            Console.ReadLine();
        }

        public static void DoDeviceSearch(CUPNP oCupnp)
        {
            if (oCupnp.StartDiscover())
            {
                Console.WriteLine("Suche erfolgreich gestartet!");
            }
        }

        private void ODiscoveryOnSearchComplete(object sender, SearchCompleteEventArgs searchCompleteEventArgs)
        {
            Console.WriteLine("Suche Beendet");
        }
    }
}
