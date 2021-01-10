namespace SpaceWindow
{
    public class ColorConfigSettings
    {
        public int CameraIndex { get; set; }
        public MinMax H { get; set; }
        public MinMax S { get; set; }
        public MinMax V { get; set; }
    }

    public class MinMax
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}