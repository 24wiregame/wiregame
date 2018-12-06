﻿using System;
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
        private Vector2 position;
        private Vector2 velocity;
        private bool isJump;
        private Vector2 originVelocty;

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
            //if (!isJump)
            //{
            //    originVelocty = Vector2.Zero;
            //}
            //移動処理
            position.X = position.X + velocity.X * sped + originVelocty.X;
            
            position.Y += velocity.Y + originVelocty.Y;

            originVelocty *= 0.99f;

            //当たり判定(衝突判定)
            var min = Vector2.Zero;
            var max = new Vector2(Screen.Width - 64, Screen.Height - 64);

            position = Vector2.Clamp(position, min, max);
            if (position.Y == max.Y)
            {
                isJump = false;
            }
            if (position.Y == min.Y)
            {
                velocity.Y = 0;
            }

        }

        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("Player0", position);
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
        public void SetPositionX(float positionX)
        {
            position.X = positionX;
        }
        public void SetPositionY(float positionY)
        {
            position.Y = positionY;
        }
        public float GetVeloity()
        {
            return velocity.X;
        }
        public void SetJump(bool flg)
        {
            isJump = flg;
        }
        public void SetSpeed(Vector2 velocity)
        {
            originVelocty = velocity;
        }
    }
}
