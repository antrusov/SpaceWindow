# Установка:

* dotnet new console
* dotnet add package OpenCvSharp4.Windows
* dotnet add package Microsoft.Extensions.Configuration.Json
* dotnet add package Microsoft.Extensions.Configuration.FileExtensions
* dotnet add package Microsoft.Extensions.Configuration.Binder
* dotnet add package OpenTK
* dotnet publish --configuration Release -r win10-x64 -p:ublishingleile=true -p:ublishrimmed=true --self-contained true

# планы
* почитать (и применить) фильтр Калмана вместо скользящего среднего: https://habr.com/ru/flows/admin/
* а еще есть двойное экспоненциальное сглаживание: https://en.wikipedia.org/wiki/Exponential_smoothing#Double_exponential_smoothing
* поработать с определением цвета (добавить пользунки?): http://robocraft.ru/blog/computervision/402.html
* разобраться с матрицей проецирования (термин head-coupled): https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/opengl-perspective-projection-matrix
* three.js умеет setViewOffset (то, что нам надо): https://threejs.org/docs/index.html#api/en/cameras/PerspectiveCamera.setViewOffset + https://github.com/auduno/headtrackr/blob/master/src/controllers.js

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