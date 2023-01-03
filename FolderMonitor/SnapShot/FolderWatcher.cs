using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderMonitor.SnapShot
{
    public class FolderWatcher
    {
        FileSystemWatcher watcher;
        List<string> files;

        public FolderWatcher(string filepath) { 
            files = new List<string>();
            watcher= new FileSystemWatcher();
            watcher.Path = filepath;

            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += new FileSystemEventHandler(OnChange);

            watcher.EnableRaisingEvents=true;       
        }

        private void OnChange(object source, FileSystemEventArgs e)
        {
            if (files.Contains(e.FullPath))
            {
                // if event is fired based on the same file, ignore the event let the thread to continue to work on previous 
                // detection
                Console.WriteLine("Duplicate events Captured: {0}", e.FullPath);
                return;
            }
            files.Add(e.FullPath);

            Thread t1 = new Thread(() => WaitForFile(e.FullPath)); 
            t1.Start();            
        }

        private void WaitForFile(string filename)
        {
            Console.WriteLine("File Detected: {0}",filename);
            while (!IsFileReady(filename)) { }  
            /*
             *  Do you job here!
             */

            files.Remove(filename);
            Console.WriteLine("File Ready: {0}", filename);
        }
        private bool IsFileReady(string filename)
        {
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            } 
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            watcher.Changed -= OnChange; 
            watcher.Dispose();
        }

    }
}
