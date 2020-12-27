using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class GloveSettings
    {
        public string Url { get; set; }
        public int Period { get; set; }
        public int MinArea { get; set; }
        public Area Area { get; set; }

        public int CameraIndex { get; set; }
        public Camera2Color.ShowMode CameraMode { get; set; }
        public Ranges ColorRanges { get; set; }
    }

    public class Area
    {
        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
    }

    public class Ranges
    {
        public int[] Lower1 { get; set; }
        public int[] Upper1 { get; set; }
        public int[] Lower2 { get; set; }
        public int[] Upper2 { get; set; }
    }
}