using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.ComponentModel;
using System.Threading;
using System.Drawing;

namespace YoutubeMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            main = this;
            //Setup worker
            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);

        }
        private static readonly VideoDownloader aVideo = new VideoDownloader();
        public BackgroundWorker worker;
        string destinationPath = @"C:\Users\ferya\Music\iTunes\iTunes Media\Music\";
        string link;
        internal static MainWindow main;
        internal string Status
        {
            get { return status_label.Content.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { status_label.Content = value; })); }
        }



        public void destination_button_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog browser = new VistaFolderBrowserDialog();
            browser.Description = "Por favor seleccione la carpeta de destino";
            if (browser.ShowDialog() == true)
            {
                destinationPath = browser.SelectedPath+@"\";
                destination_textBox.Text = destinationPath;
            }
        }

        public void convert_button_Click(object sender, RoutedEventArgs e)
        {

            link = youtube_textbox.Text;
            if (link == "" || !link.StartsWith("http"))
            {
                MessageBox.Show("Por favor ponga un link valido");
            }
            else if (destinationPath == null)
            {
                MessageBox.Show("Por favor seleccione la carpeta de destino");
            }
            else
            {
                worker.RunWorkerAsync();
                //startStatus();
            }
        }

        /*public async Task startStatus()
        {
            var progress = new Progress<int>(update => progressbar1.Value = update);

            try
            {
                await aVideo.getStatus(progress);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                progressbar1.Value = 100;
                //Start_button.IsEnabled = true;
                //progressbar1.Value = 0;
            }
        }*/

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //var progress = new Progress<int>(update => progressbar1.Value = update);
                //await Task.Run(() => aVideo.processFile(sender, e, link, destinationPath, progress));
                aVideo.processFile(sender, e, link, destinationPath);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error trying to Process: " + ex);
            }
        }
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressbar1.Value = e.ProgressPercentage;
            //progressbar1.Value = e.ProgressPercentage -1;
            if (e.ProgressPercentage != 0)
                progressbar1.Value = e.ProgressPercentage - 1;
            progressbar1.Value = e.ProgressPercentage;
            if (progressbar1.Maximum == e.ProgressPercentage)
                progressbar1.Value = 0;
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Conversion se ha cancelado");
            }
            else if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            else
            {
                progressbar1.Value = 100;
                return;
            }

        }
        private void Cancel_button_Click(object sender, RoutedEventArgs e)
        {
            worker.CancelAsync();
        }
    }
}
