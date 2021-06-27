using System;
using System.Text;
using System.Diagnostics;
using IFSTool.Expression;
using IFSTool.ArithmeticExpression;

namespace IFSTool.IntFuzzyExpression
{
	public class IFBool
	{
		private double mValidity; // are these names correct?
		private double mNonvalidity;

		public IFBool(double aValidity, double aNonvalidity)
		{
			Validity = aValidity;
			Nonvalidity = aNonvalidity;
            if (!(mValidity + mNonvalidity <= 1 + Constants.EPSILON)) {
                Debug.Assert(mValidity + mNonvalidity <= 1 + Constants.EPSILON);
            }
		}

		public double Validity
		{
			get
			{
                Debug.Assert(mValidity + mNonvalidity <= 1 + Constants.EPSILON);
				return mValidity;
			}
			set
			{
				if (value >= 0 - Constants.EPSILON && value <= 1 + Constants.EPSILON)
				{
					mValidity = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public double Nonvalidity
		{
			get
			{
                Debug.Assert(mValidity + mNonvalidity <= 1 + Constants.EPSILON);
				return mNonvalidity;
			}
			set
			{
				if (value >= 0 - Constants.EPSILON && value <= 1 + Constants.EPSILON)
				{
					mNonvalidity = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public bool IsTautology
		{
			get 
			{
				bool result = Math.Abs(mValidity - 1.0) <= Constants.EPSILON &&
					Math.Abs(mNonvalidity) <= Constants.EPSILON;
				return result;
			}
		}

		public bool IsIntFuzzyTautology
		{
			get 
			{
				return mValidity >= mNonvalidity - Constants.EPSILON;
			}
		}

		public static bool operator <= (IFBool ifb1, IFBool ifb2)
		{
			//vij dobre!
			return ifb1.Validity <= ifb2.Validity + Constants.EPSILON
				&& ifb1.Nonvalidity >= ifb2.Nonvalidity - Constants.EPSILON;
		}

		public static bool operator >= (IFBool ifb1, IFBool ifb2)
		{
			//vij o6te po-dobre!
			return ifb1.Validity >= ifb2.Validity - Constants.EPSILON
				&& ifb1.Nonvalidity <= ifb2.Nonvalidity + Constants.EPSILON;
		}

		public static IFBool operator & (IFBool ifb1, IFBool ifb2) //& e za pobitovi, ama aide
		{
			double validity = ifb1.Validity < ifb2.Validity ? ifb1.Validity : ifb2.Validity;
			double nonvalidity = ifb1.Nonvalidity > ifb2.Nonvalidity ? ifb1.Nonvalidity : ifb2.Nonvalidity;
			return new IFBool(validity, nonvalidity);
		}

		public static IFBool operator | (IFBool ifb1, IFBool ifb2) //| e za pobitovi, ama aide
		{
			double validity = ifb1.Validity > ifb2.Validity ? ifb1.Validity : ifb2.Validity;
			double nonvalidity = ifb1.Nonvalidity < ifb2.Nonvalidity ? ifb1.Nonvalidity : ifb2.Nonvalidity;
			return new IFBool(validity, nonvalidity);
		}

		public override bool Equals(object obj)
		{
			if (! (obj is IFBool))
				return false;
			IFBool ifb = (IFBool)obj;
			return Math.Abs(mValidity - ifb.Validity) < Constants.EPSILON
				&& Math.Abs(mNonvalidity - ifb.Nonvalidity) < Constants.EPSILON;
		}

		public override int GetHashCode()
		{
			return mValidity.GetHashCode() ^ mNonvalidity.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<");
			result.Append(mValidity);//globalization...
			result.Append(", ");
			result.Append(mNonvalidity);
			result.Append('>');
			return result.ToString();
		}

	}
}
