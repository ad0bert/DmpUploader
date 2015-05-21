using System;
using System.Configuration;
using System.IO;

namespace DmpUploader {
  class DmpUploader {
    static void Main() {
      string baseDir = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["baseDir"]);
      Watcher watcher = new Watcher();
      watcher.ProcessAllExistingFiles(baseDir);
      watcher.Run(baseDir);
    }
  }
}


