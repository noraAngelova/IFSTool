using System;
using System.Text;
using IFSTool.Expression;

namespace IFSTool.IntFuzzyExpression
{
	public class IntFuzzyContext //code repeat...
	{
		private IFBool[] values;
		private const int CAPACITY = 26;
		private CachedValues mCachedValues;

		public IntFuzzyContext(CachedValues aCachedValues, params IFBool[] values)
		{
			mCachedValues = aCachedValues;//allow nulls

			this.values = new IFBool[CAPACITY];
			int count = values.Length;
			if (count > CAPACITY)
				count = CAPACITY;
			for (int index = 0; index < count; index++)
				this.values[index] = values[index];
		}

		public CachedValues Cache
		{
			get
			{
				return mCachedValues;
			}
		}

		public IFBool this[char variable]
		{
			get
			{
                int index = IndexOf(variable);
				IFBool result = values[index];
				if (result == null)
					//result = new IFBool(0, 1);//?
					throw new ArgumentOutOfRangeException();
				return result;
			}
			set
			{
				values[IndexOf(variable)] = value;
			}
		}

		private int IndexOf(char variable)
		{
			int index = (int)variable - (int)'A'; //mazalo... glavni-malki bukvi...
			return index;
		}
	}

	/// <summary>
	/// Summary description for IntFuzzyNode.
	/// </summary>
	public interface IFNode : INode // bad name: "I" is used for interfaces; why not IfNode?
	{
		IFBool Evaluate(IntFuzzyContext context);
	}

	public class IFConstantNode : IFNode
	{
		private IFBool mValue;

		public IFConstantNode(IFBool aValue)
		{
			mValue = aValue;
		}

		public IFConstantNode(double aTruth, double aUntruth)
		{
			mValue = new IFBool(aTruth, aUntruth);
		}

		#region IFNode Members

		public IFBool Evaluate(IntFuzzyContext context)
		{
			return mValue;
		}

		#endregion

		#region INode Members

		public INode ShallowCopy()
		{
			return new IFConstantNode(mValue);
		}

		public void AddChildRight2Left(INode aNode)
		{
			//Exception...
		}

		public int NumberOfChilds
		{
			get
			{
				return 0;
			}
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			return new IFConstantNode(mValue);
		}

		#endregion

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("<");
			result.Append(mValue.Validity);
			result.Append(",");
			result.Append(mValue.Nonvalidity);
			result.Append('>');
			return result.ToString();
		}

	}


	public class IFVariableNode : IFNode
	{
		private char mVariable;

		public IFVariableNode(char aVariable)
		{
			if (aVariable >= 'A' && aVariable <= 'Z') //leko, ei
			{
				mVariable = aVariable;
			}
			else
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		public IFBool Evaluate(IntFuzzyContext context)
		{
			return context[mVariable];
		}

		public int NumberOfChilds
		{
			get
			{
				return 0;
			}
		}

		public override string ToString()
		{
			return mVariable.ToString();
		}

		public INode ShallowCopy()
		{
			return new IFVariableNode(mVariable);
		}

		public object Clone()
		{
			return new IFVariableNode(mVariable);
		}

		public void AddChildRight2Left(INode aNode)
		{
			//some Exception
		}
	}

	public abstract class IFUnaryNode : IFNode
	{
		private IFNode argument;

		public IFUnaryNode()
		{
			argument = null;
		}

		public IFNode Argument
		{
			get
			{
				return argument;
			}
			set
			{
				argument = value;
			}
		}

		#region IFNode Members

		public abstract IFBool Evaluate(IntFuzzyContext context);

		#endregion

		#region INode Members

		public abstract INode ShallowCopy();

		public void AddChildRight2Left(INode aNode)
		{
			if (argument == null)
				argument = (IFNode)aNode;
			//else exception; also class cast exception
		}

		public int NumberOfChilds
		{
			get
			{
				return 1;
			}
		}

		#endregion

		#region ICloneable Members

		public object Clone()
		{
			IFUnaryNode result = (IFUnaryNode)ShallowCopy();
			result.Argument = (IFNode)argument.Clone();
			return result;
		}

		#endregion

	}

	public class IFNegationNode : IFUnaryNode
	{
		private Negation negation; // cache for them too?

        private int index = -1;

		public Negation Negation
		{
			get
			{
				return negation;
			}
			set
			{
				negation = value;
			}
		}

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        #region IFNode Members

		public override IFBool Evaluate(IntFuzzyContext context)
		{
			IFBool result = null;
            if (context.Cache != null)
                result = context.Cache.GetValue(negation, Argument.Evaluate(context));
            else
				result = negation.Evaluate(Argument.Evaluate(context));

//			tova se testva pri komentirani 3 reda (if, result=, else)
//			IFBool arg = Argument.Evaluate(context);
//			IFBool cached = context.Cache.GetValue(negation, arg);
//			if (! result.Equals(cached))
//			{
//				IFBool a = context['A'];
//				IFBool b = context['B'];
//				IFBool c = context['C'];
//				string neg = negation.ToString();
//			}

			return result;
		}

		#endregion

		#region INode Members

		public override INode ShallowCopy()
		{
			IFNegationNode result = new IFNegationNode();
			result.negation = this.negation;
			result.Argument = this.Argument;
			return result;
		}

		#endregion

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("not");
            if (index >= 0)
                result.Append(index);
            result.Append(' ');
			result.Append(Argument.ToString());
			return result.ToString();
		}
	}

    public class IFNecessityNode : IFUnaryNode
	{
	
        public override IFBool Evaluate(IntFuzzyContext context)
		{
            IFBool argumentValue = Argument.Evaluate(context);
            return new IFBool(argumentValue.Validity, 1 - argumentValue.Validity);
		}

        public override INode ShallowCopy()
        {
            IFNecessityNode result = new IFNecessityNode();
            result.Argument = this.Argument;
            return result;
        }

		public override string ToString()
		{
            StringBuilder result = new StringBuilder("necessity ");
			result.Append(Argument.ToString());
			return result.ToString();
		}

	}



    public class IFPossibilityNode : IFUnaryNode
    {

        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool argumentValue = Argument.Evaluate(context);
            return new IFBool(1 - argumentValue.Nonvalidity, argumentValue.Nonvalidity);
        }

        public override INode ShallowCopy()
        {
            IFPossibilityNode result = new IFPossibilityNode();
            result.Argument = this.Argument;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("possibility ");
            result.Append(Argument.ToString());
            return result.ToString();
        }

    }

    public class IFCircleNode : IFUnaryNode
    {


        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool argumentValue = Argument.Evaluate(context);

            double temp = (argumentValue.Validity + argumentValue.Nonvalidity);

            if (temp == 0)
                temp = 0.1;

            return new IFBool(argumentValue.Validity / temp,
                argumentValue.Nonvalidity / temp);
        }

        public override INode ShallowCopy()
        {
            IFCircleNode result = new IFCircleNode();
            result.Argument = this.Argument;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("* ");
            result.Append(Argument.ToString());
            return result.ToString();
        }

    }

















	public abstract class IFBinaryNode : IFNode
	{
		private IFNode left;
		private IFNode right;

		public IFBinaryNode() : this(null, null)
		{
		}

		public IFBinaryNode(IFNode leftArgument, IFNode rightArgument)
		{
			//if (leftArgument == null || rightArgument == null)
			//	throw new NullReferenceException();
			left = leftArgument;
			right = rightArgument;
		}

		public IFNode LeftArgument
		{
			get
			{
				return left;
			}
			set
			{
				left = value;
			}
		}

		public IFNode RightArgument
		{
			get
			{
				return right;
			}
			set
			{
				left = value;
			}
		}

		public int NumberOfChilds
		{
			get
			{
				return 2;
			}
		}

		public abstract IFBool Evaluate(IntFuzzyContext context);

		public abstract INode ShallowCopy();

		public object Clone()
		{
			IFBinaryNode result = (IFBinaryNode)ShallowCopy();
			result.LeftArgument = (IFNode)left.Clone();
			result.RightArgument = (IFNode)right.Clone();
			return result;
		}

		public void AddChildRight2Left(INode aNode)
		{
			if (right == null)
				right = (IFNode)aNode;
			else if (left == null)
				left = (IFNode)aNode;
			//else some Exception; ili pri class cast ako ne moje...
		}
	}

	public class IFConjunctionNode : IFBinaryNode
	{
		public override IFBool Evaluate(IntFuzzyContext context)
		{
			return LeftArgument.Evaluate(context) & RightArgument.Evaluate(context);
		}

		public override INode ShallowCopy()
		{
			IFConjunctionNode result = new IFConjunctionNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append(" and ");
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}
	}

    public class IFMultiplyConjunctionNode : IFBinaryNode
    {
        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool left = LeftArgument.Evaluate(context);
            IFBool right = RightArgument.Evaluate(context);
            return new IFBool(left.Validity * right.Validity,
                left.Nonvalidity + right.Nonvalidity - left.Nonvalidity * right.Nonvalidity);
        }

        public override INode ShallowCopy()
        {
            IFMultiplyConjunctionNode result = new IFMultiplyConjunctionNode();
            result.LeftArgument = this.LeftArgument;
            result.RightArgument = this.RightArgument;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("(");
            result.Append(LeftArgument.ToString());
            result.Append(" m_and ");
            result.Append(RightArgument.ToString());
            result.Append(')');
            return result.ToString();
        }
    }

	public class IFDisjunctionNode : IFBinaryNode
	{
		public override IFBool Evaluate(IntFuzzyContext context)
		{
			return LeftArgument.Evaluate(context) | RightArgument.Evaluate(context);
		}

		public override INode ShallowCopy()
		{
			IFDisjunctionNode result = new IFDisjunctionNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append(" or ");
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}
	}

	public class IFImplicationNode : IFBinaryNode
	{
		private Implication implication;

        private int index = -1;

		public Implication Implication
		{
			get { return implication; }
			set { implication = value; }
		}

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

		public override IFBool Evaluate(IntFuzzyContext context)
		{
			IFBool result = null;
			if (context.Cache != null)
				result = context.Cache.GetValue(implication, LeftArgument.Evaluate(context), RightArgument.Evaluate(context));
			else
				result = implication.Evaluate(LeftArgument.Evaluate(context), RightArgument.Evaluate(context));

//			tova se testva pri komentirani 3 reda (if, result=, else)
            
            //IFBool arg1 = LeftArgument.Evaluate(context);
            //IFBool arg2 = RightArgument.Evaluate(context);
            //IFBool cached = context.Cache.GetValue(implication, arg1, arg2);
            //IFBool cached2 = context.Cache.GetValue(implication, arg2, arg1);

            //if (cached.IsIntFuzzyTautology) {
            //    Console.WriteLine(implication.Name);
            //}

            //if (cached.IsIntFuzzyTautology && cached2.IsIntFuzzyTautology && !arg1.Equals(arg2))
            //{
            //    Console.WriteLine(implication.Name);
            //}

//			if (! result.Equals(cached))
//			{
//				IFBool a = context['A'];
//				IFBool b = context['B'];
//				IFBool c = context['C'];
//				string imp = implication.ToString();
//			}

			// return new IFBool(1,0);
            return result;
		}

		public override INode ShallowCopy()
		{
			IFImplicationNode result = new IFImplicationNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			result.implication = implication;
            result.index = index;
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
            if (index >= 0)
                result.Append(" -->")
                    .Append(index)
                    .Append(' ');
            else
                result.Append(" --> ");
			result.Append(RightArgument.ToString())
			    .Append(')');
			return result.ToString();
		}
	}

	//tiq ne sa ot nai-normalnite
	public class IFEqualNode : IFBinaryNode
	{
		public override IFBool Evaluate(IntFuzzyContext context)
		{
			IFBool left = LeftArgument.Evaluate(context);
			IFBool right = RightArgument.Evaluate(context);
			if (Math.Abs(left.Validity - right.Validity) < Constants.EPSILON
				&& Math.Abs(left.Nonvalidity - right.Nonvalidity) < Constants.EPSILON)
				return new IFBool(1, 0);
			else
				return new IFBool(0, 1);
		}

		public override INode ShallowCopy()
		{
			IFEqualNode result = new IFEqualNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append(" = ");
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}		
	}

    public class IFGeqNode : IFBinaryNode
    {
        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool left = LeftArgument.Evaluate(context);
            IFBool right = RightArgument.Evaluate(context);
            if (left.Validity >= right.Validity - Constants.EPSILON
                && left.Nonvalidity <= right.Nonvalidity + Constants.EPSILON)
                return new IFBool(1, 0);
            else
                return new IFBool(0, 1);
        }

        public override INode ShallowCopy()
        {
            IFGeqNode result = new IFGeqNode();
            result.LeftArgument = this.LeftArgument;
            result.RightArgument = this.RightArgument;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("(");
            result.Append(LeftArgument.ToString());
            result.Append(" >= ");
            result.Append(RightArgument.ToString());
            result.Append(')');
            return result.ToString();
        }
    }

    public class IFLeqNode : IFBinaryNode
    {
        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool left = LeftArgument.Evaluate(context);
            IFBool right = RightArgument.Evaluate(context);
            if (left.Validity <= right.Validity + Constants.EPSILON
                && left.Nonvalidity >= right.Nonvalidity - Constants.EPSILON)
                return new IFBool(1, 0);
            else
                return new IFBool(0, 1);
        }

        public override INode ShallowCopy()
        {
            IFLeqNode result = new IFLeqNode();
            result.LeftArgument = this.LeftArgument;
            result.RightArgument = this.RightArgument;
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("(");
            result.Append(LeftArgument.ToString());
            result.Append(" <= ");
            result.Append(RightArgument.ToString());
            result.Append(')');
            return result.ToString();
        }
    }

    public class IFClassicNegationNode : IFUnaryNode
    {
        #region IFNode Members

        public override IFBool Evaluate(IntFuzzyContext context)
        {
            IFBool result = Argument.Evaluate(context);

            //double d = result.Validity;
            //result.Validity = result.Nonvalidity;
            //result.Nonvalidity = d;
            //return result;

            return new IFBool(result.Nonvalidity, result.Validity);
        }

        #endregion

        #region INode Members

        public override INode ShallowCopy()
        {
            IFClassicNegationNode result = new IFClassicNegationNode();
            result.Argument = this.Argument;
            return result;
        }

        #endregion

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("classic_not ");
            result.Append(Argument.ToString());
            return result.ToString();
        }
    }

    //public class IFCrispNode : IFUnaryNode //<a,b> -> <1,0> or <0,1>
    //{
    //    #region IFNode Members

    //    public override IFBool Evaluate(IntFuzzyContext context)
    //    {
    //        IFBool argumentValue = Argument.Evaluate(context);
    //        if (argumentValue.IsIntFuzzyTautology)
    //            return new IFBool(1.0, 0.0);
    //        else
    //            return new IFBool(0.0, 1.0);
    //    }

    //    #endregion

    //    #region INode Members

    //    public override INode ShallowCopy()
    //    {
    //        IFCrispNode result = new IFCrispNode();
    //        result.Argument = this.Argument;
    //        return result;
    //    }

    //    #endregion

    //    public override string ToString()
    //    {

    //        StringBuilder result = new StringBuilder("crisp ");
    //        result.Append(Argument.ToString());
    //        return result.ToString();
    //    }
    //}
}
