# Установка:

* dotnet new console
* dotnet add package OpenCvSharp4.Windows
* dotnet add package Microsoft.Extensions.Configuration.Json
* dotnet add package Microsoft.Extensions.Configuration.FileExtensions
* dotnet add package Microsoft.Extensions.Configuration.Binder
* dotnet add package OpenTK
* dotnet publish --configuration Release -r win10-x64 -p:ublishingleile=true -p:ublishrimmed=true --self-contained true

# выполнено
* поработать с определением цвета (добавить ползунки?): http://robocraft.ru/blog/computervision/402.html

# планы
* почитать (и применить) фильтр Калмана вместо скользящего среднего: https://habr.com/ru/flows/admin/
* а еще есть двойное экспоненциальное сглаживание: https://en.wikipedia.org/wiki/Exponential_smoothing#Double_exponential_smoothing

* разобраться с матрицей проецирования (термин head-coupled): https://www.scratchapixel.com/lessons/3d-basic-rendering/perspective-and-orthographic-projection-matrix/opengl-perspective-projection-matrix
* public static Matrix4 CreatePerspectiveOffCenter(float left, float right, float bottom, float top, float depthNear, float depthFar);

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

кожа:
def preprocess(action_frame):

    blur = cv2.GaussianBlur(action_frame, (3,3), 0)
    hsv = cv2.cvtColor(blur, cv2.COLOR_RGB2HSV)

    lower_color = np.array([108, 23, 82])
    upper_color = np.array([179, 255, 255])

    mask = cv2.inRange(hsv, lower_color, upper_color)
    blur = cv2.medianBlur(mask, 5)

    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (8, 8))
    hsv_d = cv2.dilate(blur, kernel)

return hsv_d