﻿using System;
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
  public partial class EdgeWindow : Form
  {
    public EdgeWindow()
    {
      InitializeComponent();

      this.StartPosition = FormStartPosition.Manual;
      this.TopMost = true;
      this.Opacity = 1;

      var screenSize = Screen.FromControl(this).Bounds.Size;
      int h = screenSize.Height / 2;
      int skip = (screenSize.Height - h) / 2;

      int x = 0;
      int y = 0;
      this.Location = new Point(x, y + skip);
      this.Size = new Size(15, h);
    }
  }
}
