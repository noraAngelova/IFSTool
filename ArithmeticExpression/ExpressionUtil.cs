using System;
using IFSTool.Expression;

namespace IFSTool.ArithmeticExpression
{
	public class ExpressionUtil
	{
		private static readonly IArithmeticNode ZERO = new ConstantNode(0.0);
		private static readonly IArithmeticNode ONE = new ConstantNode(1.0);

		//requires: da sa normalizirani; neka i NormalizePolynomial da se vika ot Normalize
		//vsa6tnost kogato proverqvame za <1,0>, togava vse izrazite sa si 1 i 0
		public static bool AreIdentical(IArithmeticNode aNode1, IArithmeticNode aNode2)
		{
			//trqbva da moje user-at da zadava i svoi pravila!
			
			if (aNode1 is ConstantNode && aNode2 is ConstantNode)
			{
				return Math.Abs((aNode1.Evaluate(0, 0, 0, 0) - aNode2.Evaluate(0, 0, 0, 0))) < Constants.EPSILON;
			}
			if (aNode1 is VariableNode && aNode2 is VariableNode)
			{
				return ((VariableNode)aNode1).Variable.Equals(((VariableNode)aNode2).Variable);
			}
			if (aNode1 is UnaryNode && aNode1.GetType() == aNode2.GetType()
				&& AreIdentical(((UnaryNode)aNode1).Argument, ((UnaryNode)aNode2).Argument))
			{
				return true;
			}
			if (aNode1 is BinaryNode && aNode1.GetType() == aNode2.GetType())
			{
				if (AreIdentical(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).LeftArgument)
					&& AreIdentical(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).RightArgument))
				{
					return true;
				}
				//don't forget this!: commutativity
				if (aNode1 is PlusNode || aNode1 is MultiplyNode || aNode1 is MaxNode || aNode1 is MinNode || aNode1 is EqualNode)
					if (AreIdentical(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).RightArgument)
						&& AreIdentical(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).LeftArgument))
					{
						return true;
					}
			}
			//x-y=0 <== x=y
			if (aNode1 is MinusNode && IsZero(aNode2))
			{
				if (AreIdentical(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode1).LeftArgument))
					return true;
			}
			if (aNode2 is MinusNode && IsZero(aNode1))
			{
				if (AreIdentical(((BinaryNode)aNode2).RightArgument, ((BinaryNode)aNode2).LeftArgument))
					return true;
			}
			//1-x=1 <== x=0
			if (aNode1 is MinusNode && IsOne(aNode2) && IsOne(((MinusNode)aNode1).LeftArgument))
			{
				if (AreIdentical(((MinusNode)aNode1).RightArgument, ZERO))
					return true;
			}
			if (aNode2 is MinusNode && IsOne(aNode1) && IsOne(((MinusNode)aNode2).LeftArgument))
			{
				if (AreIdentical(((MinusNode)aNode2).RightArgument, ZERO))
					return true;
			}
			//sg(x)=1 <== x>0
			if (aNode1 is SgNode && IsOne(aNode2))
			{
				if (AreGreater(((UnaryNode)aNode1).Argument, ZERO))
					return true;
			}
			if (aNode1 is SgNode && IsZero(aNode2))
			{
				if (AreGreaterOrEqual(ZERO, ((UnaryNode)aNode1).Argument))
					return true;
			}
			if (aNode2 is SgNode && IsOne(aNode1))
			{
				if (AreGreater(((UnaryNode)aNode2).Argument, ZERO))
					return true;
			}
			if (aNode2 is SgNode && IsZero(aNode1))
			{
				if (AreGreaterOrEqual(ZERO, ((UnaryNode)aNode2).Argument))
					return true;
			}
			if (aNode1 is OsNode && IsOne(aNode2))
			{
				if (AreGreaterOrEqual(ZERO, ((UnaryNode)aNode1).Argument))
					return true;
			}
			if (aNode1 is OsNode && IsZero(aNode2))
			{
				if (AreGreater(((UnaryNode)aNode1).Argument, ZERO))
					return true;
			}
			if (aNode2 is OsNode && IsOne(aNode1))
			{
				if (AreGreaterOrEqual(ZERO, ((UnaryNode)aNode2).Argument))
					return true;
			}
			if (aNode2 is OsNode && IsZero(aNode1))
			{
				if (AreGreater(((UnaryNode)aNode2).Argument, ZERO))
					return true;
			}
			//x*y=0 <== x==0||y==0
//			if (aNode1 is MultiplyNode && IsZero(aNode2))
//			{
//				if (AreIdentical(((BinaryNode)aNode1).LeftArgument, aNode2) || AreIdentical(((BinaryNode)aNode1).RightArgument, aNode2))
//					return true;
//			}
//			if (aNode2 is MultiplyNode && IsZero(aNode1))
//			{
//				if (AreIdentical(((BinaryNode)aNode2).LeftArgument, aNode1) || AreIdentical(((BinaryNode)aNode2).RightArgument, aNode1))
//					return true;
//			}
			//x==const <== ako x e ot vida sg(var), sg(1-var), 1-s, min(s1,s2)..., to dostata4no e samo da proverim v <0,0>, <1,0>, <0,1>, koeto ne se pravi vinagi, zatova go napravi!
//			if (aNode2 is ConstantNode && Is0Or1Deep(aNode1))
//			{
//				//brei, otkade 6te razberem kolko sa promenlivite? grr
//			}
			//if (				|| aNode1 is ConstantNode && Is0Or1Deep(aNode2))

			//if polynomial over a, b, c, d, sg(...), max(...), ^(1-d) (tova zasega slava bogu go nqma)...
			//podminahme x^y==x^y i x^2==x^2 -  mai ne bi moglo da stignat dotuk, a i da stignat, nqma kakvo da napravim mai (nali gore ima6e UnaryNode s proverka za identical arguments)
			//moje oba4e da se stigne do x^2==x*x
//			if ((aNode1 is SquareNode || aNode1 is PlusNode || aNode1 is MinusNode || aNode1 is MultiplyNode || aNode1 is PowerNode)
//				&& (aNode2 is SquareNode || aNode2 is PlusNode || aNode2 is MinusNode || aNode2 is MultiplyNode || aNode2 is PowerNode))
//			{
//				IArithmeticNode leftPolynomial = (IArithmeticNode)aNode1.Clone();
//				IArithmeticNode rightPolynomial = (IArithmeticNode)aNode2.Clone();
//				NormalizePolynomial(ref leftPolynomial);//kato e v normalen vid, popada v lesnite slu4ai po-gore i stiga do slojnotii kato sg i min/max
//				NormalizePolynomial(ref rightPolynomial);
//				//~dobre, ama: ako na left korena e +, a na right korena e -, 6te trqbva o6te normalizaciq!!!
//				//~vsa6tnost mai ne e taka, nali Normalize promenq i korena!
//				bool result = false;//zasega; inak IdenticalExpressions(leftPolynomial, rightPolynomial);//!ako ne e svar6eno ni6to, tuk stava gaf
//				//!!!ei, vnimavai! ako ne savpadat dvata normalizirani polinoma, tova pak 6te se izvika!!!
//				return result;
//			}

			/*
			za min, max: 1-max(a,b) = min(1-a,1-b), razgl. slu4ai, min(min(max(b,c),max(a,b)),max(c,d)) = min(min(max(c,b),max(d,c)),max(b,a))
			za sg, sg2: tuk e gadnoto! sg*sg = sg, sg(a)*sg(1-b) = sg(a), sg2 = 1-sg...
			ako e ?,x i sadarja samo nqkoi bukvi, posle pri ?+3,x pak 6te e sa6tata... vnimavai! moje da sadarjat razli4ni bukvi i pak da savpadat!
			*/
			//nakraq test this funk, kato q pusne6 samostoqtelno varhu vsi4ki impl - bez statistical shots, dali 6te dade sa6tite resultati?

			return false; //default answer; ne e ba6 sigurno, ama aide
		}

		public static bool AreGreaterOrEqual(IArithmeticNode aNode1, IArithmeticNode aNode2)
		{
			if (aNode1 is ConstantNode && aNode2 is ConstantNode)
			{
				return aNode1.Evaluate(0, 0, 0, 0) >= aNode2.Evaluate(0, 0, 0, 0) - Constants.EPSILON;
			}
			if (aNode1 is VariableNode && aNode2 is VariableNode)
			{
				return aNode1.ToString().Equals(aNode2.ToString());//
			}
			if (IsBetween0And1(aNode1) && IsZero(aNode2)
				|| IsOne(aNode1) && IsBetween0And1(aNode2))
			{
				return true;
			}
			//sg(x)>=sg(y) <== x>0||y<=0||x>=y
			if (aNode1 is SgNode && aNode2 is SgNode)
			{
				if (AreGreater(((UnaryNode)aNode1).Argument, ZERO)
					|| AreGreaterOrEqual(ZERO, ((UnaryNode)aNode2).Argument)
					|| AreGreaterOrEqual(((UnaryNode)aNode1).Argument, ((UnaryNode)aNode2).Argument))
					return true;
			}
			//sg2(x)>=sg2(y) <== sg(y)>=sg(x)
			if (aNode1 is OsNode && aNode2 is OsNode)
			{
				if (AreGreaterOrEqual(new SgNode(((UnaryNode)aNode2).Argument), new SgNode(((UnaryNode)aNode1).Argument)))
					return true;
			}
			//max(x,y) >= min(y,x) //tova se pokriva s dolnite i bavi, kogato ne e izpalneno - dvoina proverka
			//max(a,b) >= min(c,d) <== a>=c||a>=d||b>=c||b>=d //tova se pokriva s dolnite i bavi, kogato ne e izpalneno - dvoina proverka
//			if (aNode1 is MaxNode && aNode2 is MinNode)
//			{
//				if (AreIdentical(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).LeftArgument)
//					&& AreIdentical(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).RightArgument)
//					|| AreIdentical(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).RightArgument)
//					&& AreIdentical(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).LeftArgument))
//					return true;
//				if (AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).LeftArgument)
//					|| AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).RightArgument)
//					|| AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).RightArgument)
//					|| AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).LeftArgument))
//					return true;
//			}
			//min(x,y) >= max(z,t) <== a>=c&&a>=d&&b>=c&&b>=d //tova se pokriva s dolnite i bavi, kogato ne e izpalneno - dvoina proverka
//			if (aNode1 is MinNode && aNode2 is MaxNode)
//			{
//				if (AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).LeftArgument)
//					&& AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).RightArgument)
//					&& AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, ((BinaryNode)aNode2).RightArgument)
//					&& AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, ((BinaryNode)aNode2).LeftArgument))
//					return true;
//			}
			//max(x,y)>=z <== x>=z||y>=z
			if (aNode1 is MaxNode)
				if (AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, aNode2)
					|| AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, aNode2))
					return true;
			//min(x,y)>=z <== x>=z&&y>=z
			if (aNode1 is MinNode)
				if (AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, aNode2)
					&& AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, aNode2))
					return true;
			//z>=max(x,y) <== z>=x&&z>=y
			if (aNode2 is MaxNode)
				if (AreGreaterOrEqual(aNode1, ((BinaryNode)aNode2).LeftArgument)
					&& AreGreaterOrEqual(aNode1, ((BinaryNode)aNode2).RightArgument))
					return true;
			//z>=min(x,y) <== z>=x||z>=y
			if (aNode2 is MinNode)
				if (AreGreaterOrEqual(aNode1, ((BinaryNode)aNode2).LeftArgument)
					|| AreGreaterOrEqual(aNode1, ((BinaryNode)aNode2).RightArgument))
					return true;
			//x >= x*y, y is between 0 and 1
			if (aNode2 is MultiplyNode && 
				(IsBetween0And1(((BinaryNode)aNode2).RightArgument) 
					&& AreIdentical(aNode1, ((BinaryNode)aNode2).LeftArgument)
				|| IsBetween0And1(((BinaryNode)aNode2).LeftArgument) 
					&& AreIdentical(aNode1, ((BinaryNode)aNode2).RightArgument))
				)
				return true;
			//x >= x
			if (AreIdentical(aNode1, aNode2))
				return true;
			//x-y>=0 <== x>=y
			if (aNode1 is MinusNode && IsZero(aNode2) && 
				AreGreaterOrEqual(((MinusNode)aNode1).LeftArgument, ((MinusNode)aNode1).RightArgument))
				return true;
			//0>=x-y <== y>=x
			if (aNode2 is MinusNode && IsZero(aNode1) && 
				AreGreaterOrEqual(((MinusNode)aNode2).RightArgument, ((MinusNode)aNode2).LeftArgument))
				return true;
			//1-x>=x <== x==0 (sa6to i x<=1/2...)
			if (aNode1 is MinusNode && IsOne(((MinusNode)aNode1).LeftArgument) && AreIdentical(((MinusNode)aNode1).RightArgument, aNode2)
				&& AreIdentical(aNode2, ZERO))
				return true; //ne6to ne se polzva
			//x*y>=0 <== x>=0&&y>=0 || ...
			if (aNode1 is MultiplyNode && IsZero(aNode2))
			{
				if (AreGreaterOrEqual(((BinaryNode)aNode1).LeftArgument, aNode2) && AreGreaterOrEqual(((BinaryNode)aNode1).RightArgument, aNode2))
					return true;//ne6to ne se polzva
			}
			//(a+b)-ab>=ab ; ami ako e a+(b-ab)?
			if (aNode1 is MinusNode && aNode2 is MultiplyNode)
			{
				string left = aNode1.ToString();//aideee
				if (left.Equals("((a+b)-(a*b))") || left.Equals("((a+b)-(b*a))") ||
					left.Equals("((b+a)-(a*b))") || left.Equals("((b+a)-(b*a))"))
				{
					string right = aNode2.ToString();
					if (right.Equals("(a*b)") || right.Equals("(b*a)"))
						return true;
				}
			}
			//sg(y)>=y, y is between 0 and 1
			if (aNode1 is SgNode && IsBetween0And1(aNode2) && AreIdentical(((UnaryNode)aNode1).Argument, aNode2))
				return true;
			//y>=sg2(1-y)
			if (aNode2 is OsNode && IsBetween0And1(aNode1) && ((UnaryNode)aNode2).Argument is MinusNode &&
				IsOne(((MinusNode)((UnaryNode)aNode2).Argument).LeftArgument)
				&& AreIdentical(((MinusNode)((UnaryNode)aNode2).Argument).RightArgument, aNode1))
				return true;
			//...
			return false;
		}

		public static bool AreGreater(IArithmeticNode aNode1, IArithmeticNode aNode2)
		{
			if (aNode1 is ConstantNode && aNode2 is ConstantNode)
			{
				return aNode1.Evaluate(0, 0, 0, 0) > aNode2.Evaluate(0, 0, 0, 0) + Constants.EPSILON;
			}
			if (aNode1 is VariableNode && aNode2 is VariableNode)
			{
				return false;
			}
			//x-y>0 -> x>y
			if (aNode1 is MinusNode && IsZero(aNode2) && 
				AreGreater(((MinusNode)aNode1).LeftArgument, ((MinusNode)aNode1).RightArgument))
				return true;
			//0>x-y -> y>x
			if (aNode2 is MinusNode && IsZero(aNode1) && 
				AreGreater(((MinusNode)aNode2).RightArgument, ((MinusNode)aNode2).LeftArgument))
				return true;
			//x*y>0 <== x>0&&y>0
//			if (aNode1 is MultiplyNode && IsZero(aNode2))
//			{
//				if (AreGreater(((BinaryNode)aNode1).LeftArgument, aNode2) && AreGreater(((BinaryNode)aNode1).RightArgument, aNode2))
//					return true;//ne6to ne se polzva
//			}
			//...
			return false;
		}

		public static void Normalize(ref IArithmeticNode aNode)
		{
            //tiq polzval li si gi?
            //sg2(1-sg(1-sg(1-b))) = sg2(1-b) //ami bi trqbvalo posledovatelni izvikvaniq na os da se normalizirat...
            //1-sg(sg(a)+sg(1-b)) = sg2(1-b)

			if (aNode is UnaryNode)
			{
				UnaryNode unaryNode = (UnaryNode)aNode;

				IArithmeticNode arg = unaryNode.Argument;
				Normalize(ref arg);
				unaryNode.Argument = arg;

				if (unaryNode.Argument is ConstantNode)
					aNode = new ConstantNode(unaryNode.Evaluate(0.0, 0.0, 0.0, 0.0));
				//sg(z) -> z, z is 0 or 1 (sg(sg(x)) -> sg(x), sg(sg2(x)) -> sg2(x))
				else if (unaryNode is SgNode && Is0Or1(unaryNode.Argument))
					aNode = unaryNode.Argument;
				//sg2(1-z) -> z, z is 0 or 1 //tova sa6to pravi mizerii! impl23, kam kraq 3 axiomi
				else if (unaryNode is OsNode && unaryNode.Argument is MinusNode &&
					IsOne(((MinusNode)unaryNode.Argument).LeftArgument) && Is0Or1(((MinusNode)unaryNode.Argument).RightArgument))
					aNode = ((MinusNode)unaryNode.Argument).RightArgument;
				//sg2(sg(x)) -> sg2(x)
				else if (unaryNode is OsNode && unaryNode.Argument is SgNode)
					unaryNode.Argument = ((UnaryNode)unaryNode.Argument).Argument;
					//sg2(sg2(x)) -> sg(x)
				else if (unaryNode is OsNode && unaryNode.Argument is OsNode)
					aNode = new SgNode(((UnaryNode)unaryNode.Argument).Argument);
				//sg(y) -> 1, y>0
				else if (unaryNode is SgNode && AreGreater(unaryNode.Argument, ZERO))
					aNode = new ConstantNode(1.0);
				//sg(y) -> 0, y<=0
				else if (unaryNode is SgNode && AreGreaterOrEqual(ZERO, unaryNode.Argument))
					aNode = new ConstantNode(0.0);
				//sg2(y) -> 1, y<=0
				else if (unaryNode is OsNode && AreGreaterOrEqual(ZERO, unaryNode.Argument))
					aNode = new ConstantNode(1.0);
				//sg2(y) -> 0, y>0
				else if (unaryNode is OsNode && AreGreater(unaryNode.Argument, ZERO))
					aNode = new ConstantNode(0.0);
				//tiq dolnite gi premesti v AreGreater ili AreGreaterOrEqual
				//sg(y-1) -> 0 (y is between 0 and 1)
				else if (unaryNode is SgNode && unaryNode.Argument is MinusNode &&
					IsOne(((MinusNode)unaryNode.Argument).RightArgument) && IsBetween0And1(((MinusNode)unaryNode.Argument).LeftArgument))
					aNode = new ConstantNode(1.0);
					//sg2(y-1) -> 1
				else if (unaryNode is OsNode && unaryNode.Argument is MinusNode &&
					IsOne(((MinusNode)unaryNode.Argument).RightArgument) && IsBetween0And1(((MinusNode)unaryNode.Argument).LeftArgument))
					aNode = new ConstantNode(0.0);
				//sg(y-sg(y)) -> 0
				else if (unaryNode is SgNode && unaryNode.Argument is MinusNode &&
					((MinusNode)unaryNode.Argument).RightArgument is SgNode && IsBetween0And1(((MinusNode)unaryNode.Argument).LeftArgument)
					&& AreIdentical(((MinusNode)unaryNode.Argument).LeftArgument, ((UnaryNode)((MinusNode)unaryNode.Argument).RightArgument).Argument))
					aNode = new ConstantNode(0.0);
				//sg(max(x,y)) -> max(sgx,sgy), min too; za6to pri ->20,14 ne stava s min?
				else if (unaryNode is SgNode && (unaryNode.Argument is MaxNode || unaryNode.Argument is MinNode))
				{
					aNode = unaryNode.Argument;
					((BinaryNode)aNode).LeftArgument = new SgNode(((BinaryNode)aNode).LeftArgument);
					((BinaryNode)aNode).RightArgument = new SgNode(((BinaryNode)aNode).RightArgument);
					Normalize(ref aNode);//leko
				}
				//ima o6te takiva...
				else if (unaryNode is SgNode && unaryNode.Argument is PlusNode && Is0Or1(((PlusNode)unaryNode.Argument).LeftArgument) && Is0Or1(((PlusNode)unaryNode.Argument).RightArgument))
					aNode = new MaxNode(((PlusNode)unaryNode.Argument).LeftArgument, ((PlusNode)unaryNode.Argument).RightArgument);
					//Normalize na gornoto?
				//...
			}
			else if (aNode is BinaryNode)
			{
				BinaryNode binaryNode = (BinaryNode)aNode;

				IArithmeticNode arg = binaryNode.LeftArgument;
				Normalize(ref arg);
				binaryNode.LeftArgument = arg;
				arg = binaryNode.RightArgument;
				Normalize(ref arg);
				binaryNode.RightArgument = arg;

				//const1 op const2 -> const3
				if (binaryNode.LeftArgument is ConstantNode && binaryNode.RightArgument is ConstantNode)
					aNode = new ConstantNode(binaryNode.Evaluate(0.0, 0.0, 0.0, 0.0));
				else if (binaryNode is PlusNode)
				{
					//0+x -> x
					if (IsZero(binaryNode.LeftArgument))
						aNode = binaryNode.RightArgument;
					//x+0 -> x
					else if (IsZero(binaryNode.RightArgument))
						aNode = binaryNode.LeftArgument;
					//sg2(1-y)+y*sg(1-y) -> y, y is between 0 and 1
					//...
					//sg(x)+sg2(x) -> 1 //za drugo ne moje!
					else if (((binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is OsNode)
						|| (binaryNode.LeftArgument is OsNode && binaryNode.RightArgument is SgNode))
						&& AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, ((UnaryNode)binaryNode.RightArgument).Argument))
						aNode = new ConstantNode(1.0);
				}
				else if (binaryNode is MinusNode)
				{
					//x-0 -> x
					if (IsZero(binaryNode.RightArgument))
						aNode = binaryNode.LeftArgument;
					//x-x -> 0 - tova pri NormalizePolynomial 6te se opravi, no zasega taka
					else if (AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
						aNode = new ConstantNode(0.0);
					//1-sgx -> sg2(x)
					else if (IsOne(binaryNode.LeftArgument) && binaryNode.RightArgument is SgNode)
						aNode = new OsNode(((UnaryNode)binaryNode.RightArgument).Argument);
					//1-sg2(x) -> sg(x)
					else if (IsOne(binaryNode.LeftArgument) && binaryNode.RightArgument is OsNode)
						aNode = new SgNode(((UnaryNode)binaryNode.RightArgument).Argument);
					//1-(1-x) -> x //tova po-natatak 6te go pravi normalizepoly
					else if (IsOne(binaryNode.LeftArgument) && binaryNode.RightArgument is MinusNode
						&& IsOne(((MinusNode)binaryNode.RightArgument).LeftArgument))
						aNode = ((MinusNode)binaryNode.RightArgument).RightArgument;
					//(x+1)-1 -> x - tova pri NormalizePolynomial 6te se opravi, no zasega taka
					//ami (1+x)-1 - zasega ne
					else if (IsOne(binaryNode.RightArgument) && binaryNode.LeftArgument is PlusNode
						&& IsOne(((PlusNode)binaryNode.LeftArgument).RightArgument))
						aNode = ((PlusNode)binaryNode.LeftArgument).LeftArgument;
					//za neg14, ama da se opravi:
					else if (binaryNode.ToString().Equals("(sg2(a)-min(sg2(a),sg(1-b)))"))
					{
						UnaryNode left = new OsNode();
						left.Argument = new VariableNode('a');
						UnaryNode right = new OsNode();
						right.Argument = new MinusNode();
						((MinusNode)right.Argument).LeftArgument = new ConstantNode(1.0);
						((MinusNode)right.Argument).RightArgument = new VariableNode('b');
						aNode = new MinNode(left, right);
					}
					//1-min/max(sg/sg2(x), sg/sg2(x)) -> max/min(sg2/sg(x), sg2/sg(y))
					//.....
					//damn
					else if (binaryNode.ToString().Equals("(min(sg2(a),sg2(c))-sg2max(a,c))"))
						aNode = new ConstantNode(0.0);
				}
				else if (binaryNode is MultiplyNode)
				{
					//0*x -> 0, x*0 -> 0
					if (IsZero(binaryNode.LeftArgument) || IsZero(binaryNode.RightArgument))
						aNode = new ConstantNode(0.0);
					//1*x -> x
					if (IsOne(binaryNode.LeftArgument))
						aNode = binaryNode.RightArgument;
					//x*1 -> x
					else if (IsOne(binaryNode.RightArgument))
						aNode = binaryNode.LeftArgument;
					//dolnoto moje i da ne e nujno, ako * e zamenen s min
//					//sg(x)*sg(x) -> sg(x); same for sg2; izob6to za vsi4ko 0or1, koeto e ednakvo
//					else if (Is0Or1(binaryNode.LeftArgument) && Is0Or1(binaryNode.RightArgument)
//						&& AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
//						aNode = binaryNode.LeftArgument;
                    ////sg(1-a)*b -> b; b*sg(1-a) -> b
                    //else if (binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is VariableNode
                    //    && ((UnaryNode)binaryNode.LeftArgument).Argument is MinusNode && 
                    //    IsOne(((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).LeftArgument) &&
                    //    ((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument is VariableNode
                    //    && GetCoVariable(((VariableNode)binaryNode.RightArgument).Variable).Equals(((VariableNode)((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument).Variable))
                    //    aNode = binaryNode.RightArgument;
                    //else if (binaryNode.RightArgument is SgNode && binaryNode.LeftArgument is VariableNode
                    //    && ((UnaryNode)binaryNode.RightArgument).Argument is MinusNode && 
                    //    IsOne(((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).LeftArgument) &&
                    //    ((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument is VariableNode
                    //    && GetCoVariable(((VariableNode)binaryNode.LeftArgument).Variable).Equals(((VariableNode)((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument).Variable))
                    //    aNode = binaryNode.LeftArgument;
                    ////y*sg2(1-y) -> sg2(1-y), posle i simetri4noto
                    //else if (IsBetween0And1(binaryNode.LeftArgument) && binaryNode.RightArgument is OsNode
                    //    && ((UnaryNode)binaryNode.RightArgument).Argument is MinusNode
                    //    && IsOne(((BinaryNode)((UnaryNode)binaryNode.RightArgument).Argument).LeftArgument)
                    //    && AreIdentical(binaryNode.LeftArgument, ((BinaryNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument))
                    //    aNode = binaryNode.RightArgument;
                    //else if (IsBetween0And1(binaryNode.RightArgument) && binaryNode.LeftArgument is OsNode
                    //    && ((UnaryNode)binaryNode.LeftArgument).Argument is MinusNode
                    //    && IsOne(((BinaryNode)((UnaryNode)binaryNode.LeftArgument).Argument).LeftArgument)
                    //    && AreIdentical(binaryNode.RightArgument, ((BinaryNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument))
                    //    aNode = binaryNode.LeftArgument;
                    ////y*sg(y) -> y; ne6to ne se sre6tat zasega
                    //else if (IsBetween0And1(binaryNode.LeftArgument) && binaryNode.RightArgument is SgNode
                    //    && AreIdentical(((UnaryNode)binaryNode.RightArgument).Argument, binaryNode.LeftArgument))
                    //    aNode = binaryNode.LeftArgument;
                    //else if (IsBetween0And1(binaryNode.RightArgument) && binaryNode.LeftArgument is SgNode
                    //    && AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, binaryNode.RightArgument))
                    //    aNode = binaryNode.RightArgument;
                    ////sg * sg -> min(sg, sg), izob6to pri Is0Or1
                    //else if (Is0Or1(binaryNode.LeftArgument) && Is0Or1(binaryNode.RightArgument))
                    //{
                    //    aNode = new MinNode(binaryNode.LeftArgument, binaryNode.RightArgument);
                    //    Normalize(ref aNode); //brei
                    //}
                    //sg*sg, sg*var, izob6to Between01*0or1 -> min(,)
                    else if ((Is0Or1(binaryNode.LeftArgument) && IsBetween0And1(binaryNode.RightArgument))
                        || (Is0Or1(binaryNode.RightArgument) && IsBetween0And1(binaryNode.LeftArgument)))
                    {
                        aNode = new MinNode(binaryNode.LeftArgument, binaryNode.RightArgument);
                        Normalize(ref aNode); //brei
                    }
                    ////sg(x)*sg2(x) -> 0 - vremenno
                    //else if (((binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is OsNode)
                    //    || (binaryNode.LeftArgument is OsNode && binaryNode.RightArgument is SgNode))
                    //    && AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, ((UnaryNode)binaryNode.RightArgument).Argument))
                    //    aNode = new ConstantNode(0.0);
				}
                else if (binaryNode is DivideNode)
                {
                    //0/x -> x
                    if (IsZero(binaryNode.LeftArgument))
                        aNode = new ConstantNode(0.0);
                    //x/1 -> x
                    if (IsOne(binaryNode.RightArgument))
                        aNode = binaryNode.LeftArgument;
                    //x/x -> 1, what if x == 0?
                    if (AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
                        aNode = new ConstantNode(1.0);
                }
				else if (binaryNode is MinNode)
				{
					//min(x,y) -> y, if x >= y //tova vkliu4va i slu4aq min(y,0), kogato y m/u 0 i 1
					if (AreGreaterOrEqual(binaryNode.LeftArgument, binaryNode.RightArgument))
						aNode = binaryNode.RightArgument;
					else if (AreGreaterOrEqual(binaryNode.RightArgument, binaryNode.LeftArgument))
						aNode = binaryNode.LeftArgument;
//					//min(0,y) -> 0, min(y,0) -> 0
//					if (IsZero(binaryNode.LeftArgument) && IsBetween0And1(binaryNode.RightArgument)
//						|| IsZero(binaryNode.RightArgument) && IsBetween0And1(binaryNode.LeftArgument))
//						aNode = new ConstantNode(0.0);
//					//min(1,y) -> y
//					else if (IsOne(binaryNode.LeftArgument) && IsBetween0And1(binaryNode.RightArgument))
//						aNode = binaryNode.RightArgument;
//						//min(y,1) -> y
//					else if (IsOne(binaryNode.RightArgument) && IsBetween0And1(binaryNode.LeftArgument))
//						aNode = binaryNode.LeftArgument;
//						//min(x,x) -> x
//					else if (AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
//						aNode = binaryNode.LeftArgument;
				    //min(x,max(x,_)) -> x
					else if (binaryNode.RightArgument is MaxNode)
					{
						BinaryNode right = (BinaryNode)binaryNode.RightArgument;
						if (AreIdentical(binaryNode.LeftArgument, right.LeftArgument) || AreIdentical(binaryNode.LeftArgument, right.RightArgument))
							aNode = binaryNode.LeftArgument;
					}
					//min(max(x,_),x) -> x
					else if (binaryNode.LeftArgument is MaxNode)
					{
						BinaryNode left = (BinaryNode)binaryNode.LeftArgument;
						if (AreIdentical(binaryNode.RightArgument, left.LeftArgument) || AreIdentical(binaryNode.RightArgument, left.RightArgument))
							aNode = binaryNode.RightArgument;
					}
					//min(1-x, 1-y) -> 1-max(x,y)
					else if (binaryNode.LeftArgument is MinusNode && IsOne(((MinusNode)binaryNode.LeftArgument).LeftArgument)
						&& binaryNode.RightArgument is MinusNode && IsOne(((MinusNode)binaryNode.RightArgument).LeftArgument))
					{
						MinusNode result = new MinusNode();
						result.LeftArgument = new ConstantNode(1.0);
						result.RightArgument = new MaxNode(
							((MinusNode)binaryNode.LeftArgument).RightArgument,
							((MinusNode)binaryNode.RightArgument).RightArgument);
						aNode = result;
					}

                    //sg(1-a)*b -> b; b*sg(1-a) -> b
                    else if (binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is VariableNode
                        && ((UnaryNode)binaryNode.LeftArgument).Argument is MinusNode &&
                        IsOne(((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).LeftArgument) &&
                        ((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument is VariableNode
                        && GetCoVariable(((VariableNode)binaryNode.RightArgument).Variable).Equals(((VariableNode)((MinusNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument).Variable))
                        aNode = binaryNode.RightArgument;
                    else if (binaryNode.RightArgument is SgNode && binaryNode.LeftArgument is VariableNode
                        && ((UnaryNode)binaryNode.RightArgument).Argument is MinusNode &&
                        IsOne(((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).LeftArgument) &&
                        ((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument is VariableNode
                        && GetCoVariable(((VariableNode)binaryNode.LeftArgument).Variable).Equals(((VariableNode)((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument).Variable))
                        aNode = binaryNode.LeftArgument;
                    //y*sg2(1-y) -> sg2(1-y), posle i simetri4noto
                    else if (IsBetween0And1(binaryNode.LeftArgument) && binaryNode.RightArgument is OsNode
                        && ((UnaryNode)binaryNode.RightArgument).Argument is MinusNode
                        && IsOne(((BinaryNode)((UnaryNode)binaryNode.RightArgument).Argument).LeftArgument)
                        && AreIdentical(binaryNode.LeftArgument, ((BinaryNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument))
                        aNode = binaryNode.RightArgument;
                    else if (IsBetween0And1(binaryNode.RightArgument) && binaryNode.LeftArgument is OsNode
                        && ((UnaryNode)binaryNode.LeftArgument).Argument is MinusNode
                        && IsOne(((BinaryNode)((UnaryNode)binaryNode.LeftArgument).Argument).LeftArgument)
                        && AreIdentical(binaryNode.RightArgument, ((BinaryNode)((UnaryNode)binaryNode.LeftArgument).Argument).RightArgument))
                        aNode = binaryNode.LeftArgument;
                    //y*sg(y) -> y; ne6to ne se sre6tat zasega
                    else if (IsBetween0And1(binaryNode.LeftArgument) && binaryNode.RightArgument is SgNode
                        && AreIdentical(((UnaryNode)binaryNode.RightArgument).Argument, binaryNode.LeftArgument))
                        aNode = binaryNode.LeftArgument;
                    else if (IsBetween0And1(binaryNode.RightArgument) && binaryNode.LeftArgument is SgNode
                        && AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, binaryNode.RightArgument))
                        aNode = binaryNode.RightArgument;
                        
                    //min(y,sg2(y)), min(sg2(y),y) -> 0, y is between 0 and 1
					else if (IsBetween0And1(binaryNode.LeftArgument) && binaryNode.RightArgument is OsNode &&
						AreIdentical(((UnaryNode)binaryNode.RightArgument).Argument, binaryNode.LeftArgument)
						|| IsBetween0And1(binaryNode.RightArgument) && binaryNode.LeftArgument is OsNode &&
						AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, binaryNode.RightArgument))
						aNode = new ConstantNode(0.0);
					//min(sg(x), sg2(x)) -> 0
					else if (((binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is OsNode)
						|| (binaryNode.LeftArgument is OsNode && binaryNode.RightArgument is SgNode))
						&& AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, ((UnaryNode)binaryNode.RightArgument).Argument))
						aNode = new ConstantNode(0.0);
					//min(1-z,z) -> 0, min(z,1-z) -> 0
					else if (binaryNode.LeftArgument is MinusNode && Is0Or1(binaryNode.RightArgument)
						&& IsOne(((MinusNode)binaryNode.LeftArgument).LeftArgument)
						&& AreIdentical(((MinusNode)binaryNode.LeftArgument).RightArgument, binaryNode.RightArgument)
						||
						binaryNode.RightArgument is MinusNode && Is0Or1(binaryNode.LeftArgument)
						&& IsOne(((MinusNode)binaryNode.RightArgument).LeftArgument)
						&& AreIdentical(((MinusNode)binaryNode.RightArgument).RightArgument, binaryNode.LeftArgument))
						aNode = new ConstantNode(0.0);
					//sg(a)*sg(1-b) -> sg(a); leko, 4e moje da e sg(1-b)*sg(a); ...
					//sg2?
					//po-nai-tapiq na4in: !!!!!!!!!!!da se opravi!
					else if (binaryNode.ToString().Equals("min(sg(a),sg(1-b))") //aaa, tuk mnogo pati se izvikva edin i sa6t ToString!
						|| binaryNode.ToString().Equals("min(sg(c),sg(1-d))")
						|| binaryNode.ToString().Equals("min(sg(e),sg(1-f))"))
						aNode = binaryNode.LeftArgument;
					else if (binaryNode.ToString().Equals("min(sg2(a),sg2(1-b))")
						|| binaryNode.ToString().Equals("min(sg2(c),sg2(1-d))")
						|| binaryNode.ToString().Equals("min(sg2(e),sg2(1-f))"))
						aNode = binaryNode.RightArgument;
				}
				else if (binaryNode is MaxNode)
				{
					//max(x,y) -> x, if x >= y //tova vkliu4va i slu4aq max(x,0), kogato x m/u 0 i 1
					if (AreGreaterOrEqual(binaryNode.LeftArgument, binaryNode.RightArgument))
						aNode = binaryNode.LeftArgument;
					else if (AreGreaterOrEqual(binaryNode.RightArgument, binaryNode.LeftArgument))
						aNode = binaryNode.RightArgument;
//					//max(1,y) -> 1, max(y,1) -> 1
//					if (IsOne(binaryNode.LeftArgument) && IsBetween0And1(binaryNode.RightArgument)
//						|| IsOne(binaryNode.RightArgument) && IsBetween0And1(binaryNode.LeftArgument))
//						aNode = new ConstantNode(1.0);
//						//max(0,y) -> y
//					else if (IsZero(binaryNode.LeftArgument) && IsBetween0And1(binaryNode.RightArgument))
//						aNode = binaryNode.RightArgument;
//						//max(y,0) -> y
//					else if (IsZero(binaryNode.RightArgument) && IsBetween0And1(binaryNode.LeftArgument))
//						aNode = binaryNode.LeftArgument;
//						//max(x,x) -> x
//					else if (AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
//						aNode = binaryNode.LeftArgument;
					//max(x,min(x,_)) -> x
					else if (binaryNode.RightArgument is MinNode)
					{
						BinaryNode right = (BinaryNode)binaryNode.RightArgument;
						if (AreIdentical(binaryNode.LeftArgument, right.LeftArgument) || AreIdentical(binaryNode.LeftArgument, right.RightArgument))
							aNode = binaryNode.LeftArgument;
					}
					//max(min(x,_),x) -> x
					else if (binaryNode.LeftArgument is MinNode)
					{
						BinaryNode left = (BinaryNode)binaryNode.LeftArgument;
						if (AreIdentical(binaryNode.RightArgument, left.LeftArgument) || AreIdentical(binaryNode.RightArgument, left.RightArgument))
							aNode = binaryNode.RightArgument;
					}
					//max(1-x,1-y) -> 1-min(x,y)
					else if (binaryNode.LeftArgument is MinusNode && IsOne(((MinusNode)binaryNode.LeftArgument).LeftArgument)
						&& binaryNode.RightArgument is MinusNode && IsOne(((MinusNode)binaryNode.RightArgument).LeftArgument))
					{
						MinusNode result = new MinusNode();
						result.LeftArgument = new ConstantNode(1.0);
						result.RightArgument = new MinNode(
							((MinusNode)binaryNode.LeftArgument).RightArgument,
							((MinusNode)binaryNode.RightArgument).RightArgument);
						aNode = result;
					}
					//max(sg(x), sg2(x)) -> 1
					else if (((binaryNode.LeftArgument is SgNode && binaryNode.RightArgument is OsNode)
						|| (binaryNode.LeftArgument is OsNode && binaryNode.RightArgument is SgNode))
						&& AreIdentical(((UnaryNode)binaryNode.LeftArgument).Argument, ((UnaryNode)binaryNode.RightArgument).Argument))
						aNode = new ConstantNode(1.0);
					//max(1-z,z) -> 1, max(z,1-z) -> 1
					else if (binaryNode.LeftArgument is MinusNode && Is0Or1(binaryNode.RightArgument)
						&& IsOne(((MinusNode)binaryNode.LeftArgument).LeftArgument)
						&& AreIdentical(((MinusNode)binaryNode.LeftArgument).RightArgument, binaryNode.RightArgument)
						||
						binaryNode.RightArgument is MinusNode && Is0Or1(binaryNode.LeftArgument)
						&& IsOne(((MinusNode)binaryNode.RightArgument).LeftArgument)
						&& AreIdentical(((MinusNode)binaryNode.RightArgument).RightArgument, binaryNode.LeftArgument))
						aNode = new ConstantNode(1.0);
					//max(y,sg(1-y)) -> 1
					if (binaryNode.RightArgument is SgNode && IsBetween0And1(binaryNode.LeftArgument) && 
						((UnaryNode)binaryNode.RightArgument).Argument is MinusNode && 
						IsOne(((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).LeftArgument) &&
						AreIdentical(binaryNode.LeftArgument, ((MinusNode)((UnaryNode)binaryNode.RightArgument).Argument).RightArgument))
						aNode = new ConstantNode(1.0);
				}
                else if (binaryNode is EqualNode)
                {
                    if (AreIdentical(binaryNode.LeftArgument, binaryNode.RightArgument))
                        aNode = new ConstantNode(1.0);//ne slagai else!
                }
				//more: (po-slojni, 6toto sravnqvame darveta; za plitki e lesno, no nqma smisal)
				//x+x -> 2*x, x*x -> sq(x)
				//also: NormalizePolynomial
			}
		}
		
		private static void NormalizePolynomial(ref IArithmeticNode aNode)
		{
			//privedi v normalen vid ~(razmestvane sabiraemi, distributivnost, x*2 == x+x, x^2 == x*x, sakra6tavane na sabiraemi...)
			/*
			+ - * 2 ^
			vsi4ki + i - trqbva da izpluvat nagore, da nqma * nad tqh!
			^ zasega 6te e samo s estestven pokazatel
			taka 6te stane spisak ot edno4leni (tree, left e arg, right e next), koito mogat da se sakratqt i priravnqt ednakvite stepeni
			nakraq lesno sravnqvane
			*/
			if (aNode is PlusNode || aNode is MinusNode)
			{
				IArithmeticNode leftArgument = ((BinaryNode)aNode).LeftArgument;
				NormalizePolynomial(ref leftArgument);
				((BinaryNode)aNode).LeftArgument = leftArgument;

				IArithmeticNode rightArgument = ((BinaryNode)aNode).RightArgument;
				NormalizePolynomial(ref rightArgument);
				((BinaryNode)aNode).RightArgument = rightArgument;
				
				//! sortirai lexicographski (t.e. merge) i sakrati +x-x -> 0(dali taka e dobre?), 2*x+x -> 3*x
				//qka rabota e tova! trqbva da stane "list" ot "edno4leni"
			}
			else if (aNode is MultiplyNode)
			{
				IArithmeticNode leftArgument = ((BinaryNode)aNode).LeftArgument;
				NormalizePolynomial(ref leftArgument);
				((BinaryNode)aNode).LeftArgument = leftArgument;

				IArithmeticNode rightArgument = ((BinaryNode)aNode).RightArgument;
				NormalizePolynomial(ref rightArgument);
				((BinaryNode)aNode).RightArgument = rightArgument;

				//! sled tva razkrii skobite i go napravi kakto e opisano po-gore; ako izvikame Normalize pak dali 6te zacikli?
			}
			else if (aNode is SquareNode)
			{
				IArithmeticNode argument = ((UnaryNode)aNode).Argument;
				if (argument is ConstantNode)
				{
					double argumentValue = argument.Evaluate(0, 0, 0, 0);
					argumentValue *= argumentValue;
					aNode = new ConstantNode(argumentValue);
				}
				else if (argument is SquareNode || argument is MultiplyNode ||
					argument is PlusNode || argument is MinusNode || argument is PowerNode)
				{
					IArithmeticNode copyOfArgument = (IArithmeticNode)argument.Clone();
					aNode = new MultiplyNode();//moje6e v konstruktora, ama aide
					((MultiplyNode)aNode).LeftArgument = argument;
					((MultiplyNode)aNode).RightArgument = copyOfArgument;
					NormalizePolynomial(ref aNode);
				}
				//ako e var^2 - OK, kakto i ako e max^2, sg^2
			}
			else if (aNode is PowerNode) //not needed for now
			{
				//ako slu4aino e const^2 - smetni q
				//ako e var^int - OK
				//ako e izraz^int - rekursivno davai...
				//ako e izraz^izraz - mai nqma da butame tuk
			}
			//ako e var/const/M/m/sg/sg2... nqmame rabota tuk; tva e danoto na rekursiqta
		}

		public static bool IsZero(IArithmeticNode aNode)
		{
			return aNode is ConstantNode && Math.Abs(aNode.Evaluate(0.0, 0.0, 0.0, 0.0)) < Constants.EPSILON;
		}

		public static bool IsOne(IArithmeticNode aNode)
		{
			return aNode is ConstantNode && Math.Abs(aNode.Evaluate(0.0, 0.0, 0.0, 0.0) - 1.0) < Constants.EPSILON;
		}

		public static bool IsBetween0And1(IArithmeticNode aNode)
		{
			if (aNode is VariableNode || aNode is SgNode || aNode is OsNode || aNode is EqualNode)
				return true;
			if ((aNode is MinNode || aNode is MaxNode || aNode is MultiplyNode)
				&& IsBetween0And1(((BinaryNode)aNode).LeftArgument) && IsBetween0And1(((BinaryNode)aNode).RightArgument))
				return true;
			//1-y
			if (aNode is MinusNode && IsOne(((MinusNode)aNode).LeftArgument) && IsBetween0And1(((MinusNode)aNode).RightArgument))
				return true;
			//a+b
			if (aNode is PlusNode && ((PlusNode)aNode).LeftArgument is VariableNode && ((PlusNode)aNode).RightArgument is VariableNode
				&& ((VariableNode)((PlusNode)aNode).LeftArgument).Variable.Equals(GetCoVariable(((VariableNode)((PlusNode)aNode).RightArgument).Variable)))
				return true;
			return false;
		}

		public static bool Is0Or1(IArithmeticNode aNode)
		{
			if (IsZero(aNode) || IsOne(aNode) || aNode is SgNode || aNode is OsNode
                || aNode is EqualNode)
				return true;
			if ((aNode is MinNode || aNode is MaxNode || aNode is MultiplyNode)
				&& Is0Or1(((BinaryNode)aNode).LeftArgument) && Is0Or1(((BinaryNode)aNode).RightArgument))
				return true;
			//1-z
			if (aNode is MinusNode && IsOne(((MinusNode)aNode).LeftArgument) && Is0Or1(((MinusNode)aNode).RightArgument))
				return true;
			return false;
		}

		public static bool Is0Or1Deep(IArithmeticNode aNode)
		{
			if (IsZero(aNode) || IsOne(aNode))
				return true;
			if (aNode is SgNode || aNode is OsNode)
			{
				IArithmeticNode arg = ((UnaryNode)aNode).Argument;
				if (arg is VariableNode || Is0Or1Deep(arg) ||
					(arg is MinusNode && IsOne(((MinusNode)arg).LeftArgument) && ((MinusNode)arg).RightArgument is VariableNode))
				return true;
			}
			if ((aNode is MinNode || aNode is MaxNode || aNode is MultiplyNode)
				&& Is0Or1Deep(((BinaryNode)aNode).LeftArgument) && Is0Or1Deep(((BinaryNode)aNode).RightArgument))
				return true;
			//1-z
			if (aNode is MinusNode && IsOne(((MinusNode)aNode).LeftArgument) && Is0Or1Deep(((MinusNode)aNode).RightArgument))
				return true;
			return false;
		}

        public static bool IsLinear(IArithmeticNode aNode)
        {
            //drugi constanti zasega sa leko opasni
            //if (aNode is ConstantNode && 
//                (Math.Abs(aNode.Evaluate(null)) < Constants.EPSILON) || (Math.Abs(aNode.Evaluate(null) - 1.0) < Constants.EPSILON))
  //              return true;
            if (IsZero(aNode) || IsOne(aNode))
                return true;
            if (aNode is VariableNode)
                return true;
            if (aNode is UnaryNode)
            {
                IArithmeticNode argument = ((UnaryNode)aNode).Argument;
                if ((aNode is SgNode || aNode is OsNode)
                    && IsLinear(argument))
                    return true;
            }
            if (aNode is BinaryNode)
            {
                IArithmeticNode left = ((BinaryNode)aNode).LeftArgument;
                IArithmeticNode right = ((BinaryNode)aNode).RightArgument;
                if ((aNode is PlusNode || aNode is MinusNode || aNode is MinNode || aNode is MaxNode || aNode is EqualNode)
                    && IsLinear(left) && IsLinear(right))
                    return true;
                //leko: sg(lin)*lin->lin? da, no moje i max(sg(lin),sg(lin))*lin->lin, tova go propuska
                if (aNode is MultiplyNode)
                {
                    if ((left is SgNode || left is OsNode) && IsLinear(((UnaryNode)left).Argument)
                        && IsLinear(right))
                        return true;
                    if ((right is SgNode || right is OsNode) && IsLinear(((UnaryNode)right).Argument)
                        && IsLinear(left))
                        return true;
                }
            }
            return false;
        }

		// aReplacements: xyz means a->x, b->y, c->z
		public static IArithmeticNode ReplaceVars(IArithmeticNode aNode, string aReplacements)
		{
			IArithmeticNode result = null;
			if (aNode is VariableNode)
			{
				char var = ((VariableNode)aNode).Variable;
				var = aReplacements[(int)var - (int)'a'];
				result = new VariableNode(var);
			}
			else
			{
				result = (IArithmeticNode)aNode.ShallowCopy();
				if (aNode is UnaryNode)
				{
					((UnaryNode)result).Argument = ReplaceVars(((UnaryNode)aNode).Argument, aReplacements);
				}
				else if (aNode is BinaryNode)
				{
					((BinaryNode)result).LeftArgument = ReplaceVars(((BinaryNode)aNode).LeftArgument, aReplacements);
					((BinaryNode)result).RightArgument = ReplaceVars(((BinaryNode)aNode).RightArgument, aReplacements);
				}
			}
			return result;
		}

		public static IArithmeticNode ReplaceVars(IArithmeticNode aNode, params IArithmeticNode[] aReplacements)
		{
			IArithmeticNode result = null;
			if (aNode is VariableNode)
			{
				char var = ((VariableNode)aNode).Variable;
				//tanak moment: pravi clone, za da ne stavat mazotii posle!
				result = (IArithmeticNode)aReplacements[(int)var - (int)'a'].Clone();
			}
			else
			{
				result = (IArithmeticNode)aNode.ShallowCopy();
				if (aNode is UnaryNode)
				{
					((UnaryNode)result).Argument = ReplaceVars(((UnaryNode)aNode).Argument, aReplacements);
				}
				else if (aNode is BinaryNode)
				{
					((BinaryNode)result).LeftArgument = ReplaceVars(((BinaryNode)aNode).LeftArgument, aReplacements);
					((BinaryNode)result).RightArgument = ReplaceVars(((BinaryNode)aNode).RightArgument, aReplacements);
				}
			}
			return result;
		}

		//a->b, d->c, ...
		public static char GetCoVariable(char variable)
		{
			int index = (int)variable;
			if (index % 2 == 1)
				index ++;
			else index--;
			return (char)index;
		}
	}
}
