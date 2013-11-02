using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UrmSimulator
{
	class ProgramLoader
	{
		private static ProgramLoader instance;
		private readonly string CommentSymbol = @"//";
		private string[] NewLineDelimiter = new string[] { Environment.NewLine };
		private readonly string DefaultFolder = @"SamplePrograms/";
		private readonly string UrmExtension = ".urm";

		private ConcurrentDictionary<string, UrmMachine> preloadedMachines;

		public static ProgramLoader Instance
		{
			get
			{
				if (instance == null)
					instance = new ProgramLoader();

				return instance;
			}
		}

		public ProgramLoader()
		{
			this.preloadedMachines = new ConcurrentDictionary<string, UrmMachine>();
		}

		public UrmMachine LoadProgram(string fileName)
		{
			UrmMachine machine;
			if (!preloadedMachines.TryGetValue(fileName, out machine))
			{
				string programText = File.ReadAllText(Path.Combine(DefaultFolder, fileName) + UrmExtension);
				string[] commands = programText.Split(NewLineDelimiter, StringSplitOptions.RemoveEmptyEntries);
				commands =
					(from command in commands
					where !command.StartsWith(CommentSymbol)
					let normalized = Regex.Replace(command, @"\(", " ")
					let secondNormalization = Regex.Replace(normalized, @"\)|,", string.Empty)
					select secondNormalization)
					.ToArray();

				machine = new UrmMachine(commands, CalculateUsedMemory(programText));
				preloadedMachines.TryAdd(fileName, machine);
			}
			return new UrmMachine(machine);
		}

		private int CalculateUsedMemory(string program)
		{
			string formatted = Regex.Replace(program, @"Z|S|T|J|\(|\)", string.Empty).Replace(',', ' ');
			int max = 0;
			string[] lines = formatted.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string line in lines)
			{
				// Ignore if comment or meta command
				if (line.StartsWith("#") || line.StartsWith(CommentSymbol))
					continue;

				string[] numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < Math.Min(numbers.Length, 2); i++)
				{
					int num;
					if (int.TryParse(numbers[i], out num))
					{
						if (num > max)
							max = num;
					}
				}
			}

			return max;
		}
	}
}
