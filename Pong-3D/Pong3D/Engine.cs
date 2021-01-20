using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNALib;

namespace Pong3D
{
    public class Engine : IEngine
    {
        public static Engine Instance;

        public bool IsDebugMode = false;
        public IActiveState ActiveState { get; set; }
        public Game Game { get; set; }
        //public XactMgr Audio = new XactMgr("Tetris");
        public Keyboard1 KB = new Keyboard1();
        public  GraphicsDeviceManager Graphics;
        public  int Width { get { return Graphics.PreferredBackBufferWidth; } }
        public  int Height { get { return Graphics.PreferredBackBufferHeight; } }

        public XNALib._3D.Camera3D Camera3D;

        public Rectangle ScreenArea { get { return new Rectangle(0, 0, Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight); } }

        GraphicsDeviceManager IEngine.Graphics { get; set; }

        public Rectangle SafeArea
        {
            get { throw new NotImplementedException(); }
        }

        public SpriteBatch SpriteBatch { get; set; }

        public GameTime GameTime { get; set; }

    }
}
