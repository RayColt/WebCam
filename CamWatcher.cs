using System;
using System.IO;
using WebCam;

namespace Webcam
{
    class CamWatcher
    {
        private string log_file_path = "";
        private string watch_dir_path = "";
        private ConsoleWriter consoleWriter;

        public CamWatcher(string path, string logFile)
        {
            watch_dir_path = path;
            log_file_path = path + "\\" + logFile;
            consoleWriter = new ConsoleWriter();
            initWatcher();
        }

        private void initWatcher()
        { 
            if (!File.Exists(path: log_file_path))
            {
                try
                {
                    FileStream fs = File.Create(log_file_path);
#if DEBUG
                    consoleWriter.WriteLine("CamWatcher Log File:");
#endif
                }
                catch (Exception e)
                {
#if DEBUG
                    consoleWriter.WriteLine(e.ToString());
#endif
                }
            }
            else
            {
#if DEBUG
                consoleWriter.WriteLine("File " + watch_dir_path + "\\" + log_file_path + " already exists.");
#endif
            }

            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(watch_dir_path);
            FileSystemWatcher watcher = fileSystemWatcher;
            watcher.NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size;

            //watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            //watcher.Renamed += OnRenamed;
            watcher.Error += OnError;
            watcher.Filter = "*.jpg";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            consoleWriter.WriteLine("HOTFOLDER SAVE DIRECTORY(compare with log file):");
            // consoleWriter.WriteLine("Press enter to start Webcam!!");
            // consoleWriter.ReadLine();
        }

        private void Log(string logmsg)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(log_file_path, true))
                {
                    sw.WriteLine(logmsg);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
#if DEBUG
                consoleWriter.WriteLine("Watcher can not acces log file in directory " + log_file_path + " its being used by other proces, please try again.");
                consoleWriter.WriteLine(e.Message);
#endif
            }
        }
/*
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            consoleWriter.WriteLine($"Changed: {e.FullPath}");
            Log($"Changed: {e.FullPath}");
        }
*/
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            consoleWriter.WriteLine(value);
            Log(value);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            consoleWriter.WriteLine($"Deleted: {e.FullPath}");
            Log($"Deleted: {e.FullPath}");
        }
/*
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            consoleWriter.WriteLine($"Renamed:");
            Log($"Renamed:");
            consoleWriter.WriteLine($"    Old: {e.OldFullPath}");
            Log($"    Old: {e.OldFullPath}");
            consoleWriter.WriteLine($"    New: {e.FullPath}");
            Log($"    New: {e.FullPath}");
        }
*/
        private void OnError(object sender, ErrorEventArgs e)
        {
#if DEBUG
            PrintException(e.GetException());
            Log($"Exception: {e.GetException()}");
#endif
        }

        private void PrintException(Exception ex)
        {
            if (ex != null)
            {
#if DEBUG
                consoleWriter.WriteLine($"Message: {ex.Message}");
                Log($"Message: {ex.Message}");
                consoleWriter.WriteLine("Stacktrace:");
                Log("Stacktrace:");
                consoleWriter.WriteLine(ex.StackTrace);
                Log("{ex.Stacktrace}");
                consoleWriter.WriteLine("");
                PrintException(ex.InnerException);
                Log("{ex.InnerException)}");
#endif
            }
        }
    }
}
