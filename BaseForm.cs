using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace IFSTool
{
	/// <summary>
	/// Summary description for BaseForm.
	/// </summary>
	public class BaseForm : System.Windows.Forms.Form
	{
		protected System.Windows.Forms.Button ButtonSaveToFile;
		protected System.Windows.Forms.SaveFileDialog SaveDataToFileDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BaseForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonSaveToFile = new System.Windows.Forms.Button();
			this.SaveDataToFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// ButtonSaveToFile
			// 
			this.ButtonSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 224);
			this.ButtonSaveToFile.Name = "ButtonSaveToFile";
			this.ButtonSaveToFile.Size = new System.Drawing.Size(112, 23);
			this.ButtonSaveToFile.TabIndex = 100;
			this.ButtonSaveToFile.Text = "Save Data to File...";
			// 
			// BaseForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 254);
			this.Controls.Add(this.ButtonSaveToFile);
			this.Name = "BaseForm";
			this.ShowInTaskbar = false;
			this.Text = "IFS Tool";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
