using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using IFSTool.IntFuzzyExpression;

namespace IFSTool
{
	public interface IGraphicObject
	{
		void Draw(Graphics aGraphics, bool aPrintable);
	}

	public class Rib : IGraphicObject
	{
		private Vertex mVertex1; //sux
		private Vertex mVertex2;
		
		private bool mDisabled; //taq tapotiq e zaradi Hasse

		public Rib(Vertex aVertex1, Vertex aVertex2)
		{
			mVertex1 = aVertex1;
			mVertex2 = aVertex2;
			mDisabled = false;
		}

		public Vertex Vertex1
		{
			get
			{
				return mVertex1;
			}
		}

		public Vertex Vertex2
		{
			get
			{
				return mVertex2;
			}
		}

		public bool Disabled
		{
			get
			{
				return mDisabled;
			}
			set
			{
				mDisabled = value;
			}
		}

		public void Draw(Graphics aGraphics, bool aPrintable)
		{
			Pen pen;
            float width = 1f;
            if (aPrintable)
                width = 2f;
			if (!mDisabled)
				pen = new Pen(Color.FromArgb(240, Color.Black), width);
			else
				pen = new Pen(Color.FromArgb(48, Color.Black), width);//moje6e i svetlosivo s po-malka prozra4nost, ama taka ne capa varhu 4ernite
			Point point1 = new Point(mVertex1.X, mVertex1.Y);
			Point point2 = new Point(mVertex2.X, mVertex2.Y);
			aGraphics.DrawLine(pen, point1, point2);
			pen.Dispose();
		}
	}

	public class Vertex : IGraphicObject
	{
		public Vertex(Implication aImplication, int aIndex)
		{
			mImplication = aImplication;
			mX = 0;
			mY = 0;
			mIndex = aIndex;
		}

		private Implication mImplication;
		private int mX;
		private int mY;
		private int mIndex; //tova e nujno zasega, inak trqbva da prenapisvam vsi4ko

		public int Index
		{
			get
			{
				return mIndex;
			}
		}

		public Implication CorrespondingImplication //ama 4e ime!
		{
			get
			{
				return mImplication;
			}
			/*set //nqma kak, 6tom e masiv
			{
				mImplication = value;
			}*/
		}

		public int X
		{
			get
			{
				return mX;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				else
				{
					mX = value;
				}
			}
		}
		
		public int Y
		{
			get
			{
				return mY;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				else
				{
					mY = value;
				}
			}
		}
		
		public void Draw(Graphics aGraphics, bool aPrintable)
		{
			Font font;
			SolidBrush brush;
            SolidBrush textBrush;
//			if (mIndex < 29) //lo6o
//				brush = new SolidBrush(Color.FromArgb(232, 48 - 28 + mIndex%29, 48 - 28 + mIndex%29, 255 - 28 + mIndex%29));//leko kogato tova 29 stane pove4e!!!
//			else if (mIndex < 29 * 3)
//				brush = new SolidBrush(Color.FromArgb(232, (32 - 28 + mIndex%29)/2, 112 - 28 + mIndex%29, 224 - 28 + mIndex%29));
//			else
//				brush = new SolidBrush(Color.FromArgb(232, 96 - 28 + mIndex%29, 144 - 28 + mIndex%29, 240 - 28 + mIndex%29));
			//zasega:
            if (!aPrintable)
            {
                brush = new SolidBrush(Color.FromArgb(232, 48, 48, 255));
                //brush = new SolidBrush(Color.FromArgb(232, mIndex%176 / 2, mIndex%176 / 2, 255 - 176/2 + mIndex%176 / 2));
                textBrush = new SolidBrush(Color.FromArgb(240, 255, 32));
                aGraphics.FillEllipse(brush, mX - 20, mY - 12, 40, 24);
                int leftOffset = -18;
                //if (mIndex < 29) leftOffset += 6;
                font = new Font("Tahoma", 12);
                aGraphics.DrawString(this.CorrespondingImplication.Name, font, textBrush, mX + leftOffset, mY - 10);
            }
            else
            {
                brush = new SolidBrush(Color.FromArgb(180, 200, 200, 200));
                textBrush = new SolidBrush(Color.Black);
                aGraphics.FillEllipse(brush, mX - 50, mY - 30, 100, 60);
                Pen pen = new Pen(Color.Black, 1f);
                aGraphics.DrawEllipse(pen, mX - 50, mY - 30, 100, 60);
                font = new Font("Tahoma", 30, FontStyle.Bold);
                int leftOffset = -32;
                if (this.CorrespondingImplication.Name.Length == 3) leftOffset -= 14; //breiii
                aGraphics.DrawString(this.CorrespondingImplication.Name, font, textBrush, mX + leftOffset, mY - 25);
            }
			textBrush.Dispose();
			brush.Dispose();
			font.Dispose();
		}
	}

	public class Graph : IGraphicObject //be to ne e ba6 k'uv da e graph, ama are; to e reflexivna tranzitivna a(nti)simetri4na (acikli4na) relaciq
	{
		//Hashtables
		private Vertex[] mVertexes;
		private ArrayList mRibs;
		private Size mSize;

		public Vertex[] Vertexes
		{
			get
			{
				return mVertexes;
			}
			set
			{
				mVertexes = value;
			}
		}

		public ArrayList Ribs
		{
			get
			{
				return mRibs;
			}
			set
			{
				mRibs = value;
			}
		}

		public Graph(int aVertexesCount)//bool aPrintable
		{
			mVertexes = new Vertex[aVertexesCount];
			mRibs = new ArrayList(aVertexesCount * 10); //prili4na stoinost?
			mSize = new Size(64, 64); //tova da sa konstanti nqkade!
		}

		public Graph(ArrayList aImplications, ArrayList aRibs)
		{
			mVertexes = new Vertex[aImplications.Count];
			for (int i = 0; i < aImplications.Count; i++)
			{
//				if (aImplications[i] as Implication == null) //dava nek'uv bug
//					throw new ArgumentException();
				mVertexes[i] = new Vertex((Implication)(aImplications[i]), i);
			}

			//type check za ribs...
			mRibs = aRibs;
			mSize = new Size(64, 64); //tova da sa konstanti nqkade!
		}

		public bool ExistRib(int aVertex1, int aVertex2)
		{
			//polzvai binsearch!!! ama vnimavai
			for (int i = 0; i < mRibs.Count && ((Rib)(mRibs[i])).Vertex1.Index <= aVertex1; i++)
			{
				if (((Rib)(Ribs[i])).Vertex1.Index == aVertex1)
				{
					for (int j = i; j < mRibs.Count && ((Rib)(mRibs[j])).Vertex1.Index == aVertex1; j++)
					{
						if (((Rib)(mRibs[j])).Vertex2.Index == aVertex2)
						{
							return true;
							//return ((Rib)(mRibs[j])).Disabled;
						}
					}
					break;
				}
			}
			return false;
		}
		
		public IList GetLinkedComponents() //dali se kazva taka?
		{
			IList result = new ArrayList(10);

			bool [] notSupremums = new bool[mVertexes.Length];
			int[] vertexBelongs = new int[mVertexes.Length];//koi vrah kam koq svarzana komponenta prinadleji; ama 4e lo6o ime
			foreach (Rib rib in mRibs)
			{
				notSupremums[rib.Vertex1.Index] = true;
			}
			int currentComponent = 1;
			for (int i = 0; i < mVertexes.Length; i++)
			{
				if (!notSupremums[i])
				{
					int firstVertex = -1;
					for (int j = 0; j < mVertexes.Length; j++)
					{
						if (vertexBelongs[j] == 0)
						{
							vertexBelongs[j] = currentComponent;
							firstVertex = i;//uffff, tuk vsi4ko se obarka, koe e i, koe e j...
							break;
						}
					}
					if (firstVertex == -1)
					{
						break;
					}
					//obhojdai dokato moje - vseki dostignat vrah stava s komp. currentCom...
					//ako oba4e stignem vrah s ve4e opredelena komponenta, to zna4i trqbva da smenim vsi4ko ot sega6nata s tazi opredelenata
					currentComponent++;
				}
			}
			//................
			//zasega:
			result.Add(this);
			return result;
		}

		public void PrepareHasse() //optimizirai q!
		{
			//int counta = 0;
			for (int i = 0; i < mRibs.Count; i++)
			{
				int leftVertex = ((Rib)(mRibs[i])).Vertex1.Index;
				int middleVertex = ((Rib)(mRibs[i])).Vertex2.Index;
				int middleVertexPosition = -1;
				//s binsearch!!! nameri kade po4vat tiq
				int m;
				for (int j = 0; j < mRibs.Count && (m = ((Rib)(mRibs[j])).Vertex1.Index) <= middleVertex; j++)
				{
					//if (((Rib)(mRibs[j])).Vertex1.Index == middleVertex)
					if (m == middleVertex)
					{
						middleVertexPosition = j;
						break;
					}
				}
				if (middleVertexPosition >= 0)
				{
					for (int j = 0;//middleVertexPosition; 
						j < mRibs.Count && ((Rib)(mRibs[j])).Vertex1.Index <= middleVertex; j++)
					{
						int rightVertex = ((Rib)(mRibs[j])).Vertex2.Index;
						//sega pak s binsearch!!! nameri ima li rebro m/u left i middle
						if(((Rib)(mRibs[j])).Vertex1.Index == middleVertex)//vremenno
						for (int k = 0;//i + 1; 
							k < mRibs.Count && ((Rib)(mRibs[k])).Vertex1.Index <= leftVertex; k++)
						{
							if (((Rib)(mRibs[k])).Vertex1.Index == leftVertex//taka e bavno, no 6te deistva
								&& ((Rib)(mRibs[k])).Vertex2.Index == rightVertex)
							{
								//if(((Rib)(mRibs[k])).Disabled == false) counta++;
								((Rib)(mRibs[k])).Disabled = true;
								break;
							}
						}
					}
				}
			}
		}

		private ArrayList GetRows()
		{
			ArrayList result = new ArrayList();//ArrayList of ArrayList of int - za teku6tata komponenta

			bool[] marked = new bool[mVertexes.Length];
			bool[] current = new bool[mVertexes.Length];//lo6i imena!
			bool flag = true;

			int currentRowNumber = 0;
			bool directionDown = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < current.Length; i++)
					current[i] = marked[i];
				foreach (Rib rib in mRibs)
				{
					int leftIndex = rib.Vertex1.Index;
					int rightIndex = rib.Vertex2.Index;
					if (directionDown)
					{
						if ((flag || !current[leftIndex]) && !marked[rightIndex])
						{
							current[leftIndex] = true;//tova e rabotata, drugoto e za da znae imalo li e promqna ili ne
							flag = true;
						}
					}
					else
					{
						if ((flag || !current[rightIndex]) && !marked[leftIndex])
						{
							current[rightIndex] = true;//tova e rabotata, drugoto e za da znae imalo li e promqna ili ne
							flag = true;
						}
					}
				}

				ArrayList currentVertexes = new ArrayList();
				for (int i = 0; i < mVertexes.Length; i++)
				{
					if (!current[i])
					{
						currentVertexes.Add(i);
						marked[i] = true;
					}
				}
				if (currentVertexes.Count > 0)
				{
					if (!directionDown)
                        result.Insert(currentRowNumber, currentVertexes);
					else
						result.Insert(result.Count - currentRowNumber, currentVertexes);
				}
				if (directionDown)
					currentRowNumber++;
				directionDown = !directionDown;
			}

			return result;
		}

		public void PrepareDraw(bool hasse, out Size aSize, bool aPrintable)
		{
			int vertexWidth = 80;
			int vertexHeight = 80;
            if (aPrintable)
            {
                vertexWidth = vertexHeight = 200;
            }

			aSize = new Size(vertexWidth, vertexHeight);

			ArrayList matrix = new ArrayList(); // ArrayList of ArrayList of int

			foreach (Rib rib in Ribs) //trqbva, 6toto nali pre4ertavame formata
			{
				rib.Disabled = false;
			}

			IList linkedComponents = GetLinkedComponents(); //ne vzema kopie, samite varhove sa!
			foreach (Graph graph in linkedComponents)//ne znam tova li e nai-dobroto; probvai mk pokriva6to darvo.
			{
				if (hasse)
					graph.PrepareHasse();

				ArrayList localMatrix = graph.GetRows();//ArrayList of ArrayList of int - za teku6tata komponenta

				//sega da gi popodredim!!! dai po-dobar algoritam! probvai daje parvo gornata polovina, posle dolnata - v sredata vinagi sa malko, taka 4e nqma stra6no
				int maxLocalRowLength = 0;//pri graph s po4ti nikvi rebra tova ne se promenq!!!
				for (int currentRowNumber = 1; currentRowNumber < localMatrix.Count/2/*zasega*/; currentRowNumber++)
				{
					ArrayList currentRowSorted = new ArrayList();
					ArrayList currentVertexes = (ArrayList)(localMatrix[currentRowNumber]);
					int previousRowLength = ((ArrayList)(localMatrix[currentRowNumber - 1])).Count;
					for (int i = 0; i < previousRowLength; i++)
					{
						int vertex2 = (int)(((ArrayList)(localMatrix[currentRowNumber - 1]))[i]);
						for (int j = 0; j < currentVertexes.Count; j++)
						{
							int v = (int)(currentVertexes[j]);
							if (ExistRib(v, vertex2))//da, ama i tova ne ba4ka dobre!
							{
								//tova dolnoto samo ako v ne e svarzano ve4e s drug vrah!
								/*for (int k = 0; k < i-j; k++)
									{
										currentRowSorted.Add(-1);//daje po-skoro trqbva da se butat gornite redove, ama stava slojno!
									}*/
								currentRowSorted.Add(v);
								currentVertexes.Remove(v);
								j--;//grozno, no vajno
							}
						}
					}
					foreach (int v in currentVertexes)//tiq, deto ostanaha!
					{
						//!!tqh gi napahai kadeto ima svobodno mqsto (-1)!
						currentRowSorted.Add(v);
					}
					localMatrix[currentRowNumber] = currentRowSorted;
					if (currentRowSorted.Count > maxLocalRowLength)
						maxLocalRowLength = currentRowSorted.Count;
				}
				//dolnoto - zasega
				for (int currentRowNumber = localMatrix.Count-2; currentRowNumber > localMatrix.Count-localMatrix.Count/2/*zasega*/; currentRowNumber--)
				{
					ArrayList currentRowSorted = new ArrayList();
					ArrayList currentVertexes = (ArrayList)(localMatrix[currentRowNumber]);
					int previousRowLength = ((ArrayList)(localMatrix[currentRowNumber + 1])).Count;
					for (int i = 0; i < previousRowLength; i++)
					{
						int vertex2 = (int)(((ArrayList)(localMatrix[currentRowNumber + 1]))[i]);
						for (int j = 0; j < currentVertexes.Count; j++)
						{
							int v = (int)(currentVertexes[j]);
							if (ExistRib(vertex2, v))//da, ama i tova ne ba4ka dobre!
							{
								//tova dolnoto samo ako v ne e svarzano ve4e s drug vrah!
								/*for (int k = 0; k < i-j; k++)
									{
										currentRowSorted.Add(-1);//daje po-skoro trqbva da se butat gornite redove, ama stava slojno!
									}*/
								currentRowSorted.Add(v);
								currentVertexes.Remove(v);
								j--;//grozno, no vajno
							}
						}
					}
					foreach (int v in currentVertexes)//tiq, deto ostanaha!
					{
						//!!tqh gi napahai kadeto ima svobodno mqsto (-1)!
						currentRowSorted.Add(v);
					}
					localMatrix[currentRowNumber] = currentRowSorted;
					if (currentRowSorted.Count > maxLocalRowLength)
						maxLocalRowLength = currentRowSorted.Count;
				}
				
				//vmesto raztqgane - centrirane - linked-components-safe :)
				//posle go napravi s razrejdane s -1-ci!
				foreach (ArrayList row in localMatrix)
				{
					int n = row.Count;
					for (int i = 0; i < (maxLocalRowLength - n)/2; i++)
						row.Insert(0, -1);
				}

				//!!!dobavi localMatrix kam ob6tata
				matrix = localMatrix;//!!!zasega, posle ne!!!

				//size = ne6to sprqmo staroto i graph.size;
			}

            ////vremenna boza:
            //if (matrix.Count > 4)
            //{
            //    matrix = new ArrayList();
            //    ArrayList r = new ArrayList();
            //    r.Add(34);
            //    r.Add(10);
            //    r.Add(29);
            //    r.Add(3);
            //    r.Add(35);
            //    r.Add(-1);
            //    r.Add(4);
            //    r.Add(11);

            //    r.Add(9);
            //    r.Add(12);
            //    r.Add(14);

            //    matrix.Add(r);
            //    r = new ArrayList();
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(27);
            //    r.Add(5);
            //    r.Add(33);
            //    r.Add(-1);
            //    r.Add(1);
            //    r.Add(6);

            //    r.Add(18);
            //    r.Add(19);
            //    r.Add(20);
            //    matrix.Add(r);
            //    r = new ArrayList();
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(7);
            //    r.Add(17);

            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(21);
            //    r.Add(22);
            //    r.Add(23);
            //    matrix.Add(r);
            //    r = new ArrayList();
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(2);

            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(-1);
            //    r.Add(24);
            //    r.Add(25);
            //    r.Add(28);

            //    matrix.Add(r);
            //    r = new ArrayList();
            //    r.Add(26);
            //    r.Add(13);
            //    r.Add(8);
            //    r.Add(0);
            //    r.Add(15);
            //    r.Add(32);
            //    r.Add(16);

            //    r.Add(-1);
            //    r.Add(30);
            //    r.Add(31);
            //    r.Add(36);

            //    matrix.Add(r);
            //}
            ////

            //mai tova zadava koordinatite na vseki vrah
			int maxRowLength = 0;
			for (int rowIndex = 0; rowIndex < matrix.Count; rowIndex++)
			{
				ArrayList currentRow = (ArrayList)(matrix[rowIndex]);
				int offset = vertexWidth/8 - (rowIndex%2)*vertexWidth/4 + (rowIndex+3)%6 - 3;
				for (int columnIndex = 0; columnIndex < currentRow.Count; columnIndex++)
				{
					int currentVertex = (int)(currentRow[columnIndex]);
					if (currentVertex >= 0)
					{
						mVertexes[currentVertex].X = columnIndex*vertexWidth + vertexWidth/2 + offset;
						mVertexes[currentVertex].Y = rowIndex*vertexHeight + vertexHeight/2;
					}
				}
				if (currentRow.Count > maxRowLength)
				{
					maxRowLength = currentRow.Count;
				}
			}

			mSize.Width = maxRowLength * vertexWidth;
			mSize.Height = matrix.Count * vertexHeight;
			aSize = mSize;
		}

		public void Draw(Graphics aGraphics, bool aPrintable)
		{
			SolidBrush brush = new SolidBrush(Color.White);
			aGraphics.FillRectangle(brush, 0, 0, mSize.Width - 1, mSize.Height - 1);
			brush.Dispose();

            //Font font = new Font("Tahoma", 12);
            //SolidBrush textBrush = new SolidBrush(Color.Black);
            //aGraphics.DrawString("Intuitionistic Fuzzy Implications", font, textBrush, mSize.Width/2 - 150, 4);
            //textBrush.Dispose();
            //font.Dispose();
			//eventualno da pi6e, 4e ne e dokazan rezultat

			foreach (Rib rib in mRibs)
			{
				rib.Draw(aGraphics, aPrintable);//daje mai nai-dobre parvo disabled, a posle otgore hubavite rebra, no i taka ne e zle
			}

			foreach (Vertex vertex in mVertexes)
			{
				vertex.Draw(aGraphics, aPrintable);
			}
		}
	}

//	class DrawGraph//dosta omazana organizaciq; napravo MAHNI TVA!
//	{
//		public static void DrawGraphInFile(bool aHasse)
//		{
//			Graph graph = new Graph(176);//broika vrah4eta
//
//			StreamReader reader = new StreamReader("Ribs.txt");//rebrata trqbva da sa sorted!!!
//			using (reader)
//			{
//				string row = reader.ReadLine();
//				while (row != null && row.IndexOf(' ') >= 0)
//				{
//					string [] vertexes = row.Split(' ');
//					int vertex1 = Int32.Parse(vertexes[0]);
//					int vertex2 = Int32.Parse(vertexes[1]);
//
//					if (graph.Vertexes[vertex1] == null)
//						graph.Vertexes[vertex1] = new Vertex(vertex1);
//					if (graph.Vertexes[vertex2] == null)
//						graph.Vertexes[vertex2] = new Vertex(vertex2);
//					graph.Ribs.Add(new Rib((Vertex)graph.Vertexes[vertex1], (Vertex)graph.Vertexes[vertex2]));
//
//					row = reader.ReadLine();
//				}
//
//				while (row != null)
//				{
//					int impl = Int32.Parse(row);
//					graph.Vertexes[impl] = new Vertex(impl);
//					row = reader.ReadLine();
//				}
//			}
//
//			Size size;
//			graph.PrepareDraw(aHasse, out size);//ili puk da moje da risuva spored zadaden ot user-a razmer?
//
//			Bitmap bitmap = new Bitmap(size.Width, size.Height);
//			Graphics g = Graphics.FromImage(bitmap);
//			g.SmoothingMode = SmoothingMode.AntiAlias; //da, ama GIF-a se razvalq!!! ako e GIF, de
//			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
//
//			graph.Draw(g);
//
//			FileStream fs = new FileStream("IFImplicationsGraph.png", FileMode.Create);
//			using (fs)
//			{
//				bitmap.Save(fs, ImageFormat.Png);
//			}
//
//			bitmap.Dispose();
//			g.Dispose();
//		}
//	}
}