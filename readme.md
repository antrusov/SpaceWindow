Установка:

* dotnet new console
* dotnet add package OpenCvSharp4.Windows
* dotnet add package Microsoft.Extensions.Configuration.Json
* dotnet add package Microsoft.Extensions.Configuration.FileExtensions
* dotnet add package Microsoft.Extensions.Configuration.Binder
* dotnet publish --configuration Release -r win10-x64 -p:ublishingleile=true -p:ublishrimmed=true --self-contained true