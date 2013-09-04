using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace nGot
{
    class NGot : GameWindow
    {
        public const int DEFAULT_WINDOW_WIDTH = 800;
        public const int DEFAULT_WINDOW_HEIGHT = 600;
        public const int DEFAULT_UPDATES_PER_SECOND = 60;
        public const int DEFAULT_FRAMES_PER_SECOND = 60;

        #region Initialization

        public NGot() 
            : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, GraphicsMode.Default, "nGot")
        {
            VSync = VSyncMode.On;
        }
        
        ~NGot() { }

        #endregion

        #region Main

        static void Main(string[] args)
        {
            using (NGot nGot = new NGot())
            {
                nGot.Run(DEFAULT_UPDATES_PER_SECOND, DEFAULT_FRAMES_PER_SECOND);
            }
        }

        #endregion

        #region Load

        public void OnLoad() { }

        #endregion

        #region Draw

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookAt = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitX);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookAt);

            GL.Begin(BeginMode.Quads);

            GL.Color4(Color4.Red); GL.Vertex3(-1.0f, -1.0f, 0.0f);
            GL.Color4(Color4.Green); GL.Vertex3(1.0f, -1.0f, 0.0f);
            GL.Color4(Color4.Blue); GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.Color4(Color4.Yellow); GL.Vertex3(-1.0f, 1.0f, 0.0f);

            GL.End();

            SwapBuffers();
        }

        #endregion
    }
}
