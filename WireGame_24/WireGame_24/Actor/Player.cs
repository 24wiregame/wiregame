using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Device;
using WireGame_24.Def;

namespace WireGame_24.Actor
{
    class Player
    {
        public Vector2 position;
        private Vector2 velocity;
        private bool isJump;


        private float gravity;
        public Player(Vector2 position, GameDevice gameDevice)
        {
            this.position = position;
        }

        public void Initialize()
        {
            gravity = 0.5f;
            isJump = false;
        }

        public void Update(GameTime gameTime)
        {
            //キー入力の移動量を取得
            velocity.X = Input.Velocity().X;
            JumpStart();

            //移動量
            float sped = 20.0f;
            //移動処理
            position.X = position.X + velocity.X * sped;

            position.Y += velocity.Y;

            //当たり判定(衝突判定)
            var min = Vector2.Zero;
            var max = new Vector2(Screen.Width - 64, Screen.Height - 64);

            position = Vector2.Clamp(position, min, max);
            if (position.Y == max.Y)
            {
                isJump = false;
            }

        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("Player0", position);
        }
        /// <summary>
        /// ジャンプ処理
        /// </summary>
        public void Jump()
        {

        }
        /// <summary>
        /// ジャンプスタート
        /// </summary>
        public void JumpStart()
        {
            if (Input.GetKeyTrigger(Keys.Z) && !isJump)
            {
                isJump = true;
                velocity.Y = -15;
            }
            if (isJump)
            {
                velocity.Y += gravity;
            }
        }

        public Vector2 GetPosition()
        {
            return position;
        }
        public float GetVeloity()
        {
            return velocity.X;
        }
    }
}
