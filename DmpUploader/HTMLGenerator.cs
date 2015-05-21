using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DmpUploader {
  public class HTMLGenerator {

    public void Generate(byte[] response, string serverUrl, string filePath) {
      string result = System.Text.Encoding.Default.GetString(response);
      string url = result.Replace("{\"files\":[{\"url\":\"", serverUrl).Replace("/upload", "").Replace("\"}]}", "");

      using (FileStream fs = new FileStream(filePath + "." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".html", FileMode.Create)) {
        Console.WriteLine("Created: \"{0}\"", fs.Name);
        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8)) {
          w.WriteLine("<!DOCTYPE HTML>");
          w.WriteLine("<html lang=\"en-US\">");
          w.WriteLine("<head>");
          w.WriteLine("<meta charset=\"UTF-8\">");
          w.WriteLine("<meta http-equiv=\"refresh\" content=\"1;url={0}\">", url);
          w.WriteLine("<script type=\"text/javascript\">");
          w.WriteLine("window.location.href = \"{0}\"", url);
          w.WriteLine("</script>");
          w.WriteLine("<title>Page Redirection</title>");
          w.WriteLine("</head>");
          w.WriteLine("<body>");
          w.WriteLine("<!-- Note: don't tell people to `click` the link, just tell them that it is a link. -->");
          w.WriteLine("If you are not redirected automatically, follow the <a href='{0}'>link to example</a>", url);
          w.WriteLine("</body>");
          w.WriteLine("</html>");
        }
      }
    }
  }
}
