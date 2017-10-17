using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Walfrido.Backup.Views
{
    public partial class MainWindow : Form
    {
        private Thread thread;
        private Controllers.BackupController controller;
        private Boolean drag;
        private int mouseX;
        private int mouseY;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            try
            {
                this.controller = new Controllers.BackupController(this.textBoxSource.Text, this.textBoxDestination.Text);
                this.thread = new Thread(new ThreadStart(controller.RunBackup));
                this.thread.IsBackground = true;
                this.thread.Start();
                setProgress();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.labelProgress.Text = "Falha ao copiar os arquivos";
            }
        }

        private void setLabelProgress(String value)
        {
            this.labelProgress.Text = String.Format("Copiando {0}", value);
        }

        private void setProgress()
        {
            this.progressBar.Visible = true;
            while (this.thread.IsAlive)
            {
                setLabelProgress(this.controller.Backup.CurrentDestination);
                Application.DoEvents();
            }
            this.progressBar.Visible = false;
            this.labelProgress.Text = "Arquivos copiados com sucesso!";
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            if(this.thread != null) this.thread.Abort();
            Application.Exit();
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            mouseX = System.Windows.Forms.Cursor.Position.X - this.Left;
            mouseY = System.Windows.Forms.Cursor.Position.Y - this.Top;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                this.Top = System.Windows.Forms.Cursor.Position.Y - mouseY;
                this.Left = System.Windows.Forms.Cursor.Position.X - mouseX;
            }
        }

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void buttonFindDest_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK )
            {
                this.textBoxDestination.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonFindSource_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.textBoxSource.Text = this.folderBrowserDialog.SelectedPath;
            }
        }
    }
}
