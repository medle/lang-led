using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangLed
{
  public partial class FlashWindow : Form
  {
    private Timer timer;

    public FlashWindow(string text, Color color)
    {
      InitializeComponent();
      this.label1.Text = text;
      this.label1.ForeColor = color;

      this.StartPosition = FormStartPosition.Manual;

      var screenSize = Screen.FromControl(this).Bounds.Size;
      int x = (screenSize.Width - this.Width) / 2;
      int y = (screenSize.Height - this.Height) / 2;
      this.Location = new Point(x, y);

      //SetStyle(ControlStyles.UserPaint, true);
      //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      //SetStyle(ControlStyles.SupportsTransparentBackColor, true);

      //this.BackColor = Color.Transparent;
      //this.label1.BackColor = Color.Transparent;
      //this.TransparencyKey = Color.Transparent; // I had to add this to get it to work.

      //this.BackColor = Color.LimeGreen;
      //this.TransparencyKey = Color.LimeGreen;

      this.Load += delegate {
        timer = new Timer();
        timer.Interval = 200;
        timer.Tick += delegate { Close(); timer.Stop(); };
        timer.Start();
      };
    }
  }
}
