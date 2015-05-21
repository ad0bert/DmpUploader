using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Threading;

namespace DmpUploader {
  public class Uploader {
    private string serverUrl;
    private UploadHelper uploadHelper;
    private HTMLGenerator htmlGenerator;
    private const int MAX_TRYS = 5; 
    
    public Uploader() {
      serverUrl = ConfigurationManager.AppSettings["serverUrl"];
      uploadHelper = new UploadHelper();
      htmlGenerator = new HTMLGenerator();
    }

    public void Upload(String file) {
      FileStream fileStream;
      int trys = 0;

      Console.WriteLine("New dmp file Detected!");
      Thread.Sleep(500); // wait for system to release the file
      while (true) {
        try {
          fileStream = File.Open(file, FileMode.Open);
          break;
        }
        catch (IOException ex) {
          if (trys < MAX_TRYS) {
            trys++;
            Console.WriteLine("Failed ({0}x) to open \"{1}\"", trys, file);
            Thread.Sleep(500); // wait and retry open file
          }
          else {
            Console.WriteLine("Can not open file: \"{0}\"", file);
            throw ex;
          }
        }
      }

      var files = new[] 
      {
        new UploadFile
        {
            Name = "file",
            Filename = Path.GetFileName(file),
            ContentType = "text/plain",
            Stream = fileStream
        }
      };

      string[] tags = ConfigurationManager.AppSettings["tags"].Split(',');
      var values = new NameValueCollection();
      foreach (var t in tags)
        values.Add("tags", Environment.ExpandEnvironmentVariables(t));

      Console.WriteLine("Start upload \"{0}\"", file);
      byte[] result;
      try {
        result = uploadHelper.UploadFiles(serverUrl, files, values);
      }
      catch {
        Console.WriteLine("Failed upload \"{0}\"", file);
        fileStream.Close();
        return;
      }
      Console.WriteLine("Finished upload \"{0}\"", file);
      try {
        htmlGenerator.Generate(result, serverUrl, file);
      }
      catch {
        Console.WriteLine("Failed to create \"{0}.html\"", file);
      }
      fileStream.Close();
      File.Delete(file);
      Console.WriteLine("Deleted: \"{0}\"", file);
    }
  }
}
