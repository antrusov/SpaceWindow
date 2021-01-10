using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class Camera2ColorFilter
    {
        int _cameraIndex;

        public Camera2ColorFilter (int cameraIndex)
        {
            _cameraIndex = cameraIndex;
            //...
        }
    }
}