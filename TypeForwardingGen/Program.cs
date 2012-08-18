using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeForwardingGen
{
    class Program
    {
        static int Usage()
        {
            Console.WriteLine("Usage :");
            Console.WriteLine("TypeForwardingGen dst_file.cs assembly1 assembly2 ...");
            Console.WriteLine("dst_file.cs : name of the generated C# file");
            Console.WriteLine("assembly 1 .. n : assemblies which will have their types forwarding from the generated C# file");
            return 1;
        }

        static int Main(string[] args)
        {
            Console.WriteLine("TypeForwarding Assembly generator");
            Console.WriteLine("Copyright (c) Karim Audrey Luccin");

            if (args == null || args.Length < 2)
            {
                Console.WriteLine("Invalid argument counts");
                return Usage();
            }
            else
            {
                try
                {
                    var dest_file = args[0];

                    var assemblyNames = new string[args.Length - 1];

                    for (int i = 1; i < args.Length; i++)
                        assemblyNames[i-1] = args[i];

                    var generator = new ForwardGenerator();
                    generator.Generate(dest_file, assemblyNames);

                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return 2;
                }
            }
        }
    }
}
