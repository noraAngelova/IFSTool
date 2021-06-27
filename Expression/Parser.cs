using System;
using System.Collections;

namespace IFSTool.Expression
{
	/// <summary>
	/// Summary description for Parser.
	/// </summary>
	public abstract class Parser
	{
		public virtual INode Parse(string aString)
		{
			Stack reversePolish = ConvertToReversePolish(aString);

			INode node = GenerateSubTree(reversePolish);

			return node;
		}

//		protected abstract string ConvertToReversePolish(string aExpression);

		protected virtual string NextToken(string aExpression, ref int aIndex)
		{
			if (aIndex >= aExpression.Length)
				return null;
			while (aIndex < aExpression.Length && aExpression[aIndex] == ' ')
				aIndex++;
			if (aIndex >= aExpression.Length)
				return null;
			//			char currentChar = aExpression[aIndex];
			//			if (currentChar == '(' || currentChar == ')' || currentChar == '+'
			//				|| currentChar == '-' || currentChar == '*' || currentChar == '^'
			//ne trqbva da e taka, 6toto se angajirame s konkretnite parseri! nali moje i drugi znaci da ima!
			
			int startIndex = aIndex;
			
			while (aIndex < aExpression.Length && 
				(Char.IsDigit(aExpression[aIndex])||aExpression[aIndex]=='.'))//leko, "." e i umnojenie!
				aIndex++;
			if (startIndex < aIndex)
			{
				string result = aExpression.Substring(startIndex, aIndex - startIndex);
				return result;
			}

			if (aExpression[aIndex] == '\\') //taka oba4e pozvolqvame \abc1 - ne6to s cifri, ne samo bukvi
				aIndex++;
			while (aIndex < aExpression.Length &&
                (Char.IsLetterOrDigit(aExpression[aIndex]) || aExpression[aIndex] == '_'))
				aIndex++;
			if (startIndex < aIndex)
			{
				string result = aExpression.Substring(startIndex, aIndex - startIndex);
				return result;
			}

			// tova vkli4va vsi4ki operatori, no samo po 1 char; za po-dalgi - override
			aIndex++;
			return aExpression[aIndex - 1].ToString();
		}

		protected virtual bool IsPrefixOperator(string aToken)
		{
			return false;
		}

		protected virtual Stack ConvertToReversePolish(string aExpression) // Stack<string>
		{
			//TODO: proverki za nevaliden string...
			Stack result = new Stack();
			Stack st = new Stack();
			st.Push("(");

			string x;
			string token;
			int index = 0;
			while((token = NextToken(aExpression, ref index)) != null)
			{
				if (Char.IsDigit(token[0]))
					result.Push(token);
				else if (IsPrefixOperator(token))
					st.Push(token);
				else if (Char.IsLetter(token[0]))
				{
					//tuk ima o6te edin moment: ako e prefixen operator kato \\neg...
					int tempIndex = index;
					string nextToken = NextToken(aExpression, ref tempIndex);
					if (nextToken == null || nextToken.Equals("("))
						st.Push(token);
					else
						result.Push(token);
				}
				else if (token.Equals("("))
					st.Push(token);
				else if (token.Equals(")"))
				{
					x = (string)(st.Peek());
					st.Pop();
					while (! x.Equals("("))
					{
						if (!x.Equals(","))//dano e OK taka
							result.Push(x);
						x = (string)(st.Peek());
						st.Pop();
					}
					//moje li x da ne e "(" ?
					string peek = (string)(st.Peek());
					if (/*x == '(' && */ Char.IsLetter(peek[0]))
					{
						result.Push(peek);
						st.Pop();
					}
				}
				else //if (aExpression[i] == ',' || aExpression[i] == '+' || aExpression[i] == '-' ||
					//aExpression[i] == '.' || aExpression[i] == '*' || aExpression[i] == '^')
					// leko 4e nqkade umnojenieto se izpuska (napr. ab ili c(c+d) )!!! zasega zabranqvame tova!
					// ^2 napravo da otiva sqr, t.e. napr. @ (v momenta Char na ^2 e '2'
				{
					x = (string)(st.Peek());
					st.Pop();
					while (x != "(" && //x != '[' && x != '{' &&
						Weight(x) <= Weight(token)) //tuk da proverqva, ako e 0 (ili -1) - Exception!
					{
						result.Push(x);
						x = (string)(st.Peek());
						st.Pop();
					}
					st.Push(x);
					st.Push(token);
				}
			}
			x = (string)(st.Peek());
			st.Pop();
			while (x != "(")
			{
				result.Push(x);
				x = (string)(st.Peek());
				st.Pop();
			}

			return result;
		}

		protected virtual int Weight(string aToken)
		{
			if (aToken.Equals(","))
				return Int32.MaxValue;//dali da ne dadem vazmojnost i za po-tejki?
			return 0;
		}

//		protected abstract INode GenerateSubTree(string aReversePolish, ref int aIndex);

		protected virtual INode GenerateSubTree(Stack aReversePolish)
		{
			string token = (string)aReversePolish.Pop();

			INode root = NodeFactory(token);

			int childs = root.NumberOfChilds;
			for (int index = 0; index < childs; index++)
			{
				root.AddChildRight2Left(GenerateSubTree(aReversePolish));
			}

			return root;
		}

		protected abstract INode NodeFactory(string aToken);
	}
}
