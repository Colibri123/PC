using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;

namespace PC
{
    /// <summary>
    /// Interaction logic for MainForm1.xaml
    /// </summary>
    public partial class MainForm1 : ThemedWindow
    {
        [DllImport("Netapi32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int NetServerEnum(
               string serverName,
               int dwLevel,
               ref IntPtr pBuf,
               int dwPrefMaxLen,
               out int dwEntriesRead,
               out int dwTotalEntries,
               int dwServerType,
               string domain,
               out int dwResumeHandle
               );

        [DllImport("Netapi32", SetLastError = true)]
        public static extern int NetApiBufferFree(IntPtr pBuf);

        [StructLayout(LayoutKind.Sequential)]
        public struct ServerInfo100
        {
            internal int sv100_platform_id;
            [MarshalAs(UnmanagedType.LPWStr)]
            internal string sv100_name;
        }

        public static List<string> GetNetworkComputers(string domain = null)
        {
            List<string> networkComputers = new List<string>();
            const int MAX_PREFERRED_LENGTH = -1;
            int SV_TYPE_WORKSTATION = 1;
            int SV_TYPE_SERVER = 2;
            IntPtr buffer = IntPtr.Zero;
            IntPtr tmpBuffer = IntPtr.Zero;
            int entriesRead;
            int totalEntries;
            int resHandle;
            int sizeofInfo = Marshal.SizeOf(typeof(ServerInfo100));
            string elem;

            try
            {
                int ret = NetServerEnum(null, 100, ref buffer,
                                        MAX_PREFERRED_LENGTH, out entriesRead, out totalEntries,
                                        SV_TYPE_WORKSTATION | SV_TYPE_SERVER, domain, out resHandle);

                if (ret == 0)
                {
                    for (int i = 0; i < totalEntries; i++)
                    {
                        tmpBuffer = new IntPtr((long)buffer + (i * sizeofInfo));

                        ServerInfo100 svrInfo = (ServerInfo100)
                                                   Marshal.PtrToStructure(tmpBuffer,
                                                                          typeof(ServerInfo100));

                        elem = svrInfo.sv100_name;

                        try
                        {
                            IPAddress[] ips = Dns.GetHostAddresses(svrInfo.sv100_name);
                            IPAddress ip = ips.First();

                            if (ip != null) elem += ": " + ip.ToString();
                        }
                        catch (Exception ex)
                        {
                            elem += " " + ex.Message;
                        }

                        networkComputers.Add(elem);
                    }
                }
            }
            finally
            {
                NetApiBufferFree(buffer);
            }
            return networkComputers;
        }

        public void ip()
        {
            var list = GetNetworkComputers();

            Grid.Dispatcher.Invoke(() =>
            {
                Grid.ItemsSource = list;
            });

            
        }
    
    public MainForm1()
        {
            InitializeComponent();
            new Thread(ip).Start();            
        }

        public void CMD()
        {
            try
            {
                ConnectionOptions connection = new ConnectionOptions();
                

                ManagementScope scope = new ManagementScope(
                    "\\\\DESKTOP-02AC0BA\\root\\CIMV2", connection);
                scope.Connect();

                ObjectQuery query = new ObjectQuery(
                    "SELECT * FROM Win32_Service");

                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher(scope, query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    
                }
                Close();
            }
            catch (ManagementException err)
            {
                
            }
        }
    }
}
