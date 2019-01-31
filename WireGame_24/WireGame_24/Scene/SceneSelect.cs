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
            };
            sourceRects = new List<Rectangle>()
            {
                new Rectangle(0,0,320,180),
                new Rectangle(320,0,320,180),
                new Rectangle(640,0,320,180),
            };
        }
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            for (int i = 0; i < 1; i = i + 1)//
            {
                renderer.DrawTexture("stage", positions[i], sourceRects[i],Color.White);
            }
            renderer.DrawTexture("cursor", positions[cursor]);
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
                if (cursor > 2)
                    cursor = 0;
                sound.PlaySE("click");
            }
            if (Input.GetKeyTrigger(Keys.Left))
            {
                cursor -= 1;
                if (cursor < 0)
                    cursor = 2;
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
