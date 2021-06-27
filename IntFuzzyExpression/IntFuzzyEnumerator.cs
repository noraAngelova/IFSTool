using System;
using System.Collections;

namespace IFSTool.IntFuzzyExpression
{
	/// <summary>
	/// Summary description for IntFuzzyEnumerator.
	/// </summary>
	public class IntFuzzyEnumerator : IEnumerator
	{
		private int mShots;
		private double mStep;

		private int currentTruthIndex;
		private int currentUntruthIndex;

		public IntFuzzyEnumerator(int aShots)
		{
			if (aShots <= 1)
				throw new ArgumentOutOfRangeException("Number of shots must be at least 2");
			mShots = aShots;
			mStep = 1.0/(mShots - 1);

			Reset();
		}

		public void Reset()
		{
			currentTruthIndex = -1;
			currentUntruthIndex = mShots;
		}

		public bool MoveNext()
		{
			//ami diagonalat da se vzema li? da, pri sravnenie na impl e zadaljitelno!
			if (currentTruthIndex + currentUntruthIndex < mShots)
			{
				if (currentTruthIndex + currentUntruthIndex < mShots - 1)
				{
					currentUntruthIndex++;
				}
				else
				{
					currentTruthIndex++;
					currentUntruthIndex = 0;
				}
				if (currentTruthIndex >= mShots) //gross
					return false;
				return true;
			}
			return false;
		}

		public object Current
		{
			get
			{
				//ami ako sme izvan?
				return new IFBool(mStep * currentTruthIndex, mStep * currentUntruthIndex);
			}
		}
	}
}
