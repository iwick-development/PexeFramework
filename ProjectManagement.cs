using System;
using System.IO;
using System.Net;
using System.Linq;
using System.IO.Compression;

namespace PexeFramework
{
    class ProjectManagement
    {
        public static void Caller(string file, string arguments, bool hide_output)
        {
            using (System.Diagnostics.Process callerP = new())
            {
                callerP.StartInfo.FileName = file;
                callerP.StartInfo.Arguments = arguments;
                callerP.StartInfo.UseShellExecute = false;
                callerP.StartInfo.CreateNoWindow = hide_output;
                callerP.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                callerP.Start();
                callerP.WaitForExit();
            }
        }

        public static void GenerateProject(string name)
        {
            Console.WriteLine("[Proj] Generating new project...");

            Directory.CreateDirectory(name); // Create project directory

            File.Create($"{name}/main.py").Dispose(); // Create blank main.py file
            File.Create($"{name}/requirements.txt").Dispose(); // Create blank requirements.txt file

            Console.WriteLine("[Proj] Generation complete");
        }

        public static void PackageProject(string path, string name)
        {
            Console.WriteLine("[Pack] Packing project...");

            if (!Directory.Exists(path))
            {
                Console.WriteLine("[Err] Project path did not exist");
                return;
            }

            if (!File.Exists(path + "/main.py"))
            {
                Console.WriteLine("[Err] Entry 'main.py' does not exist");
                return;
            }

            if (!File.Exists(path + "/requirements.txt"))
            {
                Console.WriteLine("[Err] Requirements.txt is missing");
                return;
            }

            ZipFile.CreateFromDirectory(path, name + ".pexe"); // Zip the files in the project and save the .pexe file

            Console.WriteLine($"[Pack] Project packed to: {name}.pexe");
        }

        public static void RunProject(string path)
        {
            string validchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string execute_token = new string(Enumerable.Repeat(validchars, 25)
                .Select(s => s[random.Next(s.Length)]).ToArray()); // Generate random execute token

            string execute_path = Settings.pexe_env + execute_token; // Convert to execute path

            ZipFile.ExtractToDirectory(path, execute_path); // Extract .pexe file to execute path

            if (!File.Exists(execute_path + "/main.py"))
            {
                Console.WriteLine("[Err] Invalid or corrupt file: Missing main.py");
                Directory.Delete(execute_path, true);
                return;
            }

            if (!File.Exists(execute_path + "/requirements.txt"))
            {
                Console.WriteLine("[Err] Invalid or corrupt file: Missing requirements.txt");
                Directory.Delete(execute_path, true);
                return;
            }

            Caller(Settings.pexe_env + "python.exe", "-m pip install -r \"" + execute_path + "/requirements.txt\"", true); // Install pip requirements

            Caller(Settings.pexe_env + "python.exe", execute_path + "/main.py", false); // Execute main.py

            Directory.Delete(execute_path, true); // Cleanup and delete execute directory
        }

        public static void InstallEnv()
        {
            if (!Directory.Exists(Settings.pexe_env))
            {
                Directory.CreateDirectory(Settings.pexe_env); // Create env directory
            }

            using (WebClient webclient = new()) // Download python embeded and get_pip
            {
                webclient.DownloadFile(Settings.pythonenv_url, Settings.pexe_env + "pyembed.zip");
                webclient.DownloadFile(Settings.getpip_url, Settings.pexe_env + "get_pip.py");
            }

            ZipFile.ExtractToDirectory(Settings.pexe_env + "pyembed.zip", Settings.pexe_env); // Extract python

            File.Delete(Settings.pexe_env + "pyembed.zip"); // Delete zipfile

            File.AppendAllText(Settings.pexe_env + "python39._pth", "import site"); // Import site in config for pip to work

            Caller(Settings.pexe_env + "python.exe", Settings.pexe_env + "get_pip.py", true); // Run get_pip
        }
    }
}
