using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;


namespace WireGame_24.Util
{
    class TimerUI
    {
        private Timer timer;
        public TimerUI(Timer timer)
        {
            this.timer = timer;
        }
        public void Draw(Renderer renderer)
        {
            //timer画像の描画
            renderer.DrawTexture("timer", new Vector2(400, 10));
            //現在の時間の描画
            renderer.DrawNumber("number", new Vector2(600, 13), timer.Now());
        }
    }
}
