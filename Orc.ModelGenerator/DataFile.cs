using System;
using System.IO;

namespace Orc.ModelGenerator
{
    public class DataFile
    {
        private readonly string _path;

        public DataFile(string path)
        {
            _path = path;
            FileName = Path.GetFileName(_path);
            if (Path.GetExtension(_path) == ".csv") DataFileType = DataFileType.Csv;
            else if (Path.GetExtension(_path) == ".xlsx") DataFileType = DataFileType.Excel;
            else throw new Exception("invalid data file extension");
        }

        public string FullFileName { get { return _path; } }
        public string FileName { get; private set; }
        public DataFileType DataFileType { get; private set; }
    }
}
