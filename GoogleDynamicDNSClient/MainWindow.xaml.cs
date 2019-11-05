using GoogleDynamicDNSLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Services = new ObservableCollection<KeyValuePair<string,string>>();
            
            foreach (var key in DynProcessor.ProcessorList.Keys)
            {
                var item = new KeyValuePair<string, string>(key, DynProcessor.ProcessorList[key]);
                Services.Add(item);
            }
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

        public static readonly DependencyProperty SelectedHostProperty =
            DependencyProperty.Register("SelectedHost", typeof(HostConfig),
            typeof(MainWindow), new PropertyMetadata(OnSelectedHostChanged));

        private static void OnSelectedHostChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow me = d as MainWindow;
            if (me != null)
            {
                me.UpdateHostIP();
            }
        }

        public HostConfig SelectedHost
        {
            get
            {
                return (HostConfig)GetValue(SelectedHostProperty);
            }
            set
            {
                this.SetValue(SelectedHostProperty, value);
            }
        }
        public static readonly DependencyProperty ServicesProperty =
       DependencyProperty.Register("Services", typeof(ObservableCollection<KeyValuePair<string, string>>),
       typeof(MainWindow));
        public ObservableCollection<KeyValuePair<string,string>> Services
        {
            get
            {
                return (ObservableCollection<KeyValuePair<string, string>>)GetValue(ServicesProperty);
            }
            set
            {
                this.SetValue(ServicesProperty, value);
            }
        }


        public static readonly DependencyProperty NewHostProperty =
         DependencyProperty.Register("NewHost", typeof(string),
         typeof(MainWindow));
        public string NewHost
        {
            get
            {
                return (string)GetValue(NewHostProperty);
            }
            set
            {
                this.SetValue(NewHostProperty, value);
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
            Button btn = sender as Button;
            if (btn != null)
            {
                HostConfig config = btn.CommandParameter as HostConfig;
                if (config != null)
                {
                    DynProcessor processor = DynProcessor.GetProcessor(config);
                    if (processor.SubmitUpdate())
                    {
                        UpdateHostIP();
                        MessageBox.Show(processor.StatusMessage);
                    }
                    else
                    {
                        MessageBox.Show("Update Failed:\r\n" + processor.StatusMessage);

                    }
                }
            }
            
        }

       
        void UpdateHostIP()
        {
            if (SelectedHost != null && !string.IsNullOrEmpty(SelectedHost.Hostname))
            {
                CurrentHostIP = Process.GetIP(SelectedHost.Hostname);
            }
        }
        private void OnHostnameLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateHostIP();
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow();
            win.ShowDialog();
        }

        private void OnAddHost(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(NewHost))
            {
                var newHost = new HostConfig(NewHost);
                Data.Hosts.Add(newHost);
            }
            else
            {
                MessageBox.Show("Please enter the host name first", "Add Host", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                HostConfig host = btn.CommandParameter as HostConfig;
                if (host != null)
                {
                    host.Delete();
                    Data.Hosts.Remove(host);
                }
            }
        }

        private void ImportGoogle(object sender, RoutedEventArgs e)
        {
            var list = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey("Google Dynamic DNS Client").GetSubKeyNames();
            foreach (var key in list)
            {
                var host = new HostConfig(key);
                host.Username = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey("Google Dynamic DNS Client").OpenSubKey(host.Hostname).GetValue(nameof(HostConfig.Username)) as string;
                host.Password = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey("Google Dynamic DNS Client").OpenSubKey(host.Hostname).GetValue(nameof(HostConfig.Password), null) as string;
                host.Processor = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey("Google Dynamic DNS Client").OpenSubKey(host.Hostname).GetValue(nameof(HostConfig.Processor), nameof(GoogleDynProcessor)) as string;
                Data.Hosts.Add(host);
            }
            MessageBox.Show("Import complete");
        }
    }
}
