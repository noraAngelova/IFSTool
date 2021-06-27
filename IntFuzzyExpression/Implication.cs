using System;
using System.Text;
using IFSTool.ArithmeticExpression;

namespace IFSTool.IntFuzzyExpression
{
	/// <summary>
	/// Summary description for Implication.
	/// </summary>
	public class Implication
	{
		private IArithmeticNode mTruth; // are these names correct?
		private IArithmeticNode mUntruth;
		private string mName;

		public Implication(IArithmeticNode aTruth, IArithmeticNode aUntruth, string aName)
		{
			mTruth = aTruth;
			mUntruth = aUntruth;
			mName = aName;
		}

		public Implication() : this(null, null, "")
		{
		}

		public IArithmeticNode Truth
		{
			get
			{
				return mTruth;
			}
			set
			{
				mTruth = value;
			}
		}

		public IArithmeticNode Untruth
		{
			get
			{
				return mUntruth;
			}
			set
			{
				mUntruth = value;
			}
		}

		public string Name
		{
			get
			{
				return mName;
			}
			set
			{
				mName = value;
			}
		}

		public IFBool Evaluate(IFBool ifb1, IFBool ifb2)
		{
			double validity = mTruth.Evaluate(ifb1.Validity, ifb1.Nonvalidity, ifb2.Validity, ifb2.Nonvalidity);
			double nonvalidity = mUntruth.Evaluate(ifb1.Validity, ifb1.Nonvalidity, ifb2.Validity, ifb2.Nonvalidity);
			//assert that (validity + nonvalidity > 1.1) ...
			return new IFBool(validity, nonvalidity);
		}

		public override string ToString()
		{
			//dali i indexat da e tuk?
			StringBuilder result = new StringBuilder(mName);
			result.Append(": ");
			result.Append(mTruth.ToString());
			result.Append(", ");
			result.Append(mUntruth.ToString());
			return result.ToString();
		}

		public override bool Equals(object obj)
		{
			Implication impl = obj as Implication;
			if (impl == null)
				return false;
			//return this.Truth == impl.Truth && this.Untruth == impl.Untruth;
			//predi vika6e Expression.AreIdentical...
			return mName.Equals(impl.mName);
		}

		public override int GetHashCode()
		{
			return mName.GetHashCode(); // could be better
		}
	}
}
