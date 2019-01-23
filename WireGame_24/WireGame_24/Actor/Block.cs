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
            Vector2 VN;
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
            
            //プレイヤーがブロックの上面
            if (dir == Direction.Top)
            {
                VW = position + new Vector2(32, 0) - position;
                VN = new Vector2(0, -1);
                VN.Normalize();
                VA = -VF * VN;
                VR = VF + 2 * VA * VN;
                player.SetVelocity(VR);
            }
            //プレイヤーがブロックの側面右
            else if (dir == Direction.Right)
            {
                VW = position + new Vector2(0, 32) - position;
                VN = new Vector2(-1, 0);
                VN.Normalize();
                VA = -VF * VN;
                VR = VF + 2 * VA * VN;
                player.SetVelocity(VR);
            }
            //プレイヤーがブロックの側面左
            else if (dir == Direction.Left)
            {
                VW = position + new Vector2(32,32) - position + new Vector2(32,0);
                VN = new Vector2(1, 0);
                VN.Normalize();
                VA = -VF * VN;
                VR = VF + 2 * VA * VN;
                player.SetVelocity(VR);
            }
            //プレイヤーがブロックの底面
            else if (dir == Direction.Bottom)
            {
                VW = position + new Vector2(32, 32) - position + new Vector2(0,32);
                VN = new Vector2(0, 1);
                VN.Normalize();
                VA = -VF * VN;
                VR = VF + 2 * VA * VN;
                player.SetVelocity(VR);
            }
        }
        

    }
}
