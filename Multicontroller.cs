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
    internal enum MulticontrollerMode
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

    class Multicontroller
    {
        private static Multicontroller _instance = null;

        internal static Multicontroller Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Multicontroller();

                    for (int i = 0; i < Properties.Settings.Default.numberOfGroups; i++)
                    {
                        _instance.AddControllerGroup();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// The multicontroller was activated or deactivated
        /// </summary>
        public event EventHandler ActiveChanged;

        /// <summary>
        /// The mode of the multicontroller changed
        /// </summary>
        public event EventHandler ModeChanged;

        /// <summary>
        /// The controllers that are active changed
        /// </summary>
        public event EventHandler ActiveControllersChanged;

        /// <summary>
        /// A group was added or removed
        /// </summary>
        public event EventHandler GroupsChanged;

        /// <summary>
        /// A misc. setting of the multicontroller was changed
        /// </summary>
        public event EventHandler SettingChanged;

        /// <summary>
        /// The multicontroller should be actived (due to a hotkey)
        /// </summary>
        public event EventHandler ShouldActivate;

        /// <summary>
        /// A controlled window was activated
        /// </summary>
        public event EventHandler WindowActivated;

        /// <summary>
        /// All controlled windows are now inactive
        /// </summary>
        public event EventHandler AllWindowsInactive;
        
        internal List<ControllerGroup> ControllerGroups { get; } = new List<ControllerGroup>();

        internal IEnumerable<ToontownController> ActiveControllers
        {
            get
            {
                switch (CurrentMode)
                {
                    case MulticontrollerMode.Group:
                    case MulticontrollerMode.MirrorGroup:
                        return ControllerGroups[CurrentGroupIndex].AllControllers;
                    case MulticontrollerMode.Pair:
                        if (CurrentControllerPair != null)
                        {
                            return CurrentControllerPair.AllControllers;
                        }
                        break;
                    case MulticontrollerMode.AllGroup:
                    case MulticontrollerMode.MirrorAll:
                        return AllControllers;
                    case MulticontrollerMode.MirrorIndividual:
                        if (CurrentIndividualController != null)
                        {
                            return new[] { CurrentIndividualController };
                        }
                        break;
                }

                return new ToontownController[] { };
            }
        }

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
                }

                return currentGroupIndex;
            }
            private set
            {
                if (currentGroupIndex != value)
                {
                    currentGroupIndex = value;

                    ActiveControllersChanged?.Invoke(this, EventArgs.Empty);
                }
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
                }

                return _currentPairIndex;
            }
            set
            {
                if (_currentPairIndex != value)
                {
                    _currentPairIndex = value;

                    ActiveControllersChanged?.Invoke(this, EventArgs.Empty);
                }
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
                }

                return _currentIndividualControllerIndex;
            }
            private set
            {
                if (_currentIndividualControllerIndex != value)
                {
                    _currentIndividualControllerIndex = value;

                    ActiveControllersChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Left controllers of the current group, or all groups if all groups are being controlled at once
        /// </summary>
        internal IEnumerable<ToontownController> LeftControllers
        {
            get
            {
                if (CurrentMode == MulticontrollerMode.AllGroup)
                {
                    return ControllerGroups.SelectMany(g => g.LeftControllers);
                }
                else if (CurrentMode == MulticontrollerMode.Pair)
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
                if (CurrentMode == MulticontrollerMode.AllGroup)
                {
                    return ControllerGroups.SelectMany(g => g.RightControllers);
                }
                else if (CurrentMode == MulticontrollerMode.Pair)
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

                    SettingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private bool _isActive = false;
        internal bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                    ActiveChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        MulticontrollerMode _currentMode = MulticontrollerMode.Group;

        internal MulticontrollerMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                if (_currentMode != value)
                {
                    _currentMode = value;
                    ModeChanged?.Invoke(this, EventArgs.Empty);
                    ActiveControllersChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        Dictionary<Keys, List<Keys>> leftKeys = new Dictionary<Keys, List<Keys>>(),
            rightKeys = new Dictionary<Keys, List<Keys>>();

        int lastMoveX, lastMoveY;

        internal Multicontroller()
        {
            UpdateOptions();
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
        }

        internal ControllerGroup AddControllerGroup()
        {
            ControllerGroup group = new ControllerGroup(ControllerGroups.Count + 1);

            group.ControllerWindowActivated += Controller_WindowActivated;
            group.ControllerWindowDeactivated += Controller_WindowDeactivated;
            group.ControllerWindowHandleChanged += Controller_WindowHandleChanged;
            group.ControllerShouldActivate += Controller_ShouldActivate;
            group.MouseEvent += Controller_MouseEvent;

            ControllerGroups.Add(group);
            GroupsChanged?.Invoke(this, EventArgs.Empty);

            return group;
        }

        private void Controller_ShouldActivate(object sender, EventArgs e)
        {
            ToontownController controller = sender as ToontownController;

            if (!ActiveControllers.Contains(controller))
            {
                switch (CurrentMode)
                {
                    case MulticontrollerMode.Group:
                    case MulticontrollerMode.MirrorGroup:
                        ControllerGroup group = ControllerGroups.First(g => g.AllControllers.Contains(controller));

                        CurrentGroupIndex = ControllerGroups.IndexOf(group);
                        break;
                    case MulticontrollerMode.Pair:
                        ControllerPair pair = AllControllerPairs.First(p => p.AllControllers.Contains(controller));

                        CurrentPairIndex = AllControllerPairsWithWindows.ToList().IndexOf(pair);
                        break;
                    case MulticontrollerMode.MirrorIndividual:
                        CurrentIndividualControllerIndex = AllControllersWithWindows.ToList().IndexOf(controller);
                        break;
                }
            }
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
                    if (IsActive)
                    {
                        List<MulticontrollerMode> availableModesToCycle = new List<MulticontrollerMode>();

                        if (Properties.Settings.Default.groupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(MulticontrollerMode.Group);
                        }

                        if (Properties.Settings.Default.mirrorModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(MulticontrollerMode.MirrorAll);
                        }

                        if (Properties.Settings.Default.allGroupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(MulticontrollerMode.AllGroup);
                        }

                        if (Properties.Settings.Default.mirrorGroupModeCycleWithModeHotkey)
                        {
                            availableModesToCycle.Add(MulticontrollerMode.MirrorGroup);
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
                CurrentMode = MulticontrollerMode.Group;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.mirrorModeKeyCode)
            {
                CurrentMode = MulticontrollerMode.MirrorAll;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                CurrentMode = MulticontrollerMode.AllGroup;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.mirrorGroupModeKeyCode)
            {
                CurrentMode = MulticontrollerMode.MirrorGroup;
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.pairModeKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && IsActive && AllControllerPairsWithWindows.Count() > 0)
                {
                    if (CurrentMode == MulticontrollerMode.Pair)
                    {
                        CurrentPairIndex = (CurrentPairIndex + 1) % AllControllerPairsWithWindows.Count();
                    }
                    else
                    {
                        CurrentMode = MulticontrollerMode.Pair;
                    }
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.replicateMouseKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN)
                {
                    Properties.Settings.Default.replicateMouse = !Properties.Settings.Default.replicateMouse;
                    Properties.Settings.Default.Save();

                    SettingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.controlAllGroupsKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && CurrentMode != MulticontrollerMode.AllGroup)
                {
                    CurrentMode = MulticontrollerMode.AllGroup;
                }
            }
            else if (keysPressed == (Keys)Properties.Settings.Default.individualControlKeyCode)
            {
                if (msg == Win32.WM.KEYDOWN && IsActive && AllControllersWithWindows.Count() > 0)
                {
                    if (CurrentMode == MulticontrollerMode.MirrorIndividual)
                    {
                        CurrentIndividualControllerIndex = (CurrentIndividualControllerIndex + 1) % AllControllersWithWindows.Count();
                    }
                    else
                    {
                        CurrentMode = MulticontrollerMode.MirrorIndividual;
                    }
                }
            }
            else if ((CurrentMode == MulticontrollerMode.Group || CurrentMode == MulticontrollerMode.MirrorGroup)
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
            if (IsActive)
            {
                IEnumerable<ToontownController> affectedControllers = ActiveControllers;

                if (CurrentMode == MulticontrollerMode.Group
                    || CurrentMode == MulticontrollerMode.AllGroup
                    || CurrentMode == MulticontrollerMode.Pair)
                {
                    if (sourceController.Type == ControllerType.Left)
                    {
                        affectedControllers = affectedControllers.Where(c => c.Type == ControllerType.Left);
                    }
                    else
                    {
                        affectedControllers = affectedControllers.Where(c => c.Type == ControllerType.Right);
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
            if (IsActive)
            {
                Keys keysPressed = (Keys)wParam;

                IEnumerable<ToontownController> affectedControllers = ActiveControllers;
                List<Keys> keysToPress = new List<Keys>();

                if (CurrentMode == MulticontrollerMode.Group 
                    || CurrentMode == MulticontrollerMode.AllGroup 
                    || CurrentMode == MulticontrollerMode.Pair)
                {
                    if (leftKeys.ContainsKey(keysPressed) && !rightKeys.ContainsKey(keysPressed))
                    {
                        affectedControllers = affectedControllers.Where(c => c.Type == ControllerType.Left);

                        keysToPress.AddRange(leftKeys[keysPressed]);
                    }
                    else if (!leftKeys.ContainsKey(keysPressed) && rightKeys.ContainsKey(keysPressed))
                    {
                        affectedControllers = affectedControllers.Where(c => c.Type == ControllerType.Right);

                        keysToPress.AddRange(rightKeys[keysPressed]);
                    }
                    else if (leftKeys.ContainsKey(keysPressed) && rightKeys.ContainsKey(keysPressed))
                    {
                        keysToPress.AddRange(leftKeys[keysPressed]);
                        keysToPress.AddRange(rightKeys[keysPressed]);
                    }
                }
                
                if (CurrentMode == MulticontrollerMode.MirrorAll
                    || CurrentMode == MulticontrollerMode.MirrorGroup
                    || CurrentMode == MulticontrollerMode.MirrorIndividual)
                {
                    affectedControllers.ToList().ForEach(c => c.PostMessage(msg, wParam, lParam));
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

        private void Controller_WindowHandleChanged(object sender, EventArgs e)
        {
            if (!AllControllersWithWindows.Any(c => c.IsWindowActive))
            {
                AllWindowsInactive?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Controller_WindowActivated(object sender, EventArgs e)
        {
            WindowActivated?.Invoke(this, EventArgs.Empty);
        }

        private void Controller_WindowDeactivated(object sender, EventArgs e)
        {
            if (!AllControllersWithWindows.Any(c => c.IsWindowActive))
            {
                AllWindowsInactive?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    
}
