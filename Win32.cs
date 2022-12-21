using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HWND = System.IntPtr;
namespace KeyRemap
{
    public static class Win32
    {
        // DLL libraries used to manage hotkeys

        public delegate bool EnumDelegate(IntPtr hWnd, int lParam);
        public delegate bool EnumWindowsProc(HWND hWnd, int lParam);
        public const int MOD_SHIFT = 0x4;
        public const int MOD_CONTROL = 0x2;
        public const int MOD_ALT = 0x1;
        public const int MOD_WIN = 0x8;
        public const int WM_HOTKEY = 0x312;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_PASTE = 0x0302;

        [DllImport("kernel32.dll")] 
        public static extern int GlobalAddAtom(string lpString);
        [DllImport("kernel32.dll")]
        public static extern int GlobalDeleteAtom(int nAtom);
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd,uint Msg,int wParam,int lParam);
        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}
