namespace SpaceWindow
{
    public class FaceTrackerSettings
    {
        // расстояние между камерами (в метрах)
        public double DistanceBetweenCameras { get; set; }
        // высота, на которой расположены камеры (в метрах)
        public double CameraHeight { get; set; }
        // горизонтальный угол обзора "по ширине" (в градусах)
        public double CameraHorizontalAngle { get; set; }
        // вертикальный угол обзора "по высоте" (в градусах)
        public double CameraVerticalAngle { get; set; }
        // ширина изображения (в пикселях)
        public int ImageWidth { get; set; }
        // высота изображени (в пикселях)
        public int ImageHeight { get; set; }
    }
}