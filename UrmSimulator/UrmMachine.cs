using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UrmSimulator
{
	class UrmMachine
	{
		private string program;

		public int[] Registers { get; private set; }
		
		public int Memory
		{
			get { return this.Registers.Length; }
		}

		public UrmMachine(string program)
		{
			string formatted = Regex.Replace(program, @"Z|S|T|J|\(|\)", string.Empty).Replace(',', ' ');
			int max = 0;
			string[] lines = formatted.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string  line in lines)
			{
				string[] numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < Math.Min(numbers.Length, 2); i++)
				{
					try
					{
						int num = int.Parse(numbers[i]);
						if (num > max)
							max = num;
					}
					catch (Exception)
					{
						
					}
				}
			}

			this.program = program;
			this.Registers = new int[max + 1];
		}

		public int ExecuteProgram()
		{
			char[] delimiters = new char[] { ' ' };

			string[] commands = this.program.Split('\n');

			for (int counter = 0; counter < commands.Length; counter++)
			{
				string normalized = commands[counter].Replace('(', ' ').Replace(',', ' ').Replace(")", string.Empty);
				Debug.WriteLine(this.Registers.PrintCollection());
				counter = ExecuteCommand(delimiters, counter, normalized);
			}
			return this.Registers[1];
		}

		private int ExecuteCommand(char[] delimiters, int counter, string normalized)
		{
			switch (normalized[0])
			{
				case 'Z':
					int zeroRegister = int.Parse(normalized.Substring(1));
					this.Registers[zeroRegister] = 0;
					break;
				case 'S':
					int successorRegister = int.Parse(normalized.Substring(1));
					this.Registers[successorRegister]++;
					break;
				case 'T':
					string[] registers = normalized.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
					int source = int.Parse(registers[1]);
					int destination = int.Parse(registers[2]);
					this.Registers[destination] = this.Registers[source];
					break;
				case 'J':
					string[] operands = normalized.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
					int first = int.Parse(operands[1]);
					int second = int.Parse(operands[2]);
					int jump = int.Parse(operands[3]);
					if (this.Registers[first] == this.Registers[second])
						counter = jump - 1;
					break;
				case '#':
					string includeCommand = "#INCLUDE";
					if (normalized.ToUpperInvariant().StartsWith(includeCommand))
					{
						UrmMachine machine = new UrmMachine(File.ReadAllText(@"SamplePrograms/" + normalized.Remove(0, includeCommand.Length).Trim() + ".urm"));
						for (int i = 0; i < machine.Memory; i++)
						{
							machine.Registers[i] = this.Registers[i];
						}
						this.Registers[1] = machine.ExecuteProgram();
					}
					else
					{
						throw new NotImplementedException();
					}
					break;
			}
			return counter;
		} 
	}
}
