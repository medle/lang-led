using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LangLed
{
    public partial class LangLedForm : Form
    {
        RedLightController redLight = null;
        int totalUpdateCounter = 0;

        private Timer refreshTimer;

        public LangLedForm()
        {
            InitializeComponent();

            if (LangLedMain.EnableArduino) redLight = new RedLightController();

            this.Icon = Resource1.Keyboard;

            this.notifyIcon1.Icon = Resource1.Keyboard;
            this.notifyIcon1.Click += delegate
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };

            this.Resize += LangLedForm_Resize;
            this.Closing += LangLedForm_Closing;
            this.Load += LangLedForm_Loaded;

            this.refreshTimer = new Timer();
            this.refreshTimer.Interval = 1000 * 60 * 3; // every three minutes
            this.refreshTimer.Tick += delegate { PerformRefresh(); };
            this.refreshTimer.Start();
        }

        private void PerformRefresh()
        {
            LangLedMain.RestartHook();
            Win32.ResetCache();
        }

        private bool lastStateWasRu = false;
        private EdgeWindow edgeWindow = null;
        private EdgePopup edgePopup = null;

        public void AddLog(string what)
        {
            string line = $"{DateTime.Now}: {what}";
            this.logListBox.Items.Add(line);
            this.logListBox.SelectedIndex = this.logListBox.Items.Count - 1;
        }

        private void UpdateIndicatorVisuals()
        {
            var timer = new MilliTimer();
            bool ru = Win32.IsLanguageRussian();
            AddLog($"{totalUpdateCounter}/{probeCounter}: isRu={ru}, {timer}ms");

            if (lastStateWasRu != ru)
            {
                ShowEdgePopup(ru);
                lastStateWasRu = ru;
            }

            totalUpdateCounter += 1;
            string text = (ru ? "RU" : "EN");

            this.label1.Text = totalUpdateCounter.ToString() + ": " + text;
            this.label2.Text = this.redLight?.GetState();

            //var color = ru ? Color.DarkRed : Color.DarkBlue;
            //LaunchFlashWindow(text, color);

            Action a = delegate { this.redLight?.TurnLight(ru); };
            Invoke(a);
        }

        private void ShowEdgePopup(bool on)
        {
            if (edgePopup == null)
            {
                edgePopup = new EdgePopup(this);
                this.Controls.Add(edgePopup);
            }
            if (on)
            {
                edgePopup.Show();
            }
            else
            {
                edgePopup.Hide();
            }
        }

        private void ShowEdgeWindow(bool on)
        {
            if (edgeWindow == null)
            {
                edgeWindow = new EdgeWindow();
            }
            if (on)
            {
                edgeWindow.Show(this);
            }
            else
            {
                edgeWindow.Hide();
            }
        }

        private void LaunchFlashWindow(string text, Color color)
        {
            if (flashWindow != null) flashWindow.Close();
            flashWindow = new FlashWindow(text, color);
            flashWindow.Show();
        }

        public void RefreshIndicatorOnSignal()
        {
            BeginSoftProbing();
        }

        private Timer probeTimer;
        private FlashWindow flashWindow;
        private int probeCounter = 0;

        private void BeginSoftProbing()
        {
            if (this.probeTimer == null)
            {
                this.probeTimer = new Timer();
                this.probeTimer.Interval = 20;
                this.probeTimer.Tick += delegate { PerformProbing(); };
            }

            // reset probe counter so if sequence is already running it gets restarted
            this.probeCounter = 0;

            if (!this.probeTimer.Enabled)
            {
                this.probeTimer.Start();
            }
        }

        private void PerformProbing()
        {
            UpdateIndicatorVisuals();
            if (++this.probeCounter >= 3) this.probeTimer.Stop();
        }

        private void LangLedForm_Loaded(object sender, EventArgs e)
        {
            RefreshIndicatorOnSignal();
        }

        private void LangLedForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                this.notifyIcon1.Visible = false;
            }
        }

        private void LangLedForm_Closing(object sender, EventArgs e)
        {
            this.redLight?.TurnLight(false);
        }
    }
}
