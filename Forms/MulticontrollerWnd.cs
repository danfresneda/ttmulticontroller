using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace TTMulti.Forms
{
    internal partial class MulticontrollerWnd : Form, IMessageFilter
    {
        IntPtr _llHookID = IntPtr.Zero;
        Win32.HookProc llKeyboardProc = null;

        bool ignoreMessages = false;

        Thread activationThread = null;

        Multicontroller controller;

        IntPtr LLKeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Win32.WM msg = (Win32.WM)wParam;

                if (msg == Win32.WM.KEYDOWN || msg == Win32.WM.KEYUP) 
                {
                    Keys vkCode = (Keys)Marshal.ReadInt32(lParam);

                    if (vkCode == (Keys)Properties.Settings.Default.modeKeyCode)
                    {
                        bool ret = controller.ProcessKey(vkCode, (uint)msg);

                        if (ret)
                        {
                            return (IntPtr)1;
                        }
                    }
                }
            }

            return Win32.CallNextHookEx(_llHookID, nCode, wParam, lParam);
        }

        internal MulticontrollerWnd()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
        }

        internal void TryActivate()
        {
            if (activationThread == null || activationThread.ThreadState != System.Threading.ThreadState.Running)
            {
                activationThread = new Thread(activationThreadFunc) { IsBackground = true };
                activationThread.Start();
            }
        }

        void activationThreadFunc()
        {
            IntPtr hWnd = IntPtr.Zero;
            this.InvokeIfRequired(() => hWnd = this.Handle);

            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                if (this.IsDisposed)
                {
                    return;
                }

                if (sw.ElapsedMilliseconds < 100)
                {
                    this.InvokeIfRequired(() => this.Activate());
                }
                else
                {
                    this.InvokeIfRequired(() => this.TopMost = true);

                    int x = this.DisplayRectangle.Width / 2 + this.Location.X;
                    int y = this.Location.Y + SystemInformation.CaptionHeight / 2;

                    var virtualScreen = SystemInformation.VirtualScreen;
                    x = (x - virtualScreen.Left) * 65536 / virtualScreen.Width;
                    y = (y - virtualScreen.Top) * 65536 / virtualScreen.Height;

                    int oldX = (int)Math.Ceiling((MousePosition.X - virtualScreen.Left) * 65536.0 / virtualScreen.Width),
                        oldY = (int)Math.Ceiling((MousePosition.Y - virtualScreen.Top) * 65536.0 / virtualScreen.Height);

                    var pInputs = new[]{
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = x,
                                    dy = y,
                                    dwFlags = Win32.MOUSEEVENTF.ABSOLUTE | Win32.MOUSEEVENTF.VIRTUALDESK | Win32.MOUSEEVENTF.MOVE | Win32.MOUSEEVENTF.LEFTDOWN
                                }
                            }
                        },
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = 0,
                                    dy = 0,
                                    dwFlags = Win32.MOUSEEVENTF.LEFTUP
                                }
                            }
                        },
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = oldX,
                                    dy = oldY,
                                    dwFlags = Win32.MOUSEEVENTF.ABSOLUTE | Win32.MOUSEEVENTF.VIRTUALDESK | Win32.MOUSEEVENTF.MOVE
                                }
                            }
                        }
                    };

                    Win32.SendInput((uint)pInputs.Length, pInputs, Win32.INPUT.Size);
                }

                Thread.Sleep(10);

                if (Win32.GetForegroundWindow() == hWnd)
                {
                    this.InvokeIfRequired(() => this.TopMost = Properties.Settings.Default.onTopWhenInactive);
                    break;
                }
            } while (sw.Elapsed.TotalSeconds < 5);

            sw.Stop();
        }

        internal void UpdateCrosshairs()
        {
            leftToonCrosshair.SelectedWindowHandle = controller.LeftController.TTWindowHandle;
            rightToonCrosshair.SelectedWindowHandle = controller.RightController.TTWindowHandle;

            leftStatusLbl.Text = "Group " + (controller.CurrentGroupIndex + 1) + " active.";
            rightStatusLbl.Text = controller.ControllerGroups.Count + " groups.";

            if (!statusStrip1.Visible && controller.ControllerGroups.Count > 1)
            {
                statusStrip1.Visible = true;
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right, this.Padding.Bottom + statusStrip1.Height);
            }
            else if (statusStrip1.Visible && controller.ControllerGroups.Count == 1)
            {
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right, this.Padding.Bottom - statusStrip1.Height);
                statusStrip1.Visible = false;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // ignore all relevant toontown keys so they can be processed by the message filter
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Alt:
                    return true;
                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (ignoreMessages)
            {
                return false;
            }

            bool ret = false;
            
            var msg = (Win32.WM)m.Msg;
            
            if (msg == Win32.WM.KEYDOWN || msg == Win32.WM.KEYUP || msg == Win32.WM.SYSKEYDOWN || msg == Win32.WM.SYSKEYUP || msg == Win32.WM.SYSCOMMAND)
            {
                var key = (Keys)m.WParam.ToInt32();
                var settings = Properties.Settings.Default;

                ret = controller.ProcessKey(key, (uint)m.Msg, m.LParam);
            }

            return ret;
        }

        internal void SaveLocation()
        {
            Properties.Settings.Default.lastLocation = this.Location;
            Properties.Settings.Default.Save();
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                activationThread.Abort();
            }
            catch { }

            Win32.UnhookWindowsHookEx(_llHookID);
            SaveLocation();
        }

        private void optionsBtn_Click(object sender, EventArgs e)
        {
            OptionsDlg optionsDlg = new OptionsDlg();

            ignoreMessages = true;

            if (optionsDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ReloadOptions();
            }

            ignoreMessages = false;
        }

        private void ReloadOptions()
        {
            this.TopMost = Properties.Settings.Default.onTopWhenInactive;
            wndGroup.Visible = !Properties.Settings.Default.compactUI;
            controller.UpdateKeys();

            //if (Properties.Settings.Default.compactUI)
            //{
            //    this.ClientSize = new Size(this.ClientSize.Width, 49);
            //}
            //else
            //{
            //    this.Height = 150;
            //}
        }

        private void MainWnd_Load(object sender, EventArgs e)
        {
            controller = Multicontroller.Instance;
            controller.mainWnd = this;
            controller.Init();

            statusStrip1.Padding = new Padding(statusStrip1.Padding.Left, statusStrip1.Padding.Top, statusStrip1.Padding.Left, statusStrip1.Padding.Bottom);

            Application.AddMessageFilter(this);

            llKeyboardProc = new Win32.HookProc(LLKeyboardHookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _llHookID = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, llKeyboardProc, Win32.GetModuleHandle(curModule.ModuleName), 0);
            }
            
            ReloadOptions();

            if (Properties.Settings.Default.lastLocation != Point.Empty)
            {
                var location = Properties.Settings.Default.lastLocation;
                var isNotOffScreen = false;

                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.Bounds.Contains(location))
                    {
                        isNotOffScreen = true;
                        break;
                    }
                }

                if (isNotOffScreen)
                {
                    this.Location = Properties.Settings.Default.lastLocation;
                }
            }
        }

        private void selectWindowsBtn_Click(object sender, EventArgs e)
        {
            ignoreMessages = true;
            new WindowGroupsForm().ShowDialog(this);
            ignoreMessages = false;

            UpdateCrosshairs();
        }

        private void leftToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            Multicontroller.Instance.LeftController.TTWindowHandle = handle;
        }

        private void rightToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            Multicontroller.Instance.RightController.TTWindowHandle = handle;
        }
    }
}
