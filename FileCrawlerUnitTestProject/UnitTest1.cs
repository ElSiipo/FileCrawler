using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileCrawler;

namespace FileCrawlerUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FileListNotNullFromStart()
        {
            FileHandler fh = new FileHandler();
            Assert.AreEqual(true, fh.ListOfFiles != null);
        }


        [TestMethod]
        public void FileListNotContainingOtherThanMovieFormat()
        {
            FileHandler fh = new FileHandler();
            fh.SourcePath = @"D:\FILM";

            var test = fh.ListOfFiles.Where(p => !p.FileName.Any(d => p.FileName.EndsWith(".mov")))
                                     .Where(p => !p.FileName.Any(d => p.FileName.EndsWith(".mp4")))
                                     .Where(p => !p.FileName.Any(d => p.FileName.EndsWith(".avi"))).ToList();

            Assert.AreEqual(true, test.Count() <= 0);
        }


        [TestMethod]
        public void TestEmptyFileCreation()
        {
            File f = new File();
            Assert.AreEqual(true, f.FileName == "" && 
                                  f.FilePath == "" && 
                                  f.FileSize == 0 && 
                                  f.FileCategory == null);
        }
    }
}
