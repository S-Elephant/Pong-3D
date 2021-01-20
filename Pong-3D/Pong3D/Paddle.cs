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
    public class Paddle
    {
        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            set { m_PlayerIdx = value; }
        }

        public readonly Vector3 PaddleSize = new Vector3(10, 4, 20);

        public AModel AModel;

        public Paddle(PlayerIndex playerIdx)            
        {
            PlayerIdx = playerIdx;

            AModel = new AModel(new Vector3(0,PaddleSize.Y/2,0), "Paddle");

            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    AModel.Location = new Vector3(AModel.Location.X + Level.HALF_LEVEL_SIZE_X, AModel.Location.Y, AModel.Location.Z);
                    break;
                case PlayerIndex.Two:
                    AModel.Location = new Vector3(AModel.Location.X - Level.HALF_LEVEL_SIZE_X, AModel.Location.Y, AModel.Location.Z);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Update(GameTime gameTime)
        {
            AModel.Update();
        }

        public void Draw()
        {
            AModel.Draw(Engine.Instance.Camera3D);
            if(Engine.Instance.IsDebugMode)
                DebugShapeRenderer.AddBoundingBox(AModel.BoundingBox, Color.White);
        }
    }
}
