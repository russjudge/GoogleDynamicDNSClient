using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSLibrary
{
    public class HostConfig : INotifyPropertyChanged
    {
        
        public HostConfig(string host)
        {
            Hostname = host;
            Username = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey(Constants.Application).OpenSubKey(Hostname).GetValue(nameof(Username), null) as string;
            Password = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey(Constants.Application).OpenSubKey(Hostname).GetValue(nameof(Password), null) as string;
            Processor = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey(Constants.Application).OpenSubKey(Hostname).GetValue(nameof(Processor), nameof(GoogleDynProcessor)) as string;

        }
        string _hostname = null;
        string _username = null;
        string _password = null;
        string _processor = null;

        public void Delete()
        {
            //Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).OpenSubKey(Hostname).DeleteValue(nameof(Username));
            //Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).OpenSubKey(Hostname).DeleteValue(nameof(Password));
            Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).DeleteSubKey(Hostname);
        }
        void SaveProperties()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).CreateSubKey(Hostname,true).SetValue(nameof(Username), Username);
            }
            if (!string.IsNullOrEmpty(Password))
            {
                Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).CreateSubKey(Hostname, true).SetValue(nameof(Password), Password);
            }
            if (!string.IsNullOrEmpty(Processor))
            {
                Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).CreateSubKey(Hostname, true).SetValue(nameof(Processor), Processor);
            }

        }
        public string Processor
        {
            get
            {
                return _processor;
            }
            set
            {
               
                _processor = value;
              
                SaveProperties();
               
                DoChanged();
            }
        }
        public string Hostname
        {
            get
            {
                return _hostname;
            }
            set
            {
                if (!string.IsNullOrEmpty(_hostname))
                {
                    Delete();
                }
                _hostname = value;
                if (!string.IsNullOrEmpty(value))
                {
                    Registry.CurrentUser.OpenSubKey(Constants.Software, true).CreateSubKey(Constants.Company, true).CreateSubKey(Constants.Application, true).CreateSubKey(value);
                }
                SaveProperties();
                CurrentIP = Process.GetIP(value);
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
                SaveProperties();
                
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
                SaveProperties();
                
                DoChanged();
            }
        }

        string _currentIP = string.Empty;
        public string CurrentIP
        {
            get
            {
                return _currentIP;
            }
            set
            {
                _currentIP = value;
                DoChanged();
            }
        }
        void DoChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
