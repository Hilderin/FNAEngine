using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class WindowHelper
    {
        /// <summary>
        /// Last window switch
        /// </summary>
        public static DateTime LastWindowSwitch { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Check if we can switch window again
        /// </summary>
        public static bool IsIntervalOKToSwitchWindow()
        {
            return DateTime.Now.Subtract(WindowHelper.LastWindowSwitch).TotalMilliseconds >= 100;
        }

        /// <summary>
        /// Get the main window handle
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetMainWindowHandle()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
        }
        


        /// <summary>
        /// Assure that all these windows are visible
        /// </summary>
        public static void ShowAllWindows(List<IntPtr> windowHandles, IntPtr focusWindowHandle)
        {
            
            if (!IsIntervalOKToSwitchWindow())
                return;

            LastWindowSwitch = DateTime.Now;

            foreach (IntPtr handle in windowHandles)
            {
                if (handle != focusWindowHandle)
                    Win32.SetForegroundWindow(handle);
            }

            Win32.SetForegroundWindow(focusWindowHandle);


            //Win32.SetForegroundWindow(focusWindowHandle);

            Timer timer = new Timer(s =>
            {
                Win32.SetForegroundWindow(focusWindowHandle);
            }, null, 10, 0);

        }

    }
}
