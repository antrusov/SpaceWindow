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
                var pos = _pos.Value;
                Console.WriteLine($"{pos.X:0.###}\t{pos.Y:0.###}\t{pos.Z:0.###}");

                if (OpenCvSharp.Cv2.WaitKey(100) != -1)
                    break;
            }
            //OpenCvSharp.Cv2.WaitKey();
        }
    }
}