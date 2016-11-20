using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedUPnP;

namespace UPNP_WEB_DLL_TEST
{
    class CUPNP
    {
        public Discovery oDiscovery;
        public CUPNP(string sURI)
        {
            oDiscovery = new Discovery(sURI, AddressFamilyFlags.IPvBoth, false); ;
            oDiscovery.SearchComplete += ODiscoveryOnSearchComplete;
        }

        private void ODiscoveryOnSearchComplete(object sender, SearchCompleteEventArgs searchCompleteEventArgs)
        {
            Console.WriteLine("Suche Beendet!");
        }

        public bool StartDiscover()
        {
            return oDiscovery.Start();
        }
        
        
    }
}
