using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace GoogleDynamicDNSLibrary
{
    public class SaveData : INotifyPropertyChanged
    {
        public SaveData()
        {
            try
            {
                Load();
            }
            catch (Exception ex)
            {

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        void DoChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        string _hostname = null;
        string _username = null;
        string _password = null;

        public string Hostname
        {
            get
            {
                return _hostname;
            }
            set
            {
                _hostname = value;
                Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Russ Judge", true).CreateSubKey("GoogleDynamicDNS", true).SetValue(nameof(Hostname), value);
                DoChanged();
            }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Russ Judge", true).CreateSubKey("GoogleDynamicDNS", true).SetValue(nameof(Username), value);
                DoChanged();
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                Registry.CurrentUser.OpenSubKey("SOFTWARE", true).CreateSubKey("Russ Judge", true).CreateSubKey("GoogleDynamicDNS", true).SetValue(nameof(Password), value);
                DoChanged();
            }
        }
        
        void Load()
        {

            Hostname = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Russ Judge").OpenSubKey("GoogleDynamicDNS").GetValue(nameof(Hostname), null) as string;
            Username = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Russ Judge").OpenSubKey("GoogleDynamicDNS").GetValue(nameof(Username), null) as string;
            Password = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Russ Judge").OpenSubKey("GoogleDynamicDNS").GetValue(nameof(Password), null) as string;
        }
    }
}
