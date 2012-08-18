using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TypeForwardingGen
{
    public class ForwardGenerator
    {
        private IEnumerable<Type> GetAllPublicTypes(string[] assemblyNames)
        {
            foreach (var assemblyName in assemblyNames)
            {
                Console.WriteLine("Processing {0}", assemblyName);

                Assembly assembly;

                if (File.Exists(assemblyName))
                    assembly = Assembly.LoadFile(assemblyName);
                else
                    assembly = Assembly.LoadWithPartialName(assemblyName);

                if (assembly == null)
                    throw new InvalidOperationException(String.Format("Could not load {0}", assemblyName));

                foreach (var assembly_type in assembly.GetTypes())
                {
                    if (assembly_type.IsPublic)
                    {
                        yield return assembly_type;
                    }
                }
            }

            Console.WriteLine("Finished");
        }

        /// <summary>
        /// Generate a C# file source containing a [assembly:TypeForwardedToAttribute(typeof(Example))] attribute for each
        /// types of the specified dlls
        /// </summary>
        /// <param name="dest_file"></param>
        /// <param name="assemblyNames"></param>
        public void Generate(string dest_file, string[] assemblyNames)
        {
            using (var dest_stream = new FileStream(dest_file, FileMode.Create))
            {
                using (var dest_writer = new StreamWriter(dest_stream))
                {
                    Generate(dest_writer, assemblyNames);

                    dest_writer.Flush();
                }
            }
        }


        /// <summary>
        /// Generate a C# file source containing a [assembly:TypeForwardedToAttribute(typeof(Example))] attribute for each
        /// types of the specified dlls
        /// </summary>
        /// <param name="dest_writer"></param>
        /// <param name="assemblyNames"></param>
        public void Generate(StreamWriter dest_writer, string[] assemblyNames)
        {
            dest_writer.WriteLine("using System;");
            dest_writer.WriteLine("using System.Runtime.CompilerServices");
            dest_writer.WriteLine();
            dest_writer.WriteLine();

            dest_writer.WriteLine("/* Type Forwarded from");

            foreach (var assemblyName in assemblyNames)
            {
                dest_writer.WriteLine(String.Format("\t{0}", assemblyName));
            }

            dest_writer.WriteLine(" */");

            dest_writer.WriteLine();
            dest_writer.WriteLine();

            foreach (var asm_type in GetAllPublicTypes(assemblyNames))
            {
                dest_writer.WriteLine(String.Format("[assembly:TypeForwardedToAttribute(typeof({0}))]", asm_type.FullName));
            }
        }
    }
}
