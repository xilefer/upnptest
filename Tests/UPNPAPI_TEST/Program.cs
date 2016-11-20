using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPNPLib;


namespace UPNPAPI_TEST
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.ReadLine();
            DoDeviceSearch("");
            Console.WriteLine("Fertig");
            Console.ReadLine();
        }

        public static void DoDeviceSearch(string sTypeURI)
        {
            IUPnPDeviceFinderCallback oCallback =  new UPnPDeviceFinderCallback();
            UPnPDeviceFinderClass oDeviceFinderClass = new UPnPDeviceFinderClass();
            oDeviceFinderClass.CreateAsyncFind(sTypeURI, 0, oCallback);
            oDeviceFinderClass.StartAsyncFind(0);
            
        }

    }

    class Test : UPnPDeviceClass
    {
        
    }



    public class UPnPDeviceFinderCallback : IUPnPDeviceFinderCallback
    {
        List<UPnPDevice> cDevices = new List<UPnPDevice>();

        public void DeviceAdded(int lFindData, UPnPDevice pDevice)
        {
            cDevices.Add(pDevice);
            try
            {
                string sDeviceName = pDevice.UniqueDeviceName;
                string sFriendlyName = pDevice.FriendlyName;
                Console.WriteLine("Gerät hinzugefügt: udn = " + sDeviceName + " name = " + sFriendlyName);
            }
            catch (Exception gEx)
            {
                Console.WriteLine(gEx.ToString());
            }
        }

        public void DeviceRemoved(int lFindData, string bstrUDN)
        {
            if (cDevices.RemoveAll(e => e.UniqueDeviceName == bstrUDN) > 0)
            {
                Console.WriteLine("Gerät mit udn = " + bstrUDN + " entfernt!");
            }
        }

        public void SearchComplete(int lFindData)
        {
            Console.WriteLine("Suche abgeschlossen! " + lFindData + " Geräte gefunden.");
        }
    }
}
