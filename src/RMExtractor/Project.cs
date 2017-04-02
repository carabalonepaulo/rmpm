using IronRuby.Builtins;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RMExtractor {
    /// <summary>
    /// Estrutura que aramzenará temporariamente o script após carregado.
    /// </summary>
    struct Script {
        /// <summary>
        /// Correspondente ao 'section' do script.
        /// </summary>
        public int Id;

        /// <summary>
        /// Título do script, o nome que aparecerá na lista de scripts do RPG Maker.
        /// </summary>
        public string Title;

        /// <summary>
        /// Conteúdo do script, o código em si.
        /// </summary>
        public string Content;
    }

    class Project {
        /// <summary>
        /// Recursos usados pelo IronRuby para criar o ambientem em Ruby.
        /// </summary>
        ScriptRuntime runtime;
        ScriptEngine engine;
        ScriptScope scope;

        /// <summary>
        /// Lista de extensões associadas a versão do projeto.
        /// </summary>
        Dictionary<string, string> extensions;

        /// <summary>
        /// Pasta do projeto.
        /// </summary>
        string projectPath;

        /// <summary>
        /// Caminho para o arquivo Scripts.ext.
        /// </summary>
        string scriptsFilePath;

        /// <summary>
        /// Caminho para a pasta onde serão extraídos os scripts.
        /// </summary>
        string outputScriptsPath;

        /// <summary>
        /// Caminho para o arquivo de projeto do RPG Maker.
        /// </summary>
        string rmProjectFilePath;

        /// <summary>
        /// Scripts carregados do projeto.
        /// </summary>
        Script [] scripts;

        /// <summary>
        /// Ordem dos scripts.
        /// </summary>
        List<int> scriptsOrder;

        public Project (string path) {
            scriptsOrder = new List<int> ();

            extensions = new Dictionary<string, string> ();
            extensions.Add ("RPGXP", "rxdata");
            extensions.Add ("RPGVX", "rvdata");
            extensions.Add ("RPGVXAce", "rvdata2");

            rmProjectFilePath = path;
            projectPath = new FileInfo (path).DirectoryName;
            scriptsFilePath = $"{projectPath}\\Data\\Scripts.{GetExtension ()}";
            outputScriptsPath = $"{projectPath}\\_scripts";

            runtime = IronRuby.Ruby.CreateRuntime ();
            engine = IronRuby.Ruby.GetEngine (runtime);
            scope = engine.CreateScope ();
            engine.Execute (@"load_assembly 'IronRuby.Libraries', 'IronRuby.StandardLibrary.Zlib'", scope);
        }

        /// <summary>
        /// Rotina de serialização com Ruby.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte [] MarshalDump (object data) {
            scope.SetVariable ("data", data);
            MutableString result = engine.Execute (@"Marshal.dump(data)", scope);
            scope.RemoveVariable ("data");
            return result.ToByteArray ();
        }

        /// <summary>
        /// Rotina de deserialização com Ruby.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object MarshalLoad (byte [] data) {
            scope.SetVariable ("data", MutableString.CreateBinary (data));
            object result = engine.Execute (@"Marshal.load(data)", scope);
            scope.RemoveVariable ("data");
            return result;
        }

        /// <summary>
        /// Obtém a extensão dos arquivos serialziados.
        /// </summary>
        /// <returns></returns>
        string GetExtension () {
            string ver = File.ReadAllText (rmProjectFilePath).Split (' ') [0];
            return extensions [ver];
        }

        /// <summary>
        /// Carrega os scripts do projeto.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        void Load () {
            RubyArray r = (RubyArray)MarshalLoad (File.ReadAllBytes (scriptsFilePath));
            scripts = new Script [r.Count];
            for (int i = 0; i < r.Count; i++) {
                RubyArray n = (RubyArray)r [i];
                scripts [i] = new Script () {
                    Id = int.Parse (n [0].ToString ()),
                    Title = ((MutableString)n [1]).ToString (Encoding.UTF8),
                    Content = Encoding.UTF8.GetString (Ionic.Zlib.ZlibStream.UncompressBuffer (((MutableString)n [2]).ToByteArray ()))
                };
                scriptsOrder.Add (scripts [i].Id);
            }
        }

        /// <summary>
        /// Remove caracteres ilegais do nome do arquivo.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string SanitizeFileName (string input) {
            char [] invalidChars = Path.GetInvalidFileNameChars ();
            return new string (input.Where (x => !invalidChars.Contains (x)).ToArray ());
        }

        /// <summary>
        /// Junta todos os scripts da pasta _scripts em arquivo próprio para o RPG Maker.
        /// </summary>
        public void Pack () {
            List<Script> order = new List<Script> ();
            List<string> files = new List<string> ();
            
            // Recupera a lista contendo a ordem dos arquivos.
            using (MemoryStream ms = new MemoryStream (File.ReadAllBytes ($"{outputScriptsPath}\\scripts.dat"))) {
                using (BinaryReader reader = new BinaryReader (ms, Encoding.UTF8)) {
                    try {
                        while (ms.CanRead) {
                            order.Add (new Script () {
                                Id = reader.ReadInt32 (),
                                Title = reader.ReadString ()
                            });
                        }
                    } catch (EndOfStreamException) { }
                }
            }

            files.AddRange (Directory.GetFiles (outputScriptsPath));
            files.Remove (files.Where (f => f.Contains ("scripts.dat")).First ());
            files.Remove ($"{outputScriptsPath}\\{SanitizeFileName (order [order.Count - 1].Title)}.rb");

            RubyArray rmScripts = new RubyArray ();
            string content = string.Empty;
            for (int i = 0; i < order.Count - 1; i++) {
                string filename = $"{outputScriptsPath}\\{SanitizeFileName (order [i].Title)}.rb";

                if (files.Contains (filename)) { files.Remove (filename); }
                if (!File.Exists (filename)) { continue; }

                content = File.ReadAllText (filename);

                RubyArray rb = new RubyArray ();
                rb.Add (order [i].Id);
                rb.Add (MutableString.Create (order [i].Title, RubyEncoding.UTF8));
                rb.Add (MutableString.CreateBinary (Ionic.Zlib.ZlibStream.CompressString (content)));

                rmScripts.Add (rb);
            }

            // Verifica se existem novos arquivos para serem adicionados.
            if (files.Count > 0) {
                Random rdn = new Random ();
                for (int i = 0; i < files.Count; i++) {
                    FileInfo info = new FileInfo (files [i]);
                    RubyArray rb = new RubyArray ();

                    rb.Add (rdn.Next (11111111, 99999999));
                    rb.Add (MutableString.Create (info.Name.Replace (".rb", ""), RubyEncoding.UTF8));
                    rb.Add (MutableString.CreateBinary (Ionic.Zlib.ZlibStream.CompressString (File.ReadAllText (files [i]))));

                    rmScripts.Add (rb);
                }
            }

            rmScripts.Add (new RubyArray () {
                order [order.Count - 1].Id,
                MutableString.Create (order [order.Count - 1].Title, RubyEncoding.UTF8),
                MutableString.CreateBinary (Ionic.Zlib.ZlibStream.CompressString (
                    File.ReadAllText($"{outputScriptsPath}\\{SanitizeFileName (order [order.Count - 1].Title)}.rb")))
            });

            File.WriteAllBytes (scriptsFilePath, MarshalDump (rmScripts));
            DarkUI.Forms.DarkMessageBox.ShowInformation ("Importação concluída com sucesso!", "Relatório");
        }

        /// <summary>
        /// Exporta todos os scripts para uma pasta dentro da pasta do projeto.
        /// </summary>
        /// <param name="scripts"></param>
        /// <param name="path"></param>
        public void Unpack () {
            if (!Directory.Exists (outputScriptsPath))
                Directory.CreateDirectory (outputScriptsPath);

            Load ();

            using (MemoryStream stream = new MemoryStream ()) {
                using (BinaryWriter writer = new BinaryWriter (stream, Encoding.UTF8)) {
                    foreach (int id in scriptsOrder) {
                        Script script = scripts.Where (s => s.Id == id).First ();
                        if (string.IsNullOrEmpty (script.Title))
                            continue;

                        writer.Write (script.Id);
                        writer.Write (script.Title);
                    }
                }
                File.WriteAllBytes ($"{outputScriptsPath}\\scripts.dat", stream.ToArray ());
            }

            for (int i = 0; i < scripts.Count (); i++) {
                if (string.IsNullOrEmpty (scripts [i].Title))
                    continue;
                Console.WriteLine (scripts [i].Id);

                using (StreamWriter writer = new StreamWriter ($"{outputScriptsPath}\\{SanitizeFileName (scripts [i].Title)}.rb")) {
                    writer.Write (scripts [i].Content);
                }
            }

            DarkUI.Forms.DarkMessageBox.ShowInformation ("Extração concluída com sucesso!", "Relatório");
        }
    }
}
