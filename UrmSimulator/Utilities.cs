using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrmSimulator
{
	public static class Utilities
	{
		public static string PrintCollection<T>(this IEnumerable<T> collection)
		{
			return collection.PrintCollection(", ");
		}

		public static string PrintCollection<T>(this IEnumerable<T> collection, string delimiter)
		{
			return collection.Aggregate(string.Empty, (x, y) => x += y.ToString() + delimiter);
		}
		
	}
}
