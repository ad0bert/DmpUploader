using System;
using System.Configuration;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace DmpUploader {
  public class Watcher {
    private FileSystemWatcher watcher;
    private Uploader uploader;

    public Watcher() {
      watcher = new FileSystemWatcher();
      uploader = new Uploader();
    }

    public void ProcessAllExistingFiles(string path) {
      // process all dmp files
      string[] fileEntries = Directory.GetFiles(path);
      foreach (string fileName in fileEntries) {
        if (fileName.EndsWith(".dmp"))
          new Thread(() => uploader.Upload(fileName)).Start();
      }
      // go down all sub directories
      string[] subdirectoryEntries = Directory.GetDirectories(path);
      foreach (string subdirectory in subdirectoryEntries)
        ProcessAllExistingFiles(subdirectory);
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public void Run(string toWatch) {
      watcher.Path = toWatch;

      watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
         | NotifyFilters.FileName | NotifyFilters.DirectoryName;

      watcher.Filter = "*.dmp";
      watcher.IncludeSubdirectories = true;
      watcher.Created += new FileSystemEventHandler(OnCreated);
      watcher.EnableRaisingEvents = true;
      
      Console.WriteLine("Press the 'any' key to exit!");
      Console.Read();
    }

    private void OnCreated(object source, FileSystemEventArgs e) {
      try {
        // workaround: known bug: oncreate event is fired twice
        watcher.EnableRaisingEvents = false;
        new Thread(() => uploader.Upload(e.FullPath)).Start();
      }
      catch (Exception ex) {
        throw ex;
      }
      finally {
        watcher.EnableRaisingEvents = true;
      }
    }
  }
}
