using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileSystemWatcherExplorer.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// MainWindow Title
        /// </summary>
        public string Title { get; set; }

        FileSystemWatcher _fileSystemWatcher;




        // NotifyFilters
        private bool _nfFileName;
        private bool _nfDirectoryName;
        private bool _nfAttributes;
        private bool _nfSize;
        private bool _nfLastWrite;
        private bool _nfLastAccess;
        private bool _nfCreationTime;
        private bool _nfSecurity;

        // 
        



        public bool EnableRaisingEvents { get; set; }
        public string Filter { get; set; }
        public bool IncludeSubdirectpries { get; set; }
        public int InternalBuffersize { get; set; }
        public string Path { get; set; }
//        public 
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
            IncludeSubdirectpries = _fileSystemWatcher.IncludeSubdirectories;
            InternalBuffersize = _fileSystemWatcher.InternalBufferSize;
            
            NfFileName      = (_fileSystemWatcher.NotifyFilter & NotifyFilters.FileName) !=0 ? true : false;
            NfDirectoryName = (_fileSystemWatcher.NotifyFilter & NotifyFilters.DirectoryName) != 0 ? true : false;
            NfAttributes    = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Attributes) != 0 ? true : false;
            NfSize          = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Size) != 0 ? true : false;
            NfLastWrite     = (_fileSystemWatcher.NotifyFilter & NotifyFilters.LastWrite) != 0 ? true : false;
            NfLastAccess    = (_fileSystemWatcher.NotifyFilter & NotifyFilters.LastAccess) != 0 ? true : false;
            NfCreationTime  = (_fileSystemWatcher.NotifyFilter & NotifyFilters.CreationTime) != 0 ? true : false;
            NfSecurity      = (_fileSystemWatcher.NotifyFilter & NotifyFilters.Security) != 0 ? true : false;
            //
            Path = _fileSystemWatcher.Path;


            _fileSystemWatcher.Changed += Fsw_Changed;
            _fileSystemWatcher.Created += Fsw_Created;
            _fileSystemWatcher.Deleted += Fsw_Deleted;
            _fileSystemWatcher.Renamed += Fsw_Renamed;
            _fileSystemWatcher.Error += Fsw_Error;

            // 
            SelectDirectoryCommand = new DelegateCommand(SelectDirectoryExecute);


        }



        private void UpdateSetting()
        {
            CommonOpenFileDialog cod = new CommonOpenFileDialog();


        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e ) 
        {
            Debug.WriteLine($"Changed {e.ChangeType}");
        }
        private void Fsw_Created(object sender, FileSystemEventArgs e) 
        {
            Debug.WriteLine($"Created {e.ChangeType}");
        }
        private void Fsw_Deleted(object sender, FileSystemEventArgs e) 
        {
            Debug.WriteLine($"Deleted {e.ChangeType}");
        }
        private void Fsw_Renamed(object sender, RenamedEventArgs e) 
        {
            Debug.WriteLine($"Reanmed {e.ChangeType}");
        }
        private void Fsw_Error(object sender,ErrorEventArgs e) 
        {
            Debug.WriteLine($"Error {e.GetException().Message}");
        }


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



    }
}
