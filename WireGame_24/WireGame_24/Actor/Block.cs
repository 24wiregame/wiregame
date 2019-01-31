using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using WireGame_24.Device;

namespace WireGame_24.Actor
{
    class Block : GameObject
    {
        public Block( string name, Vector2 position, GameDevice gameDevice)
            :base(name, position,32,32,gameDevice)
        { }


        public Block(Vector2 position, GameDevice gameDevice)
            :base("block", position,32,32,gameDevice)
        {
        }

        public Block(Block other)
            :this( other.name, other.position, other.gameDevice)
        {
        }

        public override object Clone()
        {
            return new Block(this);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Hit(GameObject gameObject)
        {
            var player = gameObject as Player;
            if (player == null)
            {
                return;
            }
            //playerの変数置き換え
            Vector2 PlayPos = player.GetPosition();

            Vector2 distance = PlayPos - position;


            ///プレイヤーのどこが当たっているか判定
            //壁の当たってる表面ベクトル
            Vector2 VW;
            //壁の法線ベクトル
            Vector2 VN = new Vector2();
            //a
            Vector2 VA;
            //反射ベクトル
            Vector2 VR;

            ///進行ベクトル
            //playerとblockの距離
            float pbDistance = (float)Math.Sqrt
                ((PlayPos.X - position.X) * (PlayPos.X - position.X) +
                ((PlayPos.Y - position.Y) * (PlayPos.Y - position.Y)));

            Vector2 pbDis = new Vector2((float)Math.Sqrt
                ((PlayPos.X - position.X) * (PlayPos.X - position.X)),
                (float)Math.Sqrt(PlayPos.Y - position.Y) * (PlayPos.Y - position.Y));

            //playerとblockの角度
            float pbRadian = (float)Math.Atan2
                (PlayPos.Y - position.Y, PlayPos.X - position.X);
            //角度方法変換
            float pbDegree = pbRadian * (float)Math.PI / 180;

            //進行ベクトルF
            float F = pbDistance * pbDegree;
            //進行ベクトル
            Vector2 VF = player.GetVeloity();

            Direction dir = CheckDirection(gameObject);

            //上下どちらか
            if (Math.Abs(distance.X) < width)
            {
                //上下判定

                //上にはねる
                if (distance.Y < 0)
                {
                    VN = new Vector2(0, -1);
                }
                //下にはねる
                else if (distance.Y > 0)
                {
                    VN = new Vector2(0, 1);
                }
            }
            else
            {
                //斜めと左右判定
                //左向きに反射する
                if (distance.X < 0)
                {
                    if (distance.Y < height)
                    {
                        VN = new Vector2(-1, -1);
                    }
                    else if (distance.Y > height)
                    {
                        VN = new Vector2(-1, 1);
                    }
                    else
                    {
                        VN = new Vector2(-1, 0);
                    }


                }
                // 右向きに反射する
                else if (distance.X > 0)
                {
                    if (distance.Y < height)
                    {
                        VN = new Vector2(1, -1);
                    }
                    else if (distance.Y > height)
                    {
                        VN = new Vector2(1, 1);
                    }
                    else
                    {
                        VN = new Vector2(1, 0);
                    }
                }


            }
            VN.Normalize();

            //もし法線と同じ方向であれば無視　（跳ね返す方向に対して移動しているから）
            if (Vector2.Dot(Vector2.Normalize(VF), VN) > 0)
            {
                return;
            }
            VA = Vector2.Dot(VF, VN) * VN;
            VR = VF - (2 * VA);
            player.SetVelocity(VR);

            return;
        }
        

    }
}
