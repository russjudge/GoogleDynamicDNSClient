using GoogleDynamicDNSLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDynamicDNSService
{
    public partial class DynamicDNSService : ServiceBase
    {
        public DynamicDNSService()
        {
            InitializeComponent();
        }

        System.Timers.Timer timer = null;
        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            var data = new SaveData();
            
            if (!string.IsNullOrEmpty(data.Hostname) && GoogleDynamicDNSLibrary.Process.CheckIfUpdateRequired(data.Hostname))
            {
                var result = GoogleDynamicDNSLibrary.Process.SubmitUpdate(data.Username, data.Password, data.Hostname);
                if (GoogleDynamicDNSLibrary.Responses.GetStatus(result))
                {
                    using (EventLog evtLog = new EventLog("Application"))
                    {
                        evtLog.Source = "Application";
                        evtLog.WriteEntry("Updated Google Dynamic DNS:\r\n" + Responses.GetDescription(result) + "\r\n(" + result + ")", EventLogEntryType.Information, 1, 1);
                    }

                    timer.Interval = 60000;
                }
                else
                {
                    using (EventLog evtLog = new EventLog("Application"))
                    {
                        evtLog.Source = "Application";
                        evtLog.WriteEntry("Failed to update Google Dynamic DNS:\r\n" + Responses.GetDescription(result) + "\r\n(" + result + ")", EventLogEntryType.Error, 1, 1);
                    }

                    if (timer.Interval < 360000)
                    {
                        timer.Interval += 60000;
                    }
                }
            }

            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
