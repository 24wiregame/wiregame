using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Device;
namespace WireGame_24.Actor
{
    class Wire
    {
        private Vector2 wirePosition;    //ワイヤー始点
        private Vector2 wireTop;　　　　 //ワイヤーの終点
        private Vector2 line;            //ワイヤーの始点と終点をつなぐ線
        private float length;            //画像を引き延ばす長さ
        private float rotate;            //回転角度
        private Player player;
        private TarGetBlock tarGetBlock; //ターゲットブロック

        private bool isDraw;             //描画中かどうか

        private float gravity;           //重力
        private float rot = 0;           //重りの角度
        private float rot_spd = 0;	　　 // 角速度
        //ワイヤーの描画用
        private Vector2 originPosition;  //playerPosition固定用
        private Vector2 originLine;
        private float originLength;
        private float originRotate;

        private Vector2 velocity;

        private bool isHit;

        private bool isUse;

        public Wire()
        {
            
            isDraw = false;
            gravity = 0.5f;

            isUse = false;
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
            player.OnHitEvent += OnHit;
        }

        private void OnHit()
        {
            isHit = true;
            rot_spd = 0;

        }

        public void Update(GameTime gameTime)
        {
            if (player.IsDead())
            {
                return;
            }
            
            // 重りの座標
            isUse = false;
            ////////////////////////////////////////////////
            if (Input.GetKeyTrigger(Keys.A))
            {
                wirePosition = player.GetPosition();
                line = wireTop - wirePosition;
                length = line.Length();                       //線の長さ
                rot = MathHelper.ToDegrees(
                    (float)Math.Atan2(
                        player.GetPosition().Y - wireTop.Y,
                        player.GetPosition().X - wireTop.X)
                    );
                //rot_spd = 0;
                Console.WriteLine("trigger");

                rot_spd = (float)(180 - 2 * Math.Acos(player.GetVeloity().Length() / (2 * length)) * 180 / Math.PI);
                rot_spd *= Math.Sign(tarGetBlock.GetPosition().Y - player.GetPosition().Y) * Math.Sign(player.GetVeloity().X);
                Console.WriteLine("WireHit!!!!!");
                tarGetBlock.SetName("TG_green");
                isUse = true;
                return;
            }



            if (Input.GetKeyState(Keys.A))
            {
                isUse = true;
                //描画用の線
                originPosition = player.GetPosition();
                originLine = wireTop - originPosition;
                originLength = originLine.Length();
                originRotate = (float)Math.Atan2(originLine.Y, originLine.X);
                Vector2 playerVelocity = player.GetVeloity();

                Console.WriteLine("RotSpd:"+rot_spd+" /PlayerSpd:"+playerVelocity);

                isDraw = true;
                rotate = (float)Math.Atan2(line.Y, line.X);
                ///////////////////////////////////////////

                // 現在の重りの位置
                var rad = rot * Math.PI / 180;
                var px = wireTop.X + Math.Cos(rad) * length;
                var py = wireTop.Y + Math.Sin(rad) * length;
                // 重力移動量を反映した重りの位置
                var vx = px - wireTop.X;
                var vy = py - wireTop.Y;
                var t = -(vy * gravity) / (vx * vx + vy * vy);
                var gx = px + t * vx;
                var gy = py + gravity + t * vy;
                // ２つの重りの位置の角度差
                var r = Math.Atan2(gy - wireTop.Y, gx - wireTop.X) * 180 / Math.PI;
                // 角度差を角速度に加算
                var sub = r - rot;
                sub -= Math.Floor(sub / 360.0) * 360.0;
                if (sub < -180.0) sub += 360.0;
                if (sub > 180.0) sub -= 360.0;
                rot_spd += (float)sub;
                rot_spd *= 0.999f;
                // 角度に角速度を加算
                rot += rot_spd;
                // 新しい重りの位置
                rad = rot * Math.PI / 180;
                px = wireTop.X + Math.Cos(rad) * length;
                py = wireTop.Y + Math.Sin(rad) * length;


                velocity = new Vector2((float)px - player.GetPosition().X, (float)py - player.GetPosition().Y);

                player.SetVelocity(new Vector2());
                player.SetPositionX((float)px);
                player.SetPositionY((float)py);
                player.SetJump(false);
            }
            else
            {
                isDraw = false;
            }
            if (Input.GetKeyRelease(Keys.A))
            {
                tarGetBlock.SetName("TG_yellow");
                player.SetVelocity(velocity);
                Console.WriteLine("こここここここここｋ"+ velocity);
                player.SetJump(true);

            }

        }

        public void SetTarget(TarGetBlock block)
        {
            if (tarGetBlock != null)
            {
                tarGetBlock.SetName("TG_black");
            }
            this.tarGetBlock = block;
            tarGetBlock.SetName("TG_yellow");
            wireTop = tarGetBlock.GetPosition();
        }
        
        public void Draw(Renderer renderer)
        {
            if (isDraw)
            {
                renderer.DrawTexture(
                "pointer",
                wireTop +new Vector2(16,16) +GameDevice.Instance().GetDisplayModify(),　　　　　　　　　　　　　　　//wireTopからプレイヤーに向かって伸びている
                originRotate + (float)Math.PI / 2.0f,
                Vector2.Zero,
                new Vector2(1, length));
            }
        }
        public bool IsUse()
        {
            return isUse;
        }
        public Vector2 GetVelocity()
        {
            return velocity;
        }
    }
}
