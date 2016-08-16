namespace csFormDX
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.screen = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// screen
			// 
			this.screen.Dock = System.Windows.Forms.DockStyle.Fill;
			this.screen.Location = new System.Drawing.Point(0, 0);
			this.screen.Name = "screen";
			this.screen.Size = new System.Drawing.Size(960, 720);
			this.screen.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(960, 720);
			this.Controls.Add(this.screen);
			this.Name = "Form1";
			this.Text = "test";
			this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Panel screen;
	}
}

