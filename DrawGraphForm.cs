using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;

namespace IFSTool
{
	public class DrawGraphForm : IFSTool.BaseForm
	{
		private System.Windows.Forms.PictureBox PictureBoxGraph;
		private System.Windows.Forms.CheckBox CheckBoxBW;
		private System.Windows.Forms.CheckBox CheckBoxHasse;
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.VScrollBar vScrollBar1;
		private System.ComponentModel.IContainer components = null;

		public DrawGraphForm(Graph aGraph)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			graph = aGraph;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.PictureBoxGraph = new System.Windows.Forms.PictureBox();
			this.CheckBoxBW = new System.Windows.Forms.CheckBox();
			this.CheckBoxHasse = new System.Windows.Forms.CheckBox();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.SuspendLayout();
			// 
			// ButtonSaveToFile
			// 
			this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 536);
			this.ButtonSaveToFile.Name = "ButtonSaveToFile";
			this.ButtonSaveToFile.Click += new System.EventHandler(this.ButtonSaveToFile_Click);
			// 
			// PictureBoxGraph
			// 
			this.PictureBoxGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.PictureBoxGraph.Location = new System.Drawing.Point(8, 8);
			this.PictureBoxGraph.Name = "PictureBoxGraph";
			this.PictureBoxGraph.Size = new System.Drawing.Size(760, 504);
			this.PictureBoxGraph.TabIndex = 101;
			this.PictureBoxGraph.TabStop = false;
			// 
			// CheckBoxBW
			// 
			this.CheckBoxBW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CheckBoxBW.Location = new System.Drawing.Point(392, 536);
			this.CheckBoxBW.Name = "CheckBoxBW";
			this.CheckBoxBW.Size = new System.Drawing.Size(224, 24);
			this.CheckBoxBW.TabIndex = 103;
			this.CheckBoxBW.Text = "&Black and White (suitable for printing)";
			this.CheckBoxBW.CheckedChanged += new System.EventHandler(this.CheckBoxBW_CheckedChanged);
			// 
			// CheckBoxHasse
			// 
			this.CheckBoxHasse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CheckBoxHasse.Location = new System.Drawing.Point(616, 536);
			this.CheckBoxHasse.Name = "CheckBoxHasse";
			this.CheckBoxHasse.Size = new System.Drawing.Size(168, 24);
			this.CheckBoxHasse.TabIndex = 104;
			this.CheckBoxHasse.Text = "Apply &Transitive Reduction";
			this.CheckBoxHasse.CheckedChanged += new System.EventHandler(this.CheckBoxHasse_CheckedChanged);
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.hScrollBar1.Location = new System.Drawing.Point(8, 512);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(760, 17);
			this.hScrollBar1.TabIndex = 105;
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.vScrollBar1.Location = new System.Drawing.Point(768, 8);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 504);
			this.vScrollBar1.TabIndex = 106;
			// 
			// DrawGraphForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.vScrollBar1);
			this.Controls.Add(this.hScrollBar1);
			this.Controls.Add(this.CheckBoxHasse);
			this.Controls.Add(this.CheckBoxBW);
			this.Controls.Add(this.PictureBoxGraph);
			this.Name = "DrawGraphForm";
			this.Text = "Implications Graph";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DrawGraphForm_Closing);
			this.Load += new System.EventHandler(this.DrawGraphForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawGraphForm_Paint);
			this.Controls.SetChildIndex(this.PictureBoxGraph, 0);
			this.Controls.SetChildIndex(this.CheckBoxBW, 0);
			this.Controls.SetChildIndex(this.CheckBoxHasse, 0);
			this.Controls.SetChildIndex(this.hScrollBar1, 0);
			this.Controls.SetChildIndex(this.vScrollBar1, 0);
			this.Controls.SetChildIndex(this.ButtonSaveToFile, 0);
			this.ResumeLayout(false);

		}
		#endregion

		private Graph graph;
		private Bitmap bitmap;

		private void DrawGraphForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Size size;
			graph.PrepareDraw(CheckBoxHasse.Checked, out size, CheckBoxBW.Checked);
            //ili puk da moje da risuva spored zadaden ot user-a razmer?

			if (bitmap != null)
				bitmap.Dispose();
			bitmap = new Bitmap(size.Width, size.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.SmoothingMode = SmoothingMode.AntiAlias; //da, ama GIF-a se razvalq! ako e GIF, de
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            graph.Draw(graphics, CheckBoxBW.Checked);

			PictureBoxGraph.Image = bitmap;
			PictureBoxGraph.Height = (int)((double)PictureBoxGraph.Width * size.Height / size.Width);
			PictureBoxGraph.SizeMode = PictureBoxSizeMode.StretchImage;
			//da, ama nqkoi pat moje da raste mnogo nadolu; trqbva da moje i da naglasq horizontala spored vertikala

			graphics.Dispose();
		}

		private void DrawGraphForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bitmap.Dispose();
		}

		private void DrawGraphForm_Load(object sender, System.EventArgs e)
		{
			CheckBoxHasse.Checked = true;//6to ne e v konstruktora? mai tuk po-dobre??
		}

		private void ButtonSaveToFile_Click(object sender, System.EventArgs e)
		{
			SaveDataToFileDialog = new SaveFileDialog();
			SaveDataToFileDialog.Filter = "PNG files (*.png)|*.png";//da moje i BMP, ...
			SaveDataToFileDialog.ShowDialog();
			if (SaveDataToFileDialog.FileName != "")
			{
				FileStream fs = new FileStream(SaveDataToFileDialog.FileName, FileMode.Create);
				using (fs)
				{
					bitmap.Save(fs, ImageFormat.Png);
				}
			}
			SaveDataToFileDialog.Dispose();
		}

		private void CheckBoxBW_CheckedChanged(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}

		private void CheckBoxHasse_CheckedChanged(object sender, System.EventArgs e)
		{
			this.Invalidate();
		}
	}
}

