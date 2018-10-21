#region Using Directives
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
#endregion

namespace MacroUO
{
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        #region Imports
        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean AttachThreadInput([In] UInt32 threadIdFrom, [In] UInt32 threadIdTo, [In, MarshalAs(UnmanagedType.U1)] Boolean attach);

        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean EnumWindows([In] EnumWindowsProcess enumWindowsProcess, [In] IntPtr lParameter);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetKeyboardState([Out] Byte[] state);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetWindowRect([In] IntPtr windowHandle, [Out] out RECT rectangle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean IsIconic([In] IntPtr windowHandle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean IsWindow(IntPtr windowHandle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean PostMessage([In, Optional] IntPtr windowHandle, [In] UInt32 message, [In] IntPtr wParameter, [In] IntPtr lParameter);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetForegroundWindow([In] IntPtr windowHandle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetKeyboardState([In] Byte[] state);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetWindowPos([In] IntPtr windowHandle, [In, Optional] IntPtr handleAfter, [In] Int32 left, [In] Int32 top, [In] Int32 right, [In] Int32 bottom, [In] SETWINDOWPOS_FLAGS flags);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean ShowWindow([In] IntPtr windowHandle, [In] SHOWWINDOW_COMMAND command);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean UnhookWindowsHookEx([In] IntPtr hookHandle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern Int32 GetClassName([In] IntPtr windowHandle, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer, [In] Int32 maximumCharacters);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern Int32 GetWindowText([In] IntPtr windowHandle, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder buffer, [In] Int32 maximumCharacters);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern IntPtr CallNextHookEx([In, Optional] IntPtr hookHandle, [In] Int32 code, [In] IntPtr wParameter, [In] IntPtr lParameter);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern IntPtr GetKeyboardLayout([In] UInt32 threadId);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern IntPtr SetWindowsHookEx([In] HOOK_TYPE hookType, [In] HookProcess hookProcess, [In] IntPtr handle, [In] UInt32 threadId);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern IntPtr SendMessage([In] IntPtr windowHandle, [In] UInt32 message, [In] IntPtr wParameter, [In] IntPtr lParameter);

        [DllImport("Dwmapi.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern UInt32 DwmGetWindowAttribute([In] IntPtr windowHandle, [In] DWMWINDOW_ATTRIBUTE attribute, [In, Out] IntPtr attributeValue, [In] UInt32 attributeSize);

        [DllImport("Dwmapi.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern UInt32 DwmIsCompositionEnabled([Out, MarshalAs(UnmanagedType.Bool)] out Boolean enabled);

        [DllImport("Kernel32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern UInt32 GetCurrentThreadId();

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=true, SetLastError=true)]
        private static extern UInt32 GetWindowThreadProcessId([In] IntPtr windowHandle, [Out] out UInt32 processId); 

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern UInt32 MapVirtualKeyEx([In] UInt32 code, [In] MAPVK_TYPE type, [In, Out, Optional] IntPtr keyboardLayoutHandle);

        [DllImport("User32.dll", CallingConvention=CallingConvention.StdCall, CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
        private static extern UInt32 RegisterWindowMessage([In] String message);
        #endregion

        #region Methods
        private static UInt32 GetSize(Type type)
        {
            if (type.IsEnum)
                type = Enum.GetUnderlyingType(type);

            return (UInt32)Marshal.SizeOf(type);
        }

        internal static Boolean GetRectangle(IntPtr windowHandle, out Rectangle rectangle)
        {
            RECT nativeRectangle;

            if (DwmIsCompositionEnabled(out Boolean aeroEnabled) != 0)
            {
                OperatingSystem os = Environment.OSVersion;
                Version osVersion = os.Version;

                if ((os.Platform == PlatformID.Win32NT) && ((osVersion.Major > 6) || ((osVersion.Major) == 6 && (osVersion.Minor >= 2))))
                    aeroEnabled = true;
                else
                    aeroEnabled = false;
            }

            if (aeroEnabled)
            {
                IntPtr attributeValue = IntPtr.Zero;
                UInt32 size = GetSize(typeof(RECT));

                try
                {
                    attributeValue = Marshal.AllocCoTaskMem((Int32)size);

                    UInt32 result = DwmGetWindowAttribute(windowHandle, DWMWINDOW_ATTRIBUTE.DWMWA_EXTENDED_FRAME_BOUNDS, attributeValue, size);

                    if (result == 0)
                        nativeRectangle = (RECT)Marshal.PtrToStructure(attributeValue, typeof(RECT));
                    else if (!GetWindowRect(windowHandle, out nativeRectangle))
                    {
                        rectangle = Rectangle.Empty;
                        return false;
                    }
                }
                finally
                {
                    if (attributeValue != IntPtr.Zero)
                        Marshal.FreeCoTaskMem(attributeValue);
                }
            }
            else if (!GetWindowRect(windowHandle, out nativeRectangle))
            {
                rectangle = Rectangle.Empty;
                return false;
            }

            Int32 x = nativeRectangle.Left;
            Int32 y = nativeRectangle.Top;
            Int32 width = nativeRectangle.Right - x;
            Int32 height = nativeRectangle.Bottom - y;

            rectangle = new Rectangle(x, y, width, height);

            return true;
        }

        internal static Boolean HookedActivate(IntPtr lParameter, out IntPtr windowHandle)
        {
            CWPRET_STRUCT parameters = (CWPRET_STRUCT)Marshal.PtrToStructure(lParameter, typeof(CWPRET_STRUCT));

            if (parameters.Message != (UInt32)CWPRET_HOOK.HCBT_ACTIVATE)
            {
                windowHandle = IntPtr.Zero;
                return false;
            }

            windowHandle = parameters.Handle;
            return true;
        }

        internal static Boolean HookedInitialize(IntPtr lParameter, out IntPtr windowHandle)
        {
            CWPRET_STRUCT parameters = (CWPRET_STRUCT)Marshal.PtrToStructure(lParameter, typeof(CWPRET_STRUCT));

            if (parameters.Message != (UInt32)WINDOW_MESSAGE.WM_INITDIALOG)
            {
                windowHandle = IntPtr.Zero;
                return false;
            }

            windowHandle = parameters.Handle;
            return true;
        }

        internal static Boolean IsValidWindow(IntPtr windowHandle)
        {
            return IsWindow(windowHandle);
        }

        internal static Boolean ThreadInputAttach(UInt32 threadIdFrom, UInt32 threadIdTo)
        {
            return AttachThreadInput(threadIdFrom, threadIdTo, true);
        }

        internal static Byte[] GetKeyboardState()
        {
            Byte[] state = new Byte[256];
            GetKeyboardState(state);

            return state;
        }

        internal static IntPtr GetKeyboardLayout()
        {
            return GetKeyboardLayout(GetCurrentThreadId());
        }

        internal static IntPtr Hook(HookProcess process)
        {
            UInt32 threadId = GetCurrentThreadId();
            return ((threadId == 0) ? IntPtr.Zero : SetWindowsHookEx(HOOK_TYPE.WH_CALLWNDPROCRET, process, IntPtr.Zero, threadId));
        }

        internal static IntPtr NextHook(IntPtr handle, Int32 code, IntPtr wParameter, IntPtr lParameter)
        {
            return CallNextHookEx(handle, code, wParameter, lParameter);
        }

        internal static String GetWindowClass(IntPtr windowHandle)
        {
            StringBuilder buffer = new StringBuilder(256);

            return ((GetClassName(windowHandle, buffer, buffer.Capacity) == 0) ? String.Empty : buffer.ToString().Trim());
        }

        internal static String GetWindowText(IntPtr windowHandle)
        {
            StringBuilder buffer = new StringBuilder(1024);
            return ((GetWindowText(windowHandle, buffer, buffer.Capacity) == 0) ? String.Empty : buffer.ToString().Trim());
        }

        internal static UInt32 GetThreadId()
        {
            return GetCurrentThreadId();
        }

        internal static UInt32 GetWindowThreadId(IntPtr windowHandle)
        {
            return GetWindowThreadProcessId(windowHandle, out UInt32 _);
        }

        internal static UInt32 RegisterMessage(String message)
        {
            return RegisterWindowMessage(message);
        }

        internal static void BroadcastMessage(UInt32 message)
        {
            PostMessage((new IntPtr(0xFFFF)), message, IntPtr.Zero, IntPtr.Zero);
        }

        internal static void EnumerateWindows(EnumWindowsProcess enumWindowsProcess)
        {
            EnumWindows(enumWindowsProcess, IntPtr.Zero);
        }

        internal static void KeyboardState(Byte[] state)
        {
            SetKeyboardState(state);
        }

        internal static void Restore(Form form)
        {
            if (form == null)
                return;

            IntPtr handle = form.Handle;

            if (IsIconic(handle))
                ShowWindow(handle, SHOWWINDOW_COMMAND.SW_RESTORE);

            SetForegroundWindow(handle);
        }

        internal static void SendKeyDown(IntPtr windowHandle, IntPtr keyboardLayoutHandle, UInt32 key)
        {
            UInt32 scanCode = (1 | (MapVirtualKeyEx(key, MAPVK_TYPE.MAPVK_VK_TO_VSC, keyboardLayoutHandle) << 16));
            SendMessage(windowHandle, (UInt32)WINDOW_MESSAGE.WM_KEYDOWN, (IntPtr)key, (IntPtr)scanCode);
        }

        internal static void SetPosition(IntPtr windowHandle, Rectangle rectangle)
        {
            SetWindowPos(windowHandle, IntPtr.Zero, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, SETWINDOWPOS_FLAGS.SWP_DEFAULT);
        }

        internal static void ThreadInputDetach(UInt32 threadIdFrom, UInt32 threadIdTo)
        {
            AttachThreadInput(threadIdFrom, threadIdTo, false);
        }

        internal static void Unhook(IntPtr hookHandle)
        {
            UnhookWindowsHookEx(hookHandle);
        }
        #endregion

        #region Nesting (Delegates)
        internal delegate Boolean EnumWindowsProcess(IntPtr windowHandle, IntPtr lParameter);
        internal delegate IntPtr HookProcess(Int32 code, IntPtr wParameter, IntPtr lParameter);
        #endregion

        #region Nesting (Enumerators)
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum CWPRET_HOOK : uint
        {
            #region Values
            HCBT_MOVESIZE = 0x0,
            HCBT_MINMAX = 0x1,
            HCBT_QS = 0x2,
            HCBT_CREATEWND = 0x3,
            HCBT_DESTROYWND = 0x4,
            HCBT_ACTIVATE = 0x5,
            HCBT_CLICKSKIPPED = 0x6,
            HCBT_KEYSKIPPED = 0x7,
            HCBT_SYSCOMMAND = 0x8,
            HCBT_SETFOCUS = 0x9
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum DWMWINDOW_ATTRIBUTE : uint
        {
            #region Values
            DWMWA_NCRENDERING_ENABLED = 0x1,
            DWMWA_NCRENDERING_POLICY = 0x2,
            DWMWA_TRANSITIONS_FORCEDISABLED = 0x3,
            DWMWA_ALLOW_NCPAINT = 0x4,
            DWMWA_CAPTION_BUTTON_BOUNDS = 0x5,
            DWMWA_NONCLIENT_RTL_LAYOUT = 0x6,
            DWMWA_FORCE_ICONIC_REPRESENTATION = 0x7,
            DWMWA_FLIP3D_POLICY = 0x8,
            DWMWA_EXTENDED_FRAME_BOUNDS = 0x9,
            DWMWA_HAS_ICONIC_BITMAP = 0xA,
            DWMWA_DISALLOW_PEEK = 0xB,
            DWMWA_EXCLUDED_FROM_PEEK = 0xC,
            DWMWA_CLOAK = 0xD,
            DWMWA_CLOAKED = 0xE,
            DWMWA_FREEZE_REPRESENTATION = 0xF
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum HOOK_TYPE : uint
        {
            #region Values
            WH_JOURNALRECORD = 0x0,
            WH_JOURNALPLAYBACK = 0x1,
            WH_KEYBOARD = 0x2,
            WH_GETMESSAGE = 0x3,
            WH_CALLWNDPROC = 0x4,
            WH_CBT = 0x5,
            WH_SYSMSGFILTER = 0x6,
            WH_MOUSE = 0x7,
            WH_HARDWARE = 0x8,
            WH_DEBUG = 0x9,
            WH_SHELL = 0xA,
            WH_FOREGROUNDIDLE = 0xB,
            WH_CALLWNDPROCRET = 0xC,
            WH_KEYBOARD_LL = 0xD,
            WH_MOUSE_LL = 0xE
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum MAPVK_TYPE : uint
        {
            #region Values
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
            MAPVK_VK_TO_VSC_EX = 0x4
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        [Flags]
        private enum SETWINDOWPOS_FLAGS : uint
        {
            #region Values
            SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_NOREDRAW = 0x0008,
            SWP_NOACTIVATE = 0x0010,
            SWP_DRAWFRAME = 0x0020,
            SWP_FRAMECHANGED = 0x0020,
            SWP_SHOWWINDOW = 0x0040,
            SWP_HIDEWINDOW = 0x0080,
            SWP_NOCOPYBITS = 0x0100,
            SWP_NOOWNERZORDER = 0x0200,
            SWP_NOREPOSITION = 0x0200,
            SWP_NOSENDCHANGING = 0x0400,
            SWP_DEFERERASE = 0x2000,
            SWP_ASYNCWINDOWPOS = 0x4000,
            SWP_DEFAULT = SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_ASYNCWINDOWPOS
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum SHOWWINDOW_COMMAND
        {
            #region Values
            SW_HIDE = 0x0,
            SW_SHOWNORMAL = 0x1,
            SW_SHOWMINIMIZED = 0x2,
            SW_SHOWMAXIMIZED = 0x3,
            SW_SHOWNOACTIVATE = 0x4,
            SW_SHOW = 0x5,
            SW_MINIMIZE = 0x6,
            SW_SHOWMINNOACTIVE = 0x7,
            SW_SHOWNA = 0x8,
            SW_RESTORE = 0x9,
            SW_SHOWDEFAULT = 0xA,
            SW_FORCEMINIMIZE = 0xB
            #endregion
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        private enum WINDOW_MESSAGE : uint
        {
            #region Values
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUIT = 0x0012,
            WM_QUERYOPEN = 0x0013,
            WM_ERASEBKGND = 0x0014,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_ENDSESSION = 0x0016,
            WM_SHOWWINDOW = 0x0018,
            WM_SETTINGCHANGE = 0x001A,
            WM_WININICHANGE = 0x001A,
            WM_DEVMODECHANGE = 0x001B,
            WM_ACTIVATEAPP = 0x001C,
            WM_FONTCHANGE = 0x001D,
            WM_TIMECHANGE = 0x001E,
            WM_CANCELMODE = 0x001F,
            WM_SETCURSOR = 0x0020,
            WM_MOUSEACTIVATE = 0x0021,
            WM_CHILDACTIVATE = 0x0022,
            WM_QUEUESYNC = 0x0023,
            WM_GETMINMAXINFO = 0x0024,
            WM_PAINTICON = 0x0026,
            WM_ICONERASEBKGND = 0x0027,
            WM_NEXTDLGCTL = 0x0028,
            WM_SPOOLERSTATUS = 0x002A,
            WM_DRAWITEM = 0x002B,
            WM_MEASUREITEM = 0x002C,
            WM_DELETEITEM = 0x002D,
            WM_VKEYTOITEM = 0x002E,
            WM_CHARTOITEM = 0x002F,
            WM_SETFONT = 0x0030,
            WM_GETFONT = 0x0031,
            WM_SETHOTKEY = 0x0032,
            WM_GETHOTKEY = 0x0033,
            WM_QUERYDRAGICON = 0x0037,
            WM_COMPAREITEM = 0x0039,
            WM_GETOBJECT = 0x003D,
            WM_COMPACTING = 0x0041,
            WM_COMMNOTIFY = 0x0044,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_POWER = 0x0048,
            WM_COPYDATA = 0x004A,
            WM_CANCELJOURNAL = 0x004B,
            WM_NOTIFY = 0x004E,
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_TCARD = 0x0052,
            WM_HELP = 0x0053,
            WM_USERCHANGED = 0x0054,
            WM_NOTIFYFORMAT = 0x0055,
            WM_CONTEXTMENU = 0x007B,
            WM_STYLECHANGING = 0x007C,
            WM_STYLECHANGED = 0x007D,
            WM_DISPLAYCHANGE = 0x007E,
            WM_GETICON = 0x007F,
            WM_SETICON = 0x0080,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCCALCSIZE = 0x0083,
            WM_NCHITTEST = 0x0084,
            WM_NCPAINT = 0x0085,
            WM_NCACTIVATE = 0x0086,
            WM_GETDLGCODE = 0x0087,
            WM_SYNCPAINT = 0x0088,
            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMBUTTONDBLCLK = 0x00A9,
            WM_NCXBUTTONDOWN = 0x00AB,
            WM_NCXBUTTONUP = 0x00AC,
            WM_NCXBUTTONDBLCLK = 0x00AD,
            WM_INPUT_DEVICE_CHANGE = 0x00FE,
            WM_INPUT = 0x00FF,
            WM_KEYDOWN = 0x0100,
            WM_KEYFIRST = 0x0100,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,
            WM_DEADCHAR = 0x0103,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCHAR = 0x0106,
            WM_SYSDEADCHAR = 0x0107,
            WM_KEYLAST_PRENT501 = 0x0108,
            WM_KEYLAST = 0x0109,
            WM_UNICHAR = 0x0109,
            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_KEYLAST = 0x010F,
            WM_INITDIALOG = 0x0110,
            WM_COMMAND = 0x0111,
            WM_SYSCOMMAND = 0x0112,
            WM_TIMER = 0x0113,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_MENUSELECT = 0x011F,
            WM_MENUCHAR = 0x0120,
            WM_ENTERIDLE = 0x0121,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_MENUCOMMAND = 0x0126,
            WM_CHANGEUISTATE = 0x0127,
            WM_UPDATEUISTATE = 0x0128,
            WM_QUERYUISTATE = 0x0129,
            WM_CTLCOLORMSGBOX = 0x0132,
            WM_CTLCOLOREDIT = 0x0133,
            WM_CTLCOLORLISTBOX = 0x0134,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORDLG = 0x0136,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_CTLCOLORSTATIC = 0x0138,
            MN_GETHMENU = 0x01E1,
            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MOUSEWHEEL = 0x020A,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C,
            WM_XBUTTONDBLCLK = 0x020D,
            WM_MOUSEHWHEEL = 0x020E,
            WM_PARENTNOTIFY = 0x0210,
            WM_ENTERMENULOOP = 0x0211,
            WM_EXITMENULOOP = 0x0212,
            WM_NEXTMENU = 0x0213,
            WM_SIZING = 0x0214,
            WM_CAPTURECHANGED = 0x0215,
            WM_MOVING = 0x0216,
            WM_POWERBROADCAST = 0x0218,
            WM_DEVICECHANGE = 0x0219,
            WM_MDICREATE = 0x0220,
            WM_MDIDESTROY = 0x0221,
            WM_MDIACTIVATE = 0x0222,
            WM_MDIRESTORE = 0x0223,
            WM_MDINEXT = 0x0224,
            WM_MDIMAXIMIZE = 0x0225,
            WM_MDITILE = 0x0226,
            WM_MDICASCADE = 0x0227,
            WM_MDIICONARRANGE = 0x0228,
            WM_MDIGETACTIVE = 0x0229,
            WM_MDISETMENU = 0x0230,
            WM_ENTERSIZEMOVE = 0x0231,
            WM_EXITSIZEMOVE = 0x0232,
            WM_DROPFILES = 0x0233,
            WM_MDIREFRESHMENU = 0x0234,
            WM_IME_SETCONTEXT = 0x0281,
            WM_IME_NOTIFY = 0x0282,
            WM_IME_CONTROL = 0x0283,
            WM_IME_COMPOSITIONFULL = 0x0284,
            WM_IME_SELECT = 0x0285,
            WM_IME_CHAR = 0x0286,
            WM_IME_REQUEST = 0x0288,
            WM_IME_KEYDOWN = 0x0290,
            WM_IME_KEYUP = 0x0291,
            WM_NCMOUSEHOVER = 0x02A0,
            WM_MOUSEHOVER = 0x02A1,
            WM_NCMOUSELEAVE = 0x02A2,
            WM_MOUSELEAVE = 0x02A3,
            WM_WTSSESSION_CHANGE = 0x02B1,
            WM_TABLET_FIRST = 0x02C0,
            WM_TABLET_LAST = 0x02DF,
            WM_CUT = 0x0300,
            WM_COPY = 0x0301,
            WM_PASTE = 0x0302,
            WM_CLEAR = 0x0303,
            WM_UNDO = 0x0304,
            WM_RENDERFORMAT = 0x0305,
            WM_RENDERALLFORMATS = 0x0306,
            WM_DESTROYCLIPBOARD = 0x0307,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_PAINTCLIPBOARD = 0x0309,
            WM_VSCROLLCLIPBOARD = 0x030A,
            WM_SIZECLIPBOARD = 0x030B,
            WM_ASKCBFORMATNAME = 0x030C,
            WM_CHANGECBCHAIN = 0x030D,
            WM_HSCROLLCLIPBOARD = 0x030E,
            WM_QUERYNEWPALETTE = 0x030F,
            WM_PALETTEISCHANGING = 0x0310,
            WM_PALETTECHANGED = 0x0311,
            WM_HOTKEY = 0x0312,
            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,
            WM_APPCOMMAND = 0x0319,
            WM_THEMECHANGED = 0x031A,
            WM_CLIPBOARDUPDATE = 0x031D,
            WM_DWMCOMPOSITIONCHANGED = 0x031E,
            WM_DWMNCRENDERINGCHANGED = 0x031F,
            WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,
            WM_GETTITLEBARINFOEX = 0x033F,
            WM_HANDHELDFIRST = 0x0358,
            WM_HANDHELDLAST = 0x035F,
            WM_AFXFIRST = 0x0360,
            WM_AFXLAST = 0x037F,
            WM_PENWINFIRST = 0x0380,
            WM_PENWINLAST = 0x038F,
            WM_USER = 0x0400,
            WM_REFLECT = 0x2000,
            WM_APP = 0x8000
            #endregion
        }
        #endregion

        #region Nesting (Structures)
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Local")]
        [StructLayout(LayoutKind.Sequential)]
        private struct CWPRET_STRUCT
        {
            #region Members: Instance
            public IntPtr Result;
            public IntPtr lParameter;
            public IntPtr wParameter;
            public UInt32 Message;
            public IntPtr Handle;
            #endregion
        }

        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            #region Members: Instance
            public Int32 Left;
            public Int32 Top;
            public Int32 Right;
            public Int32 Bottom;
            #endregion
        }
        #endregion
    }
}
