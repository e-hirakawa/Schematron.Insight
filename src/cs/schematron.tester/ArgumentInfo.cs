using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace schematron.tester.cui
{
    /// <summary>
    /// 実行時パラメータ情報
    /// </summary>
    public class ArgumentInfo
    {
        private string Separator = new String('*', 50);

        #region Assembly Informations
        private string _appName = "";
        private string _appVersion = "";
        private string _appModified = "";
        private string _appPermisson = "";
        #endregion
        #region Private Properties
        private List<FileInfo> _xmlfiles = new List<FileInfo>();
        private FileInfo _schfile = null;
        private FileInfo _outfile = null;
        #endregion  
        #region Public Properties
        /// <summary>
        /// 検証対象XMLファイルリスト
        /// </summary>
        public List<FileInfo> XmlFiles
        {
            get
            {
                if (_xmlfiles.Count == 0)
                {
                    string path = _XmlFile.Value?.ToString() ?? "";
                    List<string> files = new List<string>();
                    if (File.Exists(path) && Path.GetExtension(path).ToLower() == ".xml")
                    {
                        files.Add(path);
                    }
                    else if (Directory.Exists(path))
                    {
                        files.AddRange(Directory.GetFiles(path, "*.xml", SearchOption.TopDirectoryOnly));
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        _xmlfiles.Add(new FileInfo(files[i]));
                    }
                }
                return _xmlfiles;
            }
        }
        /// <summary>
        /// スキマトロンファイル
        /// </summary>
        public FileInfo SchFile
        {
            get
            {
                if (_schfile == null)
                {
                    string path = _SchFile.Value?.ToString() ?? "";
                    _schfile = !String.IsNullOrWhiteSpace(path) ? new FileInfo(path) : null;
                }
                return _schfile;
            }
        }
        /// <summary>
        /// レポートファイル
        /// </summary>
        public FileInfo OutFile
        {
            get
            {
                if (_outfile == null)
                {
                    string path = _OutFile.Value?.ToString() ?? "";
                    _outfile = !String.IsNullOrWhiteSpace(path) ? new FileInfo(path) : null;
                }
                return _outfile;
            }
        }
        /// <summary>
        /// レポート形式
        /// </summary>
        public string OutFormat => _OutFormat.Value?.ToString() ?? "";
        /// <summary>
        /// 検証フェーズ
        /// </summary>
        public string Phase => _Phase.Value?.ToString() ?? "";
        /// <summary>
        /// ヘルプ
        /// </summary>
        public bool Help => (_Help.Value is Boolean) ? (Boolean)_Help.Value : false;
        #endregion
        #region ArgumentItem
        private ArgumentItem _XmlFile = new ArgumentItem()
        {
            Keys = new[] { "-x", "-xml" },
            Name = "検証対象XML",
            IsRequired = true,
            IsOmitValue = false,
            Description = "ファイルパス．または複数ファイルがある場合にはそのフォルダパス．"
        };
        private ArgumentItem _SchFile =
            new ArgumentItem()
            {
                Keys = new[] { "-s", "-sch" },
                Name = "スキーマファイル",
                IsRequired = true,
                IsOmitValue = false,
                Description = "ファイルパス"
            };
        private ArgumentItem _OutFile =
            new ArgumentItem()
            {
                Keys = new[] { "-o", "-out" },
                Name = "レポートファイル",
                IsRequired = false,
                IsOmitValue = false,
                Description = "レポートファイルの出力先"
            };
        private ArgumentItem _OutFormat = new ArgumentItem()
        {
            Keys = new[] { "-f", "-format" },
            Name = "レポート形式",
            IsRequired = false,
            IsOmitValue = false,
            TrustOfValues = new string[] { "log", "tab", "xml", "json", "html" },
            Description = "形式一覧：log|tab|xml|json|html. 初期値:html."
        };
        private ArgumentItem _Phase = new ArgumentItem()
        {
            Keys = new[] { "-p", "-phase" },
            Name = "検証フェーズ",
            IsRequired = false,
            IsOmitValue = false,
            Description = "検証フェーズのIDを指定. 初期値:#ALL, defaultPhase:#DEFAULT, それ以外：ID値（#不要）."
        };
        private ArgumentItem _Help = new ArgumentItem()
        {
            Keys = new[] { "-h", "-help" },
            Name = "ヘルプ",
            IsRequired = false,
            IsOmitValue = true,
            Description = ""
        };
        private List<ArgumentItem> _items = new List<ArgumentItem>();
        #endregion
        public ArgumentInfo()
        {
            #region Set Assembly Informations
            Assembly asm = Assembly.GetEntryAssembly();
            AssemblyName asmn = asm.GetName();
            _appName = asmn.Name;
            _appVersion = asmn.Version.ToString();
            _appModified = System.IO.File.GetLastWriteTime(asm.Location).ToString("yyyy.MM.dd HH:mm:ss");
            AssemblyCopyrightAttribute copyright = asm.GetCustomAttribute<AssemblyCopyrightAttribute>();
            _appPermisson = copyright?.Copyright ?? "";
            #endregion
        }
        /// <summary>
        /// 引数読み込み
        /// </summary>
        /// <param name="args"></param>
        public void Load(string[] args)
        {
            try
            {
                #region Set Arguments Dat

                FieldInfo[] fields = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    ArgumentItem item = field.GetValue(this) as ArgumentItem;
                    if (item != null)
                    {
                        _items.Add(item);
                    }
                }
                foreach (string arg in args)
                {
                    foreach (ArgumentItem item in _items)
                    {
                        if (item.IsMatch(arg))
                            break;
                    }
                }
                #endregion
                List<string> msgs = new List<string>();
                if (XmlFiles.Count == 0)
                    msgs.Add($"{_XmlFile.Name}がありません");
                if (!(SchFile?.Exists ?? false))
                    msgs.Add($"{_SchFile.Name}がありません");
                if(OutFormat!="" && OutFile==null)
                    msgs.Add($"{OutFile.Name}がありません");

                if (msgs.Count > 0)
                    throw new Exception(String.Join("\n", msgs));

                if (Help)
                {
                    PrintHelp();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);

                PrintHelp();
            }
        }
        /// <summary>
        /// 実行可能判定
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                foreach (ArgumentItem item in _items)
                {
                    if (item.IsRequired && item.Value == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private void PrintHelp()
        {
            string op = ArgumentItem.OPERATOR;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("【使い方】");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{_appName} ");
            Console.Write($"{_XmlFile.Keys[0]}{op}<{_XmlFile.Name}> ");
            Console.Write($"{_SchFile.Keys[0]}{op}<{_SchFile.Name}> ");
            Console.WriteLine("[with options]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            Console.WriteLine("--");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("【パラメータ】");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (ArgumentItem item in _items)
            {
                string req = item.IsRequired ? "*" : "";
                Console.WriteLine($"<{item.Name}> {req}");
                if (!String.IsNullOrWhiteSpace(item.Description))
                    Console.WriteLine($"\t{item.Description}");
                Console.WriteLine($"\tキー：{String.Join(" | ", item.Keys)}");
                if (item.IsOmitValue)
                    Console.WriteLine($"\t（値不要）");

                Console.WriteLine("");

            }
            Console.WriteLine("");
            Console.WriteLine("*…必須");
            Console.WriteLine("--");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("【例：minimum】");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" {_appName} ");
            Console.Write($"{_XmlFile.Keys[0]}{op}c:\\input.xml ");
            Console.WriteLine($"{_SchFile.Keys[0]}{op}c:\\schema.sch ");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("【例：log出力】");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" {_appName} ");
            Console.Write($"{_XmlFile.Keys[0]}{op}c:\\input.xml ");
            Console.Write($"{_SchFile.Keys[0]}{op}c:\\schema.sch ");
            Console.Write($"{_OutFile.Keys[0]}{op}c:\\result.log ");
            Console.WriteLine($"{_OutFormat.Keys[0]}{op}{_OutFormat.TrustOfValues[0]}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("【例：フェーズ指定】");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" {_appName} ");
            Console.Write($"{_XmlFile.Keys[0]}{op}c:\\input.xml ");
            Console.Write($"{_SchFile.Keys[0]}{op}c:\\schema.sch ");
            Console.WriteLine($"{_Phase.Keys[0]}{op}PHASE-1");


            Console.WriteLine("--");
        }
        /// <summary>
        /// アプリケーション名、バージョンなどの
        /// コンソール出力
        /// </summary>
        public void PrintHeader()
        {
            Console.Title = $"{_appName} -ver.{_appVersion}";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Separator);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(_appName);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Version:{_appVersion}");
            Console.WriteLine($"Modified:{_appModified}");
            Console.WriteLine(_appPermisson);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine(Separator);
        }
    }
}
