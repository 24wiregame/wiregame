using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireGame_24.Device;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace WireGame_24.Scene
{
    class Title : IScene
    {
        private bool isEndFlag;

        public  Title()
        {
            isEndFlag = false;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("block", Vector2.Zero);
            renderer.End();
        }

        public void Initialize()
        {
            isEndFlag = false;
        }

        public bool IsEnd()
        {
            return isEndFlag;
        }

        public Scene Next()
        {
            Scene nextScene = Scene.PlayerSelect;
            //Scene nextScene = Scene.SceneSelect;
            return nextScene;
        }

        public void Shutdown()
        {
        }

        public void Update(GameTime gameTime)
        {
            if(Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
            }
        }
    }
}
