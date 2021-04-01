using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace run2apps
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static string configPath = "run2apps/config.cfg";
        static Process mainProc;
        static Process slaveProc;

        static void ShowErr(int errCode)
        {
            switch (errCode)
            {
                case 0:
                    Console.WriteLine("ERROR: Config file not found!");
                    break;
                case 1:
                    Console.WriteLine("ERROR: Application .exe file names not configured properly!");
                    break;
                case 3:
                    Console.WriteLine("ERROR: Some application file is not exist!");
                    break;
                case 4:
                    Console.WriteLine("UNKNOWN ERROR: Sum ting went wong :/");
                    break;
            }

            Console.ReadLine();

            if (mainProc != null)
                mainProc.Kill();

            if (slaveProc != null)
                slaveProc.Kill();
        }

        static Process StartProcess(string fullPathWithArgs)
        {
            string[] pathWithArgsArr = fullPathWithArgs.Split(" ");
            string pathOnly = pathWithArgsArr.GetValue(0).ToString();

            if (!File.Exists(pathOnly))
                return null;

            ProcessStartInfo startInfo = new ProcessStartInfo(pathOnly);
            startInfo.WorkingDirectory = Path.GetDirectoryName(pathOnly);

            if (pathWithArgsArr.Length == 2)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            return Process.Start(startInfo);
        }

        static void Main(string[] args)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    ShowErr(0);
                    return;
                }

                string[] toRun = File.ReadAllLines(configPath);
                object mainAppPathObj = toRun.GetValue(0);
                object slaveAppPathObj = toRun.GetValue(1);

                if (mainAppPathObj == null || slaveAppPathObj == null)
                {
                    ShowErr(1);
                    return;
                }

                string mainAppPath = mainAppPathObj.ToString();
                string slaveAppPath = slaveAppPathObj.ToString();

                if (String.IsNullOrEmpty(mainAppPath) || String.IsNullOrEmpty(slaveAppPath)) {
                    ShowErr(1);
                    return;
                }

                mainProc = StartProcess(mainAppPath);
                slaveProc = StartProcess(slaveAppPath);

                if (mainProc == null || slaveProc == null)
                {
                    ShowErr(3);
                    return;
                }

                if (mainProc.StartInfo.WindowStyle == ProcessWindowStyle.Hidden || slaveProc.StartInfo.WindowStyle == ProcessWindowStyle.Hidden)
                    ShowWindow(GetConsoleWindow(), SW_HIDE);

                mainProc.WaitForExit();

                if (slaveProc != null)
                    slaveProc.Kill();
            }
            catch
            {
                ShowErr(4);
            }
        }
    }
}