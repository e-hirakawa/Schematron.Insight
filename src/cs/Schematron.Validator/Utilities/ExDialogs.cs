using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Forms;
using icon = System.Windows.Forms.MessageBoxIcon;
using static System.Windows.Forms.MessageBox;
using static System.Windows.Forms.MessageBoxButtons;


namespace Schematron.Validator.Utilities
{
    /// <summary>
    /// ダイアログ拡張クラス
    /// </summary>
    public class ExDialogs
    {
        /// <summary>
        /// required "Microsoft.WindowsAPICodePack.dll" &amp; "Microsoft.WindowsAPICodePack.Shell.dll"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        /// <param name="ReadOnlyEnabled"></param>
        /// <returns></returns>
        public static string SelectionFolder(string title, string path, bool ReadOnlyEnabled)
        {
            string dir = "";
            try
            {
                // Windows Vista/7 or newer
                if (CommonOpenFileDialog.IsPlatformSupported)
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    dialog.Title = title;

                    //読み取り専用フォルダも返すかどうか
                    dialog.EnsureReadOnly = ReadOnlyEnabled;
                    //trueにするとフォルダ選択モード
                    dialog.IsFolderPicker = true;

                    if (Directory.Exists(path))
                    {
                        dialog.InitialDirectory = path;
                    }
                    dialog.Multiselect = false;

                    //コントロールパネルなどの項目も選択可能にするかどうか
                    dialog.AllowNonFileSystemItems = false;

                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        dir = dialog.FileName;
                    }
                }
                else
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = title;
                    dialog.ShowNewFolderButton = true;
                    if (Directory.Exists(path))
                        dialog.SelectedPath = path;
                    DialogResult res = dialog.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        dir = dialog.SelectedPath;
                    }
                }
            }
            catch { }
            return dir;
        }
        /// <summary>
        /// required "Microsoft.WindowsAPICodePack.dll" &amp; "Microsoft.WindowsAPICodePack.Shell.dll"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="path"></param>
        /// <param name="ReadOnlyEnabled"></param>
        /// <returns></returns>
        public static string[] SelectionFolders(string title, string path, bool ReadOnlyEnabled)
        {
            string[] dir = new string[] { };
            try
            {
                // Windows Vista/7 or newer
                if (CommonOpenFileDialog.IsPlatformSupported)
                {
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    dialog.Title = title;

                    //読み取り専用フォルダも返すかどうか
                    dialog.EnsureReadOnly = ReadOnlyEnabled;
                    //trueにするとフォルダ選択モード
                    dialog.IsFolderPicker = true;

                    if (Directory.Exists(path))
                    {
                        dialog.InitialDirectory = path;
                    }
                    dialog.Multiselect = true;

                    //コントロールパネルなどの項目も選択可能にするかどうか
                    dialog.AllowNonFileSystemItems = false;

                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        dir = dialog.FileNames.ToArray();
                    }
                }
                else
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = title;
                    dialog.ShowNewFolderButton = true;
                    if (Directory.Exists(path))
                        dialog.SelectedPath = path;
                    DialogResult res = dialog.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        dir = new string[] { dialog.SelectedPath };
                    }
                }
            }
            catch { }
            return dir;
        }
        /// <summary>
        /// ファイル選択ダイアログ（複数）
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="filefilters">選択拡張子フィルタ</param>
        /// <param name="initdir">初期ディレクトリ</param>
        /// <returns>選択ファイルパスリスト</returns>
        public static string[] SelectionFiles(string title, string[] filefilters, string initdir = "")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = title;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.Multiselect = true;
            dialog.RestoreDirectory = true;
            if (Directory.Exists(initdir))
            {
                dialog.InitialDirectory = initdir;
            }
            StringBuilder sb = new StringBuilder();
            foreach (string ff in filefilters)
            {
                sb.Append($"{ff.Replace(".", "")}ファイル(*{ff})|*{ff}|");
            }
            sb.Append("全てのファイル(*.*)|*.*");
            dialog.FilterIndex = filefilters.Count();
            dialog.Filter = sb.ToString();
            DialogResult res = dialog.ShowDialog();
            return (res == DialogResult.OK) ? dialog.FileNames : new string[] { };
        }
        /// <summary>
        /// ファイル選択ダイアログ
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="filefilters">選択拡張子フィルタ</param>
        /// <param name="initdir">初期ディレクトリ</param>
        /// <returns>選択ファイルパス</returns>
        public static string SelectionFile(string title, string[] filefilters, string initdir = "")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = title;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.Multiselect = false;
            dialog.RestoreDirectory = true;
            if (Directory.Exists(initdir))
            {
                dialog.InitialDirectory = initdir;
            }
            StringBuilder sb = new StringBuilder();
            foreach (string ff in filefilters)
            {
                sb.Append($"{ff.Replace(".", "")}ファイル(*{ff})|*{ff}|");
            }
            sb.Append("全てのファイル(*.*)|*.*");
            dialog.FilterIndex = filefilters.Count();
            dialog.Filter = sb.ToString();

            return (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : "";
        }
        /// <summary>
        /// ファイル保存ダイアログ
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="filter">拡張子フィルタ</param>
        /// <param name="defautpath">初期ディレクトリ</param>
        /// <returns>保存先パス</returns>
        public static string SaveFileDialog(string title, string[] filter, string defautpath = "")
        {
            string file = "";
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = title;
            dialog.AddExtension = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.CheckPathExists = true;
            StringBuilder sb = new StringBuilder();
            foreach (string ff in filter)
            {
                sb.Append($"{ff.Replace(".", "")}ファイル(*{ff})|*{ff}|");
            }
            sb.Append("全てのファイル(*.*)|*.*");
            dialog.FilterIndex = 0;
            dialog.Filter = sb.ToString();
            dialog.OverwritePrompt = true;
            dialog.RestoreDirectory = true;
            if (defautpath != "")
            {
                dialog.InitialDirectory = Path.GetDirectoryName(defautpath);
                dialog.FileName = Path.GetFileName(defautpath);
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                file = dialog.FileName;
            }
            return file;
        }
        /// <summary>
        /// 警告ダイアログ
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public static void Warning(string msg)
        {
            Show(msg, "warning", OK, icon.Warning);
        }
        /// <summary>
        /// 情報ダイアログ
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public static void Information(string msg)
        {
            Show(msg, "", OK, icon.Information);
        }
        /// <summary>
        /// エラーダイアログ
        /// </summary>
        /// <param name="msg">メッセージ</param>
        public static void Error(string msg)
        {
            Show(msg, "", OK, icon.Error);
        }
        /// <summary>
        /// 確認ダイアログ
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <returns>true:OK</returns>
        public static bool Question(string msg)
        {
            return (Show(msg, "", YesNo, icon.Question) == DialogResult.Yes);
        }
        /// <summary>
        /// 警告ダイアログ
        /// </summary>
        /// <param name="msg">メッセージ</param>
        /// <returns>DialogResult</returns>
        public static DialogResult QuestionYesNoCancel(string msg)
        {
            return Show(msg, "", YesNoCancel, icon.Question);
        }
    }

}
