using System;
using System.Collections;

namespace IFSTool
{
	/// <summary>
	/// Summary description for Set.
	/// </summary>
	public class Set : ICollection
	{
		//q napravi i ukazatel kam parviq dobaven element
		private Hashtable hashtable;

		public Set()
		{
			hashtable = new Hashtable();
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add Set.IsSynchronized getter implementation
				return false;
			}
		}

		public int Count
		{
			get
			{
				return hashtable.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			// TODO:  Add Set.CopyTo implementation
		}

		public object SyncRoot
		{
			get
			{
				// TODO:  Add Set.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			// TODO:  Add Set.GetEnumerator implementation
			return null;
		}

		#endregion

		public void Add(object element)
		{
			hashtable.Add(element.GetHashCode(), element);
		}
	}
}
