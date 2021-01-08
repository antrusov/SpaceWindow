# Установка:

* dotnet new console
* dotnet add package OpenCvSharp4.Windows
* dotnet add package Microsoft.Extensions.Configuration.Json
* dotnet add package Microsoft.Extensions.Configuration.FileExtensions
* dotnet add package Microsoft.Extensions.Configuration.Binder
* dotnet add package OpenTK.NETCore
* dotnet publish --configuration Release -r win10-x64 -p:ublishingleile=true -p:ublishrimmed=true --self-contained true

# Всяка лабуда

> computeCorrespondEpilines
> decomposeProjectionMatrix
> solvePnP
> solvePnPRansac
>
> про калибровку нескольких камер (одновременно)
> https://docs.opencv.org/3.4/d2/d1c/tutorial_multi_camera_main.html
>
> stereoRectify - по картинке с двух камер восстанавливает матрицы камер
> triangulatePoints - восстановление координат