
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LangLed
{
  static class LangLedMain
  {
    static LangLedForm _form1;
    public static bool EnableArduino = false;

    /// <summary>
    /// Главная точка входа для приложения.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      Win32.SetProcessDPIAware();

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      if (args != null && args.Length > 0) {
        string arduinoOption = "--enable-arduino";
        if (args[0] == arduinoOption) { EnableArduino = true; }
        else {
          MessageBox.Show($"Unrecognized command line option: {args[0]}. Options are: {arduinoOption}.");
          return;
        }
      }

      _form1 = new LangLedForm();
      Action longAction = delegate { _form1?.RefreshIndicatorOnSignal(); };
      Action shortAction = delegate { _form1?.BeginInvoke(longAction); };

      LangLedHook.SetShiftUpHook(shortAction);
      Application.Run(_form1);
      LangLedHook.UnhookShiftUp();
    }
  }
}
