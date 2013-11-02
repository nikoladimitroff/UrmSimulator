using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UrmSimulator
{
	public class EncodingTriple
	{
		private BigInteger Power(BigInteger @base, BigInteger exponent)
		{
			BigInteger result = 1;
			for (BigInteger i = 0; i < exponent; i++)
			{
				result *= @base;
			}
			return result;
		}

		public BigInteger Encode(BigInteger x, BigInteger y)
		{
			return Power(2, x) * (2 * y + 1) - 1;
		}

		public BigInteger Left(BigInteger z)
		{
			BigInteger counter = 0;
			z++;
			while (z % 2 == 0)
			{
				counter++;
				z /= 2;
			}
			return counter;
		}

		public BigInteger Right(BigInteger z)
		{
			return ((((z + 1) / Power(2, Left(z))) - 1) / 2);
		}
	}
}
