using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace UrmSimulator
{
	static class Program
	{
		static void Main(string[] args)
		{
			//EncodingTriple Triple = new EncodingTriple();
			//int x = 5;
			//int y = 110002;
			//var z = Triple.Encode(x, y);
			//Console.WriteLine(Triple.Left(z));
			//Console.WriteLine(Triple.Right(z));

			//List<BigInteger> list = new List<BigInteger>() { 1, 2, 100, 1000, 200, 450, 300 };
			//Console.WriteLine(list.PrintCollection());
			//EncodingScheme scheme = new EncodingScheme();
			//Console.WriteLine(scheme.DecodeTuple(scheme.EncodeTuple(list)).PrintCollection());

			//ProgramEncoder encoder = new ProgramEncoder();
			//Console.WriteLine(encoder.Decode(encoder.Encode(File.ReadAllText(@"Program.urm"))));

			UrmMachine machine = ProgramLoader.Instance.LoadProgram("Substraction");
			machine.Registers[1] = 34;
			machine.Registers[2] = 17;
			Console.WriteLine(machine.ExecuteProgram());
		}

	}
}
