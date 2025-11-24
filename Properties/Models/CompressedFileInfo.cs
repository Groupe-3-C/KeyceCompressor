using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KeyceCompressor.Models
{
    public class CompressedFileInfo
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public long SizeKB { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplaySize => SizeKB < 1024 ? $"{SizeKB} KB" : $"{SizeKB / 1024f:F1} MB";

        public CompressedFileInfo(string path)
        {
            var fi = new FileInfo(path);
            FullPath = path;
            FileName = fi.Name;
            SizeKB = fi.Length / 1024;
            CreationDate = fi.CreationTime;
        }

        public override string ToString()
        {
            return $"{FileName}\n{DisplaySize}\n{CreationDate:dd/MM/yyyy HH:mm}";
        }
    }
}
