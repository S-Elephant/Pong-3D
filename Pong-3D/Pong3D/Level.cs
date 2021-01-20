using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using XNALib._3D;

namespace Pong3D
{
    public class Level : IActiveState
    {
        public static Level Instance;
        List<AModel> Models = new List<AModel>();
        public Ball Ball = new Ball();

        public const int LEVEL_BOUNDARY_X = 400;
        public const int LEVEL_BOUNDARY_Z = 200;
        public const int HALF_LEVEL_SIZE_X = LEVEL_BOUNDARY_X / 2;
        public const int HALF_LEVEL_SIZE_Z = LEVEL_BOUNDARY_Z / 2;
        //public readonly Rectangle BallArea = new Rectangle(-HALF_LEVEL_SIZE_X, -HALF_LEVEL_SIZE_Z, LEVEL_BOUNDARY_X, LEVEL_BOUNDARY_Z);
        AxisDrawer AxisDrawer = new AxisDrawer(Engine.Instance.Graphics.GraphicsDevice);

        public readonly BoundingBox LevelAreaBB = new BoundingBox(new Vector3(-HALF_LEVEL_SIZE_X, -2, -HALF_LEVEL_SIZE_Z), new Vector3(HALF_LEVEL_SIZE_X, 3, HALF_LEVEL_SIZE_Z));

        public List<Player> Players = new List<Player>();


        public Level()
        {
            Engine.Instance.Camera3D = new XNALib._3D.Camera3D(new Vector3(0, 200, 400), Engine.Instance.Graphics.GraphicsDevice.Viewport);
            //Engine.Instance.Camera3D.LookAtOnce(Vector3.Zero);
            //Engine.Instance.Camera3D.KeepLookingAtPoint = false;
            //Engine.Instance.Camera3D.ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 500f);

            //Models.Add(new AModel(Vector3.Zero, "doggy"));            
            Models.Add(new AModel(new Vector3(0,1,0), "Cube1x1x1"));
            Models[0].Scale = Matrix.CreateScale(HALF_LEVEL_SIZE_X, 1, HALF_LEVEL_SIZE_X/2);

            Players.Add(new Player(PlayerIndex.One) { Ishuman = false,AIMode = eAIMode.FollowBall });
            Players.Add(new Player(PlayerIndex.Two) { Ishuman = false,AIMode = eAIMode.Center });

        }

        public void Update(GameTime gameTime)
        {
            Ball.Update(gameTime);
            Players.ForEach(p=>p.Update(gameTime));
            Engine.Instance.Camera3D.Update();

            Engine.Instance.Camera3D.LookAt = Ball.Model.Location;

            int modifier = 5;
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
                modifier = -5;
            if(Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Z))
                Engine.Instance.Camera3D.Location += new Vector3(0, 0, modifier);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.X))
                Engine.Instance.Camera3D.Location += new Vector3(modifier, 0, 0);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Y))
                Engine.Instance.Camera3D.Location += new Vector3(0, modifier, 0);

            /*
            if(Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Left))
                Engine.Instance.Camera3D.Location += new Vector3(-modifier, 0, 0);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Right))
                Engine.Instance.Camera3D.Location += new Vector3(modifier, 0, 0);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Up))
                Engine.Instance.Camera3D.Location += new Vector3(0, 0, -modifier);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Down))
                Engine.Instance.Camera3D.Location += new Vector3(0, 0, modifier);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.NumPad1))
                Engine.Instance.Camera3D.Location += new Vector3(0, modifier, 0);
            if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.NumPad0))
                Engine.Instance.Camera3D.Location += new Vector3(0, -modifier, 0);
            */
            //Models.ForEach(m=>m.up
            Engine.Instance.Game.Window.Title = string.Format("loc:{0} lookat: {1} ball: {2} ", Engine.Instance.Camera3D.Location, Engine.Instance.Camera3D.LookAt,  Ball.Model.Location);

            Models.ForEach(m => m.Update());
        }

        public void DrawGUI()
        {
            Players.ForEach(p => p.DrawGUI());
        }

        public void Draw()
        {
            Players.ForEach(p => p.Draw());
            Models.ForEach(m => m.Draw(Engine.Instance.Camera3D));
            Ball.Draw();

            if (Engine.Instance.IsDebugMode)
            {
                DebugShapeRenderer.AddBoundingBox(Models[0].BoundingBox, Color.Red);
                DebugShapeRenderer.AddBoundingBox(Ball.Model.BoundingBox, Color.Yellow);
                DebugShapeRenderer.AddBoundingBox(LevelAreaBB, Color.Turquoise);
                DebugShapeRenderer.Draw(Engine.Instance.GameTime, Engine.Instance.Camera3D.ViewMatrix, Engine.Instance.Camera3D.ProjectionMatrix);
                AxisDrawer.Draw(Engine.Instance.Camera3D);
            }
        }
    }
}