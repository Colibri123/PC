using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PC
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : ThemedWindow
    {
        SplashScreenManager q = SplashScreenManager.CreateWaitIndicator();
        public License()
        {
            InitializeComponent();
            q.ViewModel.Status = "Идет проверка лицензии";
            q.Show();
            q.Close();
        }      
        
    }
}
