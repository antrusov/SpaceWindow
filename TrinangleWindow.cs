using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SpaceWindow
{
    public class TrinangleWindow : GameWindow
    {
        public TrinangleWindow(int width, int height, string title)
            :base(width, height, new GraphicsMode(), title)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);
            Console.WriteLine(GL.GetString(StringName.Version));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            SwapBuffers();
        }
    }
}