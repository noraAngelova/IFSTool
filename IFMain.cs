using System;
using System.Collections;
using System.IO;
using IFSTool.Expression;
using IFSTool.ArithmeticExpression;
using IFSTool.IntFuzzyExpression;

//nakraq Release, ei!

namespace IFSTool
{
	public class IFMain
	{
		public const int SHOTS = 5;

		private static CachedValues mCachedValues;

		static IFMain()
		{
			mCachedValues = new CachedValues(SHOTS);
		}

		public static int Shots
		{
			get
			{
				return mCachedValues.Shots;
			}
			set
			{
				if (value != mCachedValues.Shots)
				{
					mCachedValues = new CachedValues(value);
				}
			}
		}

		public static ArrayList ReadImplicationsFromFile(string aFileName)
		{
			//da gi proverqva za gre6ki!
			ArrayList implications = new ArrayList();
            try
            {
                StreamReader reader = new StreamReader(aFileName);
                using (reader)
                {
                    string row = reader.ReadLine(); //ve4e nqma da ima nujda ot broika predvaritelno
                    while (row != null)
                    {
                        if (row[0] != '#')
                        {
                            string[] rowComponents = row.Split(new string[] { "\t", ": ", ", " },
                                StringSplitOptions.RemoveEmptyEntries);//ne e nai-4istoto, ama se qdva
                            Parser parser = new ArithmeticParser();
                            IArithmeticNode expressionTruth = (IArithmeticNode)parser.Parse(rowComponents[1]);
                            IArithmeticNode expressionUntruth = (IArithmeticNode)parser.Parse(rowComponents[2]);
                            //nqkoi impl ne sa optimizirani; dali oba4e pre4i posle?
                            ExpressionUtil.Normalize(ref expressionTruth);
                            ExpressionUtil.Normalize(ref expressionUntruth);
                            //
                            Implication implication = new Implication(expressionTruth, expressionUntruth, rowComponents[0]);
                            implications.Add(implication);
                        }
                        row = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                //
            }
			return implications;
		}

		//moje6e s nekvi delegati... da ne se povtarq kod!
		public static Implication GenerateImplication2From(Implication aImplication)//must-may ili may-must
		{
			//moje da se opita da napravi nqkva optimizaciq...
			IArithmeticNode truth = GenerateTree2From(aImplication.Truth);
			ExpressionUtil.Normalize(ref truth);
			IArithmeticNode untruth = GenerateTree2From(aImplication.Untruth);
			ExpressionUtil.Normalize(ref untruth);
			string name = "2," + aImplication.Name;
			Implication result = new Implication(truth, untruth, name);
			return result;
		}

		private static IArithmeticNode GenerateTree2From(IArithmeticNode aTree)
		{
			IArithmeticNode root = null;
			if (aTree is VariableNode && ((VariableNode)aTree).Variable == 'b')
			{
				root = new MinusNode();
				((BinaryNode)root).LeftArgument = new ConstantNode(1);
				((BinaryNode)root).RightArgument = new VariableNode('a');
			}
			else if (aTree is VariableNode && ((VariableNode)aTree).Variable == 'c')
			{
				root = new MinusNode();
				((BinaryNode)root).LeftArgument = new ConstantNode(1);
				((BinaryNode)root).RightArgument = new VariableNode('d');
			}
			else
			{
				root = (IArithmeticNode)aTree.ShallowCopy();
				if (aTree is UnaryNode)
				{
					((UnaryNode)root).Argument = GenerateTree2From(((UnaryNode)aTree).Argument);
				}
				else if (aTree is BinaryNode)
				{
					((BinaryNode)root).LeftArgument = GenerateTree2From(((BinaryNode)aTree).LeftArgument);
					((BinaryNode)root).RightArgument = GenerateTree2From(((BinaryNode)aTree).RightArgument);

					//(1-(1-x)) -> x; za6to tuk, nali ima normalize...
					//nqma osobena polza za axiomite, no za proving e dobre
//					if (root is MinusNode)
//					{
//						MinusNode minusNode = (MinusNode)root;
//						if (ExpressionUtil.IsOne(minusNode.LeftArgument) && minusNode.RightArgument is MinusNode)
//						{
//							MinusNode nestedMinusNode = (MinusNode)minusNode.RightArgument;
//							if (ExpressionUtil.IsOne(nestedMinusNode.LeftArgument))
//								root = nestedMinusNode.RightArgument;
//						}
//					 }
				}
			}
			return root;
		}

		public static Implication GenerateImplication3From(Implication aImplication)
		{
			IArithmeticNode truth = GenerateTree3From(aImplication.Truth);
			ExpressionUtil.Normalize(ref truth);
			IArithmeticNode untruth = GenerateTree3From(aImplication.Untruth);
			ExpressionUtil.Normalize(ref untruth);
			string name = "3," + aImplication.Name;
			Implication result = new Implication(truth, untruth, name);
			return result;
		}

		private static IArithmeticNode GenerateTree3From(IArithmeticNode aTree)
		{
			IArithmeticNode root = null;
			if (aTree is VariableNode && ((VariableNode)aTree).Variable == 'a')
			{
				root = new MinusNode();
				((BinaryNode)root).LeftArgument = new ConstantNode(1);
				((BinaryNode)root).RightArgument = new VariableNode('b');
			}
			else if (aTree is VariableNode && ((VariableNode)aTree).Variable == 'd')
			{
				root = new MinusNode();
				((BinaryNode)root).LeftArgument = new ConstantNode(1);
				((BinaryNode)root).RightArgument = new VariableNode('c');
			}
			else
			{
				root = (IArithmeticNode)aTree.ShallowCopy();
				if (aTree is UnaryNode)
				{
					((UnaryNode)root).Argument = GenerateTree3From(((UnaryNode)aTree).Argument);
				}
				else if (aTree is BinaryNode)
				{
					((BinaryNode)root).LeftArgument = GenerateTree3From(((BinaryNode)aTree).LeftArgument);
					((BinaryNode)root).RightArgument = GenerateTree3From(((BinaryNode)aTree).RightArgument);

					//(1-(1-x)) -> x
					//nqma osobena polza za axiomite, no za proving e dobre
//					if (root is MinusNode)
//					{
//						MinusNode minusNode = (MinusNode)root;
//						if (ExpressionUtil.IsOne(minusNode.LeftArgument) && minusNode.RightArgument is MinusNode)
//						{
//							MinusNode nestedMinusNode = (MinusNode)minusNode.RightArgument;
//							if (ExpressionUtil.IsOne(nestedMinusNode.LeftArgument))
//								root = nestedMinusNode.RightArgument;
//						}
//					}
				}
			}
			return root;
		}
		
		public static Implication GenerateImplicationSubstFrom(Implication aImplication)
		{
			//IArithmeticNode truth = GenerateTreeSubstFrom(aImplication.Truth);
			//IArithmeticNode untruth = GenerateTreeSubstFrom(aImplication.Untruth);
			IArithmeticNode truth = ExpressionUtil.ReplaceVars(aImplication.Truth, "dcba");
			IArithmeticNode untruth = ExpressionUtil.ReplaceVars(aImplication.Untruth, "dcba");

			//taq rabota s nomerata 6te dava dosta gre6ki, ako user-at si izmislq sam nek'vi imena!
			int commaIndex = aImplication.Name.IndexOf(',');
			string name;
			if (commaIndex < 0)
			{
				name = "4," + aImplication.Name;
			}
			else
			{
				//try...
				int i = Int32.Parse(aImplication.Name.Substring(0, commaIndex));
				i += 3;
				name = i.ToString() + ',' + aImplication.Name.Substring(commaIndex + 1);
			}
			Implication result = new Implication(truth, untruth, name);
			return result;
		}

//		private static IArithmeticNode GenerateTreeSubstFrom(IArithmeticNode aTree)
//		{
//			IArithmeticNode root = null;
//			root = (IArithmeticNode)aTree.ShallowCopy();
//			if (aTree is VariableNode)
//			{
//				((VariableNode)root).Variable = (char)((int)'a' + (int)'d' - (int)(((VariableNode)root).Variable));
//			}
//			if (aTree is UnaryNode)
//			{
//				((UnaryNode)root).Argument = GenerateTreeSubstFrom(((UnaryNode)aTree).Argument);
//			}
//			else if (aTree is BinaryNode)
//			{
//				((BinaryNode)root).LeftArgument = GenerateTreeSubstFrom(((BinaryNode)aTree).LeftArgument);
//				((BinaryNode)root).RightArgument = GenerateTreeSubstFrom(((BinaryNode)aTree).RightArgument);
//			}
//			return root;
//		}
		
//		public static bool IsPossiblyIFImplication(Implication aImplication)
//		{
//			//tova ve4e ne e nujno, ima go v Axioms
//			f(0,y)=1;
//			f(x,1)=1;
//			f(1,0)=0;
//			f(x,0)=!x;
//			return true;
//		}

		public static bool IsPossiblyTrueIFImplication(Implication aImplication, bool aIgnoreEqual)
		{
			IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
			IntFuzzyEnumerator enumerator2 = new IntFuzzyEnumerator(mCachedValues.Shots);
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				IFBool left = (IFBool)enumerator.Current;
				if (aIgnoreEqual && Math.Abs(left.Validity - left.Nonvalidity) < Constants.EPSILON)
					continue;
				enumerator2.Reset();
				while (enumerator2.MoveNext())
				{
					IFBool right = (IFBool)enumerator2.Current;
					if (aIgnoreEqual && Math.Abs(right.Validity - right.Nonvalidity) < Constants.EPSILON)
						continue;
					IFBool implValue = mCachedValues.GetValue(aImplication, left, right);
					// if aIgnoreEqual - ignore implValue with truth=untruth;
					// if !aIgnoreEqual - go ahead
					if ((!aIgnoreEqual || Math.Abs(implValue.Validity - implValue.Nonvalidity) >= Constants.EPSILON)
						&& left.IsIntFuzzyTautology && !right.IsIntFuzzyTautology && implValue.IsIntFuzzyTautology)
					{
//						string debug = left.ToString() + " -> " + right.ToString() + " = " + implValue.ToString();
//						debug.Trim();//set breakpoint here
						return false;
					}
				}
			}
			return true;
		}

		public static bool IsPossiblyTrueIFNegation(Negation aNegation, bool aIgnoreEqual)
		{
			IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				IFBool current = (IFBool)enumerator.Current;
				if (aIgnoreEqual && Math.Abs(current.Validity - current.Nonvalidity) < Constants.EPSILON)
					continue;
				IFBool result = mCachedValues.GetValue(aNegation, current);
				if (aIgnoreEqual && Math.Abs(result.Validity - result.Nonvalidity) < Constants.EPSILON)
					continue;
				if (!(current.IsIntFuzzyTautology ^ result.IsIntFuzzyTautology))
					return false;
			}
			return true;
		}

		public static bool ArePossiblyIdentical(Implication aImplication1, Implication aImplication2)
		{
			//smqtai samo s ke6a; koito iska - da si proverqva s operator==
			IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
			IntFuzzyEnumerator enumerator2 = new IntFuzzyEnumerator(mCachedValues.Shots);
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				IFBool left = (IFBool)enumerator.Current;
				enumerator2.Reset();
				while (enumerator2.MoveNext())
				{
					IFBool right = (IFBool)enumerator2.Current;
					IFBool value1 = mCachedValues.GetValue(aImplication1, left, right);
					IFBool value2 = mCachedValues.GetValue(aImplication2, left, right);
					if (! value1.Equals(value2))
						return false;
				}
			}

			/*v IFS.CPP sme pravili i po-precizna proverka, ako izglejdat ravni
			for (int a = 0; a < shots - 1; ++a)
			for (int b = 0; b < shots - 1 - a; ++b)
				for (int c = 0; c < shots - 1; ++c)
					for (int d = 0; d < shots - 1 - c; ++d)
					{
						//tiq stoinosti moje da se ke6irat nqkade!
						//polzvai impl(IFBool(step*a, ...) - zasega ne e nujno
						if (fabs(implications[index1].truth->Evaluate(step*(0.5+a), step*(0.5+b), step*(0.5+c), step*(0.5+d)) -
							implications[index2].truth->Evaluate(step*(0.5+a), step*(0.5+b), step*(0.5+c), step*(0.5+d))) > epsilon ||
							fabs(implications[index1].untruth->Evaluate(step*(0.5+a), step*(0.5+b), step*(0.5+c), step*(0.5+d)) -
							implications[index2].untruth->Evaluate(step*(0.5+a), step*(0.5+b), step*(0.5+c), step*(0.5+d))) > epsilon)
						{
							identical = false;
							a = b = c = d = shots;
						}
					}
					 */
			return true;
		}

		public static IList GetIdenticalSets(IList aImplications) //IList<Set>? vse pak 6te iskame parvata, ne koq da e
		{
			//ako ve4e sme dokazali, 4e A=B i B=C, nqma smisal za A=C, nali? vsa6tnost zavisi; a i kak 6te znaem s koq e ekvivalentna?
			//posle da pravi i proving, da pi6e i nego v ot4eta (tova da stava prez druga forma)
			
			bool[] redundant = new bool[aImplications.Count];

			IList result = new ArrayList();
			for (int i = 0; i < aImplications.Count - 1; i++)
			{
				for (int j = i + 1; j < aImplications.Count; j++)
					if (IFMain.ArePossiblyIdentical((Implication)aImplications[i], (Implication)aImplications[j]))
					{
						redundant[i] = redundant[j] = true;

						int groupIndex = 0;
						ArrayList currentGroup = null;
						for (; groupIndex < result.Count; groupIndex++)
						{
							currentGroup = (ArrayList)result[groupIndex];
							if (currentGroup.Contains(aImplications[i]))//bavnoo, no zapazva parviq element na parvo mqsto
							{
								break;
							}
						}
						if (groupIndex < result.Count)
						{
							if (!currentGroup.Contains(aImplications[j]))
								currentGroup.Add(aImplications[j]);
						}
						else
						{
							ArrayList newGroup = new ArrayList();
							newGroup.Add(aImplications[i]);
							newGroup.Add(aImplications[j]);
							result.Add(newGroup);
						}
					}
				if (!redundant[i])
				{
					ArrayList newGroup = new ArrayList();
					newGroup.Add(aImplications[i]);
					result.Add(newGroup);
				}
			}
			if (!redundant[aImplications.Count - 1])
			{
				ArrayList newGroup = new ArrayList();
				newGroup.Add(aImplications[aImplications.Count - 1]);
				result.Add(newGroup);
			}
			
			return result;
		}

		public static void RemoveIdenticalImplications(ref ArrayList aImplications, IList aIdenticalSets)
		{
			ArrayList uniqueImplications = new ArrayList();
			foreach (IList group in aIdenticalSets)
			{
				uniqueImplications.Add(group[0]);
			}
			aImplications = uniqueImplications;
		}
	
        // tova da se izvikva samo sled kato e provereno pri shots>=5, 6toto inak vry6ta true za lineinite bez da proverqva
		public static bool IsProvenIdentity(Implication aImpl1, Implication aImpl2)
		{
            bool left = ExpressionUtil.AreIdentical(aImpl1.Truth, aImpl2.Truth);
            bool right = ExpressionUtil.AreIdentical(aImpl1.Untruth, aImpl2.Untruth);
            left = left || (IFMain.Shots >= 5 && ExpressionUtil.IsLinear(aImpl1.Truth) && ExpressionUtil.IsLinear(aImpl2.Truth));
            right = right || (IFMain.Shots >= 5 && ExpressionUtil.IsLinear(aImpl1.Untruth) && ExpressionUtil.IsLinear(aImpl2.Untruth));
			return left && right;
		}

		public static bool PossiblySmallerImplication(Implication aImplication1, Implication aImplication2)//bad name?
		{
			IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
			IntFuzzyEnumerator enumerator2 = new IntFuzzyEnumerator(mCachedValues.Shots);
			enumerator.Reset();
			while (enumerator.MoveNext())
			{
				IFBool left = (IFBool)enumerator.Current;
				enumerator2.Reset();
				while (enumerator2.MoveNext())
				{
					IFBool right = (IFBool)enumerator2.Current;
					IFBool value1 = mCachedValues.GetValue(aImplication1, left, right);
					IFBool value2 = mCachedValues.GetValue(aImplication2, left, right);
					if (!(value1 <= value2))
						return false;
				}
			}

			return true;
		}

		public static Graph GetImplicationsGraph(ArrayList aImplications)//pak Possibly
		{
			ArrayList ribs = new ArrayList();
			Graph graph = new Graph(aImplications, ribs);

			bool[] notIsolatedImplications = new bool[aImplications.Count];
			bool[] notSupremums = new bool[aImplications.Count];
			bool[] notInfimums = new bool[aImplications.Count];

			for (int index1 = 0; index1 < aImplications.Count; index1++)
			{
                System.Diagnostics.Debug.WriteIf(index1 < 9, ' ');
                System.Diagnostics.Debug.Write((1 + index1).ToString() + " ");
				for (int index2 = 0; index2 < aImplications.Count; index2++)
				{
                    if (index1 != index2)//moje i s foreach, 6te sravnqva6 refs
                    {
                        if (PossiblySmallerImplication((Implication)(aImplications[index1]), (Implication)(aImplications[index2])))
                        {
                            notIsolatedImplications[index1] = notIsolatedImplications[index2] = true;
                            notSupremums[index1] = true;
                            notInfimums[index2] = true;

                            System.Diagnostics.Debug.Write("< ");

                            Rib rib = new Rib(graph.Vertexes[index1], graph.Vertexes[index2]);
                            graph.Ribs.Add(rib);
                        }
                        else System.Diagnostics.Debug.Write("- ");
                    }
                    else System.Diagnostics.Debug.Write("- ");
				}
                System.Diagnostics.Debug.WriteLine("");
			}

			return graph;
		}

		public static IList ReadAxiomsFromFile(string aFileName)
		{
			//da gi proverqva za gre6ki!
			IList axioms = new ArrayList();
            try
            {
                StreamReader reader = new StreamReader(aFileName);
                using (reader)
                {
                    string row = reader.ReadLine();
                    while (row != null)
                    {
                        if (row[0] != '#')
                        {
                            int indexOfSpace = row.IndexOfAny(new char[] { ' ', '\t' });
                            //imeto go gubim zasega - lo6o
                            row = row.Remove(0, indexOfSpace + 1);
                            //izreji malko otlqvo-otdqsno - zasega ne
                            Parser parser = new IntFuzzyParser();
                            IFNode axiom = (IFNode)parser.Parse(row);
                            axioms.Add(axiom);
                        }
                        row = reader.ReadLine();
                    }
                }
            }
            catch (IOException)
            {
                //
            }
			return axioms;
		}

		private static Hashtable mAxiomsVariables = new Hashtable(); //Hashtable<IFNode, Int32>
		//del LRU...

		public static bool ProveAxiom(Implication aImplication, Negation aNegation, IFNode aAxiom, 
            bool aCheckForIFTautology, bool aSkipLinear, IList aImplications, IList aNegations)
		{
			IFMain.SetImplicationAndNegation(aAxiom, aImplication, aNegation, aImplications, aNegations);
			IArithmeticNode truth, untruth;
			IFExpressionUtil.ConvertToArithmetic(aAxiom, out truth, out untruth);
			ExpressionUtil.Normalize(ref untruth);
			bool res = false;
			//res = ExpressionUtil.AreIdentical(truth, new ConstantNode(1)) && ExpressionUtil.AreIdentical(untruth, new ConstantNode(0));
			bool isZero = ExpressionUtil.IsZero(untruth);
			if (isZero && aCheckForIFTautology) //predpolagame, 4e lqvata 4ast e m/u 0 i 1
				return true;
			ExpressionUtil.Normalize(ref truth);
            if (aSkipLinear)
            {
                if (ExpressionUtil.IsLinear(truth) && ExpressionUtil.IsLinear(untruth))
                    return true;
            }
            res = isZero && ExpressionUtil.IsOne(truth);//to normalize var6i 4ernata rabota
			if (!res && aCheckForIFTautology)
			{
				//tova ne be6e izpolzvano
				//				res = ExpressionUtil.IsOne(truth); //pak predpolagame, 4e dqsnata 4ast e m/u 0 i 1
				//				if (!res)
				res = ExpressionUtil.AreGreaterOrEqual(truth, untruth);
			}
			return res;
		}

		public static bool CheckAxiom(Implication aImplication, Negation aNegation, IFNode aAxiom, bool aCheckForIFTautology, IList aImplications, IList aNegations)//otnovo Possibly
		{
			if (aNegation == null)
				aNegation = IFExpressionUtil.GenerateNegation(aImplication);
			//else if (aImplication == null) generirai puk neq?

			SetImplicationAndNegation(aAxiom, aImplication, aNegation, aImplications, aNegations);

			object variables = mAxiomsVariables[aAxiom];//hmmm, nqmame GetHash, nqmame Equals...
			if (variables == null)
			{
				variables = VariablesCount(aAxiom);
				mAxiomsVariables.Add(aAxiom, variables);
			}

			MultiVariableAxiomChecker checker = new MultiVariableAxiomChecker(aAxiom, aCheckForIFTautology);

			bool result = checker.Check((int)variables);

			//for testing only
//			bool altResult = true;
//			//haide parvo tap variant, 4e sme netarpelivi :)
//			IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
//			IntFuzzyEnumerator enumerator2 = new IntFuzzyEnumerator(mCachedValues.Shots);
//			IntFuzzyEnumerator enumerator3 = new IntFuzzyEnumerator(mCachedValues.Shots);
//			enumerator.Reset();
//			while (enumerator.MoveNext())
//			{
//				IFBool A = (IFBool)enumerator.Current;
//				enumerator2.Reset();
//				while (enumerator2.MoveNext())
//				{
//					IFBool B = (IFBool)enumerator2.Current;
//					enumerator3.Reset();
//					while (enumerator3.MoveNext())
//					{
//						IFBool C = (IFBool)enumerator3.Current;
//						IntFuzzyContext context = new IntFuzzyContext(mCachedValues, A, B, C);
//						IFBool implValue = aAxiom.Evaluate(context);
//						if (aCheckForIFTautology && !implValue.IsIntFuzzyTautology)
//						{
//							altResult = false;
//							goto _end;
//						}
//						else if (!aCheckForIFTautology && !implValue.IsTautology)//oprosti
//						{
//							altResult = false;
//							goto _end;
//						}
//					}
//				}
//			}
//		_end:
//			if (altResult != result)
//			{
//				int metal = 0; metal--;
//			}
			
			return result;
		}

		private class MultiVariableAxiomChecker
		{
			private IFNode mAxiom;
			private bool mCheckForIFTautology;
			private IFBool[] mContextVariables;

			public MultiVariableAxiomChecker(IFNode aAxiom, bool aCheckForIFTautology)
			{
				mAxiom = aAxiom;
				mCheckForIFTautology = aCheckForIFTautology;
				mContextVariables = new IFBool[26];//26, ami ako iskame hubavi imena?
			}

			public bool Check(int level)
			{
				if (level == 0)
				{
					IntFuzzyContext context = new IntFuzzyContext(mCachedValues, mContextVariables);
					IFBool implValue = mAxiom.Evaluate(context);
					if (mCheckForIFTautology && !implValue.IsIntFuzzyTautology)
					//if (implValue.Validity < 0.5)//IF certainty
						return false;
					else if (!mCheckForIFTautology && !implValue.IsTautology)//oprosti
						return false;
					return true;
				}
				//ei tuk ako mojem da vidim dali ima nujda ot tozi iterator, a ne ot edini4en 0->1
				IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mCachedValues.Shots);
				enumerator.Reset();
				while (enumerator.MoveNext())
				{
					IFBool ifb = (IFBool)enumerator.Current;
					mContextVariables[level - 1] = ifb;
					if (! Check(level - 1))
						return false;
				}
				return true;
			}
		}

		//requires: variables are A, B, C, ... bez propuski!
		private static int VariablesCount(IFNode aNode)
		{
			if (aNode is IFVariableNode)
			{
				int count = (int)aNode.ToString()[0] - (int)'A' + 1;
				return count;
			}
			else if (aNode is IFUnaryNode)
				return VariablesCount(((IFUnaryNode)aNode).Argument);
			else if (aNode is IFBinaryNode)
			{
				IFBinaryNode binNode = (IFBinaryNode)aNode;
				int left = VariablesCount(binNode.LeftArgument);
				//if (left == 3)
                //	return 3; // OMFG?!
				int right = VariablesCount(binNode.RightArgument);
				return Math.Max(left, right);
			}
			return 1;
		}

//		//tapo?
//		private static IFBool EvaluateWithCache(IFNode aNode, IntFuzzyContext aContext)
//		{
//			//or if is IFConstant...
//			if (aNode is IFVariableNode)
//			{
//				return aNode.Evaluate(aContext);
//			}
//			if (aNode is IFImplicationNode)
//			{
//				IFImplicationNode implNode = (IFImplicationNode)aNode;
//				IFBool left = EvaluateWithCache(implNode.LeftArgument, aContext);
//				IFBool right = EvaluateWithCache(implNode.RightArgument, aContext);
//				return mCachedValues.GetValue(implNode.Implication, left, right);
//			}
//			if (aNode is IFNegationNode)
//			{
//				IFNegationNode negNode = (IFNegationNode)aNode;
//				IFBool arg = EvaluateWithCache(negNode.Argument, aContext);
//				return mCachedValues.GetValue(negNode.Negation, arg);
//			}
//			//taka 6te trqbva za vseki vazel da povtarqme logikata, a kogato dobavim nov...
//		}

		//vremenno; be6e private, no 6te trqbva i drugade
		public static void SetImplicationAndNegation(IFNode aNode, Implication aImplication, Negation aNegation,
            IList aImplications, IList aNegations)
		{
			if (aNode is IFImplicationNode)
			{
                IFImplicationNode impl = (IFImplicationNode)aNode;
                int index = impl.Index;
                if (index >= 0)
                    impl.Implication = (Implication) aImplications[index];
                else
				    impl.Implication = aImplication;
			}
			else if (aNode is IFNegationNode)
			{
                IFNegationNode neg = (IFNegationNode)aNode;
                int index = neg.Index;
                if (index >= 0)
                    neg.Negation = (Negation) aNegations[index];
                else
                    neg.Negation = aNegation;
			}
			//tuk ne trqbva else, ei!
			if (aNode is IFUnaryNode)
			{
				SetImplicationAndNegation(((IFUnaryNode)aNode).Argument, aImplication, aNegation, aImplications, aNegations);
			}
			else if (aNode is IFBinaryNode)
			{
                SetImplicationAndNegation(((IFBinaryNode)aNode).LeftArgument, aImplication, aNegation, aImplications, aNegations);
                SetImplicationAndNegation(((IFBinaryNode)aNode).RightArgument, aImplication, aNegation, aImplications, aNegations);
			}
		}
	}
}
