# DmpUploader
DmpUploader
========
_DmpUploader_ is an C# console application that allows automatically uploading `.dmp`-files to a [dmpster](https://github.com/alexanderfloh/dmpster) instance.

##Usage##
* Adapt your app.config:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="baseDir" value="%url.logfiles.source%" />
    <add key="serverUrl" value="http://localhost:9000/upload" />
    <add key="tags" value="%#sctm_execdef_name%, %#sctm_build%, %#sctm_version%" />
  </appSettings>
  .
  .
  .
```
* `baseDir` is the directory where _DmpUploader_ is looking for `.dmp` files. (enviroment variable)
* `serverUrl` is the URL to your _dmpster_ instance.
* `tags` is a comma-separated-list of tags that will be automatically added to the dmp. (enviroment variables)

##Building##
* Clone DmpUploader
* Set baseDir, serverUrl and tags.
* Build DmpUploader.
* Run it.
