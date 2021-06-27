using System;
using System.Collections;
using IFSTool.IntFuzzyExpression;

namespace IFSTool
{
	public class CachedValues
	{
		private Hashtable mValues; //Hashtable<Implication, IFBool[]>
		private int mShots;
		private double mStep;
		private int mCombinations;

		public CachedValues(int aShots)
		{
			//za po-barzo neka da priema nqkakuv na4alen spisak, koito da ke6ira, 4e da ne varti izli6no iteratori!
			if (aShots >= 2 /*&& aImplicationsCount >= 0*/)
			{
				mShots = aShots;
				mStep = 1.0/(mShots - 1);
				mCombinations = mShots * mShots * (mShots + 1) * (mShots + 1) / 4; //be6e /2; ne e /4 zaradi intuitionizma
			}
			else
			{
				throw new ArgumentOutOfRangeException();
			}

			mValues = new Hashtable();
			//mNonRegularValues = new Hashtable();
		}

		public int Shots
		{
			get
			{
				return mShots;
			}
		}
		
		public double Step
		{
			get
			{
				return mStep;
			}
		}
		
		//requires: value is cached, otherwise rounds if1 and if2
		//tazi da e po-barza ot dolnata
		//public IFBool GetCachedValue(Implication aImplication, IFBool if1, IFBool if2)
		//{
		//}

//		public static int gadostCounter = 0;
//		public static int radostCounter = 0;

		public IFBool GetValue(Implication aImplication, IFBool if1, IFBool if2)
		{
			IFBool[] values = (IFBool[])mValues[aImplication];
			if (values == null)
			{
				values = new IFBool[mCombinations];
				// TODO: optimizaciq: da proveri dali u4astvat vsi4ki vars, ako ne - da iterira samo po drugite promenlivi, a ostanalite stoinosti da gi zapalva sas sa6tite
				IntFuzzyEnumerator enumerator = new IntFuzzyEnumerator(mShots);
				IntFuzzyEnumerator enumerator2 = new IntFuzzyEnumerator(mShots);
				enumerator.Reset();
				int counter = 0;
				while (enumerator.MoveNext())
				{
					IFBool left = (IFBool)enumerator.Current;
					enumerator2.Reset();

					while (enumerator2.MoveNext())
					{
						IFBool right = (IFBool)enumerator2.Current;
						values[counter] = aImplication.Evaluate(left, right);
						counter++;
					}
				}
				mValues.Add(aImplication, values); //ne trqbva li da q klonirame?
			}

			//start debug
			double v1 = if1.Validity / mStep;
			double n1 = if1.Nonvalidity / mStep;
			double v2 = if2.Validity / mStep;
			double n2 = if2.Nonvalidity / mStep;
			//end debug

			//start debug
//			if (Math.Abs(v1 - Math.Round(v1)) >= IFSTool.Expression.Constants.EPSILON
//				|| Math.Abs(n1 - Math.Round(n1)) >= IFSTool.Expression.Constants.EPSILON
//				|| Math.Abs(v2 - Math.Round(v2)) >= IFSTool.Expression.Constants.EPSILON
//				|| Math.Abs(n2 - Math.Round(n2)) >= IFSTool.Expression.Constants.EPSILON)
//				throw new ArgumentOutOfRangeException();
			//end debug

			IFBool result;

			int a, b, c, d;
			//tova ne e li prekaleno bavno? za6to da ne e v @requires
			//ne! pri smqtane na IFNode izrazi sled kat smetnem dadena impl, rezultatat e proizvolen, ne e nepremenno v na6iq ke6!
			if (Math.Abs(v1 - Math.Round(v1)) < IFSTool.Expression.Constants.EPSILON
				&& Math.Abs(n1 - Math.Round(n1)) < IFSTool.Expression.Constants.EPSILON
				&& Math.Abs(v2 - Math.Round(v2)) < IFSTool.Expression.Constants.EPSILON
				&& Math.Abs(n2 - Math.Round(n2)) < IFSTool.Expression.Constants.EPSILON)
			{
				a = (int)Math.Round(if1.Validity / mStep);
				b = (int)Math.Round(if1.Nonvalidity / mStep);
				c = (int)Math.Round(if2.Validity / mStep);
				d = (int)Math.Round(if2.Nonvalidity / mStep);
			
				int index1 = CalculateIndexInIFSTriangle(a, b);
				int index2 = CalculateIndexInIFSTriangle(c, d);
				int index = index1 * mShots * (mShots + 1) / 2 + index2; //tova e sqrt(combinations)

				//radostCounter++;
				result = values[index];
			}
			else
			{
//				Implication_IFBool_IFBool ifValue;
//				ifValue.implication = aImplication;
//				ifValue.left = if1;
//				ifValue.right = if2;
//				
//				result = (IFBool)mNonRegularValues[ifValue];
//				if (result == null)
//				{
					//gadostCounter++;

					result = aImplication.Evaluate(if1, if2);
//					mNonRegularValues.Add(ifValue, result);
//				}

			}

			return result;
		}
		
//		private Hashtable mNonRegularValues; //Hashtable<Implication_IFBool_IFBool, IFBool>
//		//da trie LRU?
//		private struct Implication_IFBool_IFBool
//		{
//			public Implication implication;
//			public IFBool left;
//			public IFBool right;
//			public override int GetHashCode()
//			{
//				return implication.GetHashCode() ^ left.GetHashCode() ^ right.GetHashCode();
//			}
//
//		}

		//trqbva predi tova da e izvikana saotvetnata implikaciq, ina4e vika evaluate
		public IFBool GetValue(Negation aNegation, IFBool ifb)
		{
			//maliiii... error-prone! If a negation has the same number as non-corresponding implication...
			Implication implication = new Implication(aNegation.Truth, aNegation.Untruth, aNegation.Name);
			if (mValues[implication] != null)
				return GetValue(implication, ifb, new IFBool(0.0, 1.0));
			else
				return aNegation.Evaluate(ifb);
		}

		private int CalculateIndexInIFSTriangle(int validity, int nonvalidity)
		{
			//pi4, tova zavisi ot iteratora! ako e drug, nqma da e taka!
			//nonvalidity +  (validity) 4lena na shots, shots-1, shots-2, ...
			int result = nonvalidity + validity * ( 2 * mShots - validity + 1) / 2;
			
			return result;
		}
	}
}
//!!!pri smqna na nqkoq custom implikaciq - da preiz4islqva stoinostite za neq
//mai sravnqvaneto samo po ukazateli/ime nqma da e dostata4no?
