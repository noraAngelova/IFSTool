using System;
using System.Collections;

namespace IFSTool.Expression
{
	/// <summary>
	/// Summary description for INode.
	/// </summary>
	public interface INode : ICloneable
	{
//		public abstract double Evaluate(double a, double b, double c, double d);
		INode ShallowCopy();
		void AddChildRight2Left(INode aNode);
		int NumberOfChilds { get; }
		//Accept(NodeVisitor)
		//AKO ISKAME IENUMERABLE, TRQBVA DA E ABSTRACT CLASS!
	}

	//po-dobar variant, po-blizak do immutable:
	//pri parsevane da generira decata i nakraq Factory-to osven string da priema i decata
}
