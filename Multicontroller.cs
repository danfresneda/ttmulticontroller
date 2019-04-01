using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using TTMulti.Forms;

namespace TTMulti
{
    class Multicontroller
    {
        internal static readonly Multicontroller Instance = new Multicontroller();

        internal MulticontrollerWnd mainWnd;
        IntPtr mainWndHandle = IntPtr.Zero;

        List<ControllerGroup> controllerGroups = new List<ControllerGroup>()
        {
            new ControllerGroup()
        };

        internal List<ControllerGroup> ControllerGroups
        {
            get { return controllerGroups; }
        }

        int currentGroupIndex = 0;

        internal int CurrentGroupIndex
        {
            get
            {
                if (currentGroupIndex >= controllerGroups.Count)
                {
                    currentGroupIndex = 0;
                    updateControllerBorders();
                }

                return currentGroupIndex;
            }
            private set
            {
                currentGroupIndex = value;

                updateControllerBorders();
            }
        }

        internal ToontownController LeftController
        {
            get
            {
                return controllerGroups[CurrentGroupIndex].LeftController;
            }
        }

        internal ToontownController RightController
        {
            get
            {
                return controllerGroups[CurrentGroupIndex].RightController;
            }
        }

        internal enum ControllerMode
        {
            Multi,
            Mirror
        }

        bool isActive = true;
        bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;

                updateControllerBorders();
            }
        }

        ControllerMode currentMode = ControllerMode.Multi;
        internal ControllerMode CurrentMode
        {
            get { return currentMode; }
            set
            {
                currentMode = value;

                switch (currentMode)
                {
                    case ControllerMode.Multi:
                        mainWnd.InvokeIfRequired(() => mainWnd.multiModeRadio.Checked = true);
                        break;
                    case ControllerMode.Mirror:
                        mainWnd.InvokeIfRequired(() => mainWnd.mirrorModeRadio.Checked = true);
                        break;
                }

                updateControllerBorders();
            }
        }

        Dictionary<Keys, Keys> leftKeys = new Dictionary<Keys, Keys>(),
            rightKeys = new Dictionary<Keys, Keys>();

        internal Multicontroller()
        {
            //LeftController.TTWindowClosed += () =>
            //{
            //    mainWnd.LeftWindowClosed();
            //};

            //RightController.TTWindowClosed += () =>
            //{
            //    mainWnd.RightWindowClosed();
            //};

            UpdateKeys();

            new Thread(() =>
            {
                while (true)
                {
                    if (mainWndHandle != IntPtr.Zero && !Win32.IsWindow(mainWndHandle))
                    {
                        mainWndHandle = IntPtr.Zero;
                    }

                    if (mainWndHandle != IntPtr.Zero)
                    {
                        IntPtr activeWnd = Win32.GetForegroundWindow();

                        bool active = (activeWnd == mainWndHandle || Win32.GetWindow(activeWnd, Win32.GetWindow_Cmd.GW_OWNER) == mainWndHandle);

                        if (isActive != active)
                        {
                            IsActive = active;
                        }
                    }

                    Thread.Sleep(10);
                }
            }) { IsBackground = true }.Start();
        }

        internal void UpdateKeys()
        {
            leftKeys.Clear();
            rightKeys.Clear();

            var ttBindings = Properties.SerializedSettings.Default.Bindings;
            var leftBindings = Properties.SerializedSettings.Default.LeftKeys;
            var rightBindings = Properties.SerializedSettings.Default.RightKeys;

            for (int i = 0; i < ttBindings.Count; i++)
            {
                if (ttBindings[i].Key != Keys.None && leftBindings[i].Key != Keys.None)
                {
                    leftKeys.Add(ttBindings[i].Key, leftBindings[i].Key);
                }

                if (ttBindings[i].Key != Keys.None && rightBindings[i].Key != Keys.None)
                {
                    rightKeys.Add(ttBindings[i].Key, rightBindings[i].Key);
                }
            }
        }

        internal void Init()
        {
            UpdateKeys();

            for (int i = controllerGroups.Count; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                controllerGroups.Add(CreateControllerGroup());
            }

            updateControllerBorders();
            mainWnd.UpdateCrosshairs();

            mainWnd.InvokeIfRequired(() => mainWndHandle = mainWnd.Handle);

            mainWnd.multiModeRadio.Click += (sender, e) => CurrentMode = ControllerMode.Multi;
            mainWnd.mirrorModeRadio.Click += (sender, e) => CurrentMode = ControllerMode.Mirror;
        }

        internal ControllerGroup CreateControllerGroup()
        {
            ControllerGroup group = new ControllerGroup();

            group.LeftController.TTWindowClosed += Controller_TTWindowClosed;
            group.RightController.TTWindowClosed += Controller_TTWindowClosed;

            return group;
        }

        void Controller_TTWindowClosed(object sender)
        {
            if (sender == LeftController || sender == RightController)
            {
                mainWnd.UpdateCrosshairs();
            }
        }

        void updateControllerBorders()
        {
            if (CurrentMode == ControllerMode.Multi)
            {
                LeftController.BorderColor = RightController.BorderColor = Color.LimeGreen;

                LeftController.ShowBorder = RightController.ShowBorder = isActive;

                var otherGroups = controllerGroups.Except(new[] { controllerGroups[CurrentGroupIndex] }).ToList();

                otherGroups.ForEach(g =>
                {
                    //g.LeftController.BorderColor = g.RightController.BorderColor = 
                    //    (currentMode == ControllerMode.Multi) ? Color.LimeGreen : Color.Violet;

                    g.LeftController.ShowBorder = g.RightController.ShowBorder = false;
                });
            }
            else
            {
                controllerGroups.ForEach(g =>
                {
                    g.LeftController.BorderColor = g.RightController.BorderColor = Color.Violet;
                    g.LeftController.ShowBorder = g.RightController.ShowBorder = isActive;
                });
            }
        }

        internal bool ProcessKey(Keys key, uint msg = 0, IntPtr lParam = new IntPtr()) 
        {
            var ret = false;
            var settings = Properties.Settings.Default;
            IntPtr wParam = (IntPtr)key;

            if (key == (Keys)settings.modeKeyCode)
            {
                bool interceptKey = false;

                IntPtr activeWnd = Win32.GetForegroundWindow();
                bool ttWindowActive = controllerGroups.Any(g => g.LeftController.TTWindowHandle == activeWnd || g.RightController.TTWindowHandle == activeWnd);

                if (msg == (uint)Win32.WM.KEYDOWN && (ttWindowActive || activeWnd == mainWndHandle))
                {
                    if (isActive)
                    {
                        if (currentMode == ControllerMode.Multi)
                        {
                            CurrentMode = ControllerMode.Mirror;
                        }
                        else
                        {
                            CurrentMode = ControllerMode.Multi;
                        }
                    }

                    if (ttWindowActive)
                    {
                        mainWnd.TryActivate();
                    }

                    interceptKey = true;
                }

                ret = interceptKey;
            }
            else
            {
                if (currentMode == ControllerMode.Multi)
                {
                    if (controllerGroups.Count > 1
                        && (key >= Keys.D0 && key <= Keys.D9
                        || key >= Keys.NumPad0 && key <= Keys.NumPad9))
                    {
                        int index = 0;

                        if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            index = 9 - (Keys.D9 - key);
                        }
                        else
                        {
                            index = 9 - (Keys.NumPad9 - key);
                        }

                        index = index == 0 ? 9 : index - 1;

                        if (controllerGroups.Count > index)
                        {
                            CurrentGroupIndex = index;
                            mainWnd.UpdateCrosshairs();
                        }
                    }
                    else
                    {
                        if (leftKeys.ContainsValue(key))
                        {
                            foreach (Keys actualKey in leftKeys.Where(t => t.Value == key).Select(t => t.Key))
                                LeftController.PostMessage(msg, (IntPtr)actualKey, lParam);
                        }

                        if (rightKeys.ContainsValue(key))
                        {
                            foreach (Keys actualKey in rightKeys.Where(t => t.Value == key).Select(t => t.Key))
                                RightController.PostMessage(msg, (IntPtr)actualKey, lParam);
                        }
                        
                        if (key == Keys.F1 && msg == (uint)Win32.WM.KEYDOWN)
                        {
                            LeftController.PostMessage((uint)Win32.WM.KEYDOWN, (IntPtr)Keys.ControlKey, lParam);
                            LeftController.PostMessage((uint)Win32.WM.KEYUP, (IntPtr)Keys.ControlKey, lParam);
                        }
                        else if (key == Keys.F2 && msg == (uint)Win32.WM.KEYDOWN)
                        {
                            RightController.PostMessage((uint)Win32.WM.KEYDOWN, (IntPtr)Keys.ControlKey, lParam);
                            RightController.PostMessage((uint)Win32.WM.KEYUP, (IntPtr)Keys.ControlKey, lParam);
                        }
                    }
                }
                else
                {
                    if (currentMode == ControllerMode.Mirror)
                    {
                        foreach (var group in controllerGroups)
                        {
                            group.LeftController.PostMessage(msg, wParam, lParam);
                            group.RightController.PostMessage(msg, wParam, lParam);
                        }
                    }
                }

                ret = true;
            }

            return ret;
        }
    }

    class ControllerGroup
    {
        internal ToontownController LeftController = new ToontownController();
        internal ToontownController RightController = new ToontownController();
    }
}
