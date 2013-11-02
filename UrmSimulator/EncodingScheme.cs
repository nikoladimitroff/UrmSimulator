using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UrmSimulator
{
	class EncodingScheme
	{
		public EncodingTriple Triple { get; private set; }

		public EncodingScheme()
		{
			this.Triple = new EncodingTriple();
		}

		public BigInteger EncodeTuple(IList<BigInteger> tuple)
		{
			return this.Triple.Encode(tuple.Count, this.EncodeTuple(tuple, 0));
		}

		private BigInteger EncodeTuple(IList<BigInteger> tuple, int from)
		{
			if (from == tuple.Count - 2)
				return this.Triple.Encode(tuple[from], tuple[from + 1]);

			return this.Triple.Encode(tuple[from], this.EncodeTuple(tuple, from + 1));
		}

		public IList<BigInteger> DecodeTuple(BigInteger z)
		{
			BigInteger size = this.Triple.Left(z);
			IList<BigInteger> tuple = new List<BigInteger>();
			BigInteger membersLeft = this.Triple.Right(z);
			for (int i = 0; i < size; i++)
			{
				if (i < size - 1)
				{
					BigInteger left = this.Triple.Left(membersLeft);
					membersLeft = this.Triple.Right(membersLeft);
					tuple.Add(left);
				}
				else
				{
					tuple.Add(membersLeft);
				}
			}

			return tuple;
		}
	}
}
