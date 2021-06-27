using System;
using IFSTool.Expression;
using IFSTool.ArithmeticExpression;

namespace IFSTool.IntFuzzyExpression
{
	/// <summary>
	/// Summary description for IFExpressionUtil.
	/// </summary>
	public class IFExpressionUtil
	{
		public static Negation GenerateNegation(Implication implication)
		{
			Negation result = new Negation(ReplaceCDWith01(implication.Truth),
				ReplaceCDWith01(implication.Untruth), implication.Name); //predi pravehme "Neg"+, no taka 6te uslojnim rabotata s ke6a
			return result;
		}

		/*
		 * Impl->Neg: Impl(<a,b>, <0,1>)
		 * Neg->Impl: not A or B
		 * Neg->Impl, intuitionistic variant: not A or not not B
		 */

		private static IArithmeticNode ReplaceCDWith01(IArithmeticNode node)
		{
			IArithmeticNode result = null;

			if (node is VariableNode && ((VariableNode)node).Variable == 'c')
			{
				result = new ConstantNode(0.0);
			}
			else if (node is VariableNode && ((VariableNode)node).Variable == 'd')
			{
				result = new ConstantNode(1.0);
			}
			else
			{
				result = (IArithmeticNode)node.ShallowCopy();
				if (node is UnaryNode)
				{
					UnaryNode unaryNode = (UnaryNode)result;
					unaryNode.Argument = ReplaceCDWith01(unaryNode.Argument);
				}
				else if (node is BinaryNode)
				{
					BinaryNode binaryNode = (BinaryNode)result;
					binaryNode.LeftArgument = ReplaceCDWith01(binaryNode.LeftArgument);
					binaryNode.RightArgument = ReplaceCDWith01(binaryNode.RightArgument);
				}
			}

			return result;
		}

		//public static Implication GenerateImplication(Negation)
		//ima 2 varianta oba4e!

        private static Negation classicIFNegation = new Negation(new VariableNode('b'), new VariableNode('a'), "classic_not");

		public static void ConvertToArithmetic(IFNode aNode, out IArithmeticNode aTruth, out IArithmeticNode aUntruth)
		{
			aTruth = null;
			aUntruth = null;

			if (aNode is IFConstantNode)
			{
				IFBool ifb = aNode.Evaluate(null);//dano ne gramne
				aTruth = new ConstantNode(ifb.Validity);
				aUntruth = new ConstantNode(ifb.Nonvalidity);
			}
			else if (aNode is IFVariableNode)
			{
				char var = aNode.ToString()[0];
				int number = (int)var - (int)'A';
				aTruth = new VariableNode((char)(number * 2 + (int)'a'));
				aUntruth = new VariableNode((char)(number * 2 + 1 + (int)'a'));
			}
			else if (aNode is IFNegationNode || aNode is IFClassicNegationNode)
			{
				IArithmeticNode truth, untruth;
				ConvertToArithmetic(((IFUnaryNode)aNode).Argument, out truth, out untruth);
				
                Negation negation;
                if (aNode is IFNegationNode)
                    negation = ((IFNegationNode)aNode).Negation;
                else negation = classicIFNegation; // be careful, same instance is used
				aTruth = negation.Truth;//zasega nqma clone, no vnimavai vse pak; inak ReplaceVars vra6ta nov obekt, ne buta podadeniq
				aUntruth = negation.Untruth;
				aTruth = ExpressionUtil.ReplaceVars(aTruth, truth, untruth);
				aUntruth = ExpressionUtil.ReplaceVars(aUntruth, truth, untruth);

			}
            else if (aNode is IFNecessityNode)
            {
                IArithmeticNode truth, untruth;
                ConvertToArithmetic(((IFUnaryNode)aNode).Argument, out truth, out untruth);
                aTruth = (IArithmeticNode)truth.Clone();
                aUntruth = new MinusNode(new ConstantNode(1.0), (IArithmeticNode)truth.Clone());
            }
            else if (aNode is IFPossibilityNode)
            {
                IArithmeticNode truth, untruth;
                ConvertToArithmetic(((IFUnaryNode)aNode).Argument, out truth, out untruth);
                aTruth = new MinusNode(new ConstantNode(1.0), (IArithmeticNode)untruth.Clone());
                aUntruth = (IArithmeticNode)untruth.Clone();   
            }
            else if (aNode is IFCircleNode)
            {
                IArithmeticNode truth, untruth;
                ConvertToArithmetic(((IFUnaryNode)aNode).Argument, out truth, out untruth);
                aTruth = new DivideNode((IArithmeticNode)truth.Clone(), new PlusNode((IArithmeticNode)truth.Clone(), (IArithmeticNode)untruth.Clone()));
                aUntruth = new DivideNode((IArithmeticNode)untruth.Clone(), new PlusNode((IArithmeticNode)truth.Clone(), (IArithmeticNode)untruth.Clone()));
            }
            else if (aNode is IFBinaryNode)
            {
                IArithmeticNode truthLeft, truthRight, untruthLeft, untruthRight;
                ConvertToArithmetic(((IFBinaryNode)aNode).LeftArgument, out truthLeft, out untruthLeft);
                ConvertToArithmetic(((IFBinaryNode)aNode).RightArgument, out truthRight, out untruthRight);
                if (aNode is IFConjunctionNode)
                {
                    aTruth = new MinNode(truthLeft, truthRight);
                    aUntruth = new MaxNode(untruthLeft, untruthRight);
                }
                else if (aNode is IFDisjunctionNode)
                {
                    aTruth = new MaxNode(truthLeft, truthRight);
                    aUntruth = new MinNode(untruthLeft, untruthRight);
                }
                else if (aNode is IFImplicationNode)
                {
                    Implication implication = ((IFImplicationNode)aNode).Implication;
                    aTruth = implication.Truth; // no cloning for now, but be careful
                    aUntruth = implication.Untruth;
                    aTruth = ExpressionUtil.ReplaceVars(aTruth, truthLeft, untruthLeft, truthRight, untruthRight);
                    aUntruth = ExpressionUtil.ReplaceVars(aUntruth, truthLeft, untruthLeft, truthRight, untruthRight);
                }
                else if (aNode is IFGeqNode)
                {
                    IArithmeticNode sg2left = new OsNode(new MinusNode(truthRight, truthLeft));
                    IArithmeticNode sg2right = new OsNode(new MinusNode(untruthLeft, untruthRight));
                    aTruth = new MinNode(sg2left, sg2right);
                    aUntruth = new MinusNode(new ConstantNode(1.0), (IArithmeticNode)aTruth.Clone());
                }
                else if (aNode is IFLeqNode)
                {
                    IArithmeticNode sg2left = new OsNode(new MinusNode(truthLeft, truthRight));
                    IArithmeticNode sg2right = new OsNode(new MinusNode(untruthRight, untruthLeft));
                    aTruth = new MinNode(sg2left, sg2right);
                    aUntruth = new MinusNode(new ConstantNode(1.0), (IArithmeticNode)aTruth.Clone());
                }
                else if (aNode is IFEqualNode)
                {
                    aTruth = new MinNode(new EqualNode(truthLeft, truthRight), new EqualNode(untruthLeft, untruthRight));
                    aUntruth = new MinusNode(new ConstantNode(1.0), (IArithmeticNode)aTruth.Clone());
                }
                else if (aNode is IFMultiplyConjunctionNode)
                {
                    aTruth = new MultiplyNode(truthLeft, truthRight);
                    aUntruth = new MinusNode(new PlusNode(untruthLeft, untruthRight),
                        new MultiplyNode(untruthLeft, untruthRight));
                }
                //...
                else
                {
                    throw new ApplicationException("Don't know how to convert " + aNode.GetType()); //typeof too
                }
            }
            else
            {
                throw new ApplicationException("Don't know how to convert " + aNode.GetType());
            }
		}
	}
}
