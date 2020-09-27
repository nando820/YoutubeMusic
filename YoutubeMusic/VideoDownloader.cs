using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Net;
using MediaToolkit.Model;
using MediaToolkit.Options;
using VideoLibrary;
using System.Threading;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace YoutubeMusic
{
    class VideoDownloader
    {
        string link;
        string destinationPath;
        BackgroundWorker worker;
        //IProgress<int> progress;
        class MyClient : WebClient
        {
            public bool HeadOnly { get; set; }
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest req = base.GetWebRequest(address);
                if (HeadOnly && req.Method == "GET")
                {
                    req.Method = "HEAD";
                }
                return req;
            }
        }
        public void processFile(object sender, DoWorkEventArgs e, string link, string destinationPath)
        {
            this.link = link;
            this.destinationPath = destinationPath;
            this.worker = (BackgroundWorker)sender;
            //this.progress = progress;
            
            if (checkLink()==true && checkFolder()==true)
            {
                MainWindow.main.Status = "Analizando";
                worker.ReportProgress(25);
                Download();
            }
        }

        private bool checkLink()
        {
            using (var client = new MyClient())
            {
                client.HeadOnly = true;
                try
                {   // checks good, no content downloaded
                    string s1 = client.DownloadString(link);
                    return true;
                }
                catch
                {
                    // throws 404
                    MessageBox.Show("El link no funciona");
                    return false;
                }

            }
        }
        private bool checkFolder()
        {
            if (Directory.Exists(destinationPath))
            {
                return true;
            }

            else
            {
                MessageBox.Show("Carpeta de destino no existe");
                return false;
            }

        }
        private void Download()
        {

            /*for (int i = 1; i <= 100; i++)
            {
                // Wait 100 milliseconds.
                Thread.Sleep(100);
                // Report progress.
                worker.ReportProgress(i);
            }*/

            var source = destinationPath;
            var youtube = YouTube.Default;
            //var vid = youtube.GetVideo(link);
            var videos = youtube.GetAllVideos(link);
            //get Audio
            var vid = videos.OrderByDescending(a => a.AudioBitrate).FirstOrDefault();
            //get Video
            //var mp4 = videos.Where(y => y.FileExtension == ".mp4");
            //var HQmp4 = videos.OrderByDescending(a => a.Resolution).FirstOrDefault();
            string cleanName = CleanFileName(vid.FullName);
            string cleanTitle = CleanFileName(vid.Title);
            File.WriteAllBytes(source + vid.FullName, vid.GetBytes());

            MainWindow.main.Status = "Procesando: " + vid.Title;
            worker.ReportProgress(70);
            
            var inputFile = new MediaFile { Filename = source + cleanName };
            var outputFile = new MediaFile { Filename = $"{source + cleanTitle}.mp3" };

            using (var engine = new MediaToolkit.Engine())
            {
                engine.GetMetadata(inputFile);
                var options = new ConversionOptions();
                options.VideoBitRate=192;
                engine.Convert(inputFile, outputFile, options);
            }
            //delete original video file
            File.Delete(Path.Combine(source, vid.FullName));
            //worker.ReportProgress(100);
            MainWindow.main.Status = "Completo: "+ vid.Title;
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
        public async Task getStatus(IProgress<int> progress)
        {
            //progress.Report(percent);
        }

    }
}
