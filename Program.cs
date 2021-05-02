using System;

namespace PexeFramework
{
    class Program
    {
        private static void Help()
        {
            Console.WriteLine("Usage --");
            Console.WriteLine("\tpexe [.pexe path]/[option]");
            Console.WriteLine("\t\t-new [name] : Generate a new project");
            Console.WriteLine("\t\t-pack [folder] [name] : Builds the .pexe file from the project folder");
        }

        static void Main(string[] args)
        {
            if (!System.IO.Directory.Exists(Settings.pexe_env)) // Install env
            {
                Console.WriteLine("[ENV] First time env setup (this may take a long time)...");

                try
                {
                    ProjectManagement.InstallEnv();
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error in Task [InstallEnv]: " + error.Message);
                }
            }

            if (args.Length == 0) // Check if no arguments are given
            {
                Help();
                Environment.Exit(1);
            }
            else if (args[0] == "-new") // User wants to make new project folder
            {
                if (args.Length != 2) // Check if correct arguments are given
                {
                    Help();
                    Environment.Exit(1);
                }

                if (System.IO.Directory.Exists(args[1])) // Check if the directory already exists
                {
                    Console.WriteLine($"AlreadyExists: The path [ {args[1]} ] already exists");
                    Environment.Exit(1);
                }

                try
                {
                    ProjectManagement.GenerateProject(args[1]); // Generate the project
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error in Task [GenerateProject]: " + error.Message);
                }
            }
            else if (args[0] == "-pack") // User wants to pack a existing project
            {
                if (args.Length != 3) // Check if correct arguments are given
                {
                    Help();
                    Environment.Exit(1);
                }

                if (!System.IO.Directory.Exists(args[1])) // Check if the defined project location exists
                {
                    Console.WriteLine($"NotFound: Could not locate [ {args[1]} ] on disk");
                    Environment.Exit(1);
                }

                if (System.IO.File.Exists(args[2] + ".pexe")) // Check if the packed file already exists in the current context
                {
                    Console.WriteLine($"AlreadyExists: The path [ {args[2]}.pexe ] already exists");
                    Environment.Exit(1);
                }

                try
                {
                    ProjectManagement.PackageProject(args[1], args[2]); // Package the project
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error in Task [PackageProject]: " + error.Message);
                }
            }
            else // User is trying to execute a .pexe file
            {
                if (!System.IO.File.Exists(args[0])) // Check if the path is valid
                {
                    Console.WriteLine($"NotFound: Could not locate [ {args[0]} ] on disk");
                    Environment.Exit(1);
                }

                try
                {
                    ProjectManagement.RunProject(args[0]); // Run the .pexe file
                }
                catch (Exception error)
                {
                    Console.WriteLine("Error in Task [RunProject]: " + error.Message);
                }
            }
        }
    }
}
