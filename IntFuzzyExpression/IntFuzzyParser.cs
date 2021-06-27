using System;
using System.Collections;
using IFSTool.Expression;
using System.Globalization;

namespace IFSTool.IntFuzzyExpression
{
	/// <summary>
	/// Summary description for IntFuzzyParser.
	/// </summary>
	public class IntFuzzyParser : Parser
	{
		private IDictionary mNodes; //<string, IArithmeticNode>

		public IntFuzzyParser()
		{
			mNodes = new Hashtable();

			mNodes.Add("\\supset", new IFImplicationNode());
            mNodes.Add("\\ri", new IFImplicationNode());
            mNodes.Add("\\&", new IFConjunctionNode());
            mNodes.Add("\\cap", new IFConjunctionNode());
			mNodes.Add("\\vee", new IFDisjunctionNode());
            mNodes.Add("\\mul_conj", new IFMultiplyConjunctionNode());
            //TODO: cap, cup? wege?
            mNodes.Add("\\neg", new IFNegationNode());
            mNodes.Add("=", new IFEqualNode());
            mNodes.Add(">=", new IFGeqNode());
            mNodes.Add("<=", new IFLeqNode());
            mNodes.Add("\\classicnot", new IFClassicNegationNode());
            mNodes.Add("\\necessity", new IFNecessityNode());
            mNodes.Add("\\possibility", new IFPossibilityNode());
            mNodes.Add("\\circle", new IFCircleNode());
            
            //mNodes.Add("\\crisp", new IFCrispNode());
            //for (int i = 0; i < 1000; ++i)
            //{
            //    IFImplicationNode impl = new IFImplicationNode();
            //    impl.Index = i;
            //    mNodes.Add("\\ri_" + i, impl);

            //    IFImplicationNode impl = new IFImplicationNode();
            //    impl.Index = i;
            //    mNodes.Add("\\supset_" + i, impl);
            //  TODO: negations too
            //}
		}

        protected override bool IsPrefixOperator(string aToken)
		{
			if (aToken.Equals("\\neg") || aToken.Equals("\\classicnot")
                || aToken.StartsWith("\\neg_")
                || aToken.StartsWith("\\necessity")
                || aToken.StartsWith("\\possibility")
                || aToken.StartsWith("\\circle")
                /* || aToken.Equals("\\crisp")*/)
				return true;
			return base.IsPrefixOperator (aToken);
		}


		protected override int Weight(string aToken)
		{
			switch (aToken)//i tuk po-barzo moje bi 6te e s hashtable
			{
                case "\\supset":
                case "\\ri":
                case "=":
                case ">=":
                case "<=": return 4; //dali e taka
				case "\\vee": return 3;
				case "\\&": case "\\cap": case "\\mul_conj": return 2;
				case "\\neg": case "\\classicnot": case "\\necessity": case "\\possibility": case "\\circle": /*case "\\crisp":*/ return 1; //moje i 0
			}
            if (aToken.StartsWith("\\ri_") || aToken.StartsWith("\\supset_"))
                return 4;
            if (aToken.StartsWith("\\neg_"))
                return 1;
            return base.Weight(aToken);
		}

		protected override string NextToken(string aExpression, ref int aIndex)
		{
			//mazno povtorenie
			if (aIndex >= aExpression.Length)
				return null;
			while (aIndex < aExpression.Length && aExpression[aIndex] == ' ')
				aIndex++;
			if (aIndex >= aExpression.Length)
				return null;

            if (aExpression[aIndex] == '>' && aExpression[aIndex + 1] == '=')
            {
                aIndex += 2;
                return ">=";
            }
            if (aExpression[aIndex] == '<' && aExpression[aIndex + 1] == '=')
            {
                aIndex += 2;
                return "<=";
            }

			if (aExpression[aIndex] == '<')
			{
				int start = aIndex;
				aIndex = aExpression.IndexOf('>', start + 1) + 1;
				return aExpression.Substring(start, aIndex - start);
			}

			if (aExpression[aIndex] == '\\' && aExpression[aIndex + 1] == '&')
			{
				aIndex += 2;
				return "\\&";
			}
			return base.NextToken (aExpression, ref aIndex);
		}


		protected override INode NodeFactory(string aToken)
		{
			if (aToken[0] == '<' && aToken[aToken.Length - 1] == '>')
			{
				//aToken = aToken.Trim('<', '>');
				int commaIndex = aToken.IndexOf(',');
                double truth = Double.Parse(aToken.Substring(1, commaIndex - 1),CultureInfo.InvariantCulture);
                string untruthStr = aToken.Substring(commaIndex + 1, aToken.Length - commaIndex - 2);
                double untruth = double.Parse(untruthStr, CultureInfo.InvariantCulture);
				return new IFConstantNode(truth, untruth);
			}

            if (aToken.StartsWith("\\ri_") || aToken.StartsWith("\\supset_"))
            {
                IFImplicationNode impl = new IFImplicationNode();
                impl.Index = Int32.Parse(aToken.Substring(aToken.IndexOf('_') + 1));
                return impl;
            }

            if (aToken.StartsWith("\\neg_"))
            {
                IFNegationNode neg = new IFNegationNode();
                neg.Index = Int32.Parse(aToken.Substring(aToken.IndexOf('_') + 1));
                return neg;
            }


			INode result = (INode)mNodes[aToken];
			//if no such function, return a variable
			if (result == null)
			{
				if (Char.IsLetter(aToken[0]))
					return new IFVariableNode(aToken[0]); // single letter names for now
				else
					return null;
			}
			return result.ShallowCopy();
		}
	}
}
