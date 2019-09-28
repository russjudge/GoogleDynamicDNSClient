using GoogleDynamicDNSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoogleDynamicDNSClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            Data = new SaveData();
            UpdateHostIP();
            ThisIP = Process.GetPublicIP();
        }

        public static readonly DependencyProperty DataProperty =
          DependencyProperty.Register("Data", typeof(SaveData),
          typeof(MainWindow));
        public SaveData Data
        {
            get
            {
                return (SaveData)GetValue(DataProperty);
            }
            set
            {
                this.SetValue(DataProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentHostIPProperty =
         DependencyProperty.Register("CurrentHostIP", typeof(string),
         typeof(MainWindow));
        public string CurrentHostIP
        {
            get
            {
                return (string)GetValue(CurrentHostIPProperty);
            }
            set
            {
                this.SetValue(CurrentHostIPProperty, value);
            }
        }

        public static readonly DependencyProperty ThisIPProperty =
         DependencyProperty.Register("ThisIP", typeof(string),
         typeof(MainWindow));
        public string ThisIP
        {
            get
            {
                return (string)GetValue(ThisIPProperty);
            }
            set
            {
                this.SetValue(ThisIPProperty, value);
            }
        }
        private void OnUpdate(object sender, RoutedEventArgs e)
        {
            string result = Process.SubmitUpdate(Data.Username, Data.Password, Data.Hostname);
            if (Responses.GetStatus(result))
            {
                UpdateHostIP();
            }
            else
            {
                MessageBox.Show("Update Failed:\r\n" + Responses.GetDescription(result));

            }
        }

       
        void UpdateHostIP()
        {
            if (!string.IsNullOrEmpty(Data.Hostname))
            {
                CurrentHostIP = Process.GetIP(Data.Hostname);
            }
        }
        private void OnHostnameLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateHostIP();
        }
    }
}
