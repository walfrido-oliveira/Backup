using System;
using Delimon.Win32.IO;

namespace Walfrido.Backup.Models
{
    class Backup
    {
        private String subFolders;
        public String Source { get; set; }
        public String Destination { get; set; }
        public string CurrentDestination { get; set; }
        public Boolean IsSucess { get; set; }
        public int CountFiles { get; set; }

        public Backup(String source, String destination)
        {
            this.Source = source;
            this.Destination = destination;
        }

        public void RunBackup()
        {
            if (!Directory.Exists(this.Source))
            {
                throw new System.IO.DirectoryNotFoundException(String.Format("Pasta de origem {0} não foi localizada.", this.Source));
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(this.Source);
            Run(directoryInfo);
        }

        private void Run(DirectoryInfo directoryInfo)
        {
            foreach (Delimon.Win32.IO.FileInfo fileInfo in directoryInfo.GetFiles())
            {
                
                System.Threading.Thread.Sleep(10);
                this.CurrentDestination = this.Destination + this.subFolders + "\\" + fileInfo.Name;
                try
                {
                    Delimon.Win32.IO.File.Copy(fileInfo.FullName, this.CurrentDestination, true);
                    CountFiles++;
                }
                catch (Exception)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                
            }
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                System.Threading.Thread.Sleep(10);
                if (directory.Parent.Name.Equals(new DirectoryInfo(this.Source).Name)) this.subFolders = String.Empty;
                else this.subFolders += directory.Name + "\\";
                this.subFolders = GetDestination(directory.FullName);
                this.CurrentDestination = this.Destination + this.subFolders;
                if (!Directory.Exists(this.CurrentDestination)) Models.IO.LongDirectory.CreateDirectory(this.CurrentDestination);
                CountFiles++;
                Run(directory);
            }
        }

        private String GetDestination(String fullName)
        {
            return fullName.Substring(fullName.IndexOf(this.Source) + this.Source.Length);
        }

    }
}
