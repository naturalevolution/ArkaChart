using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace ArkaChart.Tools {
    public class PathHelper : IPathHelper {
        public string RECEIVING_FOLDER = ConfigurationManager.AppSettings["ReceivingHotFolder"];

        public virtual List<FileInfo> AvalaibleFiles() {
            string[] filePaths = Directory.GetFiles(RECEIVING_FOLDER, "*.txt");
            return filePaths.Select(x => new FileInfo(x)).ToList();
        }

        public virtual bool IsExistReceivingDirectory() {
            return Directory.Exists(AbsolutePath(RECEIVING_FOLDER));
        }
        public virtual bool IsExist(string path) {
            return File.Exists(AbsolutePath(path));
        }

        public virtual string AbsolutePath(string path) {
            return Path.Combine(RECEIVING_FOLDER, path);
        }
    }

    public interface IPathHelper {
        List<FileInfo> AvalaibleFiles();
        bool IsExist(string path);
        string AbsolutePath(string path);
    }
}