using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace SpaceWindow
{
    public class TrinangleWindow : GameWindow
    {
        #region [ данные ]

        //вершины треугольника
        float[] vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };

        //id вершинного обьекта (VBO) с данными треугольника загруженного на видеокарту
        int VertexBufferObject;

        //id обьекта, хранящего настройки вершин
        int VertexArrayObject;

        //шейдерная программа
        Shader shader;

        #endregion

        public TrinangleWindow(int width, int height, string title)
            :base(
                new GameWindowSettings(),
                new NativeWindowSettings() {
                    Size = new OpenTK.Mathematics.Vector2i(width, height)
                }
            )
        {            
        }

        protected override void OnLoad()
        {
            //отладочная информация
            Console.WriteLine(GL.GetString(StringName.Extensions));
            Console.WriteLine(GL.GetString(StringName.Version));

            //общие настройки
            this.VSync = VSyncMode.Off;
            GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);

            //настройка данных
            VertexBufferObject = GL.GenBuffer(); //создать id для VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject); //считать этот VBO массивом
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); //закачать на карту данные о вершинах

            VertexArrayObject = GL.GenVertexArray(); //создать VAO для хранения настроек вершин
            GL.BindVertexArray(VertexArrayObject); //активировать вновь созданный VAO

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0); //описание того, как трактовать данные в VBO
            GL.EnableVertexAttribArray(0); //включить 0 переменную в вершинном шейдере

            //настройка шейдера
            shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            base.OnResize(e);
        }

        protected override void OnUnload()
        {
            shader.Dispose();
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferObject);
            base.OnUnload();
        }
    }
}