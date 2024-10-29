﻿using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

using dp.DpxInstructionSet;
using dp.DpxFileHandler;
using dp.DpxDisassembler;

using McMaster.Extensions.CommandLineUtils;
using System.Net.Mail;

namespace dp
{

    [Command(Name = "dp", Description = "Parse/patch UEFI module's dependency expressions")]
    [HelpOption("-h")]
    public class Program
    {
        private static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        [Argument(0, Description = "The depex file to be processed.")]
        private string filename { get; }

        [Option("-d", Description = "Disassemble the given depex")]
        public bool OptionDisassemble { get; }

        [Option("-o", Description = "File to receive the output")]
        public string OutputFile { get; } = "output.txt";

        private async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                app.ShowHelp();
                return 0;
            }

            //
            // Option "-d" was present.
            //
            if (OptionDisassemble)
            {
                Console.WriteLine("[+] Disassembled output:\n");
                var disassembler = new DpxDisassembler.DpxDisassembler();
                var depex = FileHandler.DpxReadFile(filename);

                if (depex == null && depex.Length < 0)
                {
                    Console.WriteLine("[-] Depex file seems to be null. Aborting...");
                }

                //
                // Extract & print the depex's header
                //
                Console.WriteLine("\n[i] Header: ");
                Console.WriteLine("---------------------------------");
                var headerDisassembledBytecode = disassembler.DpxDisassembleHeader(depex);
                foreach (var _byte in headerDisassembledBytecode)
                {
                    Console.Write($"{_byte:X} ");
                }

                //
                // Disassemble the depex's body
                //
                Console.WriteLine("\n\n[i] Body: ");
                Console.WriteLine("---------------------------------");
                var bodyDisassembledBytecode = disassembler.DpxDisassembleBody(depex);
                Console.Write(bodyDisassembledBytecode.ToString());
                return 0;
            }

            //
            // Option "-o" was present.
            //
            if (!string.IsNullOrWhiteSpace(OutputFile))
            {
                var disassembler = new DpxDisassembler.DpxDisassembler();
                var depex = FileHandler.DpxReadFile(filename);
                var bodyDisassembledBytecode = disassembler.DpxDisassembleBody(depex);

                File.WriteAllText(OutputFile, bodyDisassembledBytecode.ToString());

                return 0;
            }

            return 0;
        }

    }
}
