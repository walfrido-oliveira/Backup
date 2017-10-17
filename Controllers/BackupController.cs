using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Walfrido.Backup.Controllers
{
    class BackupController
    {
        public Models.Backup Backup { get; set; }

        public BackupController(string source, string destination)
        {
            this.Backup = new Walfrido.Backup.Models.Backup(source, destination);
        }

        public void RunBackup()
        {
            Backup.RunBackup();
        }
    }
}
