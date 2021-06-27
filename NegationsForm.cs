using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using IFSTool.IntFuzzyExpression;
using IFSTool.ArithmeticExpression;

namespace IFSTool
{
	public class NegationsForm : IFSTool.BaseForm
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ListBox ListBoxNegations;
		private System.Windows.Forms.Button ButtonCheckErrors;

		private IList mNegations;
		
		public NegationsForm(IList aImplications)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			mNegations = new ArrayList();

			foreach (Implication implication in aImplications)
			{
				Negation neg = IFExpressionUtil.GenerateNegation(implication);

                //TEMP CODE:
                //IFConjunctionNode node = (IFConjunctionNode) (new IntFuzzyParser().Parse("\\neg \\neg A \\& \\neg B"));
                //((IFNegationNode)node.RightArgument).Negation = neg;
                //((IFNegationNode)node.LeftArgument).Negation = neg;
                //((IFNegationNode)((IFNegationNode)node.LeftArgument).Argument).Negation = neg;
                //IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
                //IFExpressionUtil.ConvertToArithmetic(node, out truth, out untruth);
                //ArithmeticExpression.ExpressionUtil.Normalize(ref truth);
                //ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);
                //Negation r = new Negation(truth, untruth, neg.Name);
                //mNegations.Add(r);

                IArithmeticNode node = neg.Truth;
                ExpressionUtil.Normalize(ref node);
                neg.Truth = node;
                node = neg.Untruth;
                ExpressionUtil.Normalize(ref node);
                neg.Untruth = node;

                mNegations.Add(neg);
			}

			ListBoxNegations.DataSource = mNegations;
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
            this.ListBoxNegations = new System.Windows.Forms.ListBox();
            this.ButtonCheckErrors = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonSaveToFile
            // 
            this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 336);
            this.ButtonSaveToFile.Click += new System.EventHandler(this.ButtonSaveToFile_Click);
            // 
            // ListBoxNegations
            // 
            this.ListBoxNegations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ListBoxNegations.Location = new System.Drawing.Point(8, 8);
            this.ListBoxNegations.Name = "ListBoxNegations";
            this.ListBoxNegations.Size = new System.Drawing.Size(376, 316);
            this.ListBoxNegations.TabIndex = 101;
            // 
            // ButtonCheckErrors
            // 
            this.ButtonCheckErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCheckErrors.Location = new System.Drawing.Point(128, 336);
            this.ButtonCheckErrors.Name = "ButtonCheckErrors";
            this.ButtonCheckErrors.Size = new System.Drawing.Size(104, 23);
            this.ButtonCheckErrors.TabIndex = 102;
            this.ButtonCheckErrors.Text = "Check for errors...";
            this.ButtonCheckErrors.Visible = false;
            this.ButtonCheckErrors.Click += new System.EventHandler(this.ButtonCheckErrors_Click);
            // 
            // NegationsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(392, 366);
            this.Controls.Add(this.ButtonCheckErrors);
            this.Controls.Add(this.ListBoxNegations);
            this.Name = "NegationsForm";
            this.Controls.SetChildIndex(this.ListBoxNegations, 0);
            this.Controls.SetChildIndex(this.ButtonCheckErrors, 0);
            this.Controls.SetChildIndex(this.ButtonSaveToFile, 0);
            this.ResumeLayout(false);

		}
		#endregion

		private void ButtonSaveToFile_Click(object sender, System.EventArgs e)
		{
			SaveDataToFileDialog = new SaveFileDialog();
			SaveDataToFileDialog.Filter = "Text files (*.txt)|*.txt";
			SaveDataToFileDialog.ShowDialog();
			if (SaveDataToFileDialog.FileName != "")
			{
				StreamWriter writer = new StreamWriter(SaveDataToFileDialog.FileName);
				using (writer)
				{
					foreach (Negation negation in mNegations)
						writer.WriteLine(negation);
				}
			}
			SaveDataToFileDialog.Dispose();
		}

		private void ButtonCheckErrors_Click(object sender, System.EventArgs e)
		{
			//mk win, v koito da izliza spisak s lo6ite i da pita "Da gi triem li?"
			//da ima i checkbox "Do not treat <a, a> like true, nor like false"
			for (int index = 0; index < mNegations.Count; index++)
			{
				if (!(IFMain.IsPossiblyTrueIFNegation((Negation)(mNegations[index]), 
					true)))//tova true da se smeni s CheckBox...
				{
					mNegations.RemoveAt(index);//tva triene posle!
					index--;
				}
			}
			ListBoxNegations.DataSource = null;
			ListBoxNegations.DataSource = mNegations;
		}
	}
}

