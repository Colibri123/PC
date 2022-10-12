using APISERVER;
using DevExpress.Mvvm;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for mainform.xaml
    /// </summary>
    public partial class mainform : ThemedWindow
    {
        SQLRequest SQL = new SQLRequest();
        DataSet dataSet = new DataSet();
        public mainform()
        {
            InitializeComponent();
            loading();
        }

        public void loading()
        {
            var logo = SplashScreenManager.CreateFluent();
            //logo.ViewModel.Logo = new Uri("\\bin\\Debug\\123.png");
            logo.ViewModel.Copyright = "2015 - 2022 Старорусприбор\nAll Rights reserved";
            logo.ViewModel.Status = "Загрузка";
            logo.ViewModel.Subtitle = "Бета версия 0.0.0.1";
            logo.ViewModel.Title = "Парк ПК";
            logo.Show();
            Thread.Sleep(4000);
            logo.Close();
            license();
        }

        public void license()
        {
            License license = new License();
            license.ShowDialog();
            //new Thread(loadUser).Start();
        }

        public void loadUser()
        {
            dataSet = SQL.Request($@"SELECT *
                                     FROM UserDS");
            var nameUser = dataSet.Tables[0].AsEnumerable().Select(DataColumn => new 
            {
                UserID = DataColumn.Field<Guid>("UserID"),
                UserName = DataColumn.Field<string>("UserName"),
                UserLogin = DataColumn.Field<string>("UserLogin"),
                UserPassword = DataColumn.Field<string>("UserPassword"),
            }).ToList();

            CBNameUser.Dispatcher.Invoke(()=> 
            {
                CBNameUser.ItemsSource = nameUser;

                CBNameUser.ValueMember = "UserID";

                CBNameUser.DisplayMember = "UserName";
            });          

        }

        public void loginUser()
        {
            Dispatcher.Invoke(() => 
            {
                dataSet = SQL.Request($@"SELECT *
                                     FROM UserDS
                                     WHERE UserDS.UserPassword = '{Pass.Password}'");
            });
            

            if (dataSet.Tables[0].Rows.Count != 0)
            {        
                var nameUser = dataSet.Tables[0].AsEnumerable().Select(DataColumn => new
                {
                    UserID = DataColumn.Field<Guid>("UserID"),
                    UserName = DataColumn.Field<string>("UserName")
                }).ToList();

                Global.UserID = nameUser.Select(a => a.UserID).First();
                Global.UserName = nameUser.Select(a => a.UserName).First();
                
                Dispatcher.Invoke(() => 
                {
                    MainForm1 mainForm1 = new MainForm1();
                    mainForm1.Show();
                    this.Close();
                });
            }

            if (dataSet.Tables[0].Rows.Count == 0)
            {
                Dispatcher.Invoke(()=> 
                {
                    DXMessageBox.Show("Пароль неверен!");
                });
               
            }
        }

        private void SimpleButton_Click(object sender, RoutedEventArgs e)
        {
            new Thread(loginUser).Start();
        }
    }
}
