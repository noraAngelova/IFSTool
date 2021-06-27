using System;
using System.Text;
using System.Collections;
using IFSTool.Expression;

namespace IFSTool.ArithmeticExpression
{
	/// <summary>
	/// Summary description for ArithmeticParser.
	/// </summary>
	public class ArithmeticParser : Parser
	{
		private IDictionary mNodes; //<string, IArithmeticNode>

		public ArithmeticParser()
		{
			mNodes = new Hashtable();

			mNodes.Add("sg", new SgNode());
			mNodes.Add("os", new OsNode());
			//za SquareNode 6te si trqbva malko logika
			mNodes.Add("+", new PlusNode());
			mNodes.Add("-", new MinusNode());
			MultiplyNode multiply = new MultiplyNode();
			mNodes.Add("*", multiply);
			mNodes.Add(".", multiply); //tova moje da se sbarka s deseti4nata zapetaika!
            mNodes.Add("/", new DivideNode());
			mNodes.Add("^", new PowerNode());
			mNodes.Add("min", new MinNode());
			mNodes.Add("max", new MaxNode());
		}

		protected override int Weight(string aToken)
		{
			switch(aToken)//i tuk po-barzo moje bi 6te e s hashtable
			{
				case "+": return 3;
				case "-": return 3;
				case "*": case ".": return 2; //tova moje da se sbarka s deseti4nata zapetaika!
                case "/": return 2;
				case "^": return 1;
			}
			return base.Weight(aToken);
		}

		protected override INode GenerateSubTree(Stack aReversePolish)
		{
			string token = (string)aReversePolish.Peek();

			if (token.Equals("^"))
			{
				aReversePolish.Pop();
				if (aReversePolish.Peek().Equals("2"))
				{
					INode result = new SquareNode();
					aReversePolish.Pop();
					result.AddChildRight2Left(GenerateSubTree(aReversePolish));
					return result;
				}
				else aReversePolish.Push("^");
			}

			return base.GenerateSubTree (aReversePolish);
		}

		protected override INode NodeFactory(string aToken)
		{
			if (Char.IsDigit(aToken[0]))
				return new ConstantNode(Double.Parse(aToken, System.Globalization.CultureInfo.InvariantCulture));

			INode result = (INode)mNodes[aToken];
			//if no such function, return a variable
			if (result == null)
			{
				if (Char.IsLetter(aToken[0]))
					return new VariableNode(aToken[0]); //zasega ednobukveni samo!
				else
					return null;
			}
			return result.ShallowCopy();
		}
	}
}
