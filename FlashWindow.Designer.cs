namespace LangLed
{
  partial class FlashWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 200.1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(-123, -93);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(1277, 773);
      this.label1.TabIndex = 0;
      this.label1.Text = "EN";
      // 
      // FlashWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1001, 621);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FlashWindow";
      this.Opacity = 0.3D;
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "FlashWindow";
      this.TopMost = true;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label1;
    }
}