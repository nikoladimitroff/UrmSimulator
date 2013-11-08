using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UrmSimulator
{
	class ProgramEncoder
	{
		private EncodingScheme scheme = new EncodingScheme();
		private static char[] delimiters = new char[] { ' ' };

		public BigInteger Encode(UrmMachine machine)
		{
			IList<BigInteger> commandEncodings = machine.Commands.Select(EncodeCommand).ToList();

			BigInteger result = this.scheme.EncodeTuple(commandEncodings);
			return result;
		}

		private BigInteger ParseRegisterNumber(string text)
		{
			return BigInteger.Parse(text) - 1;
		}

		private BigInteger EncodeCommand(string command)
		{
			switch (command[0])
			{
				case 'Z':
					return 4 * this.ParseRegisterNumber(command.Substring(1));
				case 'S':
					return 4 * this.ParseRegisterNumber(command.Substring(1)) + 1;
				case 'T':
					string[] arguments = command.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
					return 4 * this.scheme.Triple.Encode(this.ParseRegisterNumber(arguments[1]), this.ParseRegisterNumber(arguments[2])) + 2;
				case 'J':
					string[] args = command.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
					return
						4 * this.scheme.Triple.Encode(
							this.ParseRegisterNumber(args[1]),
							this.scheme.Triple.Encode(
								this.ParseRegisterNumber(args[2]),
								this.ParseRegisterNumber(args[3]))) +
						3;
				default:
					throw new NotSupportedException("Can't encode a non-normal program!");
			};
		}

		public string Decode(BigInteger z)
		{
			IList<BigInteger> commandCodes = this.scheme.DecodeTuple(z);
			return commandCodes.Select(DecodeCommand).PrintCollection(Environment.NewLine);
		}

		private string DecodeCommand(BigInteger z)
		{
			int remainder = (int) (z % 4);
			switch (remainder)
			{
				case 0:
					return string.Format("Z({0})", (z / 4) + 1);
				case 1:
					return string.Format("S({0})", ((z - 1) / 4) + 1);
				case 2:
					return string.Format("T({0}, {1})", this.scheme.Triple.Left((z - 2) / 4) + 1, this.scheme.Triple.Right((z - 2) / 4) + 1);
				case 3:
					BigInteger m = this.scheme.Triple.Left((z - 3) / 4) + 1;
					BigInteger nPairedQ = this.scheme.Triple.Right((z - 3) / 4);
					BigInteger n = this.scheme.Triple.Left(nPairedQ) + 1;
					BigInteger q = this.scheme.Triple.Right(nPairedQ) + 1;
					return string.Format("J({0}, {1}, {2})", m, n, q);
				default:
					return string.Empty;
			}
		}
	}
}
