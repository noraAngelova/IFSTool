using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace IFSTool
{
	/// <summary>
	/// Summary description for AddImplicationForm.
	/// </summary>
	public class EditImplicationForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public EditImplicationForm(ArrayList aImplicationsList)
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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 152);
			this.label1.TabIndex = 0;
			this.label1.Text = "TODO: da ima vazmojnost za direktna proverka dali e korektna; da proverqva sintak" +
				"sisa;  da ne moje da se slaga ime kato tiq v src (1-29) i proizvodnite (2,1 ...)" +
				"; da moje da se generira impl po zadadeno otricanie";
			// 
			// AddImplicationForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddImplicationForm";
			this.ShowInTaskbar = false;
			this.Text = "Add New Implication";
			this.Load += new System.EventHandler(this.AddImplicationForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void AddImplicationForm_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
