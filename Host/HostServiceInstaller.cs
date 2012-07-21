namespace DotNetGroup.Host
{
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration.Install;
    using System.Diagnostics;
    using System.ServiceProcess;

    using Microsoft.Win32;

    [RunInstaller(true)]
    public class HostServiceInstaller : Installer
    {
        private readonly ServiceProcessInstaller serviceProcessInstaller;
        private readonly ServiceInstaller serviceInstaller;

        public HostServiceInstaller()
        {
            this.serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem,
                Password = null,
                Username = null,
            };

            this.serviceInstaller = new ServiceInstaller
            {
                DisplayName = HostService.DisplayName,
                ServiceName = HostService.Name,
                StartType = ServiceStartMode.Automatic
            };

            Installers.AddRange(new Installer[]
            {
                this.serviceProcessInstaller,
                this.serviceInstaller
            });
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            HostService.SetupEventLog();
            RegisterServiceFailureActions();
        }

        private static void RegisterServiceFailureActions()
        {
            var value = new byte[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 20, 0, 0, 0, 1, 0, 0, 0,
                96, 234, 0, 0, 1, 0, 0, 0, 96, 234,
                0, 0, 0, 0, 0, 0, 96, 234, 0, 0
            };
            var key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\" + HostService.Name);
            if (key != null)
            {
                key.SetValue("FailureActions", value);
            }
        }
    }
}