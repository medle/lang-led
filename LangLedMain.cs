
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace LangLed
{
    static class LangLedMain
    {
        private static LangLedForm _mainForm;
        public static bool EnableArduino = false;
        public static int BrightnessPercentage = 5;
        public static bool TraceKeys = false;

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Win32.SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ParseCommandLineArguments(args)) return;

            _mainForm = new LangLedForm();

            RestartHook();
            Application.Run(_mainForm);
            LangLedHook.UnhookShiftUp();
        }

        public static void RestartHook()
        {
            LangLedHook.UnhookShiftUp();

            Action longAction = delegate { _mainForm?.RefreshIndicatorOnSignal(); };
            Action shortAction = delegate { _mainForm?.BeginInvoke(longAction); };

            Action<string> fastKeyAction = delegate (string s) {
                Action a = delegate { _mainForm.AddLog(s); };
                _mainForm?.BeginInvoke(a); 
            };

            if(TraceKeys) LangLedHook.SetShiftUpHook(shortAction, fastKeyAction);
            else LangLedHook.SetShiftUpHook(shortAction);
        }

        private static bool ParseCommandLineArguments(string[] args)
        {
            if (args == null) return true;

            string arduinoOption = "--enable-arduino";
            string traceKeys = "--trace-keys";
            string brightnessOption = "--brightness=";

            foreach (var arg in args)
            {
                if (arg == arduinoOption)
                {
                    EnableArduino = true;
                    continue;
                }

                if (arg == traceKeys) {
                    TraceKeys = true;
                    continue;
                }

                if (arg.StartsWith(brightnessOption))
                {
                    string value = arg.Substring(brightnessOption.Length);
                    if (Int32.TryParse(value, out BrightnessPercentage) &&
                       BrightnessPercentage > 0 && BrightnessPercentage <= 100) continue;
                    MessageBox.Show($"Brightness value must be in range (0, 100].");
                    return false;
                }

                MessageBox.Show($"Unrecognized command line option: {arg}.\n" +
                                $"Options are: {arduinoOption}, {traceKeys}, {brightnessOption}NN.");
                return false;
            }

            return true;
        }
    }
}
