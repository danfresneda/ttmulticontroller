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
        public event EventHandler TTWindowActivated;
        public event EventHandler AllTTWindowsInactive;

        internal List<ControllerGroup> ControllerGroups { get; } = new List<ControllerGroup>();

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

        int currentIndividualControllerIndex = 0;

        internal int CurrentInvididualControllerIndex
        {
            get
            {
                if (AllControllersWithWindows.Count() > 0 && currentIndividualControllerIndex >= AllControllersWithWindows.Count())
                {
                    currentIndividualControllerIndex = 0;
                    updateControllerBorders();
                }

                return currentIndividualControllerIndex;
            }
            private set
            {
                currentIndividualControllerIndex = value;

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

        internal ToontownController CurrentIndividualController
        {
            get
            {
                if (AllControllersWithWindows.Count() > 0)
                {
                    return AllControllersWithWindows.ElementAt(CurrentInvididualControllerIndex);
                }

                return null;
            }
        }

        internal IEnumerable<ToontownController> AllControllers
        {
            get
            {
                return ControllerGroups.SelectMany(g => new[] { g.LeftController, g.RightController });
            }
        }

        internal IEnumerable<ToontownController> AllControllersWithWindows
        {
            get
            {
                return AllControllers.Where(c => c.HasWindow);
            }
        }

        internal enum ControllerMode
        {
            Multi,
            Mirror,
            Individual
        }

        public bool ErrorOccurredPostingMessage
        {
            get => ControllerGroups.Any(g => g.LeftController.ErrorOccurredPostingMessage || g.RightController.ErrorOccurredPostingMessage);
        }

        private bool showAllBorders = false;
        public bool ShowAllBorders
        {
            get => showAllBorders;
            set
            {
                if (showAllBorders != value)
                {
                    showAllBorders = value;
                    updateControllerBorders();
                }
            }
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

        Dictionary<Keys, List<Keys>> leftKeys = new Dictionary<Keys, List<Keys>>(),
            rightKeys = new Dictionary<Keys, List<Keys>>();

        internal Multicontroller()
        {

            UpdateKeys();

            for (int i = ControllerGroups.Count; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                AddControllerGroup();
            }

            updateControllerBorders();
        }

        internal void UpdateKeys()
        {
            leftKeys.Clear();
            rightKeys.Clear();

            var keyBindings = Properties.SerializedSettings.Default.Bindings;

            for (int i = 0; i < keyBindings.Count; i++)
            {
                if (!leftKeys.ContainsKey(keyBindings[i].LeftToonKey))
                {
                    leftKeys.Add(keyBindings[i].LeftToonKey, new List<Keys>());
                }

                if (!rightKeys.ContainsKey(keyBindings[i].RightToonKey))
                {
                    rightKeys.Add(keyBindings[i].RightToonKey, new List<Keys>());
                }

                if (keyBindings[i].Key != Keys.None && keyBindings[i].LeftToonKey != Keys.None)
                {
                    leftKeys[keyBindings[i].LeftToonKey].Add(keyBindings[i].Key);
                }

                if (keyBindings[i].Key != Keys.None && keyBindings[i].RightToonKey != Keys.None)
                {
                    rightKeys[keyBindings[i].RightToonKey].Add(keyBindings[i].Key);
                }
            }
        }

        internal ControllerGroup AddControllerGroup()
        {
            ControllerGroup group = new ControllerGroup();

            group.LeftController.GroupNumber = group.RightController.GroupNumber = ControllerGroups.Count + 1;
            group.LeftController.TTWindowActivated += Controller_TTWindowActivated;
            group.RightController.TTWindowActivated += Controller_TTWindowActivated;
            group.LeftController.TTWindowDeactivated += Controller_TTWindowDeactivated;
            group.RightController.TTWindowDeactivated += Controller_TTWindowDeactivated;
            group.LeftController.TTWindowClosed += Controller_TTWindowClosed;
            group.RightController.TTWindowClosed += Controller_TTWindowClosed;
            ControllerGroups.Add(group);
            GroupsChanged?.Invoke(this, EventArgs.Empty);

            updateControllerBorders();

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
        
        private void updateControllerBorders()
        {
            if (isActive)
            {
                if (CurrentMode == ControllerMode.Multi)
                {
                    IEnumerable<ControllerGroup> affectedGroups = Properties.Settings.Default.controlAllGroupsAtOnce
                        ? (IEnumerable<ControllerGroup>)ControllerGroups : new[] { ControllerGroups[CurrentGroupIndex] };

                    foreach (var group in ControllerGroups)
                    {
                        group.LeftController.BorderColor = Color.LimeGreen;
                        group.RightController.BorderColor = Color.Green;

                        group.LeftController.ShowBorder = group.RightController.ShowBorder =
                            ShowAllBorders || affectedGroups.Contains(group);

                        group.LeftController.ShowGroupNumber = group.RightController.ShowGroupNumber =
                            ShowAllBorders || ControllerGroups.Count > 1;
                    }
                }
                else if(CurrentMode == ControllerMode.Mirror)
                {
                    ControllerGroups.ForEach(g =>
                    {
                        g.LeftController.BorderColor = g.RightController.BorderColor = Color.Violet;
                        g.LeftController.ShowBorder = g.RightController.ShowBorder = true;
                        g.LeftController.ShowGroupNumber = g.RightController.ShowGroupNumber = ControllerGroups.Count > 1;
                    });
                }
                else if (CurrentMode == ControllerMode.Individual)
                {
                    foreach (var group in ControllerGroups)
                    {
                        group.LeftController.BorderColor = Color.Turquoise;
                        group.RightController.BorderColor = Color.Turquoise;

                        group.LeftController.ShowBorder = CurrentIndividualController == group.LeftController;
                        group.RightController.ShowBorder = CurrentIndividualController == group.RightController;
                    }
                }
            } 
            else
            {
                ControllerGroups.ForEach(g =>
                {
                    g.LeftController.ShowBorder = g.RightController.ShowBorder = false;
                });
            }
        }

        /// <summary>
        /// The main input processor. All input to the multicontroller window ends up here.
        /// </summary>
        internal bool ProcessKey(Keys key, uint msg = 0, IntPtr lParam = new IntPtr()) 
        {
            if (key == Keys.None)
            {
                return false;
            }

            // The return value determines whether the input is discarded (doesn't reach its intended destination)
            var shouldDiscardInput = false;
            
            IntPtr wParam = (IntPtr)key;

            if (key == (Keys)Properties.Settings.Default.modeKeyCode)
            {
                if (msg == (uint)Win32.WM.HOTKEY || msg == (uint)Win32.WM.KEYDOWN)
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
                    else
                    {
                        ShouldActivate?.Invoke(this, EventArgs.Empty);
                    }

                    shouldDiscardInput = true;
                }
            }
            else if (key == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                if (msg == (uint)Win32.WM.KEYDOWN)
                {
                    Properties.Settings.Default.controlAllGroupsAtOnce = !Properties.Settings.Default.controlAllGroupsAtOnce;
                    GroupsChanged?.Invoke(this, EventArgs.Empty);
                    updateControllerBorders();
                }
            }
            else if (key == (Keys)Properties.Settings.Default.individualControlKeyCode)
            {
                if (msg == (uint)Win32.WM.KEYDOWN)
                {
                    if (isActive)
                    {
                        if (currentMode == ControllerMode.Individual)
                        {
                            CurrentInvididualControllerIndex = (CurrentInvididualControllerIndex + 1) % AllControllers.Count();
                        }
                        else if (AllControllersWithWindows.Count() > 0)
                        {
                            CurrentMode = ControllerMode.Individual;
                        }
                    }

                    shouldDiscardInput = true;
                }
            }
            else if (isActive)
            {
                if (currentMode == ControllerMode.Multi)
                {
                    if (!Properties.Settings.Default.controlAllGroupsAtOnce 
                        && ControllerGroups.Count > 1
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
                        if (leftKeys.ContainsKey(key))
                        {
                            var affectedControllers = Properties.Settings.Default.controlAllGroupsAtOnce ?
                                ControllerGroups.Select(c => c.LeftController) : new[] { LeftController };

                            foreach (Keys actualKey in leftKeys[key])
                                affectedControllers.ToList().ForEach(c => c.PostMessage(msg, (IntPtr)actualKey, lParam));
                        }

                        if (rightKeys.ContainsKey(key))
                        {
                            var affectedControllers = Properties.Settings.Default.controlAllGroupsAtOnce ?
                                ControllerGroups.Select(c => c.RightController) : new[] { RightController };

                            foreach (Keys actualKey in rightKeys[key])
                                affectedControllers.ToList().ForEach(c => c.PostMessage(msg, (IntPtr)actualKey, lParam));
                        }
                    }
                }
                else if (CurrentMode == ControllerMode.Mirror)
                {
                    foreach (var group in ControllerGroups)
                    {
                        group.LeftController.PostMessage(msg, wParam, lParam);
                        group.RightController.PostMessage(msg, wParam, lParam);
                    }
                } 
                else if (CurrentMode == ControllerMode.Individual)
                {
                    CurrentIndividualController?.PostMessage(msg, wParam, lParam);
                }

                shouldDiscardInput = true;
            }

            return shouldDiscardInput;
        }

        private void Controller_TTWindowClosed(object sender)
        {
            if (sender == LeftController || sender == RightController)
            {
                GroupsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Controller_TTWindowActivated(object sender, IntPtr hWnd)
        {
            TTWindowActivated?.Invoke(this, EventArgs.Empty);
        }

        private void Controller_TTWindowDeactivated(object sender, IntPtr hWnd)
        {
            if (ControllerGroups.All(g => !g.LeftController.TTWindowActive && !g.RightController.TTWindowActive))
            {
                AllTTWindowsInactive?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    class ControllerGroup
    {
        internal ToontownController LeftController = new ToontownController();
        internal ToontownController RightController = new ToontownController();
    }
}
