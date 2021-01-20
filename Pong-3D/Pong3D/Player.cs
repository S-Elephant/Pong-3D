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
    public enum eAIMode { None=0, FollowBall, Center }

    public class Player
    {
        #region Members

        private int m_Score = 0;
        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        private bool m_Ishuman = true;
        public bool Ishuman
        {
            get { return m_Ishuman; }
            set { m_Ishuman = value; }
        }

        private Paddle m_Paddle;
        public Paddle Paddle
        {
            get { return m_Paddle; }
            set { m_Paddle = value; }
        }

        private PlayerIndex m_PlayerIdx;
        public PlayerIndex PlayerIdx
        {
            get { return m_PlayerIdx; }
            set { m_PlayerIdx = value; }
        }

        const int HALF_PADDLE_SIZE_Z = 15;

        private SpriteFont m_GuiFont = Common.str2Font("Gui");
        private SpriteFont GuiFont
        {
            get { return m_GuiFont; }
            set { m_GuiFont = value; }
        }

        private Dictionary<PlayerIndex, Color> m_PlayerColors = new Dictionary<PlayerIndex, Color>() {
            { PlayerIndex.One, Color.LightSeaGreen },
            { PlayerIndex.Two, Color.LightBlue }
        };
        private Dictionary<PlayerIndex, Color> PlayerColors
        {
            get { return m_PlayerColors; }
            set { m_PlayerColors = value; }
        }

        public eAIMode AIMode = eAIMode.None;

        const float AI_PADDLE_SPEED = 2f;
        const float PLAYER_PADDLE_SPEED = 1.5f;

        #endregion

        public Player(PlayerIndex playerIdx)
        {
            PlayerIdx = playerIdx;
            Paddle = new Paddle(PlayerIdx);
            //Paddle.Animation.DrawColor = PlayerColors[PlayerIdx];
        }

        public void Update(GameTime gameTime)
        {
            if (Ishuman)
            {
                List<Keys> allReleasedKeys = Engine.Instance.KB.GetAllReleasedKeys();

                #region MoveBar
                Vector3 oldBarPos = Paddle.AModel.Location;
                Vector3 moveVector = Vector3.Zero;
                switch (PlayerIdx)
                {
                    case PlayerIndex.One:
                        if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Up))
                            moveVector = new Vector3(0,0, -PLAYER_PADDLE_SPEED);
                        if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.Down))
                            moveVector = new Vector3(0,0, PLAYER_PADDLE_SPEED);
                        break;
                    case PlayerIndex.Two:
                        if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.W))
                            moveVector = new Vector3(0, 0, -PLAYER_PADDLE_SPEED);
                        if (Engine.Instance.KB.CurrentKeyboardState.IsKeyDown(Keys.S))
                            moveVector = new Vector3(0, 0, PLAYER_PADDLE_SPEED);
                        break;
                    default:
                        throw new Exception();
                }
                // Move
                Paddle.AModel.Location = Paddle.AModel.Location + moveVector;

                StayWithinLevelBoundaries();
                #endregion
            }
            else // AI
            {

                if (AIMode == eAIMode.FollowBall)
                {
                    FollowBall();
                }
                else // When the ball moves away from the AI then move paddle to the center. Else follow the ball
                {
                    if (Level.Instance.Ball.MoveDir.X < 0 &&
                        Level.Instance.Ball.Model.Location.X < 0)
                    {
                        FollowBall();
                    }
                    else
                    {
                        // Move to center
                        //int BarCenterY = (int)Paddle.AModel.Location.Y + Paddle.PaddleSize.Z / 2;
                        int BarCenterY = (int)Paddle.AModel.Location.Z;
                        int areaCenterZ = 0;
                        if (!(Math.Abs(BarCenterY - areaCenterZ) <= 2)) // only move when the paddle is not in the center
                        {
                            if (BarCenterY < areaCenterZ)
                                Paddle.AModel.Location += new Vector3(0,0, AI_PADDLE_SPEED);
                            else
                                Paddle.AModel.Location += new Vector3(0,0, -AI_PADDLE_SPEED);
                        }
                    }
                }
            }
        }

        void StayWithinLevelBoundaries()
        {
            if (Paddle.AModel.Location.Z < -Level.HALF_LEVEL_SIZE_Z + HALF_PADDLE_SIZE_Z + 1)
                Paddle.AModel.Location = new Vector3(Paddle.AModel.Location.X, Paddle.AModel.Location.Y, -Level.HALF_LEVEL_SIZE_Z + HALF_PADDLE_SIZE_Z + 1);
            if (Paddle.AModel.Location.Z > Level.HALF_LEVEL_SIZE_Z - HALF_PADDLE_SIZE_Z - 1)
                Paddle.AModel.Location = new Vector3(Paddle.AModel.Location.X, Paddle.AModel.Location.Y, Level.HALF_LEVEL_SIZE_Z - HALF_PADDLE_SIZE_Z - 1);
        }

        void FollowBall()
        {
            int adjust = 0;
            adjust = (int)MathHelper.Clamp(Level.Instance.Ball.Model.Location.Z - Ball.BALLSIZE / 2 - (int)Paddle.AModel.Location.Z, -AI_PADDLE_SPEED, AI_PADDLE_SPEED);
            Paddle.AModel.Location = new Vector3(Paddle.AModel.Location.X, Paddle.AModel.Location.Y,Paddle.AModel.Location.Z + adjust);
            StayWithinLevelBoundaries();
            //Paddle.ad.Location = new Vector2(Paddle.Location.X, MathHelper.Clamp(Paddle.Location.Y, 0, Engine.Instance.Height - Paddle.CollisionRect.Height));
        }

        public void DrawGUI()
        {
            switch (PlayerIdx)
            {
                case PlayerIndex.One:
                    Engine.Instance.SpriteBatch.DrawString(GuiFont, string.Format("Score: {0}", Score), new Vector2(10, 10), PlayerColors[PlayerIdx]);
                    break;
                case PlayerIndex.Two:
                    Engine.Instance.SpriteBatch.DrawString(GuiFont, string.Format("Score: {0}", Score), new Vector2(Engine.Instance.Width - 150, 10), PlayerColors[PlayerIdx]);
                    break;
                default:
                    throw new Exception();
            }
        }

        public void Draw()
        {
            Paddle.Draw();
        }
    }
}
