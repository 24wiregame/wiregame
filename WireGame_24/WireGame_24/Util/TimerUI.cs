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
        private int score;//得点
        private int poolScore;//一時保存用の得点

        public TimerUI()
        {
            
        }

        public TimerUI(Timer timer)
        {
            this.timer = timer;
        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("score", new Vector2(400, 10));
            renderer.DrawNumber("number_wire", new Vector2(600, 13), timer.Now());
        }
        public void Draw2(Renderer renderer)
        {
            renderer.DrawTexture("score", new Vector2(400, 500));
            renderer.DrawNumber("number_wire", new Vector2(600, 500), timer.Now());
        }
        public void Draw3(Renderer renderer)
        {
            renderer.DrawTexture("score", new Vector2(400, 800));
            renderer.DrawNumber("number_wire", new Vector2(600, 800), timer.Now());
        }
        public void Draw4(Renderer renderer)
        {
            if (timer.Now() <= 4&timer.Now()>=3)
            {
                renderer.DrawTexture("3", new Vector2(800, 300));
            }
            else if (timer.Now() <= 3&timer.Now()>=2)
            {
                renderer.DrawTexture("2", new Vector2(800, 300));
            }
            else if (timer.Now() <= 2&timer.Now()>=1)
            {
                renderer.DrawTexture("1", new Vector2(800, 300));
            }
            else if(timer.Now()<=1&timer.Now()>=0.1f)
            {
                renderer.DrawTexture("start", new Vector2(800, 300));
            }
           
        }
      
        public float GetScore()
        {
            return timer.Now();
        }
    }
}
