using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCrawler
{
    public class File
    {

        public File() : this("", "", 0, null)
        {

        }

        public File(string filename, string filepath, double fileSize, string fileCategory = null)
        {
            this.FileName = filename;
            this.FilePath = filepath;
            this.FileSize = fileSize;
            this.FileCategory = fileCategory;
        }


        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                }
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                }
            }
        }

        private double _fileSize;
        public double FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    if (value > 0)
                    {
                        // How the fuq do you get filesize the "real" way???
                        _fileSize = (value / 1024d / 1024d / 1024d);
                        if (_fileSize < 1)
                        {
                            _fileSize = _fileSize * 1024d;
                        }
                    }
                }
            }
        }


        // Not in use as of now, will add functionality for this later
        private string _fileCategory;
        public string FileCategory
        {
            get { return _fileCategory; }
            set
            {
                if (_fileCategory != value)
                {
                    _fileCategory = value;
                }
            }
        }
    }
}
