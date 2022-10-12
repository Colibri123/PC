using APISERVER;
using DevExpress.Xpf.Core;
using System;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;

namespace PC
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : ThemedWindow
    {
        SplashScreenManager q = SplashScreenManager.CreateWaitIndicator();
        SQLRequest request = new SQLRequest();
        DataSet dataSet = new DataSet();
        public License()
        {
            InitializeComponent();
            checkLicense();
        }

        private void checkLicense()
        {
            try
            {
                q.Show();
                q.ViewModel.Status = "Идет проверка лицензии!";
                if (Properties.Resources.Key == "" || Properties.Resources.Value == "")
                {
                    q.Close();
                    DXMessageBox.Show("Данная программа не активирована!");
                    return;
                }
                if (Properties.Resources.Key != "" || Properties.Resources.Value != "")
                {

                    dataSet = request.Request($@"SELECT *
                                             FROM LicenseDS
                                             WHERE LicenseDS.LicenseKey = '{Properties.Resources.Key}' AND LicenseDS.PCID = '{Properties.Resources.Value}'");
                }



            }
            catch (Exception ex)
            {
                q.Close();
                DXMessageBox.Show(ex.Message);
            }
        }


        public void updateKey()
        {
            string firstMacAddress = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();

            //Properties.Resources.Key. = firstMacAddress;

            dataSet = request.Request($@"SELECT *
                                             FROM LicenseDS
                                             WHERE LicenseDS.LicenseKey = '{KeyLic.Text}'");

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                DXMessageBox.Show("Данного ключа не существует!");
            }

            if (dataSet.Tables[0].Rows.Count == 1)
            {

                dataSet = request.Request($@"SELECT PCDS.MAC
                                             FROM PCDS
                                             WHERE PCDS.MAC = '{firstMacAddress}'");

                request.Request($@"INSERT LicenseDS 
                                   VALUES (newid(),
                                   		'key',
                                   		'2022-10-12 22:01',
                                   		'2023-10-12 22:01',
                                   		'userid',
                                   		'pcid')");
            }
        }

        private void SimpleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
