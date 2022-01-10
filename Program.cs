#define ENABLEMACRO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using TTMulti.Forms;
using TTMulti.Controls;
using System.Xml.Serialization;
using System.IO;

namespace TTMulti
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.UpgradeRequired)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.runAsAdministrator)
            {
                if (args.Length == 0 || args[0] != "--runasadmin")
                {
                    if (TryRunAsAdmin())
                    {
                        return;
                    }
                }
            }

            MulticontrollerWnd mainWindow = new MulticontrollerWnd();

            WindowWatcher.Instance.SynchronizingObject = mainWindow;

            Application.Run(mainWindow);
        }

        internal static bool TryRunAsAdmin()
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            processInfo.Arguments = "--runasadmin";
            processInfo.UseShellExecute = true;
            processInfo.Verb = "runas";

            try
            {
                Process.Start(processInfo);
                return true;
            }
            catch
            {
                Properties.Settings.Default.runAsAdministrator = false;
                Properties.Settings.Default.Save();
                return false;
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
