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
    public class Ball
    {
        public AModel Model = new AModel(Vector3.Zero, "Disk");
        float Velocity = 2f;
        public Vector3 MoveDir;
        const int LocationY = 2;
        public const int BALLSIZE = 5;

        public Ball()
        {
            Model.Location = new Vector3(0, LocationY, 0);
            SetRandomMoveDir();
        }

        void SetRandomMoveDir()
        {
            float x = Maths.RandomNr(0, 1);
            if (x == 0)
                x = -1;
            MoveDir = new Vector3(x, 0, Maths.RandomFloat(-1, 1));
        }

        public void Update(GameTime gameTime)
        {
            // Move
            Model.Location += MoveDir * Velocity;

            // Score collision
            if (Model.Location.X <= -Level.HALF_LEVEL_SIZE_X)
            {
                Level.Instance.Players[1].Score++;
                Model.Location = new Vector3(0, LocationY, 0);
                SetRandomMoveDir();
            }
            else if (Model.Location.X > Level.HALF_LEVEL_SIZE_X)
            {
                Level.Instance.Players[0].Score++;
                Model.Location = new Vector3(0, LocationY, 0);
                SetRandomMoveDir();
            }

            // Wall collision
            if (Model.Location.Z-BALLSIZE <= -Level.HALF_LEVEL_SIZE_Z || Model.Location.Z+BALLSIZE > Level.HALF_LEVEL_SIZE_Z)
                MoveDir.Z *= -1;

            // Paddle collision
            foreach (Player p in Level.Instance.Players)
            {
                if (Model.BoundingBox.Intersects(p.Paddle.AModel.BoundingBox))
                {
                    MoveDir.X *= -1;
                }
            }
        }

        public void Draw()
        {
            Model.DrawWithDiffuse(Engine.Instance.Camera3D);
        }
    }
}