using System;

namespace SpaceWindow
{
    public class TriangleRender
    {
        ThreadSafeValue<OpenCvSharp.Point3d> _pos;

        public TriangleRender(ThreadSafeValue<OpenCvSharp.Point3d> pos)
        {
            _pos = pos;
            //...
        }

        public void Run ()
        {
            using var win = new OpenCvSharp.Window("test");
            while (true)
            {
                //...
                Console.WriteLine(_pos.Value);
                if (OpenCvSharp.Cv2.WaitKey(100) != -1)
                    break;
            }
            //OpenCvSharp.Cv2.WaitKey();
        }
    }
}