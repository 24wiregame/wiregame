using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Device;

namespace WireGame_24.Scene
{
    class SceneSelect : IScene
    {
        private bool isEndFlag;
        private Sound sound;
        private List<Vector2> positions;
        private List<Rectangle> sourceRects;
        private int cursor;
        public SceneSelect()
        {
            isEndFlag = false;
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();
            positions = new List<Vector2>()
            {
                new Vector2(64,128),
                new Vector2(480,128),
                new Vector2(896,128),
                new Vector2(1285,128),
                new Vector2(64,448),
                //new Vector2(480,448),
            };
            sourceRects = new List<Rectangle>()
            {
                new Rectangle(0,0,300,250),
                new Rectangle(300,0,300,250),
                new Rectangle(600,0,300,250),
                new Rectangle(900,0,300,250),
                new Rectangle(1200,0,300,250),
            };
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("back0", Vector2.Zero);
            for (int i = 0; i < 5; i = i + 1)//
            {
                renderer.DrawTexture("stage1,5", positions[i], sourceRects[i],Color.White);
            }
            renderer.DrawTexture("SF", positions[cursor]);
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
            cursor = 0;
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            return Scene.GamePlay;
        }

        public void Shutdown()
        {
            sound.StopBGM();
        }

        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Right))
            {
                cursor += 1;
                if (cursor > 4)
                    cursor = 0;
                sound.PlaySE("click");
            }
            if (Input.GetKeyTrigger(Keys.Left))
            {
                cursor -= 1;
                if (cursor < 0)
                    cursor = 4;
                sound.PlaySE("click");
            }
            if (Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
                GameData.stageNumber = cursor + 1;
                sound.PlaySE("click");
            }

        }
    }
}
