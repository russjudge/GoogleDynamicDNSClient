using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace GoogleDynamicDNSLibrary
{
    public class SaveData : INotifyPropertyChanged
    {
        public SaveData()
        {
            _hosts = new ObservableCollection<HostConfig>();
            try
            {
                Load();
            }
            catch (Exception ex)
            {

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<HostConfig> _hosts;

        public ObservableCollection<HostConfig> Hosts
        {
            get
            {
                return _hosts;
            }
            private set
            {
                _hosts = value;
                DoChanged();
            }
        }
        void DoChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        

        void Load()
        {
            var list = Registry.CurrentUser.OpenSubKey(Constants.Software).OpenSubKey(Constants.Company).OpenSubKey(Constants.Application).GetSubKeyNames();
            foreach (var key in list)
            {
                var host = new HostConfig(key);
                Hosts.Add(host);
            }
           
        }
    }
}
