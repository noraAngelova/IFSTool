using System;
using System.Collections;

namespace IFSTool.ArithmeticExpression
{
	public enum Relation
	{
		Equal,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
	}

	public class Bound
	{
		private IArithmeticNode mLeft;
		private IArithmeticNode mRight;
		private Relation mRelation;

		public Bound(IArithmeticNode aLeft, IArithmeticNode aRight, Relation aRelation)
		{
			if (aLeft is VariableNode && (aRight is ConstantNode || aRight is VariableNode))
			{
				mLeft = aLeft;
				mRight = aRight;
				mRelation = aRelation;
			}
			else
				throw new ArgumentException("Bounds must be of type 'Variable R Variable' or 'Variable R Constant'");
		}

		public IArithmeticNode Left
		{
			get { return mLeft; }
		}

		public IArithmeticNode Right
		{
			get { return mRight; }
		}

		public Relation Relation
		{
			get { return mRelation; }
		}
	}

	/// <summary>
	/// Summary description for Bounds.
	/// </summary>
	public class Bounds
	{
		private IList mBounds;

		public Bounds()
		{
			mBounds = new ArrayList();
		}

		public void Add(Bound aBound)
		{
			//proverki... da ne e nevazmojno - imame x<=0 i dobavqme x>=1
			mBounds.Add(aBound);
		}
	}
}
