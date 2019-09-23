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
                setErrorLabelProgress(ex.Message);
            }
        }

        private void setLabelProgress(String value)
        {
            this.labelProgress.ForeColor = Color.Black;
            this.toolTip.SetToolTip(this.labelProgress, value);
            if (value != null) if (value.Length > 80) value = value.Substring(0, 75) + "[...]";
            this.labelProgress.Text = String.Format("Total de: {0} \nCopiando: {1}", this.controller.Backup.CountFiles, value);
        }

        private void setErrorLabelProgress(String value)
        {
            this.labelProgress.ForeColor = Color.Red;
            this.labelProgress.Text = String.Format("Falha ao copiar os arquivos. {0}",value);
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
            if (this.controller.Backup.IsSucess) this.labelProgress.Text = String.Format("Total de {0} arquivos copiados.", this.controller.Backup.CountFiles);
            else setErrorLabelProgress(string.Empty);
        }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.thread != null) this.thread.Abort();
                Application.Exit();
            }
            catch (Exception)
            {
                //
            }
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
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK ) this.textBoxDestination.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void buttonFindSource_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog.ShowDialog() == DialogResult.OK) this.textBoxSource.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void labelProgress_MouseLeave(object sender, EventArgs e)
        {
            this.toolTip.Hide(this.labelProgress);            
        }

        private void Teste2(string teste)
        {
            MessageBox.Show(teste);
        }

        private delegate void Teste(string s);
        private void DoWork(Teste teste) 
        {
            teste("csdcsdc");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Teste t = Teste2;
            DoWork(t);
        }
    }
}
