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
using NReco.VideoConverter;

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
            var path = destinationPath;
            var youtube = YouTube.Default;
            //var vid = youtube.GetVideo(link);
            var videos = youtube.GetAllVideos(link);
            //get Audio
            var vid = videos.OrderByDescending(a => a.AudioBitrate).FirstOrDefault();
            //var vid = videos.OrderByDescending(a => a.AudioBitrate == 128).FirstOrDefault();
            
            //get Video
            //var mp4 = videos.Where(y => y.FileExtension == ".mp4");
            //var HQmp4 = videos.OrderByDescending(a => a.Resolution).FirstOrDefault();
            string cleanName = CleanFileName(vid.FullName);
            string cleanTitle = CleanFileName(vid.Title);
            File.WriteAllBytes(path + vid.FullName, vid.GetBytes());

            MainWindow.main.Status = "Procesando: " + vid.Title;
            worker.ReportProgress(70);

            //string new_path = path.Replace(@"\\", @"\");
            string inputName = path + cleanName ;
            string outputName = path + cleanTitle+".mp3";

            ffmpeg_conversion(inputName, outputName);

            /*
            var inputFile = new MediaFile { Filename = path + cleanName };
            var outputFile = new MediaFile { Filename = $"{path + cleanTitle}.mp3" };

            using (var engine = new MediaToolkit.Engine())
            {
                engine.GetMetadata(inputFile);
                var options = new ConversionOptions();
                options.VideoBitRate=128;
                engine.Convert(inputFile, outputFile, options);
                //engine.Convert(inputFile, outputFile);
            }*/

            //delete original video file
            File.Delete(Path.Combine(path, vid.FullName));
            //worker.ReportProgress(100);
            MainWindow.main.Status = "Completo: "+ vid.Title;
        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        private static void ffmpeg_conversion(string input, string output)
        {
            //var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //ffMpeg.ConvertMedia(input, "output.mp4", Format.mp4);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //string executable_path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + @"\ffmpeg\ffmpeg.exe";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = @"C:\FFMPEG\bin\ffmpeg.exe";
            //below two line works. Do NOT delete
            //process.StartInfo.Arguments = @"-i ""C:\\Users\\ferna\\Documents\\Visual Studio 2019\\sandbox\\Therefore.webm"" -vn -b:a 128000 ""C:\\Users\\ferna\\Documents\\Visual Studio 2019\\sandbox\\Therefore.mp3"" ";
            //process.StartInfo.Arguments = @"-i ""C:\Users\ferna\Documents\Visual Studio 2019\sandbox\Therefore.webm"" -vn -b:a 128000 ""C:\Users\ferna\Documents\Visual Studio 2019\sandbox\Therefore.mp3"" ";

            //string source = @"C:\Users\ferna\Documents\Visual Studio 2019\sandbox\Therefore.webm";
            //string destination = @"C:\Users\ferna\Documents\Visual Studio 2019\sandbox\Therefore.mp3";
            string source = input;
            string destination = output;
            string args = " -i " + @"""" + source + @"""" + " -vn -b:a 128000 " + @"""" + destination + @"""";
            process.StartInfo.Arguments = args;
            //MessageBox.Show(args);
            process.StartInfo.UseShellExecute = false; 
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            //System.Threading.Thread.Sleep(6000); //sleep for 6 seconds to allow for conversion to complete

            /*System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo();
            proc.FileName = @"C:\windows\system32\cmd.exe";
            proc.Arguments = "/c ping 10.2.2.125";
            //proc.Arguments = $"ffmpeg -i {input} -vn -b:a 128000 {output}";
            System.Diagnostics.Process.Start(proc);*/

        }

        /*public async Task getStatus(IProgress<int> progress)
        {
            //progress.Report(percent);
        }*/

    }
}
