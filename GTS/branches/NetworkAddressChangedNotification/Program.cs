using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using NetworkAddressChangedNotification;

namespace NetworkAddressChangedNotification
{
    class Program
    {
        //http://msdn.microsoft.com/de-de/library/system.net.networkinformation.networkchange%28v=vs.80%29.aspx
        //Managed Wifi API http://msdn.microsoft.com/en-us/library/windows/desktop/ms705945(v=vs.85).aspx
        //http://managedwifi.codeplex.com/



        static void Main(string[] args)
        {
            NativeWifiWrapper nativeWifiWrapper = new NativeWifiWrapper();
            Console.ReadLine();
            
        }
        public static void WlanNotification(ref NativeWifiWrapper.WlanNotificationData notificationData, IntPtr context)
        {

        }
    }
}
