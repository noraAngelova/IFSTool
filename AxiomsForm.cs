using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using IFSTool.IntFuzzyExpression;

namespace IFSTool
{
	public class AxiomsForm : IFSTool.BaseForm
	{
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.Button ButtonCheck;
		private System.Windows.Forms.DataGrid DataGridAxiomsResults;
		private System.Windows.Forms.CheckBox CheckBoxIFTautologies;
		private System.Windows.Forms.Button ButtonRemoveImplications;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;

		private System.Windows.Forms.Button ButtonSaveAxioms;
		private System.Windows.Forms.GroupBox GroupBoxAxioms;
		private System.Windows.Forms.CheckedListBox CheckedListBoxAxioms;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ButtonCheckMore;
        private System.Windows.Forms.OpenFileDialog ImportAxiomsFileDialog;
        private TextBox TextBoxShots;
        private Label LabelTimer;
        private CheckBox CheckBoxNonLinear;
        private ProgressBar ProgressBarAxioms;
        private CheckBox CheckBoxCorrespondingNegations;

        private IList mImplications;
        private IList mNegations;
        private IList mAxioms;
        private IList mUniqueNegations;
        private AxiomStatus[,] mResults;
        private Thread mCalculationThread;
        private DateTime mStartTime;
        private bool mCheckOnlyPossibleSatisfied = false;
        private bool mCalculatingMore = false;
        private String mChanged = "";

        private delegate void AxiomCheckStarted(int rows, int columns);
        private delegate void UpdateCellDelegate(int row, int col, AxiomStatus status);
        private delegate void AxiomCheckFinished();

		public AxiomsForm(IList aImplications)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			CheckBoxIFTautologies.Checked = true;
            CheckBoxNonLinear.Checked = (IFMain.Shots >= 5);
            CheckBoxCorrespondingNegations.Checked = true;

			mImplications = aImplications;

			mNegations = new ArrayList();
			foreach (Implication implication in mImplications)
			{
				Negation negation = IFExpressionUtil.GenerateNegation(implication);

				ArithmeticExpression.IArithmeticNode node = negation.Truth;
				ArithmeticExpression.ExpressionUtil.Normalize(ref node);
				negation.Truth = node;
				node = negation.Untruth;
				ArithmeticExpression.ExpressionUtil.Normalize(ref node);
				negation.Untruth = node;

				mNegations.Add(negation);
			}

            // TODO: !!! not always this is needed; also if the step is big, the list won't be updated
            int sh = IFMain.Shots;//this is needed because GetIdenticalSets fills the cache with data for the negations!
            IFMain.Shots = 3;
            IFMain.Shots = 5;
            ArrayList tempImplications = new ArrayList();
            foreach (Negation neg in mNegations)
            {
                Implication impl = new Implication(neg.Truth, neg.Untruth, "neg-" + neg.Name);//rename because the cache will be polluted
                tempImplications.Add(impl);
            }
            IList identicalSets = IFMain.GetIdenticalSets(tempImplications);//this modifies the cache
            IFMain.RemoveIdenticalImplications(ref tempImplications, identicalSets);
            mUniqueNegations = new ArrayList();
            int i = 0;
            foreach (Implication impl in tempImplications)
            {
                ++i;
                mUniqueNegations.Add(new Negation(impl.Truth, impl.Untruth, "neg-" + i.ToString()));
            }
            IFMain.Shots = 3;
            IFMain.Shots = sh;

            //mUniqueNegations = mNegations;

            //mUniqueNegations = new ArrayList();
            //IFSTool.ArithmeticExpression.ArithmeticParser p = new IFSTool.ArithmeticExpression.ArithmeticParser();
            //mUniqueNegations.Add(new Negation((IFSTool.ArithmeticExpression.IArithmeticNode)p.Parse("b"), (IFSTool.ArithmeticExpression.IArithmeticNode)p.Parse("a"), "1"));
            //mUniqueNegations.Add(new Negation((IFSTool.ArithmeticExpression.IArithmeticNode)p.Parse("os(a)"), (IFSTool.ArithmeticExpression.IArithmeticNode)p.Parse("sg(a)"), "2"));

            ImportAxiomsFileDialog = new OpenFileDialog();
            ImportAxiomsFileDialog.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
            ImportAxiomsFileDialog.Title = "Import Axioms from File";
            ImportAxiomsFileDialog.ShowDialog();
            if (ImportAxiomsFileDialog.FileName != "")
            {
                mAxioms = IFMain.ReadAxiomsFromFile(ImportAxiomsFileDialog.FileName);
            }
            ImportAxiomsFileDialog.Dispose();

			//tova trqbva da e v otdelna funkciq, 6te se vika 4esto:
			((ListBox)CheckedListBoxAxioms).DataSource = null;
			((ListBox)CheckedListBoxAxioms).DataSource = mAxioms;

			//q da vidim bre
//			string result = "";
//			for (int i = 0; i < mImplications.Count; i++)
//			{
//				foreach (IFNode axiom in mAxioms)
//				{
//					IFMain.SetImplicationAndNegation(axiom, (Implication)mImplications[i], (Negation)mNegations[i]);
//					IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
//					IFExpressionUtil.ConvertToArithmetic(axiom, out truth, out untruth);
//					ArithmeticExpression.ExpressionUtil.Normalize(ref truth);
//					ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);
//					string ax = axiom.ToString();
//					string impl = ((Implication)mImplications[i]).Name;
//					string tr = truth.ToString();
//					string untr = untruth.ToString();
//					result += impl + ": " + tr + "; " + untr + "                                                                                                                ";
//					int breakpoint = 0; breakpoint++;
//				}
//			}
//			int breakp = 0; breakp++;

            TextBoxShots.Text = IFMain.Shots.ToString();

            LabelTimer.Visible = false;
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
            this.DataGridAxiomsResults = new System.Windows.Forms.DataGrid();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.ButtonRemoveImplications = new System.Windows.Forms.Button();
            this.ButtonCheck = new System.Windows.Forms.Button();
            this.CheckBoxIFTautologies = new System.Windows.Forms.CheckBox();
            this.ButtonSaveAxioms = new System.Windows.Forms.Button();
            this.CheckedListBoxAxioms = new System.Windows.Forms.CheckedListBox();
            this.GroupBoxAxioms = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonCheckMore = new System.Windows.Forms.Button();
            this.TextBoxShots = new System.Windows.Forms.TextBox();
            this.LabelTimer = new System.Windows.Forms.Label();
            this.CheckBoxNonLinear = new System.Windows.Forms.CheckBox();
            this.ProgressBarAxioms = new System.Windows.Forms.ProgressBar();
            this.CheckBoxCorrespondingNegations = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridAxiomsResults)).BeginInit();
            this.GroupBoxAxioms.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonSaveToFile
            // 
            this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 416);
            this.ButtonSaveToFile.Click += new System.EventHandler(this.ButtonSaveToFile_Click);
            // 
            // DataGridAxiomsResults
            // 
            this.DataGridAxiomsResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridAxiomsResults.BackgroundColor = System.Drawing.SystemColors.Window;
            this.DataGridAxiomsResults.CaptionVisible = false;
            this.DataGridAxiomsResults.DataMember = "";
            this.DataGridAxiomsResults.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.DataGridAxiomsResults.Location = new System.Drawing.Point(288, 8);
            this.DataGridAxiomsResults.Name = "DataGridAxiomsResults";
            this.DataGridAxiomsResults.ReadOnly = true;
            this.DataGridAxiomsResults.Size = new System.Drawing.Size(416, 402);
            this.DataGridAxiomsResults.TabIndex = 70;
            this.DataGridAxiomsResults.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
            this.DataGridAxiomsResults.CurrentCellChanged += new System.EventHandler(this.DataGridAxiomsResults_CurrentCellChanged);
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.DataGridAxiomsResults;
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.PreferredColumnWidth = 32;
            this.dataGridTableStyle1.ReadOnly = true;
            this.dataGridTableStyle1.RowHeadersVisible = false;
            // 
            // ButtonRemoveImplications
            // 
            this.ButtonRemoveImplications.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonRemoveImplications.Location = new System.Drawing.Point(8, 387);
            this.ButtonRemoveImplications.Name = "ButtonRemoveImplications";
            this.ButtonRemoveImplications.Size = new System.Drawing.Size(272, 23);
            this.ButtonRemoveImplications.TabIndex = 63;
            this.ButtonRemoveImplications.Text = "Remove Implications that do not satisfy all axioms";
            this.ButtonRemoveImplications.Click += new System.EventHandler(this.ButtonRemoveImplications_Click);
            // 
            // ButtonCheck
            // 
            this.ButtonCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCheck.Location = new System.Drawing.Point(8, 288);
            this.ButtonCheck.Name = "ButtonCheck";
            this.ButtonCheck.Size = new System.Drawing.Size(272, 23);
            this.ButtonCheck.TabIndex = 60;
            this.ButtonCheck.Text = "Check All Axioms For All Implications";
            this.ButtonCheck.Click += new System.EventHandler(this.ButtonCheck_Click);
            // 
            // CheckBoxIFTautologies
            // 
            this.CheckBoxIFTautologies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxIFTautologies.Location = new System.Drawing.Point(8, 312);
            this.CheckBoxIFTautologies.Name = "CheckBoxIFTautologies";
            this.CheckBoxIFTautologies.Size = new System.Drawing.Size(272, 24);
            this.CheckBoxIFTautologies.TabIndex = 61;
            this.CheckBoxIFTautologies.Text = "Check for Intuitionistic Fuzzy Tautologies (a >= b)";
            // 
            // ButtonSaveAxioms
            // 
            this.ButtonSaveAxioms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonSaveAxioms.Location = new System.Drawing.Point(8, 242);
            this.ButtonSaveAxioms.Name = "ButtonSaveAxioms";
            this.ButtonSaveAxioms.Size = new System.Drawing.Size(120, 23);
            this.ButtonSaveAxioms.TabIndex = 50;
            this.ButtonSaveAxioms.Text = "Save Axioms to File...";
            this.ButtonSaveAxioms.Click += new System.EventHandler(this.ButtonSaveAxioms_Click);
            // 
            // CheckedListBoxAxioms
            // 
            this.CheckedListBoxAxioms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckedListBoxAxioms.Location = new System.Drawing.Point(8, 16);
            this.CheckedListBoxAxioms.Name = "CheckedListBoxAxioms";
            this.CheckedListBoxAxioms.Size = new System.Drawing.Size(256, 184);
            this.CheckedListBoxAxioms.TabIndex = 1;
            // 
            // GroupBoxAxioms
            // 
            this.GroupBoxAxioms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.GroupBoxAxioms.Controls.Add(this.CheckedListBoxAxioms);
            this.GroupBoxAxioms.Controls.Add(this.ButtonSaveAxioms);
            this.GroupBoxAxioms.Location = new System.Drawing.Point(8, 8);
            this.GroupBoxAxioms.Name = "GroupBoxAxioms";
            this.GroupBoxAxioms.Size = new System.Drawing.Size(272, 274);
            this.GroupBoxAxioms.TabIndex = 0;
            this.GroupBoxAxioms.TabStop = false;
            this.GroupBoxAxioms.Text = "Axioms:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(288, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.TabIndex = 101;
            this.label1.Text = "Click on a cell for detailed information.";
            // 
            // ButtonCheckMore
            // 
            this.ButtonCheckMore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCheckMore.Location = new System.Drawing.Point(520, 415);
            this.ButtonCheckMore.Name = "ButtonCheckMore";
            this.ButtonCheckMore.Size = new System.Drawing.Size(184, 24);
            this.ButtonCheckMore.TabIndex = 102;
            this.ButtonCheckMore.Text = "Check not proved with more shots";
            this.ButtonCheckMore.Click += new System.EventHandler(this.ButtonCheckMore_Click);
            // 
            // TextBoxShots
            // 
            this.TextBoxShots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxShots.Location = new System.Drawing.Point(493, 418);
            this.TextBoxShots.MaxLength = 12;
            this.TextBoxShots.Name = "TextBoxShots";
            this.TextBoxShots.Size = new System.Drawing.Size(21, 20);
            this.TextBoxShots.TabIndex = 103;
            // 
            // LabelTimer
            // 
            this.LabelTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LabelTimer.AutoSize = true;
            this.LabelTimer.Location = new System.Drawing.Point(288, 411);
            this.LabelTimer.Name = "LabelTimer";
            this.LabelTimer.Size = new System.Drawing.Size(22, 13);
            this.LabelTimer.TabIndex = 104;
            this.LabelTimer.Text = "0:0";
            // 
            // CheckBoxNonLinear
            // 
            this.CheckBoxNonLinear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxNonLinear.AutoSize = true;
            this.CheckBoxNonLinear.Location = new System.Drawing.Point(8, 340);
            this.CheckBoxNonLinear.Name = "CheckBoxNonLinear";
            this.CheckBoxNonLinear.Size = new System.Drawing.Size(255, 17);
            this.CheckBoxNonLinear.TabIndex = 62;
            this.CheckBoxNonLinear.Text = "Mark only nonlinear axioms as \'Possibly Satisfied\'";
            this.CheckBoxNonLinear.UseVisualStyleBackColor = true;
            // 
            // ProgressBarAxioms
            // 
            this.ProgressBarAxioms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBarAxioms.Location = new System.Drawing.Point(291, 416);
            this.ProgressBarAxioms.Name = "ProgressBarAxioms";
            this.ProgressBarAxioms.Size = new System.Drawing.Size(196, 23);
            this.ProgressBarAxioms.TabIndex = 105;
            // 
            // CheckBoxCorrespondingNegations
            // 
            this.CheckBoxCorrespondingNegations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxCorrespondingNegations.AutoSize = true;
            this.CheckBoxCorrespondingNegations.Location = new System.Drawing.Point(8, 363);
            this.CheckBoxCorrespondingNegations.Name = "CheckBoxCorrespondingNegations";
            this.CheckBoxCorrespondingNegations.Size = new System.Drawing.Size(186, 17);
            this.CheckBoxCorrespondingNegations.TabIndex = 106;
            this.CheckBoxCorrespondingNegations.Text = "Use only corresponding negations";
            this.CheckBoxCorrespondingNegations.UseVisualStyleBackColor = true;
            // 
            // AxiomsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(712, 446);
            this.Controls.Add(this.CheckBoxCorrespondingNegations);
            this.Controls.Add(this.ProgressBarAxioms);
            this.Controls.Add(this.CheckBoxNonLinear);
            this.Controls.Add(this.LabelTimer);
            this.Controls.Add(this.TextBoxShots);
            this.Controls.Add(this.ButtonCheckMore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GroupBoxAxioms);
            this.Controls.Add(this.ButtonRemoveImplications);
            this.Controls.Add(this.DataGridAxiomsResults);
            this.Controls.Add(this.CheckBoxIFTautologies);
            this.Controls.Add(this.ButtonCheck);
            this.Name = "AxiomsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AxiomsForm_FormClosed);
            this.Controls.SetChildIndex(this.ButtonCheck, 0);
            this.Controls.SetChildIndex(this.CheckBoxIFTautologies, 0);
            this.Controls.SetChildIndex(this.ButtonSaveToFile, 0);
            this.Controls.SetChildIndex(this.DataGridAxiomsResults, 0);
            this.Controls.SetChildIndex(this.ButtonRemoveImplications, 0);
            this.Controls.SetChildIndex(this.GroupBoxAxioms, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.ButtonCheckMore, 0);
            this.Controls.SetChildIndex(this.TextBoxShots, 0);
            this.Controls.SetChildIndex(this.LabelTimer, 0);
            this.Controls.SetChildIndex(this.CheckBoxNonLinear, 0);
            this.Controls.SetChildIndex(this.ProgressBarAxioms, 0);
            this.Controls.SetChildIndex(this.CheckBoxCorrespondingNegations, 0);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridAxiomsResults)).EndInit();
            this.GroupBoxAxioms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        private void CheckAxioms()
        {
            mStartTime = DateTime.Now;

            CalculateResults(false);

            Invoke(new AxiomCheckFinished(OnAxiomCheckFinished));
        }

        private void CheckAxiomsAsync()
        {
            if (mCalculationThread != null)
                mCalculationThread.Abort();
            mCalculationThread = new Thread(new ThreadStart(CheckAxioms));
            mCalculationThread.Start();
        }

		private void ButtonCheck_Click(object sender, System.EventArgs e)
		{
            //// temp code for \neg_i \neg_j A = \neg_k \neg_l A
            //// load neg34implications, because we need the negations
            //Implication implication = new Implication(new IFSTool.ArithmeticExpression.ConstantNode(1),
            //    new IFSTool.ArithmeticExpression.ConstantNode(0), "1");
            //Negation negation = new Negation(new IFSTool.ArithmeticExpression.ConstantNode(1),
            //    new IFSTool.ArithmeticExpression.ConstantNode(0), "1");
            //StreamWriter writer = new StreamWriter("d:\\docume~1\\work\\desktop\\Axioms.txt");

            //using (writer)
            //{
            //    for (int i = 0; i < 34; ++i)
            //        //if (i == 2 || i == 11 || i >= 25) //kofti-negs, ama ne e dostaty4no taka...
            //        for (int j = 0; j < 34; ++j)
            //            //if (j == 2 || j == 11 || j >= 25) //kofti-negs
            //            for (int k = 0; k < 34; ++k)
            //                //if (k == 2 || k == 11 || k >= 25) //kofti-negs
            //                for (int l = 0; l < 34; ++l)
            //                {
            //                    //if (l == 2 || l == 11 || l >= 25) //kofti-negs
            //                    if (i * 34 + j < k * 34 + l
            //                        && !((i == 0) != (j == 0) && (k == 0) != (l == 0)))
            //                    {
            //                        String axiom = String.Format(
            //                            "{0}.{1}.{2}.{3} (\\neg_{0} \\neg_{1} A) = (\\neg_{2} \\neg_{3} A)", i, j, k, l);
            //                        IFSTool.IntFuzzyExpression.IFNode axiomNode = (IFSTool.IntFuzzyExpression.IFNode)new IntFuzzyParser().Parse(axiom);
            //                        bool result = IFMain.CheckAxiom(implication, negation, axiomNode, false, mImplications, mNegations);
            //                        if (result)
            //                        {
            //                            writer.WriteLine("{0}, {1}, {2}, {3}", i+1, j+1, k+1, l+1);
            //                            //writer.WriteLine("{0}.{1}.{2}.{3} (\\neg_{0} \\neg_{1} A) = (\\neg_{2} \\neg_{3} A)", i, j, k, l);
            //                        }
            //                    }
            //                }
            //}
            //return;

            mCheckOnlyPossibleSatisfied = false;
            CheckAxiomsAsync();
		}

		private void OnAxiomCheckFinished()
		{
            LabelTimer.Visible = true;
            LabelTimer.Text = (DateTime.Now - mStartTime).ToString();
            ProgressBarAxioms.Visible = false;

            ButtonCheckMore.Text = "Check not proved with more shots";//FIXME code duplicate!
            mCalculatingMore = false;

            DrawTable();

            if (mCheckOnlyPossibleSatisfied)
            {
                if (mChanged.Length > 0)
                    MessageBox.Show("Results changed:" + mChanged);
                else MessageBox.Show("No results changed.");
            }
        }

        private void DrawTable()
        {
            // TODO: strange bug; iff first only corresponding negations - OK, but then uncheck - column count remains the same
            
            DataTable table = new DataTable();

            DataColumn implColumn = new DataColumn("Implication", System.Type.GetType("System.String"));
			table.Columns.Add(implColumn);

			// remove this:
			DataColumn negColumn = new DataColumn("Negation", System.Type.GetType("System.String"));
			table.Columns.Add(negColumn);
			//

			for (int index = 0; index < mResults.GetLength(1); index++)
			{
				DataColumn column = new DataColumn((index + 1).ToString(), System.Type.GetType("System.String"));
				table.Columns.Add(column);
			}
			for (int implicationIndex = 0; implicationIndex < mResults.GetLength(0); implicationIndex++)
			{
				DataRow row;
				row = table.NewRow();
				row[0] = mImplications[implicationIndex].ToString();

				//remove this
				row[1] = ((Negation)mNegations[implicationIndex]).Truth.ToString() + ", "
					+ ((Negation)mNegations[implicationIndex]).Untruth.ToString();


				string text = "?";
				for (int index = 0; index < mResults.GetLength(1); index++)
				{
					switch (mResults[implicationIndex, index])
					{
						case AxiomStatus.PossiblySatisfied: text = "\u221A?"; break;
						case AxiomStatus.ProvedNotSatisfied: text = "-"; break;
						case AxiomStatus.ProvedSatisfied: text = "\u221A"; break;
						default: text = "?"; break;
					}
					row[index + 2] = text; //remove this: was +1
				}
				table.Rows.Add(row);
			}
			DataGridAxiomsResults.DataSource = null;//
			DataGridAxiomsResults.DataSource = table;
		}

        private void OnAxiomCheckStarted(int rows, int columns)
        {
            LabelTimer.Visible = false;

            ProgressBarAxioms.Visible = true;
            ProgressBarAxioms.Minimum = 1;
            ProgressBarAxioms.Maximum = rows * columns;
            ProgressBarAxioms.Step = 1;
            ProgressBarAxioms.Value = 1;
        }

        private void OnAxiomChecked(int row, int column, AxiomStatus status)
        {
            //not needed for now
            //also be careful when "...corresponding..." checkbox is toggled!

            //DataTable table = (DataTable) DataGridAxiomsResults.DataSource;

            //if (table != null)
            //{
            //    table.Rows[row][column] = status; //TODO: string actually
            //}

            ProgressBarAxioms.PerformStep();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="implicationIndex"></param>
        /// <returns>Each time list with the same length</returns>
        private IList GetNegations(int implicationIndex)
        {
            if (CheckBoxCorrespondingNegations.Checked)
            {
                IList result = new ArrayList();
                result.Add(mNegations[implicationIndex]);
                return result;
            }
            else
            {
                return mUniqueNegations;
            }
        }

		private void CalculateResults(bool aSkipOnFalse) //dali da preska4a impl, ako ne udovl. nqkoq axioma
		{
            //temp
            //this.BuildTableForPublication();
            //return;

            int uniqueNegationsCount = 1;
            int columnsCount = mAxioms.Count;
            if (! CheckBoxCorrespondingNegations.Checked)
            {
                uniqueNegationsCount = mUniqueNegations.Count;
                columnsCount *= uniqueNegationsCount;
            }

            Invoke(new AxiomCheckStarted(OnAxiomCheckStarted), mImplications.Count, columnsCount);

            mChanged = "";

            if (!mCheckOnlyPossibleSatisfied)
            {
                mResults = new AxiomStatus[mImplications.Count, columnsCount];

                //foreach (Implication implication in mImplications)
                //	foreach (IFNode axiom in mAxioms)
                for (int implicationIndex = 0; implicationIndex < mImplications.Count; implicationIndex++)
                {
                    IList negations = GetNegations(implicationIndex);
                    int negationsCount = negations.Count; //=uniqueNegationsCount
                    for (int axiomIndex = 0; axiomIndex < mAxioms.Count; axiomIndex++)
                    {
                        bool broken = false;
                        for (int negationIndex = 0; negationIndex < negations.Count; negationIndex++)
                        {
                            int columnIndex = axiomIndex * negationsCount + negationIndex;
                            if (!broken)
                            {
                                bool isTautology = IFMain.CheckAxiom(
                                    (Implication)mImplications[implicationIndex],
                                    (Negation)negations[negationIndex],
                                    (IFNode)mAxioms[axiomIndex],
                                    CheckBoxIFTautologies.Checked,
                                    mImplications, mNegations);
                                if (isTautology)
                                    mResults[implicationIndex, columnIndex] = AxiomStatus.PossiblySatisfied;
                                else mResults[implicationIndex, columnIndex] = AxiomStatus.ProvedNotSatisfied;
                                if (isTautology) //za proverka za gre6ki na proving-a: if(!isTautology), posle ako v saved html ima <td>4avka</td> - lo6o
                                {
                                    if (IFMain.ProveAxiom(
                                        (Implication)mImplications[implicationIndex],
                                        (Negation)negations[negationIndex],
                                        (IFNode)mAxioms[axiomIndex],
                                        CheckBoxIFTautologies.Checked,
                                        CheckBoxNonLinear.Checked,
                                        mImplications, mNegations))
                                    {
                                        mResults[implicationIndex, columnIndex] = AxiomStatus.ProvedSatisfied;
                                    }
                                }
                                if (aSkipOnFalse && !isTautology)
                                {
                                    broken = true;
                                }
                            }

                            Invoke(new UpdateCellDelegate(OnAxiomChecked), implicationIndex,
                                columnIndex + 2, mResults[implicationIndex, columnIndex]);
                        }
                    }
                }

                //			int gadost = CachedValues.gadostCounter;
                //			int radost = CachedValues.radostCounter;
                //			int faida = radost - gadost;
                //			double gadoriq = (double)gadost / (radost + gadost);
            }
            else
            {
                IFMain.Shots = Int32.Parse(TextBoxShots.Text);
                for (int implicationIndex = 0; implicationIndex < mImplications.Count; implicationIndex++)
                    for (int columnIndex = 0; columnIndex < mResults.GetLength(1); columnIndex++)
                    {
                        if (mResults[implicationIndex, columnIndex] == AxiomStatus.PossiblySatisfied)
                        {
                            Implication implication = (Implication)mImplications[implicationIndex];
                            Negation negation = null;
                            IFNode axiom = null;
                            if (CheckBoxCorrespondingNegations.Checked)
                            {
                                negation = (Negation)mNegations[implicationIndex];
                                axiom = (IFNode)mAxioms[columnIndex];
                            }
                            else
                            {
                                negation = (Negation)mUniqueNegations[columnIndex % mUniqueNegations.Count];
                                axiom = (IFNode)mAxioms[columnIndex / mUniqueNegations.Count];
                            }

                            bool isTautology = IFMain.CheckAxiom(
                                implication, negation, axiom,
                                CheckBoxIFTautologies.Checked,
                                mImplications, mNegations);
                            if (!isTautology)
                            {
                                mResults[implicationIndex, columnIndex] = AxiomStatus.ProvedNotSatisfied;
                                mChanged += " (" + (implicationIndex + 1) + ',' + (columnIndex + 1) + ')';
                            }
                        }
                        Invoke(new UpdateCellDelegate(OnAxiomChecked), implicationIndex,
                                columnIndex + 2, mResults[implicationIndex, columnIndex]);
                    }
            }
		}

		private void ButtonRemoveImplications_Click(object sender, System.EventArgs e)
		{
			//be6e po-dobre da ne zatvarq prozoreca, ami samo da trie redovete s prazni poleta (i implikaciite, estestveno)
			if (mResults == null)
			{
				//vsa6tnost tuk trqbva da stava po-barzo - pri parvo false da trie
                //TODO: in thread!
				CalculateResults(true);
			}
			for (int i = 0, k = 0; i < mResults.GetLength(0); i++)
			{
				bool satisfying = true;
				for (int j = 0; j < mResults.GetLength(1); j++)
					if (mResults[i,j] == AxiomStatus.ProvedNotSatisfied)
					{
						satisfying = false;
						break;
					}

				if (!satisfying)
				{
					mImplications.RemoveAt(k);//bavno, ama deistva
				}
				else
					k++;
			}

			this.Close();
		}

		private void ButtonSaveAxioms_Click(object sender, System.EventArgs e)
		{
		}

		private void ButtonSaveToFile_Click(object sender, System.EventArgs e)
		{
			if (mResults == null)
			{
				MessageBox.Show("Please check the axioms first.");
			}
			else
			{
				SaveDataToFileDialog = new SaveFileDialog();
                SaveDataToFileDialog.Filter = "HTML files (*.htm)|*.htm|TeX files (*.tex)|*.tex|Text files (*.txt)|*.txt|Satisfying pairs lists (*.txt)|*.txt";
				SaveDataToFileDialog.ShowDialog();
				if (SaveDataToFileDialog.FileName != "")
				{
					StreamWriter writer = new StreamWriter(SaveDataToFileDialog.FileName);
					using (writer)
					{
                        if (SaveDataToFileDialog.FilterIndex == 1)
                        {
                            writer.WriteLine("<HTML><STYLE>TD, TH {font-family:Tahoma; font-size:14} TH {background:#80EFEF}</STYLE><meta http-equiv='Content-Type' content='text/html; charset=utf-8'><BODY><TABLE width='100%' border='1'>");

                            DataTable table = (DataTable)DataGridAxiomsResults.DataSource;
                            int cols = table.Columns.Count;
                            writer.Write("<TR><TH>Implication</TH><TH>Negation</TH>");
                            for (int index = 1; index < cols - 1; index++)
                                writer.Write("<TH>{0}</TH>", index);
                            writer.WriteLine("</TR>");

                            foreach (DataRow row in /*rows*/ table.Rows)
                            {
                                writer.WriteLine("<TR>");
                                for (int i = 0; i < cols; ++i)
                                {
                                    string s = "N/A";
                                    if (row[i] != null)
                                        s = row[i].ToString();
                                    writer.Write("<TD>{0}</TD>", s);
                                }
                                writer.WriteLine("</TR>");
                            }

                            writer.WriteLine("</TABLE></BODY></HTML>");
                        }
                        else if (SaveDataToFileDialog.FilterIndex == 2)
                        {
                            ////temp for cartesian product
                            //DataTable table = (DataTable)DataGridAxiomsResults.DataSource;
                            //int cols = table.Columns.Count;

                            //writer.Write("\\begin{tabular}{|");
                            //for (int index = 0; index < 2+5; index++) //-1 zaradi neg
                            //    writer.Write("l|");
                            //writer.WriteLine("} \\hline \\hline");

                            //for (int n = 0; n < 34; n++)
                            //{
                            //    for (int i = 0; i < 138; i++)
                            //    {
                            //        //skip empty lines!
                            //        //revert this code!
                            //        if (table.Rows[i][2 + n].ToString() != "-" ||
                            //            table.Rows[i][2 + n + 34].ToString() != "-" ||
                            //            table.Rows[i][2 + n + 68].ToString() != "-" ||
                            //            table.Rows[i][2 + n + 102].ToString() != "-" ||
                            //            table.Rows[i][2 + n + 136].ToString() != "-")
                            //        {
                            //            writer.Write("$\\neg_{" + (n + 1).ToString() + "}$ & ");
                            //            writer.Write("$\\ri_{" + (i + 1).ToString() + "}$");
                            //            for (int a = 0; a < 5; a++)
                            //            {
                            //                writer.Write(" & " + (table.Rows[i][2 + n + a * 34].ToString() == "-" ? "-" : "+"));
                            //            }
                            //            writer.WriteLine(" \\\\ \\hline");
                            //        }
                            //    }
                            //}
                            //writer.WriteLine("\\hline \\hline\\end{tabular}");
                            ////

                            DataTable table = (DataTable)DataGridAxiomsResults.DataSource;
                            int cols = table.Columns.Count;

                            //TODO: opravi tova, da e po-ob6to!
                            //DataRow[] rows = table.Select("", "Negation");

                            writer.Write("\\begin{tabular}{|");
                            for (int index = 0; index < cols - 1; index++) //-1 zaradi neg
                                writer.Write("l|");
                            writer.WriteLine("} \\hline \\hline");

                            foreach (DataRow row in /*rows*/ table.Rows)
                            {
                                //writer.WriteLine("<TR>");
                                writer.Write("$\\ri_{" + ((String)row[0]).Substring(0, ((String)row[0]).IndexOf(':')) + "}$");
                                for (int i = 2/*0*/; i < cols; ++i)
                                {
                                    //if (i == 1) continue; //skip "Negation" column
                                    //writer.Write("<TD>{0}</TD>", row[i].ToString());
                                    writer.Write(" & " + (row[i].ToString() == "-" ? "-" : "+"));
                                }
                                //writer.WriteLine("</TR>");
                                writer.WriteLine(" \\\\ \\hline");
                            }

                            writer.WriteLine("\\hline \\hline\\end{tabular}");
                        }
                        else if (SaveDataToFileDialog.FilterIndex == 3)
                        {
                            for (int implicationIndex = 0; implicationIndex < mResults.GetLength(0); implicationIndex++)
                            {
                                writer.Write(((Implication)mImplications[implicationIndex]).Name);
                                writer.Write('\t');
                                for (int index = 0; index < mResults.GetLength(1); index++)
                                {
                                    if (mResults[implicationIndex, index] == AxiomStatus.ProvedSatisfied)
                                        writer.Write(" +");
                                    else if (mResults[implicationIndex, index] == AxiomStatus.PossiblySatisfied)
                                        writer.Write(" #");
                                    else if (mResults[implicationIndex, index] == AxiomStatus.Unknown)
                                        writer.Write("?");
                                    else
                                        writer.Write("  ");

                                }
                                writer.WriteLine();
                            }
                        }
                        else if (SaveDataToFileDialog.FilterIndex == 4)
                        {
                            Implication implication;
                            Negation negation;
                            int axiomNumber;
                            IFNode axiom;
                            AxiomStatus axiomStatus;

                            for (int implicationIndex = 0; implicationIndex < mResults.GetLength(0); implicationIndex++)
                            {
                                for (int index = 0; index < mResults.GetLength(1); index++)
                                {
                                    //if (mResults[implicationIndex, index] == AxiomStatus.ProvedSatisfied
                                    //    || mResults[implicationIndex, index] == AxiomStatus.PossiblySatisfied)
                                    if (true)
                                    {
                                        GetCellInfo(implicationIndex, index,
                                            out implication, out negation, out axiomNumber, out axiom, out axiomStatus);

                                        ////verbose version:
                                        //writer.Write("Implication ");
                                        //writer.Write(implication.ToString());
                                        //writer.Write("; Negation ");
                                        //writer.Write(negation.ToString());
                                        //writer.Write("; Axiom ");
                                        //writer.Write(axiomNumber);
                                        //writer.Write(": ");
                                        //writer.WriteLine(axiom.ToString());

                                        //writer.Write("Axiom " + axiomNumber + ": ");
                                        
                                        IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
                                        IFExpressionUtil.ConvertToArithmetic(axiom, out truth, out untruth);
                                        ArithmeticExpression.ExpressionUtil.Normalize(ref truth);//pak li be...
                                        ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);

                                        writer.Write("<" + truth.ToString() + ",  " + untruth.ToString() + ">; ");
                                        
                                        if (axiomNumber % 5 == 0)
                                            writer.Write('\n');
                                        
                                        ////XXX temporary code for Problem 3
                                        //string implName = implication.Name;
                                        //if (axiom.ToString().Contains("((A --> <0,1>) --> <0,1>)"))
                                        //{
                                        //    if (implName.Equals("1"))
                                        //        continue;
                                        //    implName = (Int32.Parse(implName) + 33).ToString();
                                        //}
                                        //string negName = negation.Name.Substring(4);
                                        //if (axiom.ToString().Contains("not not "))
                                        //{
                                        //    if (negName.Equals("1"))
                                        //        continue;
                                        //    negName = (Int32.Parse(negName) + 33).ToString();
                                        //}
                                        //writer.Write(implName);
                                        //writer.Write(", ");
                                        //writer.Write(negName);
                                        //writer.Write(", ");
                                        //string axiomName = axiom.ToString();
                                        //axiomName = axiomName.Substring(0, axiomName.Length - 4);
                                        //axiomName = axiomName.Substring(axiomName.LastIndexOf('t') + 1);
                                        //writer.WriteLine((Int32.Parse(axiomName) + 1).ToString());
                                    }
                                }
                            }
                        }
					}
				}
				SaveDataToFileDialog.Dispose();
			}
		}

        private bool GetCellInfo(int row, int column,
            out Implication implication, out Negation negation, out int axiomNumber, out IFNode axiom, out AxiomStatus status)
        {
            if (row < 0 || column < 0)
            {
                implication = null;
                negation = null;
                axiomNumber = -1;
                axiom = null;
                status = AxiomStatus.Unknown;
                return false;
            }

            implication = (Implication)mImplications[row];
            if (CheckBoxCorrespondingNegations.Checked)
            {
                negation = (Negation)mNegations[row];
                axiom = (IFNode)mAxioms[column];
            }
            else
            {
                negation = (Negation)mUniqueNegations[column % mUniqueNegations.Count];
                axiom = (IFNode)mAxioms[column / mUniqueNegations.Count];
            }

            IFMain.SetImplicationAndNegation(axiom, implication, negation, mImplications, mNegations);
            //IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
            //IFExpressionUtil.ConvertToArithmetic(axiom, out truth, out untruth);
            //ArithmeticExpression.ExpressionUtil.Normalize(ref truth);//pak li be...
            //ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);

            status = mResults[row, column];

            axiomNumber = column + 1;
            if (!CheckBoxCorrespondingNegations.Checked)
            {
                axiomNumber = column / mUniqueNegations.Count + 1;
            }

            return true;
        }

		private void DataGridAxiomsResults_CurrentCellChanged(object sender, System.EventArgs e)
		{
			int currentRow = DataGridAxiomsResults.CurrentCell.RowNumber;
			int currentColumn = DataGridAxiomsResults.CurrentCell.ColumnNumber - 2;

            Implication implication;
            Negation negation;
            int axiomNumber;
            IFNode axiom;
            AxiomStatus axiomStatus;

            GetCellInfo(currentRow, currentColumn, out implication, out negation, out axiomNumber, out axiom, out axiomStatus);

            string status;
            switch (axiomStatus)
            {
                case AxiomStatus.PossiblySatisfied: status = "Possibly satisfied (no counterexample found)"; break;
                case AxiomStatus.ProvedNotSatisfied: status = "Not satisfied (proved)"; break;
                case AxiomStatus.ProvedSatisfied: status = "Satisfied (proved)"; break;
                default: status = "Not checked"; break;
            }

            IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
            IFExpressionUtil.ConvertToArithmetic(axiom, out truth, out untruth);
            ArithmeticExpression.ExpressionUtil.Normalize(ref truth);//pak li be...
            ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);

            //if (currentColumn < 0) return;
            //Implication implication = (Implication)mImplications[currentRow];
            //Negation negation = null;
            //IFNode axiom = null;
            //if (CheckBoxCorrespondingNegations.Checked)
            //{
            //    negation = (Negation)mNegations[currentRow];
            //    axiom = (IFNode)mAxioms[currentColumn];
            //}
            //else
            //{
            //    negation = (Negation)mUniqueNegations[currentColumn % mUniqueNegations.Count];
            //    axiom = (IFNode)mAxioms[currentColumn / mUniqueNegations.Count];
            //}
            //IFMain.SetImplicationAndNegation(axiom, implication, negation, mImplications, mNegations);
            //IFSTool.ArithmeticExpression.IArithmeticNode truth, untruth;
            //IFExpressionUtil.ConvertToArithmetic(axiom, out truth, out untruth);
            //ArithmeticExpression.ExpressionUtil.Normalize(ref truth);//pak li be...
            //ArithmeticExpression.ExpressionUtil.Normalize(ref untruth);
            //string status;
            //switch (mResults[currentRow, currentColumn])
            //{
            //    case AxiomStatus.PossiblySatisfied: status = "Possibly satisfied (no counterexample found)"; break;
            //    case AxiomStatus.ProvedNotSatisfied: status = "Not satisfied (proved)"; break;
            //    case AxiomStatus.ProvedSatisfied: status = "Satisfied (proved)"; break;
            //    default: status = "Not checked"; break;
            //}

            //int axiomNumber = currentColumn + 1;
            //if (!CheckBoxCorrespondingNegations.Checked)
            //{
            //    axiomNumber = currentColumn / mUniqueNegations.Count + 1;
            //}

			MessageBox.Show("Implication: " + implication.ToString() + Environment.NewLine
				+ "Negation: " + negation.ToString() + Environment.NewLine
				+ "Axiom: " + axiomNumber.ToString() + ": " + axiom.ToString() + Environment.NewLine
				+ "Arithmetic Expression After Normalization: "
				+ "<" + truth.ToString() + ",  "
				+ untruth.ToString() + ">" + Environment.NewLine
				+ "Status: " + status);
		}

		private void ButtonCheckMore_Click(object sender, System.EventArgs e)
		{
            //da se poopravi UI-to!

            if (! mCalculatingMore)
            {
                ButtonCheckMore.Text = "Abort";
                mCheckOnlyPossibleSatisfied = true;
                mCalculatingMore = true;
                CheckAxiomsAsync();
            }
            else 
            {
                if (mCalculationThread != null)
                    mCalculationThread.Abort();
                OnAxiomCheckFinished();
            }
        }

        private void AxiomsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mCalculationThread != null)
                mCalculationThread.Abort();
        }
	
        ////temp
        //private void BuildTableForPublication()
        //{
        //    mResults = new AxiomStatus[mImplications.Count, 5];
        //    for (int implicationIndex = 0; implicationIndex < mImplications.Count; implicationIndex++)
        //        for (int axiomIndex = 0; axiomIndex < 5; axiomIndex++)
        //        {
        //            bool isTautology = IFMain.CheckAxiom(
        //                (Implication)mImplications[implicationIndex],
        //                (Negation)mNegations[implicationIndex],
        //                (IFNode)mAxioms[axiomIndex >= 2 ? axiomIndex - 2 : axiomIndex],
        //                axiomIndex < 2);
        //            if (isTautology)
        //                mResults[implicationIndex, axiomIndex] = AxiomStatus.ProvedSatisfied;//
        //            else mResults[implicationIndex, axiomIndex] = AxiomStatus.ProvedNotSatisfied;
        //        }

        //    DrawTable();

        //    StreamWriter writer = new StreamWriter("a3table.txt");
        //    writer.WriteLine(@"\begin{tabular}{|l|l|c|c|c|c|c|} \hline \hline");
        //    writer.WriteLine(@"Negation & Implication & $A \supset \neg \neg A$ as IFT & $A \supset \neg \neg A$ & $\neg \neg A \supset A$ as IFT & $\neg \neg A \supset A$ & $\neg A = \neg \neg \neg A$ \\ \hline");
        //    using (writer)
        //    {
        //        DataTable table = (DataTable)DataGridAxiomsResults.DataSource;
        //        int cols = table.Columns.Count;

        //        DataRow[] rows = table.Select("", "Negation");

        //        foreach (DataRow row in rows)
        //        {
        //            writer.Write("${0}$ & $\\ri_{{{1}}}$", row[1].ToString(), ((string)row[0]).Substring(0, ((string)row[0]).IndexOf(':')));
        //            for (int i = 2; i < cols; ++i)
        //            {
        //                writer.Write("& {0}", row[i].ToString().Equals("-") ? "" : "+ ");
        //            }
        //            writer.WriteLine(@" \\ \hline");
        //        }
        //        writer.WriteLine(@"\hline \hline\end{tabular}");
        //    }
        //}
	}
}

