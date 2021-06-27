using System;
using System.Text;
using IFSTool.Expression;

namespace IFSTool.ArithmeticExpression
{
	public class ArithmeticContext //code repeat...
	{
		private double[] values;
		private const int CAPACITY = 26;

		public ArithmeticContext(params double[] values)
		{
			this.values = new double[CAPACITY];
			int count = values.Length;
			if (count > CAPACITY)
				count = CAPACITY;
			for (int index = 0; index < count; index++)
				this.values[index] = values[index];
		}

		public double this[char variable]
		{
			get
			{
				double result = values[IndexOf(variable)];
				return result;
			}
			set
			{
				values[IndexOf(variable)] = value;
			}
		}

		private int IndexOf(char variable)
		{
			int index = (int)variable - (int)'a'; //mazalo... glavni-malki bukvi...
			return index;
		}
	}
	
	public interface IArithmeticNode : INode
	{
		double Evaluate(ArithmeticContext aContext);
		double Evaluate(double a, double b, double c, double d);
	}

	public class ConstantNode : IArithmeticNode
	{
		private double mValue;

		public ConstantNode(double aValue)
		{
            //if (aValue >= 0 - Constants.EPSILON && aValue <= 1 + Constants.EPSILON)
			mValue = aValue;
            //else throw new ArgumentOutOfRangeException();
		}

		public ConstantNode() : this(0.0)
		{
		}

		public double Evaluate(ArithmeticContext aContext)
		{
			return mValue;
		}

		public double Evaluate(double a, double b, double c, double d)
		{
			return mValue;
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
			return mValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		public INode ShallowCopy()
		{
			return new ConstantNode(mValue);
		}

		public object Clone()
		{
			return new ConstantNode(mValue);
		}

		public void AddChildRight2Left(INode aNode)
		{
			//some Exception
		}
	}

	public class VariableNode : IArithmeticNode
	{
		private char mVariable;

		public VariableNode(char aVariable)
		{
			Variable = aVariable;
		}

		public char Variable
		{
			get
			{
				return mVariable;
			}
			set
			{
				if (value >= 'a' && value <= 'z') //was 'd'
				{
					mVariable = value;
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public double Evaluate(ArithmeticContext aContext)
		{
			return aContext[mVariable];
		}

		public double Evaluate(double a, double b, double c, double d)
		{
			switch (mVariable)
			{
				case 'a' : return a;
				case 'b' : return b;
				case 'c' : return c;
				case 'd' : return d;
			}
			return 0;
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
			return new VariableNode(mVariable);
		}

		public object Clone()
		{
			return new VariableNode(mVariable);
		}

		public void AddChildRight2Left(INode aNode)
		{
			//some Exception
		}
	}

	public abstract class UnaryNode : IArithmeticNode
	{
		private IArithmeticNode mArgument;

		public UnaryNode(IArithmeticNode aArgument)
		{
			mArgument = aArgument;
		}

		public UnaryNode() : this(null)
		{
		}

		public IArithmeticNode Argument
		{
			get
			{
				return mArgument;
			}
			set
			{
				mArgument = value;
			}
		}

		public object Clone()
		{
			object result = this.ShallowCopy();
			((UnaryNode)result).Argument = (IArithmeticNode)Argument.Clone();
			return result;
		}

		public int NumberOfChilds
		{
			get
			{
				return 1;
			}
		}

		public void AddChildRight2Left(INode aNode)
		{
			if (mArgument == null)
				mArgument = (IArithmeticNode)aNode;
			//else some Exception; as in ClassCastExc...
		}

		public abstract double Evaluate(ArithmeticContext aContext);

		public abstract double Evaluate(double a, double b, double c, double d);

		public abstract INode ShallowCopy();
	}

	//ima i MinusNode, napr. -a

	public class SgNode : UnaryNode
	{
		public SgNode(IArithmeticNode aNode) : base(aNode)
		{
		}

		public SgNode() : this(null) {}

		public override double Evaluate(ArithmeticContext aContext)
		{
			double argumentValue = Argument.Evaluate(aContext);
			return argumentValue > Constants.EPSILON ? 1 : 0;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double argumentValue = Argument.Evaluate(a, b, c, d);
			return argumentValue > Constants.EPSILON ? 1 : 0;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("sg");
			if (Argument is VariableNode || Argument is ConstantNode || Argument is MaxNode || Argument is MinNode)
			{
				result.Append('(');
			}
			result.Append(Argument.ToString());
            if (Argument is VariableNode || Argument is ConstantNode || Argument is MaxNode || Argument is MinNode)
			{
				result.Append(')');
			}
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			SgNode result = new SgNode();
			result.Argument = this.Argument;
			return result;
			//posle opravi i ostanalite, daje polzvai i constructors...
		}

	}
	
	public class OsNode : UnaryNode //too much code repeat!
	{
		public OsNode(IArithmeticNode aNode) : base(aNode)
		{
		}

		public OsNode() : this(null) {}

		public override double Evaluate(ArithmeticContext aContext)
		{
			double argumentValue = Argument.Evaluate(aContext);
			return argumentValue <= Constants.EPSILON ? 1 : 0;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double argumentValue = Argument.Evaluate(a, b, c, d);
			return argumentValue <= Constants.EPSILON ? 1 : 0;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("os");
            if (Argument is VariableNode || Argument is ConstantNode || Argument is MaxNode || Argument is MinNode)
			{
				result.Append('(');
			}
			result.Append(Argument.ToString());
            if (Argument is VariableNode || Argument is ConstantNode || Argument is MaxNode || Argument is MinNode)
			{
				result.Append(')');
			}
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			OsNode result = new OsNode();
			result.Argument = this.Argument;
			return result;
		}

	}
	
	public class SquareNode : UnaryNode
	{
		public override double Evaluate(ArithmeticContext aContext)
		{
			double argumentValue = Argument.Evaluate(aContext);
			return argumentValue <= Constants.EPSILON ? 1 : 0;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double argumentValue = Argument.Evaluate(a, b, c, d);
			return argumentValue * argumentValue;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder(Argument.ToString());
			result.Append("^2");//brackets not needed
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			SquareNode result = new SquareNode();
			result.Argument = this.Argument;
			return result;
		}
	}
	
	public abstract class BinaryNode : IArithmeticNode
	{
		private IArithmeticNode mLeftArgument;
		private IArithmeticNode mRightArgument;

		public BinaryNode(IArithmeticNode aLeftArgument, IArithmeticNode aRightArgument)
		{
			mLeftArgument = aLeftArgument;
			mRightArgument = aRightArgument;
		}

		public BinaryNode() : this(null, null)
		{
		}

		public IArithmeticNode LeftArgument
		{
			get
			{
				return mLeftArgument;
			}
			set
			{
				mLeftArgument = value;
			}
		}

		public IArithmeticNode RightArgument
		{
			get
			{
				return mRightArgument;
			}
			set
			{
				mRightArgument = value;
			}
		}

		public int NumberOfChilds
		{
			get
			{
				return 2;
			}
		}

		public object Clone()
		{
			BinaryNode result = (BinaryNode)(this.ShallowCopy());
			result.LeftArgument = (IArithmeticNode)LeftArgument.Clone();
			result.RightArgument = (IArithmeticNode)RightArgument.Clone();
			return result;
		}

		public void AddChildRight2Left(INode aNode)
		{
			if (mRightArgument == null)
				mRightArgument = (IArithmeticNode)aNode;
			else if (mLeftArgument == null)
				mLeftArgument = (IArithmeticNode)aNode;
			//else some Exception; ili pri class cast ako ne moje...
		}

		public abstract double Evaluate(ArithmeticContext aContext);

		public abstract double Evaluate(double a, double b, double c, double d);

		public abstract INode ShallowCopy();
	}

	public class PlusNode : BinaryNode
	{
        public PlusNode(IArithmeticNode aLeft, IArithmeticNode aRight)
        {
            LeftArgument = aLeft;
            RightArgument = aRight;
        }

        public PlusNode()
        {
        }

        public override double Evaluate(ArithmeticContext aContext)
		{
			double result = LeftArgument.Evaluate(aContext);
			result += RightArgument.Evaluate(aContext);
			//if doesn't fit in [0,1] +- EPSILON - Exception! leko vse pak, nali ima i 2-ki, i kvo li ne o6te
			return result;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double result = LeftArgument.Evaluate(a, b, c, d);
			result += RightArgument.Evaluate(a, b, c, d);
			//if doesn't fit in [0,1] +- EPSILON - Exception! leko vse pak, nali ima i 2-ki, i kvo li ne o6te
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append('+');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			PlusNode result = new PlusNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

	public class MinusNode : BinaryNode
	{
        public MinusNode(IArithmeticNode aLeft, IArithmeticNode aRight)
        {
            LeftArgument = aLeft;
            RightArgument = aRight;
        }

        public MinusNode()
        {
        }

		public override double Evaluate(ArithmeticContext aContext)
		{
			double result = LeftArgument.Evaluate(aContext);
			result -= RightArgument.Evaluate(aContext);
			//if doesn't fit in [0,1] +- EPSILON - Exception! leko vse pak, nali ima i 2-ki, i kvo li ne o6te
			return result;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double result = LeftArgument.Evaluate(a, b, c, d);
			result -= RightArgument.Evaluate(a, b, c, d);
			//if doesn't fit in [0,1] +- EPSILON - Exception! (vij belejkata po-gore)
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append('-');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			MinusNode result = new MinusNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

	public class MultiplyNode : BinaryNode
	{
        public MultiplyNode()
        {
        }

        public MultiplyNode(IArithmeticNode aLeft, IArithmeticNode aRight)
        {
            LeftArgument = aLeft;
            RightArgument = aRight;
        }

		public override double Evaluate(ArithmeticContext aContext)
		{
			double result = LeftArgument.Evaluate(aContext);
			if (result != 0.0)
				result *= RightArgument.Evaluate(aContext);
			return result;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double result = LeftArgument.Evaluate(a, b, c, d);
			if (result != 0.0)
				result *= RightArgument.Evaluate(a, b, c, d);
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append('*');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			MultiplyNode result = new MultiplyNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

    public class DivideNode : BinaryNode
    {
        public DivideNode()
        {
        }
        
        public DivideNode(IArithmeticNode aLeft, IArithmeticNode aRight)
        {
            LeftArgument = aLeft;
            RightArgument = aRight;
        }

        public override double Evaluate(ArithmeticContext aContext)
        {
            double numerator = LeftArgument.Evaluate(aContext);
            double denominator = RightArgument.Evaluate(aContext);
            if (denominator == 0.0)
            {
                if (numerator >= 0 - Constants.EPSILON)
                    return Double.PositiveInfinity;
                else
                    return Double.NegativeInfinity;
            }
            else
            {
                return numerator / denominator;
            }
        }

        public override double Evaluate(double a, double b, double c, double d)
        {
            double numerator = LeftArgument.Evaluate(a, b, c, d);
            double denominator = RightArgument.Evaluate(a, b, c, d);
            if (denominator == 0.0)
            {
                if (numerator > 0)
                    return Double.PositiveInfinity;
                else if (numerator < 0)
                    return Double.NegativeInfinity;
                else return 0.0; // are you sure?
            }
            else
            {
                return numerator / denominator;
            }
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder("(");
            result.Append(LeftArgument.ToString());
            result.Append('/');
            result.Append(RightArgument.ToString());
            result.Append(')');
            return result.ToString();
        }

        public override INode ShallowCopy()
        {
            DivideNode result = new DivideNode();
            result.LeftArgument = this.LeftArgument;
            result.RightArgument = this.RightArgument;
            return result;
        }
    }

	public class PowerNode : BinaryNode
	{
		public override double Evaluate(ArithmeticContext aContext)
		{
			double result = LeftArgument.Evaluate(aContext);
			result = Math.Pow(result, RightArgument.Evaluate(aContext));
			return result;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double result = LeftArgument.Evaluate(a, b, c, d);
			result = Math.Pow(result, RightArgument.Evaluate(a, b, c, d));
			return result;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append('^');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			PowerNode result = new PowerNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

	public class MaxNode : BinaryNode
	{
		public MaxNode(IArithmeticNode aLeft, IArithmeticNode aRight)
		{
			LeftArgument = aLeft;
			RightArgument = aRight;
		}

		public MaxNode()
		{
		}

		public override double Evaluate(ArithmeticContext aContext)
		{
			double leftValue = LeftArgument.Evaluate(aContext);
			double rightValue = RightArgument.Evaluate(aContext);
			if (leftValue > rightValue) //EPSILON not needed here
				return leftValue;
			else
				return rightValue;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double leftValue = LeftArgument.Evaluate(a, b, c, d);
			double rightValue = RightArgument.Evaluate(a, b, c, d);
			if (leftValue > rightValue) //EPSILON not needed here
				return leftValue;
			else
				return rightValue;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("max(");
			result.Append(LeftArgument.ToString());
			result.Append(',');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			MaxNode result = new MaxNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

	public class MinNode : BinaryNode
	{
		public MinNode(IArithmeticNode aLeft, IArithmeticNode aRight)
		{
			LeftArgument = aLeft;
			RightArgument = aRight;
		}

		public MinNode()
		{
		}

		public override double Evaluate(ArithmeticContext aContext)
		{
			double leftValue = LeftArgument.Evaluate(aContext);
			double rightValue = RightArgument.Evaluate(aContext);
			if (leftValue < rightValue) //EPSILON not needed here
				return leftValue;
			else
				return rightValue;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double leftValue = LeftArgument.Evaluate(a, b, c, d);
			double rightValue = RightArgument.Evaluate(a, b, c, d);
			if (leftValue < rightValue) //EPSILON not needed here
				return leftValue;
			else
				return rightValue;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("min(");
			result.Append(LeftArgument.ToString());
			result.Append(',');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			MinNode result = new MinNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
	}

    public class EqualNode : BinaryNode
    {
		public EqualNode(IArithmeticNode aLeft, IArithmeticNode aRight)
		{
			LeftArgument = aLeft;
			RightArgument = aRight;
		}

		public EqualNode()
		{
		}

		public override double Evaluate(ArithmeticContext aContext)
		{
			double leftValue = LeftArgument.Evaluate(aContext);
			double rightValue = RightArgument.Evaluate(aContext);
			if (Math.Abs(leftValue - rightValue) < Constants.EPSILON)
				return 1.0;
			else
				return 0.0;
		}

		public override double Evaluate(double a, double b, double c, double d)
		{
			double leftValue = LeftArgument.Evaluate(a, b, c, d);
			double rightValue = RightArgument.Evaluate(a, b, c, d);
            if (Math.Abs(leftValue - rightValue) < Constants.EPSILON)
                return 1.0;
            else
                return 0.0;
        }

		public override string ToString()
		{
			StringBuilder result = new StringBuilder("(");
			result.Append(LeftArgument.ToString());
			result.Append('=');
			result.Append(RightArgument.ToString());
			result.Append(')');
			return result.ToString();
		}

		public override INode ShallowCopy()
		{
			EqualNode result = new EqualNode();
			result.LeftArgument = this.LeftArgument;
			result.RightArgument = this.RightArgument;
			return result;
		}
    }
}
