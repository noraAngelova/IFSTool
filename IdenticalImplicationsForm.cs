using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using IFSTool.IntFuzzyExpression;
using System.IO;

namespace IFSTool
{
	public class IdenticalImplicationsForm : IFSTool.BaseForm
	{
		private System.Windows.Forms.Button ButtonOK;
		private System.Windows.Forms.Button ButtonCancel;
		private System.Windows.Forms.ListBox ListBoxImplications;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label LabelNotProved;
        private CheckBox CheckBoxRenumber;
		private System.ComponentModel.IContainer components = null;

		public IdenticalImplicationsForm(IList aIdenticalSets)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			IList content = new ArrayList();
			int notProved = 0;
			foreach (IList group in aIdenticalSets)
			{
				content.Add(group[0].ToString());
				for (int index = 1; index < group.Count; index++)
				{
					string row = "\t" + group[index].ToString();
					bool areIdentical = IFMain.IsProvenIdentity((Implication)group[index], (Implication)group[0]);
					if (!areIdentical)
					{
						row += "  (not proved)";
						notProved++;
					}
					content.Add(row);
				}
			}
			ListBoxImplications.DataSource = content;

			LabelNotProved.Text = notProved.ToString();
		}

        public bool ReNumberImplications
        {
            get { return CheckBoxRenumber.Checked; }
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
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ListBoxImplications = new System.Windows.Forms.ListBox();
            this.LabelNotProved = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckBoxRenumber = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ButtonSaveToFile
            // 
            this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 416);
            this.ButtonSaveToFile.Click += new System.EventHandler(this.ButtonSaveToFile_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Location = new System.Drawing.Point(416, 416);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(128, 23);
            this.ButtonOK.TabIndex = 1;
            this.ButtonOK.Text = "Remove Duplicates";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(552, 416);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ListBoxImplications
            // 
            this.ListBoxImplications.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxImplications.Location = new System.Drawing.Point(8, 8);
            this.ListBoxImplications.Name = "ListBoxImplications";
            this.ListBoxImplications.Size = new System.Drawing.Size(616, 381);
            this.ListBoxImplications.TabIndex = 0;
            // 
            // LabelNotProved
            // 
            this.LabelNotProved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelNotProved.Location = new System.Drawing.Point(8, 392);
            this.LabelNotProved.Name = "LabelNotProved";
            this.LabelNotProved.Size = new System.Drawing.Size(48, 16);
            this.LabelNotProved.TabIndex = 104;
            this.LabelNotProved.Text = "0";
            this.LabelNotProved.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(64, 392);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 16);
            this.label2.TabIndex = 103;
            this.label2.Text = "equalities should be proved manually.";
            // 
            // CheckBoxRenumber
            // 
            this.CheckBoxRenumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckBoxRenumber.AutoSize = true;
            this.CheckBoxRenumber.Location = new System.Drawing.Point(267, 420);
            this.CheckBoxRenumber.Name = "CheckBoxRenumber";
            this.CheckBoxRenumber.Size = new System.Drawing.Size(135, 17);
            this.CheckBoxRenumber.TabIndex = 105;
            this.CheckBoxRenumber.Text = "Re-number implications";
            this.CheckBoxRenumber.UseVisualStyleBackColor = true;
            // 
            // IdenticalImplicationsForm
            // 
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CheckBoxRenumber);
            this.Controls.Add(this.LabelNotProved);
            this.Controls.Add(this.ListBoxImplications);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Name = "IdenticalImplicationsForm";
            this.Controls.SetChildIndex(this.ButtonOK, 0);
            this.Controls.SetChildIndex(this.ButtonCancel, 0);
            this.Controls.SetChildIndex(this.ListBoxImplications, 0);
            this.Controls.SetChildIndex(this.LabelNotProved, 0);
            this.Controls.SetChildIndex(this.CheckBoxRenumber, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.ButtonSaveToFile, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ButtonOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void ButtonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

        private void ButtonSaveToFile_Click(object sender, EventArgs e)
        {
            SaveDataToFileDialog = new SaveFileDialog();
            SaveDataToFileDialog.Filter = "Text files (*.txt)|*.txt|TeX files (*.tex)|*.tex";
            SaveDataToFileDialog.ShowDialog();
            if (SaveDataToFileDialog.FileName != "")
            {
                StreamWriter writer = new StreamWriter(SaveDataToFileDialog.FileName);
                //temp
                int negIndex = 0; //neg???
                using (writer)
                {
                    foreach (string str in (IList)ListBoxImplications.DataSource)
                    {
                        if (SaveDataToFileDialog.FilterIndex == 1)
                            writer.WriteLine(str);
                        else if (SaveDataToFileDialog.FilterIndex == 2)
                        {
                            //temp
                            if (str[0] != '\t')
                            {
                                negIndex++;
                                string implIndex = str.Substring(0, str.IndexOf(':'));
                                writer.Write("$ \\\\ \\hline\r\n$\\neg_{{{0}}}$ & $\\ri_{{{1}}}", negIndex, implIndex);
                            }
                            else
                            {
                                string implIndex = str.Substring(1, str.IndexOf(':') - 1);
                                writer.Write(", \\ri_{{{0}}}", implIndex);
                            }
                        }
                    }
                    //mai pri tex ne go dovar6va kakto trqbva
                }
            }
            SaveDataToFileDialog.Dispose();

        }
	}
}

