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
        public event EventHandler SettingChangedByHotkey;

        internal List<ControllerGroup> ControllerGroups { get; } = new List<ControllerGroup>();

        int currentGroupIndex = 0;

        internal int CurrentGroupIndex
        {
            get
            {
                if (ControllerGroups.Count > 0 && currentGroupIndex >= ControllerGroups.Count)
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

        private bool isActive = false;
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

        int lastMoveX, lastMoveY;

        internal Multicontroller()
        {

            UpdateOptions();

            for (int i = ControllerGroups.Count; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                AddControllerGroup();
            }

            updateControllerBorders();
        }

        internal void UpdateOptions()
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

            updateControllerBorders();
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
            group.LeftController.MouseEvent += Controller_MouseEvent;
            group.RightController.MouseEvent += Controller_MouseEvent;

            ControllerGroups.Add(group);
            GroupsChanged?.Invoke(this, EventArgs.Empty);

            updateControllerBorders();

            return group;
        }

        private void Controller_MouseEvent(object sender, Message m)
        {
            ProcessInput((uint)m.Msg, m.WParam, m.LParam, sender as ToontownController);
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

                        group.LeftController.CaptureMouseEvents = group.RightController.CaptureMouseEvents = 
                            Properties.Settings.Default.replicateMouse;
                    }
                }
                else if(CurrentMode == ControllerMode.Mirror)
                {
                    ControllerGroups.ForEach(g =>
                    {
                        g.LeftController.BorderColor = g.RightController.BorderColor = Color.Violet;
                        g.LeftController.ShowBorder = g.RightController.ShowBorder = true;
                        g.LeftController.ShowGroupNumber = g.RightController.ShowGroupNumber = ControllerGroups.Count > 1;
                        g.LeftController.CaptureMouseEvents = g.RightController.CaptureMouseEvents =
                            Properties.Settings.Default.replicateMouse;
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

                        group.LeftController.CaptureMouseEvents = CurrentIndividualController == group.LeftController;
                        group.RightController.CaptureMouseEvents = CurrentIndividualController == group.RightController;
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
        internal bool ProcessInput(uint msg = 0, IntPtr wParam = new IntPtr(), IntPtr lParam = new IntPtr(), ToontownController sourceController = null) 
        {
            // The return value determines whether the input is discarded (doesn't reach its intended destination)
            var shouldDiscardInput = false;
            bool isKeyboardInput = false;
            bool isMouseInput = false;
            Keys keysPressed = Keys.None;

            switch ((Win32.WM)msg)
            {
                case Win32.WM.KEYDOWN:
                case Win32.WM.KEYUP:
                    isKeyboardInput = true;
                    keysPressed = (Keys)wParam;
                    break;
                case Win32.WM.HOTKEY:
                    isKeyboardInput = true;
                    keysPressed = (Keys)(lParam.ToInt32() >> 16);
                    break;
                case Win32.WM.MOUSEMOVE:
                case Win32.WM.LBUTTONDOWN:
                case Win32.WM.LBUTTONUP:
                case Win32.WM.RBUTTONDOWN:
                case Win32.WM.RBUTTONUP:
                case Win32.WM.MBUTTONDOWN:
                case Win32.WM.MBUTTONUP:
                case Win32.WM.MOUSEHOVER:
                case Win32.WM.MOUSEWHEEL:
                case Win32.WM.MOUSELEAVE:
                    isMouseInput = true;
                    break;
            }

            if (isKeyboardInput && keysPressed == (Keys)Properties.Settings.Default.modeKeyCode)
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
            else if (isKeyboardInput && keysPressed == (Keys)Properties.Settings.Default.replicateMouseKeyCode)
            {
                if (msg == (uint)Win32.WM.KEYDOWN)
                {
                    Properties.Settings.Default.replicateMouse = !Properties.Settings.Default.replicateMouse;
                    SettingChangedByHotkey?.Invoke(this, EventArgs.Empty);
                    updateControllerBorders();
                }
            }
            else if (isKeyboardInput && currentMode == ControllerMode.Multi && keysPressed == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                if (msg == (uint)Win32.WM.KEYDOWN)
                {
                    Properties.Settings.Default.controlAllGroupsAtOnce = !Properties.Settings.Default.controlAllGroupsAtOnce;
                    SettingChangedByHotkey?.Invoke(this, EventArgs.Empty);
                    GroupsChanged?.Invoke(this, EventArgs.Empty);
                    updateControllerBorders();
                }
            }
            else if (isKeyboardInput && keysPressed == (Keys)Properties.Settings.Default.individualControlKeyCode)
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
                List<ToontownController> affectedControllers = new List<ToontownController>();
                List<Keys> keysToPress = new List<Keys>();

                if (currentMode == ControllerMode.Multi)
                {
                    if (isKeyboardInput
                        && !Properties.Settings.Default.controlAllGroupsAtOnce
                        && ControllerGroups.Count > 1
                        && (keysPressed >= Keys.D0 && keysPressed <= Keys.D9
                            || keysPressed >= Keys.NumPad0 && keysPressed <= Keys.NumPad9))
                    {
                        int index = 0;

                        if (keysPressed >= Keys.D0 && keysPressed <= Keys.D9)
                        {
                            index = 9 - (Keys.D9 - keysPressed);
                        }
                        else
                        {
                            index = 9 - (Keys.NumPad9 - keysPressed);
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
                        if ((isKeyboardInput && leftKeys.ContainsKey(keysPressed))
                            || (isMouseInput && sourceController == LeftController))
                        {
                            affectedControllers.AddRange(Properties.Settings.Default.controlAllGroupsAtOnce ?
                                ControllerGroups.Select(c => c.LeftController) : new[] { LeftController });

                            if (isKeyboardInput)
                            {
                                keysToPress.AddRange(leftKeys[keysPressed]);
                            }
                        }

                        if ((isKeyboardInput && rightKeys.ContainsKey(keysPressed))
                            || (isMouseInput && sourceController == RightController))
                        {
                            affectedControllers.AddRange(Properties.Settings.Default.controlAllGroupsAtOnce ?
                                ControllerGroups.Select(c => c.RightController) : new[] { RightController });

                            if (isKeyboardInput)
                            {
                                keysToPress.AddRange(rightKeys[keysPressed]);
                            }
                        }
                    }
                }
                else if (CurrentMode == ControllerMode.Mirror)
                {
                    affectedControllers.AddRange(AllControllers);
                }
                else if (CurrentMode == ControllerMode.Individual)
                {
                    if (CurrentIndividualController != null)
                    {
                        affectedControllers.Add(CurrentIndividualController);
                    }
                }

                if (isKeyboardInput && (CurrentMode == ControllerMode.Mirror || CurrentMode == ControllerMode.Individual))
                {
                    affectedControllers.ForEach(c => c.PostMessage(msg, wParam, lParam));
                }
                else
                {
                    foreach (Keys actualKey in keysToPress)
                        affectedControllers.ToList().ForEach(c => c.PostMessage(msg, (IntPtr)actualKey, lParam));
                }

                if (isMouseInput)
                {
                    bool forwardMove = false;

                    if ((Win32.WM)msg == Win32.WM.MOUSEMOVE)
                    {
                        /*
                         * Filter out small mouse movements while holding down a button. If MOUSELEAVE is
                         * fired between BUTTONDOWN & BUTTONUP events, this seems to cancel the click.
                         * MOUSELEAVE is generated on every MOUSEMOVE since Toontown is not the active window.
                         */

                        int x = (Int16)lParam;
                        int y = (Int16)(lParam.ToInt32() >> 16);

                        int xDelta = Math.Abs(lastMoveX - x),
                            yDelta = Math.Abs(lastMoveY - y);

                        bool buttonDown =
                            ((int)wParam & Win32.MK_LBUTTON) == Win32.MK_LBUTTON
                            || ((int)wParam & Win32.MK_MBUTTON) == Win32.MK_MBUTTON
                            || ((int)wParam & Win32.MK_RBUTTON) == Win32.MK_RBUTTON;

                        if (!buttonDown || xDelta > 10 || yDelta > 10)
                        {
                            lastMoveX = x;
                            lastMoveY = y;
                        }

                        if (buttonDown && (xDelta > 10 || yDelta > 10))
                        {
                            forwardMove = true;
                        }

                        foreach (ToontownController c in AllControllers)
                        {
                            c.FakeCursorPosition = new Point(x, y);
                            c.ShowFakeCursor = c != sourceController;
                        }
                    }

                    foreach (ToontownController c in AllControllers)
                    {
                        if ((Win32.WM)msg != Win32.WM.MOUSELEAVE && affectedControllers.Contains(c))
                        {
                            bool windowSizeDifferent =
                                c.TTWindowSize.Width > sourceController.TTWindowSize.Width + 5
                                || c.TTWindowSize.Width < sourceController.TTWindowSize.Width - 5
                                || c.TTWindowSize.Height > sourceController.TTWindowSize.Height + 5
                                || c.TTWindowSize.Height < sourceController.TTWindowSize.Height - 5;

                            c.TTWindowSizeMismatched = windowSizeDifferent;

                            if (!windowSizeDifferent && ((Win32.WM)msg != Win32.WM.MOUSEMOVE || forwardMove))
                            {
                                c.SendMessage(msg, wParam, lParam);
                            }
                        }
                        else
                        {
                            c.ShowFakeCursor = false;
                        }
                    }
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
