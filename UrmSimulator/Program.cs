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

			ProgramEncoder encoder = new ProgramEncoder();
			//Console.WriteLine(encoder.Encode(ProgramLoader.Instance.LoadProgram("Addition")));
			//Console.WriteLine(encoder.Decode(encoder.Encode(ProgramLoader.Instance.LoadProgram("LessThan"))));
			Console.WriteLine(encoder.Decode(2));
			//Console.WriteLine(encoder.Decode(BigInteger.Parse("454626412573189436451231236178")));

			//UrmMachine machine = ProgramLoader.Instance.LoadProgram("Multiplication");
			//machine.Registers[1] = 17;
			//machine.Registers[2] = 20;
			//Console.WriteLine(machine.ExecuteProgram());
		}

	}
}
