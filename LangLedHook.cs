
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLed
{
  public class LangLedHook
  {
    private static Win32.LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    private static Action _shiftUpAction;

    public static void SetShiftUpHook(Action action)
    {
      _shiftUpAction = action;
      if (_hookID != IntPtr.Zero) UnhookShiftUp();
      _hookID = SetHook(_proc);
    }

    public static void UnhookShiftUp()
    {
      if (_hookID != IntPtr.Zero) {
        Win32.UnhookWindowsHookEx(_hookID);
        _hookID = IntPtr.Zero;
      }
    }

    private static IntPtr SetHook(Win32.LowLevelKeyboardProc proc)
    {
      using (Process curProcess = Process.GetCurrentProcess())
      using (ProcessModule curModule = curProcess.MainModule) {
        return Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, proc,
            Win32.GetModuleHandle(curModule.ModuleName), 0);
      }
    }

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {
      if (nCode >= 0 && wParam == (IntPtr)Win32.WM_KEYUP) {
        int vkCode = Marshal.ReadInt32(lParam);
        //System.Diagnostics.Trace.WriteLine("***keyUp=" + (Keys)vkCode);
        if ((Keys)vkCode == Keys.LShiftKey || (Keys)vkCode == Keys.RShiftKey) {
          _shiftUpAction?.Invoke();
        }
      }
      return Win32.CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
  }
}
