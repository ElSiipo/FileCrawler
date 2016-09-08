using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FileCrawler
{
    public class FileHandler
    {
        public FileHandler()
        {
            _listOfFiles = new List<File>();
        }



        private void GetFiles(string value)
        {
            foreach (string directory in Directory.GetDirectories(value))
            {
                try
                {
                    if ((new DirectoryInfo(directory).Attributes & FileAttributes.Hidden) == 0)
                    {
                        if (Directory.GetDirectories(directory).Length > 0)
                        {
                            GetFiles(directory);
                        }

                        foreach (string file in Directory.GetFiles(directory))
                        {
                            if (file.EndsWith(".mkv") || file.EndsWith(".mp4") || file.EndsWith(".avi"))
                            {
                                // Handle filepath too long!
                                if (file.Length < 250)
                                {
                                    var fileInfos = new FileInfo(file);

                                    ListOfFiles.Add(new File(fileInfos.Name, fileInfos.DirectoryName, fileInfos.Length));
                                    NumberOfFiles += 1;
                                }

                                else
                                {
                                    // FIX THIS, NOT OK TO HANDLE THIS PROBLEM THIS WAY!! 
                                    FilenameTooLongFiles += 1;
                                }
                            }

                            FilesScanned += 1;
                        }
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Make a logger instead?
                    UnauthorizedFiles += 1;
                    Console.WriteLine("\n\n\nUnauthorized: " + UnauthorizedFiles + "\nFiles scanned: " + FilesScanned + "\nMovie-files found: " + NumberOfFiles);
                }
            }

            if (NumberOfFiles == 0)
            {
                foreach (string file in Directory.GetFiles(value))
                {
                    if (file.EndsWith(".mkv") || file.EndsWith(".mp4") || file.EndsWith(".avi"))
                    {
                        var fileInfos = new FileInfo(file);

                        ListOfFiles.Add(new File(fileInfos.Name, fileInfos.DirectoryName, fileInfos.Length));
                        NumberOfFiles += 1;
                    }
                }
            }
        }


        public void BeginProcessFiles()
        {
            if (ListOfFiles.Count == 0)
            {
                ClearFilenumbersAndList();
                GetFiles(SourcePath);
            }


            // Make the magic happen!
            // Hold your horses, not quite yet...

            //ProcessFiles(ListOfFiles);
        }


        private void ClearFilenumbersAndList()
        {
            NumberOfFiles = 0;
            FilesScanned = 0;
            UnauthorizedFiles = 0;
            FilesProcessedToZip = 0;

            ListOfFiles.Clear();
        }

        private async void ProcessFiles(List<File> fileList)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var zipFile = new ZipFile())
                    {
                        // add content to zip here 
                        foreach (File file in fileList)
                        {
                            zipFile.AddFile(file.FilePath);
                        }

                        zipFile.SaveProgress +=
                            (o, args) =>
                            {
                                var percentage = (int)(1.0d / args.TotalBytesToTransfer * args.BytesTransferred * 100.0d);
                                PercentageDone = percentage / NumberOfFiles;
                                FilesProcessedToZip += 1;
                            };
                        zipFile.Save(SourcePath + "\\Archive.zip");
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.Message);
                }
            });
        }



        /// <summary>
        /// Only Properties from here and downwards.
        /// Some will hate me for this "wrong order". :-)
        /// </summary>

        private string _sourcePath;
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (_sourcePath != value)
                {
                    _sourcePath = value;
                    OnPropertyChanged();
                    
                    GetFiles(_sourcePath);
                }
            }
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if (_destinationPath != value)
                {
                    _destinationPath = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _numberOfFiles;
        public int NumberOfFiles
        {
            get { return _numberOfFiles; }
            private set
            {
                if (_numberOfFiles != value)
                {
                    _numberOfFiles = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _filesProcessedToZip;
        public int FilesProcessedToZip
        {
            get { return _filesProcessedToZip; }
            private set
            {
                if (_filesProcessedToZip != value)
                {
                    _filesProcessedToZip = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _filesScanned;
        public int FilesScanned
        {
            get { return _filesScanned; }
            private set
            {
                if (_filesScanned != value)
                {
                    _filesScanned = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _unauthorizedFiles;
        public int UnauthorizedFiles
        {
            get { return _unauthorizedFiles; }
            private set
            {
                if (_unauthorizedFiles != value)
                {
                    _unauthorizedFiles = value;
                    OnPropertyChanged();
                }
            }
        }


        private int _filenameTooLongFiles;
        public int FilenameTooLongFiles
        {
            get { return _filenameTooLongFiles; }
            private set
            {
                if (_filenameTooLongFiles != value)
                {
                    _filenameTooLongFiles = value;
                    OnPropertyChanged();
                }
            }
        }


        private List<File> _listOfFiles;
        public List<File> ListOfFiles
        {
            get { return _listOfFiles; }
            private set
            {
                if (_listOfFiles != value)
                {
                    _listOfFiles = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _percentageDone;
        public double PercentageDone
        {
            get { return _percentageDone; }
            private set
            {
                if (_percentageDone != value)
                {
                    _percentageDone = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
