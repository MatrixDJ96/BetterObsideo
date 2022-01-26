using System;
using System.Runtime.InteropServices;

namespace BetterObsideo.WinApi
{
    public class UnsafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);
    }
}
