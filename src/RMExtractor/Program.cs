using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RMExtractor {
    static class Program {

        [DllImport ("kernel32.dll")]
        static extern IntPtr GetConsoleWindow ();
        [DllImport ("user32.dll")]
        static extern bool ShowWindow (IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main (string [] args) {
            if (args.Length > 0) {
                HandleArgs (args);
            } else {
                ShowWindow (GetConsoleWindow (), 0);
                Application.EnableVisualStyles ();
                Application.SetCompatibleTextRenderingDefault (false);
                Application.Run (new frmMain ());
            }
        }

        /// <summary>
        /// Processa a entrada por linha de comando.
        /// </summary>
        /// <param name="args"></param>
        static void HandleArgs (string [] args) {
            if (!File.Exists (args [0])) {
                ShowHelp ();
            } else {
                Project project = new Project (args [1]);
                if (args [1] == "-p" || args [1] == "--pack") {
                    project.Pack ();
                } else if (args [1] == "-u" || args [1] == "--unpack") {
                    project.Unpack ();
                } else {
                    ShowHelp ();
                }
            }
        }

        /// <summary>
        /// Exibe informações de ajuda.
        /// </summary>
        static void ShowHelp () {
            Console.WriteLine ("O primeiro parametro deve sempre se o caminho para o arquivo de projeto.");
            Console.WriteLine ("=> rmpm.exe {projectPath}\n");
            Console.WriteLine ("Opcoes:");
            Console.WriteLine ("=> -p\tUne todos os scripts do diretório _scripts em um arquivo de scripts do projeto.");
            Console.WriteLine ("=> -u\tExtrai todos os scripts do arquivo de scripts para o diretório '_scripts' dentro da pasta do projeto.");
        }
    }
}
