using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace LangLed
{
  public class Win32
  {
    [DllImport("User32.dll")]
    public static extern int SetProcessDPIAware();

    public delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    public const int WM_INPUTLANGUAGE = 0x0051;
    public const int WH_KEYBOARD_LL = 13;
    public const int WH_KEYBOARD = 2;
    public const int WM_KEYDOWN = 0x0100;
    public const int WM_KEYUP = 0x0101;

    private static AutomationElement cachedElement = null;
    private static DateTime cachedElementUseTime = DateTime.MinValue;

    public static bool IsLanguageRussian()
    {
      var now = DateTime.Now;

      // Dispose of cashed element if it wasn't used for the last 3 minutes.
      if (cachedElement != null) {
        var elapsed = (now - cachedElementUseTime);
        if (elapsed.Minutes > 3) cachedElement = null;
      }

      if(cachedElement == null) {
        var rootElement = AutomationElement.RootElement;
        var condition = new PropertyCondition(AutomationElement.ClassNameProperty, "InputIndicatorButton");
        cachedElement = rootElement.FindFirst(TreeScope.Subtree, condition);
      }

      if (cachedElement != null) {
        try
        {
          cachedElementUseTime = now;
          var name = cachedElement.Current.Name;
          if (name.Contains("Русский")) return true;
        }
        catch (Exception) {
          cachedElement = null;
        }
      }

      return false;
    }
  }
}
