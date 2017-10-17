using System;
using System.IO;

namespace Walfrido.Backup.Models
{
    class Backup
    {
        public String Source { get; set; }
        public String Destination { get; set; }
        private String subFolders;
        public string CurrentDestination { get; set; }

        public Backup(String source, String destination)
        {
            this.Source = source;
            this.Destination = destination;
        }

        public void RunBackup()
        {
            Run(new DirectoryInfo(this.Source));
        }

        private void Run(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                System.Threading.Thread.Sleep(1000);
                this.CurrentDestination = this.Destination + this.subFolders + "\\" + fileInfo.Name;
                File.Copy(fileInfo.FullName, this.CurrentDestination,true);
            }
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                System.Threading.Thread.Sleep(50);
                if (directory.Parent.Name.Equals(new DirectoryInfo(this.Source).Name))
                {
                    this.subFolders = String.Empty;
                }
                else
                {
                    this.subFolders += directory.Name + "\\";
                }
                this.subFolders = GetDestination(directory.FullName);
                this.CurrentDestination = this.Destination + this.subFolders;
                if (!Directory.Exists(this.CurrentDestination)) Directory.CreateDirectory(this.CurrentDestination);
                Run(directory);
            }
        }

        private String GetDestination(String fullName)
        {
            return fullName.Substring(fullName.IndexOf(this.Source) + this.Source.Length);
        }

    }
}
