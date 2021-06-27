using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using IFSTool.IntFuzzyExpression;

namespace IFSTool
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : BaseForm
	{
		private System.Windows.Forms.Button ButtonReset;
		private System.Windows.Forms.Button ButtonExit;
		private System.Windows.Forms.Button ButtonAbout;
		private System.Windows.Forms.GroupBox GroupBoxImplicationsList;
		private System.Windows.Forms.Button ButtonImport;
		private System.Windows.Forms.Button ButtonDelete;
		private System.Windows.Forms.Button ButtonEdit;
		private System.Windows.Forms.Button ButtonAdd;
		private System.Windows.Forms.CheckedListBox CheckedListBoxImplicationsList;
		private System.Windows.Forms.TextBox TextBoxShots;
		private System.Windows.Forms.Label LabelStep;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.OpenFileDialog ImportImplicationsFileDialog;
		private System.Windows.Forms.GroupBox GroupBoxActions;
		private System.Windows.Forms.Button ButtonDrawGraph;
		private System.Windows.Forms.Button ButtonGenerateNegations;
		private System.Windows.Forms.Button ButtonGenerateDerivates;
		private System.Windows.Forms.Button ButtonRemoveDuplicates;
		private System.Windows.Forms.Button ButtonCheckAxioms;
		private System.Windows.Forms.Label LabelImplicationsCount;
		private System.Windows.Forms.Button ButtonCheckErrors;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
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
				if (components != null) 
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
            this.ButtonReset = new System.Windows.Forms.Button();
            this.ButtonExit = new System.Windows.Forms.Button();
            this.ButtonAbout = new System.Windows.Forms.Button();
            this.GroupBoxActions = new System.Windows.Forms.GroupBox();
            this.ButtonCheckAxioms = new System.Windows.Forms.Button();
            this.ButtonRemoveDuplicates = new System.Windows.Forms.Button();
            this.ButtonCheckErrors = new System.Windows.Forms.Button();
            this.ButtonGenerateDerivates = new System.Windows.Forms.Button();
            this.ButtonGenerateNegations = new System.Windows.Forms.Button();
            this.ButtonDrawGraph = new System.Windows.Forms.Button();
            this.TextBoxShots = new System.Windows.Forms.TextBox();
            this.LabelStep = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GroupBoxImplicationsList = new System.Windows.Forms.GroupBox();
            this.LabelImplicationsCount = new System.Windows.Forms.Label();
            this.CheckedListBoxImplicationsList = new System.Windows.Forms.CheckedListBox();
            this.ButtonImport = new System.Windows.Forms.Button();
            this.ButtonDelete = new System.Windows.Forms.Button();
            this.ButtonEdit = new System.Windows.Forms.Button();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ImportImplicationsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.GroupBoxActions.SuspendLayout();
            this.GroupBoxImplicationsList.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonSaveToFile
            // 
            this.ButtonSaveToFile.Location = new System.Drawing.Point(8, 336);
            this.ButtonSaveToFile.Click += new System.EventHandler(this.ButtonSaveToFile_Click);
            // 
            // ButtonReset
            // 
            this.ButtonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonReset.Location = new System.Drawing.Point(360, 288);
            this.ButtonReset.Name = "ButtonReset";
            this.ButtonReset.Size = new System.Drawing.Size(48, 23);
            this.ButtonReset.TabIndex = 7;
            this.ButtonReset.Text = "&Reset";
            this.ButtonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // ButtonExit
            // 
            this.ButtonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonExit.Location = new System.Drawing.Point(632, 336);
            this.ButtonExit.Name = "ButtonExit";
            this.ButtonExit.Size = new System.Drawing.Size(72, 23);
            this.ButtonExit.TabIndex = 62;
            this.ButtonExit.Text = "E&xit";
            this.ButtonExit.Click += new System.EventHandler(this.ButtonExit_Click);
            // 
            // ButtonAbout
            // 
            this.ButtonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAbout.Location = new System.Drawing.Point(552, 336);
            this.ButtonAbout.Name = "ButtonAbout";
            this.ButtonAbout.Size = new System.Drawing.Size(72, 23);
            this.ButtonAbout.TabIndex = 61;
            this.ButtonAbout.Text = "About";
            // 
            // GroupBoxActions
            // 
            this.GroupBoxActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxActions.Controls.Add(this.ButtonCheckAxioms);
            this.GroupBoxActions.Controls.Add(this.ButtonRemoveDuplicates);
            this.GroupBoxActions.Controls.Add(this.ButtonCheckErrors);
            this.GroupBoxActions.Controls.Add(this.ButtonGenerateDerivates);
            this.GroupBoxActions.Controls.Add(this.ButtonGenerateNegations);
            this.GroupBoxActions.Controls.Add(this.ButtonDrawGraph);
            this.GroupBoxActions.Controls.Add(this.TextBoxShots);
            this.GroupBoxActions.Controls.Add(this.LabelStep);
            this.GroupBoxActions.Controls.Add(this.label1);
            this.GroupBoxActions.Location = new System.Drawing.Point(430, 8);
            this.GroupBoxActions.Name = "GroupBoxActions";
            this.GroupBoxActions.Size = new System.Drawing.Size(272, 320);
            this.GroupBoxActions.TabIndex = 20;
            this.GroupBoxActions.TabStop = false;
            this.GroupBoxActions.Text = "Select an Action to Perform:";
            // 
            // ButtonCheckAxioms
            // 
            this.ButtonCheckAxioms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCheckAxioms.Location = new System.Drawing.Point(8, 208);
            this.ButtonCheckAxioms.Name = "ButtonCheckAxioms";
            this.ButtonCheckAxioms.Size = new System.Drawing.Size(256, 23);
            this.ButtonCheckAxioms.TabIndex = 27;
            this.ButtonCheckAxioms.Text = "Check axioms...";
            this.ButtonCheckAxioms.Click += new System.EventHandler(this.ButtonCheckAxioms_Click);
            // 
            // ButtonRemoveDuplicates
            // 
            this.ButtonRemoveDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonRemoveDuplicates.Location = new System.Drawing.Point(8, 144);
            this.ButtonRemoveDuplicates.Name = "ButtonRemoveDuplicates";
            this.ButtonRemoveDuplicates.Size = new System.Drawing.Size(256, 23);
            this.ButtonRemoveDuplicates.TabIndex = 25;
            this.ButtonRemoveDuplicates.Text = "Remove duplicates";
            this.ButtonRemoveDuplicates.Click += new System.EventHandler(this.ButtonRemoveDuplicates_Click);
            // 
            // ButtonCheckErrors
            // 
            this.ButtonCheckErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonCheckErrors.Location = new System.Drawing.Point(8, 112);
            this.ButtonCheckErrors.Name = "ButtonCheckErrors";
            this.ButtonCheckErrors.Size = new System.Drawing.Size(256, 23);
            this.ButtonCheckErrors.TabIndex = 24;
            this.ButtonCheckErrors.Text = "Check implications for errors";
            this.ButtonCheckErrors.Visible = false;
            this.ButtonCheckErrors.Click += new System.EventHandler(this.ButtonCheckErrors_Click);
            // 
            // ButtonGenerateDerivates
            // 
            this.ButtonGenerateDerivates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonGenerateDerivates.Location = new System.Drawing.Point(8, 80);
            this.ButtonGenerateDerivates.Name = "ButtonGenerateDerivates";
            this.ButtonGenerateDerivates.Size = new System.Drawing.Size(256, 23);
            this.ButtonGenerateDerivates.TabIndex = 23;
            this.ButtonGenerateDerivates.Text = "Generate derivative implications";
            this.ButtonGenerateDerivates.Click += new System.EventHandler(this.ButtonGenerateDerivates_Click);
            // 
            // ButtonGenerateNegations
            // 
            this.ButtonGenerateNegations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonGenerateNegations.Location = new System.Drawing.Point(8, 48);
            this.ButtonGenerateNegations.Name = "ButtonGenerateNegations";
            this.ButtonGenerateNegations.Size = new System.Drawing.Size(256, 23);
            this.ButtonGenerateNegations.TabIndex = 22;
            this.ButtonGenerateNegations.Text = "View Negations";
            this.ButtonGenerateNegations.Click += new System.EventHandler(this.ButtonGenerateNegations_Click);
            // 
            // ButtonDrawGraph
            // 
            this.ButtonDrawGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonDrawGraph.Location = new System.Drawing.Point(8, 176);
            this.ButtonDrawGraph.Name = "ButtonDrawGraph";
            this.ButtonDrawGraph.Size = new System.Drawing.Size(256, 23);
            this.ButtonDrawGraph.TabIndex = 26;
            this.ButtonDrawGraph.Text = "Draw graph of the relation \"less than or equal\"";
            this.ButtonDrawGraph.Click += new System.EventHandler(this.ButtonDrawGraph_Click);
            // 
            // TextBoxShots
            // 
            this.TextBoxShots.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TextBoxShots.Location = new System.Drawing.Point(8, 256);
            this.TextBoxShots.Name = "TextBoxShots";
            this.TextBoxShots.Size = new System.Drawing.Size(32, 20);
            this.TextBoxShots.TabIndex = 49;
            this.TextBoxShots.Text = "0";
            this.TextBoxShots.TextChanged += new System.EventHandler(this.TextBoxShots_TextChanged);
            // 
            // LabelStep
            // 
            this.LabelStep.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LabelStep.Location = new System.Drawing.Point(152, 256);
            this.LabelStep.Name = "LabelStep";
            this.LabelStep.Size = new System.Drawing.Size(112, 13);
            this.LabelStep.TabIndex = 51;
            this.LabelStep.Text = "LabelStep";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.Location = new System.Drawing.Point(40, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "shots in [0,1] with step";
            // 
            // GroupBoxImplicationsList
            // 
            this.GroupBoxImplicationsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBoxImplicationsList.Controls.Add(this.LabelImplicationsCount);
            this.GroupBoxImplicationsList.Controls.Add(this.CheckedListBoxImplicationsList);
            this.GroupBoxImplicationsList.Controls.Add(this.ButtonImport);
            this.GroupBoxImplicationsList.Controls.Add(this.ButtonDelete);
            this.GroupBoxImplicationsList.Controls.Add(this.ButtonEdit);
            this.GroupBoxImplicationsList.Controls.Add(this.ButtonAdd);
            this.GroupBoxImplicationsList.Controls.Add(this.ButtonReset);
            this.GroupBoxImplicationsList.Location = new System.Drawing.Point(8, 8);
            this.GroupBoxImplicationsList.Name = "GroupBoxImplicationsList";
            this.GroupBoxImplicationsList.Size = new System.Drawing.Size(416, 320);
            this.GroupBoxImplicationsList.TabIndex = 0;
            this.GroupBoxImplicationsList.TabStop = false;
            this.GroupBoxImplicationsList.Text = "Intuitionistic Fuzzy Implications:";
            // 
            // LabelImplicationsCount
            // 
            this.LabelImplicationsCount.Location = new System.Drawing.Point(168, 0);
            this.LabelImplicationsCount.Name = "LabelImplicationsCount";
            this.LabelImplicationsCount.Size = new System.Drawing.Size(40, 16);
            this.LabelImplicationsCount.TabIndex = 8;
            this.LabelImplicationsCount.Text = "label2";
            // 
            // CheckedListBoxImplicationsList
            // 
            this.CheckedListBoxImplicationsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckedListBoxImplicationsList.Location = new System.Drawing.Point(8, 16);
            this.CheckedListBoxImplicationsList.Name = "CheckedListBoxImplicationsList";
            this.CheckedListBoxImplicationsList.Size = new System.Drawing.Size(400, 259);
            this.CheckedListBoxImplicationsList.TabIndex = 1;
            // 
            // ButtonImport
            // 
            this.ButtonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonImport.Location = new System.Drawing.Point(64, 288);
            this.ButtonImport.Name = "ButtonImport";
            this.ButtonImport.Size = new System.Drawing.Size(104, 23);
            this.ButtonImport.TabIndex = 3;
            this.ButtonImport.Text = "&Import from File...";
            this.ButtonImport.Click += new System.EventHandler(this.ButtonImport_Click);
            // 
            // ButtonDelete
            // 
            this.ButtonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonDelete.Location = new System.Drawing.Point(232, 288);
            this.ButtonDelete.Name = "ButtonDelete";
            this.ButtonDelete.Size = new System.Drawing.Size(48, 23);
            this.ButtonDelete.TabIndex = 5;
            this.ButtonDelete.Text = "&Delete";
            this.ButtonDelete.Visible = false;
            this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // ButtonEdit
            // 
            this.ButtonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonEdit.Location = new System.Drawing.Point(176, 288);
            this.ButtonEdit.Name = "ButtonEdit";
            this.ButtonEdit.Size = new System.Drawing.Size(48, 23);
            this.ButtonEdit.TabIndex = 4;
            this.ButtonEdit.Text = "&Edit...";
            this.ButtonEdit.Visible = false;
            this.ButtonEdit.Click += new System.EventHandler(this.ButtonEdit_Click);
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonAdd.Location = new System.Drawing.Point(8, 288);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(48, 23);
            this.ButtonAdd.TabIndex = 2;
            this.ButtonAdd.Text = "&Add...";
            this.ButtonAdd.Visible = false;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.ButtonExit;
            this.ClientSize = new System.Drawing.Size(712, 368);
            this.Controls.Add(this.ButtonAbout);
            this.Controls.Add(this.ButtonExit);
            this.Controls.Add(this.GroupBoxActions);
            this.Controls.Add(this.GroupBoxImplicationsList);
            this.Name = "MainForm";
            this.ShowInTaskbar = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.ButtonSaveToFile, 0);
            this.Controls.SetChildIndex(this.GroupBoxImplicationsList, 0);
            this.Controls.SetChildIndex(this.GroupBoxActions, 0);
            this.Controls.SetChildIndex(this.ButtonExit, 0);
            this.Controls.SetChildIndex(this.ButtonAbout, 0);
            this.GroupBoxActions.ResumeLayout(false);
            this.GroupBoxActions.PerformLayout();
            this.GroupBoxImplicationsList.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.Run(new MainForm());
		}

        private ArrayList implicationsList;

		private void MainForm_Load(object sender, System.EventArgs e)
		{
			ReadImplicationsList();
			RebindControls();
			SelectAllImplications();
		}

		private void RebindControls()
		{
			RebindControlsImplications();
			RebindControlsShots();
		}

		private void RebindControlsShots()
		{
			TextBoxShots.DataBindings.Clear();
			TextBoxShots.DataBindings.Add(new Binding("Text", IFMain.Shots, ""));
			LabelStep.Text = (1.0/(IFMain.Shots - 1)).ToString();
		}

		private void RebindControlsImplications()
		{
			((ListBox)CheckedListBoxImplicationsList).DataSource = null;
			((ListBox)CheckedListBoxImplicationsList).DataSource = implicationsList;//egati castvaneto trqbva6e!
			LabelImplicationsCount.Text = '(' + implicationsList.Count.ToString() + ')';
			SelectAllImplications();
		}

		private void TextBoxShots_TextChanged(object sender, System.EventArgs e)
		{
			int shots;
			try
			{
				shots = Int32.Parse(TextBoxShots.Text);
                if (shots < 2)
                {
                    shots = IFMain.SHOTS;
                }
			}
			catch (FormatException)
			{
				shots = IFMain.SHOTS;
			}
			IFMain.Shots = shots;
			RebindControlsShots();
		}

		private void ButtonExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ButtonAdd_Click(object sender, System.EventArgs e)
		{
			EditImplicationForm editImplicationForm = new EditImplicationForm(implicationsList);
			DialogResult result = editImplicationForm.ShowDialog();
			editImplicationForm.Dispose();
			if (result == DialogResult.OK)
			{
				//tuk ili drugade da proverqva da ne savpada po ime s nqkoq sa6testvuva6ta
				 //(osobeno s tiq ot faila, dori i da sa iztriti ot lista!) (ili 1:1 po izraz)
				//refresh list
			}
		}

		private void ButtonEdit_Click(object sender, System.EventArgs e)
		{
			//dali da e pozvoleno i bazovite (i tehnite proizvodni) da se redaktirat?
			//da ne puska nevalidni implikacii!
		}

		private void ButtonReset_Click(object sender, System.EventArgs e)
		{
			ReadImplicationsList();
			RebindControlsImplications();
		}

		private void ReadImplicationsList()
		{
			implicationsList = IFMain.ReadImplicationsFromFile("Implications.txt");//polzvai App.config za imeto
		}

		private void SelectAllImplications()
		{
			for (int i=0; i < CheckedListBoxImplicationsList.Items.Count; i++ )
			{
				CheckedListBoxImplicationsList.SetItemChecked(i, true);
			} 
		}

		private void ButtonImport_Click(object sender, System.EventArgs e)
		{
			ImportImplicationsFileDialog = new OpenFileDialog();
			ImportImplicationsFileDialog.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
			ImportImplicationsFileDialog.Title = "Import Implications from File";
			ImportImplicationsFileDialog.ShowDialog();
			if (ImportImplicationsFileDialog.FileName != "")
			{
				ArrayList importedImplications = IFMain.ReadImplicationsFromFile(ImportImplicationsFileDialog.FileName);
				//RemoveInvalid(ref importedImplications);
				//da pokazva koi sa nevalidni!
				//da ne moje da se importvat impl sas "zapazeni" indexi (1, 29, 2,1 ...)
				implicationsList.AddRange(importedImplications);
				//da, ama ne zapazva koi sa selected items!
				RebindControlsImplications();
			}
			ImportImplicationsFileDialog.Dispose();
		}

		private void ButtonSaveToFile_Click(object sender, System.EventArgs e)
		{
			SaveDataToFileDialog = new SaveFileDialog();
            SaveDataToFileDialog.Filter = "Text files (*.txt)|*.txt|TeX files (*.tex)|*.tex";
			SaveDataToFileDialog.ShowDialog();
			if (SaveDataToFileDialog.FileName != "")
			{
				StreamWriter writer = new StreamWriter(SaveDataToFileDialog.FileName);
				using (writer)
				{
                    if (SaveDataToFileDialog.FilterIndex == 1)
                    {
                        foreach (Implication implication in implicationsList)
                            writer.WriteLine(implication);
                    }
                    else if (SaveDataToFileDialog.FilterIndex == 2)
                    {
                        writer.WriteLine(@"\begin{tabular}{|l|l|} \hline \hline");
                        foreach (Implication implication in implicationsList)
                        {
                            string s = implication.ToString();
                            s = @"$\ri_{" + s.Replace(": ", "}$\t& $\\langle ").
                                Replace("sg",@"\sg").
                                Replace("os",@"\os").
                                Replace("min",@"\min").
                                Replace("max",@"\max")
                                + @"\rangle$ \\ \hline";
                            writer.WriteLine(s);
                        }
                        writer.WriteLine(@"\end{tabular}");
                    }
				}
			}
			SaveDataToFileDialog.Dispose();
		}

//tazi proverka se pravi s oniq aksiomi
//		private void RemoveInvalid(ref ArrayList aImplications)
//		{
//			//be po-dobre da ima prozorec, v koito da napi6e koi sa mahnati! i palen ot4et!
//			for (int index = 0; index < aImplications.Count; index++)
//			{
//				if (!(IFMain.IsPossiblyIFImplication((Implication)(aImplications[index]))))
//				{
//					aImplications.RemoveAt(index);
//					index--;
//				}
//			}
//		}

//		private IList GetSelectedImplications()
//		{
//			IList selectedImpl = new ArrayList();
//			for (int i = 0; i < implicationsList.Count; i++)
//			{
//				if (CheckedListBoxImplicationsList.GetItemChecked(i))
//					selectedImpl.Add(implicationsList[i]);
//			}
//			return selectedImpl;
//		}

		private void ButtonGenerateNegations_Click(object sender, System.EventArgs e)
		{
			//posle da pita dali da generira i implikacii ot tiq otricaniq ?
			NegationsForm form = new NegationsForm(CheckedListBoxImplicationsList.CheckedItems);
			form.ShowDialog();
			form.Dispose();
		}

		private void ButtonGenerateDerivates_Click(object sender, System.EventArgs e)
		{
			IList checkedImpl = CheckedListBoxImplicationsList.CheckedItems;
			IList newImpl = new ArrayList();
			int implCount = checkedImpl.Count;
			foreach (Implication impl in checkedImpl)
			{
				Implication generated = IFMain.GenerateImplication2From(impl);
				newImpl.Add(generated);
			}
			foreach (Implication impl in checkedImpl)
			{
				Implication generated = IFMain.GenerateImplication3From(impl);
				newImpl.Add(generated);
			}
			implicationsList.AddRange(newImpl);
			foreach (Implication impl in CheckedListBoxImplicationsList.CheckedItems)
			{
				implicationsList.Add(IFMain.GenerateImplicationSubstFrom(impl));
			}
			foreach (Implication impl in newImpl)
			{
				implicationsList.Add(IFMain.GenerateImplicationSubstFrom(impl));
			}
			
			RebindControlsImplications();
		}

		private void ButtonCheckErrors_Click(object sender, System.EventArgs e)
		{
            //TODO: preobrazuvai tova v axioma

			//mk win, v koito da izliza spisak s lo6ite i da pita "Da gi triem li?"
			//da ima i checkbox "Do not treat <a, a> like true, nor like false"
			for (int index = 0; index < implicationsList.Count; index++)
			{
				if (CheckedListBoxImplicationsList.GetItemChecked(index) //ne polzvame GetChecked...
					&& !(IFMain.IsPossiblyTrueIFImplication((Implication)(implicationsList[index]), 
					true)))//tova true da se smeni s CheckBox...
				{
					implicationsList.RemoveAt(index);//tva triene posle!
					index--;//tova 6te se osere 100%! gledame GetItemChecked po sa6tiq nevaliden index!
				}
			}
			RebindControlsImplications();
		}

		private void ButtonRemoveDuplicates_Click(object sender, System.EventArgs e)
		{
            if (CheckedListBoxImplicationsList.CheckedItems.Count > 0)
            {
                //po dqvolite, trie not cheked!
                IList identicalSets = IFMain.GetIdenticalSets(CheckedListBoxImplicationsList.CheckedItems);

                IdenticalImplicationsForm form = new IdenticalImplicationsForm(identicalSets);
                form.ShowDialog();
                DialogResult result = form.DialogResult;
                bool renumber = form.ReNumberImplications;
                form.Dispose();
                if (result == DialogResult.OK)
                {
                    IFMain.RemoveIdenticalImplications(ref implicationsList, identicalSets);

                    if (renumber)
                    {
                        for (int i = 0; i < implicationsList.Count; ++i)
                            ((Implication)implicationsList[i]).Name = (i + 1).ToString();
                    }

                    RebindControlsImplications();
                }
            }
            else
            {
                MessageBox.Show("Please import implications first.", "IFSTool");
            }
		}

		private void ButtonDrawGraph_Click(object sender, System.EventArgs e)
		{
            if (CheckedListBoxImplicationsList.CheckedItems.Count > 0)
            {

                //!!!ne trqbva da ima povtarq6ti se implikacii!!!!!!!!!!!!!

                Graph graph = IFMain.GetImplicationsGraph(new ArrayList(CheckedListBoxImplicationsList.CheckedItems));

                DrawGraphForm drawGraphForm = new DrawGraphForm(graph);
                drawGraphForm.ShowDialog();
                drawGraphForm.Dispose();
                //Process.Start("IFImplicationsGraph.png");
            }
            else
            {
                MessageBox.Show("Please import implications first.", "IFSTool");
            }
		}

		private void ButtonCheckAxioms_Click(object sender, System.EventArgs e)
		{
            if (CheckedListBoxImplicationsList.CheckedItems.Count > 0)
            {

                ArrayList implications = new ArrayList(CheckedListBoxImplicationsList.CheckedItems);
                AxiomsForm axiomsForm = new AxiomsForm(implications);
                DialogResult result = axiomsForm.ShowDialog();
                axiomsForm.Dispose();
                //			if (result == DialogResult.OK)
                //			{
                //				//tuk ili drugade da proverqva da ne savpada po ime s nqkoq sa6testvuva6ta
                //				//(osobeno s tiq ot faila, dori i da sa iztriti ot lista!) (ili 1:1 po izraz)
                //				//refresh list
                //			}
                //...
                for (int i = 0; i < CheckedListBoxImplicationsList.Items.Count; i++)
                    CheckedListBoxImplicationsList.SetItemChecked(i, false);
                foreach (Implication impl in implications)
                {
                    int index = CheckedListBoxImplicationsList.Items.IndexOf(impl);
                    CheckedListBoxImplicationsList.SetItemChecked(index, true);
                }
                //RebindControls(); nedei
            }
            else
            {
                MessageBox.Show("Please import implications first.", "IFSTool");
            }
		}

		private void ButtonDelete_Click(object sender, System.EventArgs e)
		{
//			foreach (Object o in CheckedListBoxImplicationsList.CheckedItems)
//				implicationsList.Remove(o);
//			RebindControlsImplications();
			//pravi problemi!
		}
	}
}
