using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;


namespace FileSystemWatcherExplorer.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        #region プロパティー 非公開

        /// <summary>
        /// MainWindow Title
        /// </summary>
        public string Title { get; set; }

        FileSystemWatcher _fileSystemWatcher;

        private bool _enableRaisingEvents;
        private string _filter;
        private string _filters;
        private bool _includeSubdirectpries;
        private int _internalBuffersize;
        private string _path;

        private bool _isEnableChange;

        private bool _currentWactherState;

        // NotifyFilters
        private bool _nfFileName;
        private bool _nfDirectoryName;
        private bool _nfAttributes;
        private bool _nfSize;
        private bool _nfLastWrite;
        private bool _nfLastAccess;
        private bool _nfCreationTime;
        private bool _nfSecurity;
        #endregion

        #region プロパティ 公開


        public bool EnableRaisingEvents 
        {
            get { return _enableRaisingEvents; } 
            set { SetProperty(ref _enableRaisingEvents, value); } 
        }
        public string Filter 
        {
            get { return _filter; } 
            set { SetProperty(ref _filter, value); } 
        }
        public string Filters 
        { 
            get { return _filters; } 
            set { SetProperty(ref _filters, value); } 
        }
        public bool IncludeSubdirectpries 
        {
            get { return _includeSubdirectpries; } 
            set { SetProperty(ref _includeSubdirectpries, value); } 
        }
        public int InternalBuffersize 
        {
            get { return _internalBuffersize; } 
            set { SetProperty(ref _internalBuffersize, value); } 
        }
        public string Path 
        { 
            get { return _path; }
            set { SetProperty(ref _path, value); } 
        }


        public bool IsEnableChange { 
            get { return _isEnableChange; }
            set { SetProperty(ref _isEnableChange, value); } 
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CurrentWactherState 
        { 
            get { return _currentWactherState; }
            set { SetProperty(ref _currentWactherState, value); } 
        }
        /// <summary>
        /// 
        /// </summary>


        // Nf : NotifyFilters
        public bool NfFileName      
        {
            get { return _nfFileName; } 
            set { SetProperty(ref _nfFileName, value); } 
        } 
        public bool NfDirectoryName 
        {
            get { return _nfDirectoryName; }
            set { SetProperty(ref _nfDirectoryName , value); } 
        }
        public bool NfAttributes 
        {
            get { return _nfAttributes; }
            set { SetProperty(ref _nfAttributes, value); } 
        }
        public bool NfSize 
        {  
            get { return _nfSize; }
            set { SetProperty(ref _nfSize, value); } 
        }
        public bool NfLastWrite  
        {
            get { return _nfLastWrite; } 
            set { SetProperty(ref _nfLastWrite, value); } 
        }
        public bool NfLastAccess 
        {
            get { return _nfLastAccess; } 
            set { SetProperty(ref _nfLastAccess, value); } 
        }
        public bool NfCreationTime 
        { 
            get { return _nfCreationTime; } 
            set { SetProperty(ref _nfCreationTime, value); } 
        }
        public bool NfSecurity 
        {  
            get { return _nfSecurity; } 
            set { SetProperty(ref _nfSecurity, value); } 
        }

        public DelegateCommand SelectDirectoryCommand { get; }
        public DelegateCommand StartWatcherCommand { get; }
        public DelegateCommand StopWatcherCommand { get; }


        public ObservableCollection<string> Logs { get; } = new ObservableCollection<string>();

        #endregion

        #region メソッド 公開

        // Wct : WatcherChangeTypes 
        /// <summary>
        /// コンストラクタ
        /// </summary>        
        public MainWindowViewModel()
        {
            Title = "FileSystemWatcherExplorer";

            // 
            _fileSystemWatcher = new FileSystemWatcher();

            EnableRaisingEvents = _fileSystemWatcher.EnableRaisingEvents;
            Filter = _fileSystemWatcher.Filter;
            Filters = string.Join(';' ,_fileSystemWatcher.Filters);

            IncludeSubdirectpries = _fileSystemWatcher.IncludeSubdirectories;
            InternalBuffersize = _fileSystemWatcher.InternalBufferSize;
            Path = _fileSystemWatcher.Path;

            NfFileName = (_fileSystemWatcher.NotifyFilter & NotifyFilters.FileName) !=0 ? true : false;
            NfDirectoryName = (_fileSystemWatcher.NotifyFilter & NotifyFilters.DirectoryName) != 0 ? true : false;
            NfAttributes    = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Attributes) != 0 ? true : false;
            NfSize          = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Size) != 0 ? true : false;
            NfLastWrite     = (_fileSystemWatcher.NotifyFilter & NotifyFilters.LastWrite) != 0 ? true : false;
            NfLastAccess    = (_fileSystemWatcher.NotifyFilter & NotifyFilters.LastAccess) != 0 ? true : false;
            NfCreationTime  = (_fileSystemWatcher.NotifyFilter & NotifyFilters.CreationTime) != 0 ? true : false;
            NfSecurity      = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Security) != 0 ? true : false;
            //
            Path = @"D:\AA\BB\CC";

            _fileSystemWatcher.Changed += Fsw_Changed;
            _fileSystemWatcher.Created += Fsw_Created;
            _fileSystemWatcher.Deleted += Fsw_Deleted;
            _fileSystemWatcher.Renamed += Fsw_Renamed;
            _fileSystemWatcher.Error += Fsw_Error;

            // 
            SelectDirectoryCommand = new DelegateCommand(SelectDirectoryExecute);
            StartWatcherCommand = new DelegateCommand(StartWatcherExecute);
            StopWatcherCommand = new DelegateCommand(StopWatcherExecute);

            IsEnableChange = ! CurrentWactherState;
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            AddLog($"コンストラクタ完了  ThreadId = {id}");
        }
        #endregion

        #region メソッド非公開


        private void AddLog(string Log)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => 
            {
                Logs.Add(Log);
            }));
        }
        private void AddLog(StringBuilder sb)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (var item in sb.ToString().Split("\r\n"))
                {
                    Logs.Add(item);
                };
            }));
        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            string falseMessage;
            bool isFileExistsAndUnlocked = IsFileExistsAndUnlocked(e.FullPath, out falseMessage);
            StringBuilder sb = new StringBuilder();
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

            sb.AppendLine($"Created {e.ChangeType}  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ThreadId = {id}");
            sb.AppendLine($"    FilePath {e.FullPath}  isFileExistsAndUnlocked = {isFileExistsAndUnlocked}  {falseMessage}");
            if (isFileExistsAndUnlocked)
            {
                FileInfo info = new FileInfo(e.FullPath);
                sb.AppendLine($"    CreationTime  {info.CreationTime}");
                sb.AppendLine($"    LastWriteTime {info.LastWriteTime}");
                sb.AppendLine($"    Length        {info.Length}");
            }
            AddLog(sb);
            Debug.Write(sb);
        }


        private void Fsw_Changed(object sender, FileSystemEventArgs e ) 
        {
            string falseMessage;
            bool isFileExistsAndUnlocked = IsFileExistsAndUnlocked(e.FullPath, out falseMessage);
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Changed {e.ChangeType}  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ThreadId = {id}");
            sb.AppendLine($"    FilePath {e.FullPath}");
            if (isFileExistsAndUnlocked)
            {
                FileInfo info = new FileInfo(e.FullPath);
                sb.AppendLine($"    CreationTime  {info.CreationTime}");
                sb.AppendLine($"    LastWriteTime {info.LastWriteTime}");
                sb.AppendLine($"    Length        {info.Length}");  // 例外は発生することがある。
            }
            AddLog(sb);
            Debug.Write(sb);

        }
        private void Fsw_Deleted(object sender, FileSystemEventArgs e) 
        {
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Deleted {e.ChangeType}  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ThreadId = {id}");
            sb.AppendLine($"    FilePath {e.FullPath}");
            AddLog(sb);
            Debug.Write(sb);
        }
        private void Fsw_Renamed(object sender, RenamedEventArgs e) 
        {
            string falseMessage;
            bool isFileExistsAndUnlocked = IsFileExistsAndUnlocked(e.FullPath, out falseMessage);
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Renamed {e.ChangeType}  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ThreadId = {id}");
            sb.AppendLine($"    FullPath {e.FullPath}  isFileExistsAndUnlocked = {isFileExistsAndUnlocked}  {falseMessage}");
            if(isFileExistsAndUnlocked)
            {
                FileInfo info = new FileInfo(e.FullPath);
                sb.AppendLine($"    CreationTime  {info.CreationTime}");
                sb.AppendLine($"    LastWriteTime {info.LastWriteTime}");
                sb.AppendLine($"    Length        {info.Length}");  // 例外は発生することがある。
            }
            sb.AppendLine($"    OldFullath {e.OldFullPath}");
            AddLog(sb);
            Debug.Write(sb);
        }
        private void Fsw_Error(object sender,ErrorEventArgs e) 
        {
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Error {e.GetException().Message} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}  ThreadId={id}");
            AddLog(sb);
            Debug.WriteLine(sb);
        }

        /// <summary>
        /// フォルダ選択コマンドを実行する。
        /// </summary>
        private void SelectDirectoryExecute( )
        {
            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog())
            {
                cofd.Title = "監視する";
                cofd.IsFolderPicker = true;   // ホルダ選択モードにする。 

                CommonFileDialogResult result = cofd.ShowDialog();
                if(result == CommonFileDialogResult.Ok)
                {
                    Path = cofd.FileName;
                }
            }
        }

        private void UpdateSetting()
        {
            try
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
                _fileSystemWatcher.Filter = Filter;

                _fileSystemWatcher.Filters.Clear();
                _fileSystemWatcher.Filters.AddRange(Filters.Split(';').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)));

                _fileSystemWatcher.IncludeSubdirectories = IncludeSubdirectpries;
                _fileSystemWatcher.InternalBufferSize = InternalBuffersize;
                InternalBuffersize = _fileSystemWatcher.InternalBufferSize;
                _fileSystemWatcher.Path = Path;
                //
                NotifyFilters notifyFilters = 0;
                notifyFilters |= NfFileName ? NotifyFilters.FileName : 0;
                notifyFilters |= NfDirectoryName ? NotifyFilters.DirectoryName : 0;
                notifyFilters |= NfAttributes ? NotifyFilters.Attributes : 0;
                notifyFilters |= NfSize ? NotifyFilters.Size : 0;
                notifyFilters |= NfLastWrite ? NotifyFilters.LastWrite : 0;
                notifyFilters |= NfLastAccess ? NotifyFilters.LastAccess : 0;
                notifyFilters |= NfCreationTime ? NotifyFilters.CreationTime : 0;
                notifyFilters |= NfSecurity ? NotifyFilters.Security  : 0;
                _fileSystemWatcher.NotifyFilter = notifyFilters;

                //
                EnableRaisingEvents = true;
                _fileSystemWatcher.EnableRaisingEvents = EnableRaisingEvents;
                IsEnableChange = false;
            }
            catch(Exception ex)
            {
                AddLog($"ERROR： {ex.Message}");                
            }
            finally
            {

            }

        }
        private void StartWatcherExecute() 
        {
            AddLog("StartWatcherExecute()");
            Debug.WriteLine("StartWatcherExecute()");

            UpdateSetting();

            CurrentWactherState = _fileSystemWatcher.EnableRaisingEvents;
            IsEnableChange = !CurrentWactherState;
        }
        private void StopWatcherExecute() 
        {
            AddLog("StopWatcherExecute()");
            Debug.WriteLine("StopWatcherExecute()");
            EnableRaisingEvents = false;
            _fileSystemWatcher.EnableRaisingEvents = EnableRaisingEvents;
            CurrentWactherState = false;
            IsEnableChange = true;
        }

        // ファイルが存在して非ロックか。
        private static bool IsFileExistsAndUnlocked(string path, out string falseMessage)
        {
            falseMessage = string.Empty;
            FileStream? stream = null;
            if (! File.Exists(path))
            {
                falseMessage = "File.Exiets() is false.";
                return false; 
            }
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch(DirectoryNotFoundException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(FileNotFoundException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(IOException e)
            {
                falseMessage = e.Message;
                return false;
            }
            catch(Exception e)
            {
                falseMessage = e.Message;
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return true;
        }
        #endregion
    }
}
