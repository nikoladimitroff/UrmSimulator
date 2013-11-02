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
		private string[] commands;

		public int[] Registers { get; private set; }
		public string[] Commands
		{
			get { return this.commands; }
		}

		public int Memory
		{
			get { return this.Registers.Length - 1; }
		}

		internal UrmMachine(string[] commands, int maxMemory)
		{
			this.commands = commands;
			this.Registers = new int[maxMemory + 1];
		}

		internal UrmMachine(UrmMachine machine)
		{
			this.commands = machine.commands;
			this.Registers = new int[machine.Memory + 1];
		}

		public int ExecuteProgram()
		{
			char[] delimiters = new char[] { ' ' };

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
					ExecuteMetaCommand(normalized);
					break;
			}
			return counter;
		}

		private void ExecuteMetaCommand(string normalized)
		{
			string includeCommand = "#INCLUDE";
			if (normalized.ToUpperInvariant().StartsWith(includeCommand))
			{
				UrmMachine machine = ProgramLoader.Instance.LoadProgram(normalized.Remove(0, includeCommand.Length).Trim());
				int memoryLimit = Math.Min(machine.Memory, this.Memory);
				for (int i = 0; i < memoryLimit; i++)
				{
					machine.Registers[i] = this.Registers[i];
				}
				this.Registers[1] = machine.ExecuteProgram();
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
