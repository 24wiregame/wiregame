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
        private TarGet tarGet;

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

        public Wire(Player player, TarGet tarGet)
        {
            this.player = player;
            this.tarGet = tarGet;
            wireTop = tarGet.GetPosition();
            isDraw = false;
            gravity = 0.5f;
        }
        public void Update(GameTime gameTime)
        {
            // 重りの座標

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
                rot_spd = 0;
            }



            if (Input.GetKeyState(Keys.A))
            {
                //描画用の線
                originPosition = player.GetPosition();
                originLine = wireTop - originPosition;
                originLength = originLine.Length();
                originRotate = (float)Math.Atan2(originLine.Y, originLine.X);


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
                // 角度に角速度を加算
                rot += rot_spd;
                // 新しい重りの位置
                rad = rot * Math.PI / 180;
                px = wireTop.X + Math.Cos(rad) * length;
                py = wireTop.Y + Math.Sin(rad) * length;


                velocity = new Vector2((float)px - player.GetPosition().X, (float)py - player.GetPosition().Y);

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
                player.SetSpeed(velocity);
                player.SetJump(true);

            }


        }

        public void Draw(Renderer renderer)
        {
            if (isDraw)
            {
                renderer.DrawTexture(
                "pointer",
                wireTop,　　　　　　　　　　　　　　　//wireTopからプレイヤーに向かって伸びている
                originRotate + (float)Math.PI / 2.0f,
                Vector2.Zero,
                new Vector2(1, length));
            }
        }
    }
}
