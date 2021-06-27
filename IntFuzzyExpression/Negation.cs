using System;
using System.Text;
using IFSTool.ArithmeticExpression;

namespace IFSTool.IntFuzzyExpression
{
	/// <summary>
	/// Summary description for Negation.
	/// </summary>
	public class Negation
	{
		private IArithmeticNode mTruth; // are these names correct?
		private IArithmeticNode mUntruth;
		private string mName;

		public Negation(IArithmeticNode aTruth, IArithmeticNode aUntruth, string aName)
		{
			mTruth = aTruth;
			mUntruth = aUntruth;
			mName = aName;
		}

		public Negation() : this(null, null, "")
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

		public IFBool Evaluate(IFBool argument)
		{
			double validity = mTruth.Evaluate(argument.Validity, argument.Nonvalidity, 0.0, 0.0);
			double nonvalidity = mUntruth.Evaluate(argument.Validity, argument.Nonvalidity, 0.0, 0.0);
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
			Negation negation = obj as Negation;
			if (negation == null)
				return false;
			return mName.Equals(negation.mName);
		}

		public override int GetHashCode()
		{
			return mName.GetHashCode(); // could be better
		}
	}
}
