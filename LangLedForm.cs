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
    int n = 0;

    public LangLedForm()
    {
      InitializeComponent();

      if (LangLedMain.EnableArduino) redLight = new RedLightController();

      this.Icon = Resource1.Keyboard;

      this.notifyIcon1.Icon = Resource1.Keyboard;
      this.notifyIcon1.Click += delegate {
        this.Show();
        this.WindowState = FormWindowState.Normal;
      };

      this.Resize += LangLedForm_Resize;
      this.Closing += LangLedForm_Closing;
      this.Load += LangLedForm_Loaded;
    }

    private bool lastStateWasRu = false;
    private EdgeWindow edgeWindow = null;

    private void UpdateIndicatorMessage()
    {
      //var timer = new MilliTimer();
      bool ru = Win32.IsLanguageRussian();
      //System.Diagnostics.Trace.WriteLine("isRu=" + timer);

      if (lastStateWasRu != ru) {
        ShowEdgeWindow(ru);
        lastStateWasRu = ru;
      }
      //System.Diagnostics.Trace.WriteLine("win=" + timer);

      n += 1;
      string text = (ru ? "RU" : "EN");

      this.label1.Text = n.ToString() + ": " + text;
      this.label2.Text = this.redLight?.GetState();

      //var color = ru ? Color.DarkRed : Color.DarkBlue;
      //LaunchFlashWindow(text, color);

      Action a = delegate { this.redLight?.TurnLight(ru); };
      Invoke(a);
    }

    private void ShowEdgeWindow(bool on)
    {
      if (edgeWindow == null) edgeWindow = new EdgeWindow();
      if (on) {
        edgeWindow.Show();
      } else {
        edgeWindow.Hide();
      }
    }

    private void LaunchFlashWindow(string text, Color color)
    {
      if (flashWindow != null) flashWindow.Close();
      flashWindow = new FlashWindow(text, color);
      flashWindow.Show();
    }

    public void RefreshIndicator()
    {
      Delegate d = (MethodInvoker)delegate { BeginSoftProbing(); };
      BeginInvoke(d);
    }

    private Timer probeTimer;
    private FlashWindow flashWindow;

    private void BeginSoftProbing()
    {
      if (this.probeTimer == null) {
        this.probeTimer = new Timer();
        this.probeTimer.Interval = 10;
        this.probeTimer.Tick += delegate { PerformOneProbing(); };
      }

      if (!this.probeTimer.Enabled) this.probeTimer.Start();
    }

    private void PerformOneProbing()
    {
      UpdateIndicatorMessage();
      this.probeTimer.Stop();
    }

    private void LangLedForm_Loaded(object sender, EventArgs e)
    {
      RefreshIndicator();
    }

    private void LangLedForm_Resize(object sender, EventArgs e)
    {
      if (FormWindowState.Minimized == this.WindowState) {
        this.notifyIcon1.Visible = true;
        this.Hide();
      } else if (FormWindowState.Normal == this.WindowState) {
        this.notifyIcon1.Visible = false;
      }
    }

    private void LangLedForm_Closing(object sender, EventArgs e)
    {
      this.redLight?.TurnLight(false);
    }
  }
}
