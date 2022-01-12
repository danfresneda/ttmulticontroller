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

        /// <summary>
        /// The index of the group that is currently being controlled, if applicable in the current mode
        /// </summary>
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

        int _currentPairIndex = 0;

        /// <summary>
        /// The index of the current pair that is being controlled (in pair mode)
        /// </summary>
        internal int CurrentPairIndex
        {
            get
            {
                if (_currentPairIndex > AllControllerPairsWithWindows.Count())
                {
                    _currentPairIndex = 0;
                    updateControllerBorders();
                }

                return _currentPairIndex;
            }
            set
            {
                _currentPairIndex = value;

                updateControllerBorders();
            }
        }

        int _currentIndividualControllerIndex = 0;

        internal int CurrentIndividualControllerIndex
        {
            get
            {
                if (AllControllersWithWindows.Count() > 0 && _currentIndividualControllerIndex >= AllControllersWithWindows.Count())
                {
                    _currentIndividualControllerIndex = 0;
                    updateControllerBorders();
                }

                return _currentIndividualControllerIndex;
            }
            private set
            {
                _currentIndividualControllerIndex = value;

                updateControllerBorders();
            }
        }

        /// <summary>
        /// Left controllers of the current group, or all groups if all groups are being controlled at once
        /// </summary>
        internal IEnumerable<ToontownController> LeftControllers
        {
            get
            {
                if (CurrentMode == ControllerMode.AllGroup)
                {
                    return ControllerGroups.SelectMany(g => g.LeftControllers);
                }
                else if (CurrentMode == ControllerMode.Pair)
                {
                    if (CurrentControllerPair != null)
                    {
                        return new[] { CurrentControllerPair?.LeftController };
                    } 
                    else
                    {
                        return new ToontownController[] { };
                    }
                }
                else
                {
                    return ControllerGroups[CurrentGroupIndex].LeftControllers;
                }
            }
        }

        /// <summary>
        /// Right controllers of the current group, or all groups if all groups are being controlled at once
        /// </summary>
        internal IEnumerable<ToontownController> RightControllers
        {
            get
            {
                if (CurrentMode == ControllerMode.AllGroup)
                {
                    return ControllerGroups.SelectMany(g => g.RightControllers);
                }
                else if (CurrentMode == ControllerMode.Pair)
                {
                    if (CurrentControllerPair != null)
                    {
                        return new[] { CurrentControllerPair?.RightController };
                    }
                    else
                    {
                        return new ToontownController[] { };
                    }
                }
                else
                {
                    return ControllerGroups[CurrentGroupIndex].RightControllers;
                }
            }
        }

        /// <summary>
        /// The current controller that is being controlled individually
        /// </summary>
        internal ToontownController CurrentIndividualController
        {
            get
            {
                if (CurrentIndividualControllerIndex < AllControllersWithWindows.Count())
                {
                    return AllControllersWithWindows.ElementAt(CurrentIndividualControllerIndex);
                }

                return null;
            }
        }

        internal IEnumerable<ToontownController> AllControllers
        {
            get
            {
                return ControllerGroups.SelectMany(g => g.ControllerPairs.SelectMany(p => new[] { p.LeftController, p.RightController }));
            }
        }

        internal IEnumerable<ToontownController> AllControllersWithWindows
        {
            get
            {
                return AllControllers.Where(c => c.HasWindow);
            }
        }

        internal IEnumerable<ControllerPair> AllControllerPairs
        {
            get
            {
                return ControllerGroups.SelectMany(g => g.ControllerPairs);
            }
        }

        internal IEnumerable<ControllerPair> AllControllerPairsWithWindows
        {
            get
            {
                return AllControllerPairs.Where(p => p.LeftController.HasWindow || p.RightController.HasWindow);
            }
        }

        internal ControllerPair CurrentControllerPair
        {
            get
            {
                if (AllControllerPairsWithWindows.Count() > 0)
                {
                    return AllControllerPairsWithWindows.ElementAt(CurrentPairIndex);
                }

                return null;
            }
        }

        internal enum ControllerMode
        {
            /// <summary>
            /// Control all pairs of toons in the current group with separate left and right controls (default mode)
            /// </summary>
            Group,

            /// <summary>
            /// Control both toons in the current pair with separate left and right controls
            /// </summary>
            Pair,

            /// <summary>
            /// Control all groups of toons with separate left and right controls
            /// </summary>
            AllGroup,

            /// <summary>
            /// Mirror all input to all groups of toons
            /// </summary>
            MirrorAll,

            /// <summary>
            /// Mirror all input to all pairs of the current group
            /// </summary>
            MirrorGroup,

            /// <summary>
            /// Mirror all input to one controller
            /// </summary>
            MirrorIndividual
        }

        /// <summary>
        /// Whether an error occurred when posting a message to a Toontown window.
        /// This usually indicated that we don't have enough privileges and need to run as administrator.
        /// </summary>
        public bool ErrorOccurredPostingMessage
        {
            get => ControllerGroups.Any(g => g.AllControllers.Any(c => c.ErrorOccurredPostingMessage));
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

        ControllerMode _currentMode = ControllerMode.Group;

        internal ControllerMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                if (_currentMode != value)
                {
                    _currentMode = value;
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
            ControllerGroup group = new ControllerGroup(ControllerGroups.Count + 1);

            group.ControllerWindowActivated += Controller_WindowActivated;
            group.ControllerWindowDeactivated += Controller_WindowDeactivated;
            group.ControllerWindowClosed += Controller_WindowClosed;
            group.MouseEvent += Controller_MouseEvent;
            group.PairAddedRemoved += Group_PairAddedRemoved;

            ControllerGroups.Add(group);
            GroupsChanged?.Invoke(this, EventArgs.Empty);

            updateControllerBorders();

            return group;
        }

        private void Group_PairAddedRemoved(object sender, EventArgs e)
        {
            updateControllerBorders();
        }

        private void Controller_MouseEvent(object sender, Message m)
        {
            ProcessInput(m.Msg, m.WParam, m.LParam, sender as ToontownController);
        }

        internal void RemoveControllerGroup(int index)
        {
            ControllerGroup controllerGroup = ControllerGroups[index];
            controllerGroup.Dispose();
            ControllerGroups.Remove(controllerGroup);
            GroupsChanged?.Invoke(this, EventArgs.Empty);
        }
        
        private void updateControllerBorders()
        {
            if (ShowAllBorders && isActive)
            {
                foreach (ToontownController controller in ControllerGroups.SelectMany(g => g.LeftControllers))
                {
                    controller.BorderColor = Color.LimeGreen;
                }

                foreach (ToontownController controller in ControllerGroups.SelectMany(g => g.RightControllers))
                {
                    controller.BorderColor = Color.Green;
                }

                foreach (ToontownController controller in AllControllers)
                {
                    controller.ShowBorder = true;
                    controller.ShowGroupNumber = true;
                    controller.ShowFakeCursor = false;
                    controller.CaptureMouseEvents = false;
                }
            }
            else if (isActive)
            {
                if (CurrentMode == ControllerMode.Group || CurrentMode == ControllerMode.AllGroup)
                {
                    IEnumerable<ControllerGroup> affectedGroups = CurrentMode == ControllerMode.AllGroup 
                        ? (IEnumerable<ControllerGroup>)ControllerGroups : new[] { ControllerGroups[CurrentGroupIndex] };

                    foreach (ControllerGroup group in affectedGroups)
                    {
                        foreach (ToontownController controller in group.LeftControllers)
                        {
                            controller.BorderColor = Color.LimeGreen;
                        }

                        foreach (ToontownController controller in group.RightControllers)
                        {
                            controller.BorderColor = Color.Green;
                        }

                        foreach (ToontownController controller in group.AllControllers)
                        {
                            controller.ShowBorder = true;
                            controller.ShowGroupNumber = ControllerGroups.Count > 1;
                            controller.CaptureMouseEvents = Properties.Settings.Default.replicateMouse;
                        }
                    }

                    foreach (ToontownController controller in ControllerGroups.Except(affectedGroups).SelectMany(g => g.AllControllers))
                    {
                        controller.ShowBorder = false;
                    }
                }
                else if (CurrentMode == ControllerMode.Pair)
                {
                    foreach (ControllerPair pair in AllControllerPairs)
                    {
                        if (pair == CurrentControllerPair)
                        {
                            pair.LeftController.BorderColor = Color.LimeGreen;
                            pair.RightController.BorderColor = Color.Green;

                            foreach (ToontownController controller in pair.AllControllers)
                            {
                                controller.ShowGroupNumber = false;
                                controller.CaptureMouseEvents = Properties.Settings.Default.replicateMouse;
                                controller.ShowBorder = true;
                            }
                        }
                        else
                        {
                            foreach (ToontownController controller in pair.AllControllers)
                            {
                                controller.ShowBorder = false;
                            }
                        }
                    }
                }
                else if (CurrentMode == ControllerMode.MirrorGroup)
                {
                    ControllerGroup currentGroup = ControllerGroups[CurrentGroupIndex];

                    foreach (ToontownController controller in currentGroup.AllControllers)
                    {
                        controller.BorderColor = Color.Violet;
                        controller.ShowGroupNumber = ControllerGroups.Count > 1;
                        controller.CaptureMouseEvents = Properties.Settings.Default.replicateMouse;
                        controller.ShowBorder = true;
                    }

                    foreach (ToontownController controller in ControllerGroups.Except(new[] { currentGroup }).SelectMany(g => g.AllControllers))
                    {
                        controller.ShowBorder = false;
                    }
                }
                else if (CurrentMode == ControllerMode.MirrorAll)
                {
                    foreach (ToontownController controller in AllControllers)
                    {
                        controller.BorderColor = Color.Violet;
                        controller.ShowBorder = true;
                        controller.ShowGroupNumber = ControllerGroups.Count > 1;
                        controller.CaptureMouseEvents = Properties.Settings.Default.replicateMouse;
                    }
                }
                else if (CurrentMode == ControllerMode.MirrorIndividual)
                {
                    foreach (ToontownController controller in AllControllers)
                    {
                        controller.BorderColor = Color.Turquoise;
                        controller.ShowBorder = CurrentIndividualController == controller;
                        controller.ShowGroupNumber = false;
                        controller.CaptureMouseEvents = CurrentIndividualController == controller;
                    }
                }
            } 
            else
            {
                foreach (ToontownController controller in AllControllers)
                {
                    controller.ShowBorder = false;
                }
            }
        }

        /// <summary>
        /// The main input processor. All input to the multicontroller window ends up here.
        /// </summary>
        /// <returns>Whether the input is discarded (doesn't reach its intended destination)</returns>
        internal bool ProcessInput(int msgCode, IntPtr wParam, IntPtr lParam, ToontownController sourceController = null) 
        {
            Win32.WM msg = (Win32.WM)msgCode;
            bool isKeyboardInput = false;
            bool isMouseInput = false;
            Keys keysPressed = Keys.None;

            switch (msg)
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

            if (isMouseInput)
            {
                return ProcessMouseInput(msg, wParam, lParam, sourceController);
            }
            else if (isKeyboardInput)
            {
                return ProcessMetaKeyboardInput(msg, keysPressed)
                    || ProcessKeyboardInput(msg, wParam, lParam);
            }

            return false;
        }

        /// <summary>
        /// Process keyboard input for meta actions (hotkeys, changing groups, etc.)
        /// </summary>
        /// <returns>True the input was handled as a meta input</returns>
        private bool ProcessMetaKeyboardInput(Win32.WM msg, Keys keysPressed)
        {
            if (keysPressed == (Keys)Properties.Settings.Default.modeKeyCode)
            {
                if (msg == Win32.WM.HOTKEY || msg == Win32.WM.KEYDOWN)
                {
                    if (isActive)
                    {
                        List<ControllerMode> availableModesToCycle = new List<ControllerMode>();

                        if (Properties.Settings.Default.groupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(ControllerMode.Group);
                        }

                        if (Properties.Settings.Default.mirrorModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(ControllerMode.MirrorAll);
                        }

                        if (Properties.Settings.Default.allGroupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(ControllerMode.AllGroup);
                        }

                        if (Properties.Settings.Default.mirrorGroupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(ControllerMode.MirrorGroup);
                        }

                        int currentModeIndex = availableModesToCycle.IndexOf(CurrentMode);

                        if (currentModeIndex >= 0)
                        {
                            currentModeIndex = (currentModeIndex + 1) % availableModesToCycle.Count;

                            CurrentMode = availableModesToCycle[currentModeIndex];
                        }
                        else if (availableModesToCycle.Count > 0)
                        {
                            CurrentMode = availableModesToCycle[0];
                        }
                    }
                    else
                    {
                        ShouldActivate?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.groupModeKeyCode)
            {
                CurrentMode = ControllerMode.Group;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.mirrorModeKeyCode)
            {
                CurrentMode = ControllerMode.MirrorAll;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                CurrentMode = ControllerMode.AllGroup;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.mirrorGroupModeKeyCode)
            {
                CurrentMode = ControllerMode.MirrorGroup;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.pairModeKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && isActive && AllControllerPairsWithWindows.Count() > 0)
                {
                    if (CurrentMode == ControllerMode.Pair)
                    {
                        CurrentPairIndex = (CurrentPairIndex + 1) % AllControllerPairsWithWindows.Count();
                    }
                    else
                    {
                        CurrentMode = ControllerMode.Pair;
                    }
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.replicateMouseKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN)
                {
                    Properties.Settings.Default.replicateMouse = !Properties.Settings.Default.replicateMouse;
                    SettingChangedByHotkey?.Invoke(this, EventArgs.Empty);
                    updateControllerBorders();
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && CurrentMode != ControllerMode.AllGroup)
                {
                    CurrentMode = ControllerMode.AllGroup;
                    GroupsChanged?.Invoke(this, EventArgs.Empty);
                    updateControllerBorders();
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.individualControlKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && isActive && AllControllersWithWindows.Count() > 0)
                {
                    if (CurrentMode == ControllerMode.MirrorIndividual)
                    {
                        CurrentIndividualControllerIndex = (CurrentIndividualControllerIndex + 1) % AllControllersWithWindows.Count();
                    }
                    else
                    {
                        CurrentMode = ControllerMode.MirrorIndividual;
                    }
                }
            }
            else if ((CurrentMode == ControllerMode.Group || CurrentMode == ControllerMode.MirrorGroup)
                && ControllerGroups.Count > 1
                && (keysPressed >= Keys.D0 && keysPressed <= Keys.D9
                    || keysPressed >= Keys.NumPad0 && keysPressed <= Keys.NumPad9))
            {
                // Change groups while in group mode
                int index;

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
                return false;
            }

            return true;
        }

        /// <summary>
        /// Process mouse input
        /// </summary>
        /// <returns>True if the input was handled</returns>
        private bool ProcessMouseInput(Win32.WM msg, IntPtr wParam, IntPtr lParam, ToontownController sourceController)
        {
            if (isActive)
            {
                List<ToontownController> affectedControllers = new List<ToontownController>();
                
                if (CurrentMode == ControllerMode.Group || CurrentMode == ControllerMode.AllGroup || CurrentMode == ControllerMode.Pair)
                {
                    if (LeftControllers.Contains(sourceController))
                    {
                        affectedControllers.AddRange(LeftControllers);
                    }

                    if (RightControllers.Contains(sourceController))
                    {
                        affectedControllers.AddRange(RightControllers);
                    }
                }
                else if (CurrentMode == ControllerMode.MirrorGroup)
                {
                    affectedControllers.AddRange(ControllerGroups[CurrentGroupIndex].AllControllers);
                }
                else if (CurrentMode == ControllerMode.MirrorAll)
                {
                    affectedControllers.AddRange(AllControllers);
                }
                else if (CurrentMode == ControllerMode.MirrorIndividual)
                {
                    if (CurrentIndividualController != null)
                    {
                        affectedControllers.Add(CurrentIndividualController);
                    }
                }

                bool forwardMove = false;

                if (msg == Win32.WM.MOUSEMOVE)
                {
                    /*
                    * Filter out small mouse movements while holding down a button. If MOUSELEAVE is
                    * fired between BUTTONDOWN & BUTTONUP events, this seems to cancel the click.
                    * MOUSELEAVE is generated on every MOUSEMOVE since Toontown is not the active window.
                    */

                    int x = (short)lParam;
                    int y = (short)(lParam.ToInt32() >> 16);

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

                    foreach (ToontownController controller in AllControllers)
                    {
                        controller.FakeCursorPosition = new Point(x, y);
                        controller.ShowFakeCursor = controller != sourceController;
                    }
                }

                foreach (ToontownController controller in AllControllers)
                {
                    if (msg != Win32.WM.MOUSELEAVE && affectedControllers.Contains(controller))
                    {
                        bool windowSizeDifferent =
                            controller.WindowSize.Width > sourceController.WindowSize.Width + 5
                            || controller.WindowSize.Width < sourceController.WindowSize.Width - 5
                            || controller.WindowSize.Height > sourceController.WindowSize.Height + 5
                            || controller.WindowSize.Height < sourceController.WindowSize.Height - 5;

                        controller.IsWindowSizeMismatched = windowSizeDifferent;

                        if (!windowSizeDifferent && (msg != Win32.WM.MOUSEMOVE || forwardMove))
                        {
                            controller.SendMessage(msg, wParam, lParam);
                        }
                    }
                    else
                    {
                        controller.ShowFakeCursor = false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Process keyboard input
        /// </summary>
        /// <returns>True if the input was handled</returns>
        private bool ProcessKeyboardInput(Win32.WM msg, IntPtr wParam, IntPtr lParam)
        {
            if (isActive)
            {
                Keys keysPressed = (Keys)wParam;

                List<ToontownController> affectedControllers = new List<ToontownController>();
                List<Keys> keysToPress = new List<Keys>();

                if (CurrentMode == ControllerMode.Group || CurrentMode == ControllerMode.AllGroup || CurrentMode == ControllerMode.Pair)
                {
                    if (leftKeys.ContainsKey(keysPressed))
                    {
                        affectedControllers.AddRange(LeftControllers);

                        keysToPress.AddRange(leftKeys[keysPressed]);
                    }

                    if (rightKeys.ContainsKey(keysPressed))
                    {
                        affectedControllers.AddRange(RightControllers);

                        keysToPress.AddRange(rightKeys[keysPressed]);
                    }
                }
                else if (CurrentMode == ControllerMode.MirrorGroup)
                {
                    affectedControllers.AddRange(ControllerGroups[CurrentGroupIndex].AllControllers);
                }
                else if (CurrentMode == ControllerMode.MirrorAll)
                {
                    affectedControllers.AddRange(AllControllers);
                }
                else if (CurrentMode == ControllerMode.MirrorIndividual)
                {
                    if (CurrentIndividualController != null)
                    {
                        affectedControllers.Add(CurrentIndividualController);
                    }
                }

                if (CurrentMode == ControllerMode.MirrorAll
                    || CurrentMode == ControllerMode.MirrorGroup
                    || CurrentMode == ControllerMode.MirrorIndividual)
                {
                    affectedControllers.ForEach(c => c.PostMessage(msg, wParam, lParam));
                }
                else
                {
                    foreach (Keys actualKey in keysToPress)
                        affectedControllers.ToList().ForEach(c => c.PostMessage(msg, (IntPtr)actualKey, lParam));
                }

                return true;
            }

            return false;
        }

        private void Controller_WindowClosed(object sender, EventArgs e)
        {
            if (LeftControllers.Contains(sender) || RightControllers.Contains(sender))
            {
                GroupsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Controller_WindowActivated(object sender, EventArgs e)
        {
            TTWindowActivated?.Invoke(this, EventArgs.Empty);
        }

        private void Controller_WindowDeactivated(object sender, EventArgs e)
        {
            if (!AllControllersWithWindows.Any(c => c.IsWindowActive))
            {
                AllTTWindowsInactive?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    
}
