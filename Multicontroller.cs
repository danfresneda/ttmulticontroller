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

        public event EventHandler ModeChanged;
        public event EventHandler GroupsChanged;
        public event EventHandler ShouldActivate;

        internal List<ControllerGroup> ControllerGroups { get; } = new List<ControllerGroup>()
        {
            new ControllerGroup()
        };

        int currentGroupIndex = 0;

        internal int CurrentGroupIndex
        {
            get
            {
                if (currentGroupIndex >= ControllerGroups.Count)
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
                return ControllerGroups[CurrentGroupIndex].LeftController;
            }
        }

        internal ToontownController RightController
        {
            get
            {
                return ControllerGroups[CurrentGroupIndex].RightController;
            }
        }

        internal enum ControllerMode
        {
            Multi,
            Mirror
        }

        private bool isActive = true;
        internal bool IsActive
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
                if (currentMode != value)
                {
                    currentMode = value;
                    ModeChanged?.Invoke(this, EventArgs.Empty);
                }

                updateControllerBorders();
            }
        }

        Dictionary<Keys, Keys> leftKeys = new Dictionary<Keys, Keys>(),
            rightKeys = new Dictionary<Keys, Keys>();

        internal Multicontroller()
        {
            // TODO: add this back in?
            //LeftController.TTWindowClosed += () =>
            //{
            //    mainWnd.LeftWindowClosed();
            //};

            //RightController.TTWindowClosed += () =>
            //{
            //    mainWnd.RightWindowClosed();
            //};

            UpdateKeys();
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

            for (int i = ControllerGroups.Count; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                AddControllerGroup();
            }

            updateControllerBorders();
        }

        internal ControllerGroup AddControllerGroup()
        {
            ControllerGroup group = new ControllerGroup();

            group.LeftController.TTWindowClosed += Controller_TTWindowClosed;
            group.RightController.TTWindowClosed += Controller_TTWindowClosed;
            ControllerGroups.Add(group);
            GroupsChanged?.Invoke(this, EventArgs.Empty);

            return group;
        }

        internal void RemoveControllerGroup(int index)
        {
            ControllerGroup controllerGroup = ControllerGroups[index];
            controllerGroup.LeftController.Shutdown();
            controllerGroup.RightController.Shutdown();
            ControllerGroups.Remove(controllerGroup);
            GroupsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Controller_TTWindowClosed(object sender)
        {
            if (sender == LeftController || sender == RightController)
            {
                GroupsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void updateControllerBorders()
        {
            if (CurrentMode == ControllerMode.Multi)
            {
                LeftController.BorderColor = RightController.BorderColor = Color.LimeGreen;
                LeftController.ShowBorder = RightController.ShowBorder = isActive;

                var otherGroups = ControllerGroups.Except(new[] { ControllerGroups[CurrentGroupIndex] }).ToList();

                otherGroups.ForEach(g =>
                {
                    g.LeftController.ShowBorder = g.RightController.ShowBorder = false;
                });
            }
            else
            {
                ControllerGroups.ForEach(g =>
                {
                    g.LeftController.BorderColor = g.RightController.BorderColor = Color.Violet;
                    g.LeftController.ShowBorder = g.RightController.ShowBorder = isActive;
                });
            }
        }

        /// <summary>
        /// The main input processor. All input to the multicontroller window ends up here.
        /// </summary>
        internal bool ProcessKey(Keys key, uint msg = 0, IntPtr lParam = new IntPtr()) 
        {
            // The return value determines whether the input is discarded (doesn't reach its intended destination)
            var shouldDiscardInput = false;
            
            IntPtr wParam = (IntPtr)key;

            if (key == (Keys)Properties.Settings.Default.modeKeyCode)
            {
                IntPtr activeWnd = Win32.GetForegroundWindow();
                bool isAnyTTWindowActive = ControllerGroups.Any(g => g.LeftController.TTWindowHandle == activeWnd || g.RightController.TTWindowHandle == activeWnd);

                if (msg == (uint)Win32.WM.KEYDOWN && (isAnyTTWindowActive || isActive))
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

                    if (isAnyTTWindowActive)
                    {
                        ShouldActivate?.Invoke(this, EventArgs.Empty);
                    }

                    shouldDiscardInput = true;
                }
            }
            else if (isActive)
            {
                if (currentMode == ControllerMode.Multi)
                {
                    if (ControllerGroups.Count > 1
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

                        if (ControllerGroups.Count > index)
                        {
                            CurrentGroupIndex = index;
                            GroupsChanged?.Invoke(this, EventArgs.Empty);
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
                        foreach (var group in ControllerGroups)
                        {
                            group.LeftController.PostMessage(msg, wParam, lParam);
                            group.RightController.PostMessage(msg, wParam, lParam);
                        }
                    }
                }

                shouldDiscardInput = true;
            }

            return shouldDiscardInput;
        }
    }

    class ControllerGroup
    {
        internal ToontownController LeftController = new ToontownController();
        internal ToontownController RightController = new ToontownController();
    }
}
