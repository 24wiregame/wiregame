using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Util;


namespace WireGame_24.Scene
{
    /// <summary>
    /// エンディングクラス
    /// </summary>
    class Ending : IScene
    {
        private bool isEndFlag;//終了フラグ
        private TimerUI timer;
        
        IScene backGrouneScene;
        private TimerUI maxscore;
        private Sound sound;
        
     

      

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Ending(IScene scene)
        {
            backGrouneScene = scene;
            isEndFlag = false;
            //maxscore = new TimerUI();
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();



        }
       
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {

            renderer.Begin();
            renderer.DrawTexture("block", Vector2.Zero);
            renderer.DrawTexture("black", new Vector2 (32, 0));
            timer.Draw2(renderer);
            
            maxscore.Draw3(renderer);
            renderer.End();
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;

            timer = ((GamePlay)backGrouneScene).returnScore();
           
            if (maxscore==null)
            {
                maxscore = timer;
            }
            if (timer.GetScore() < maxscore.GetScore())
            {
                maxscore = timer;
            }
           

        }

        /// <summary>
        /// シーン終了か？
        /// </summary>
        /// <returns>シーン終了してたらtrue</returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンへ
        /// </summary>
        /// <returns>次のシーン名</returns>
        public Scene Next()
        {
            Scene nextScene = Scene.Title;
            return nextScene;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
  

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("Ending2");
            if (Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
            }
          
        }
        public void Shutdown()
        {
            sound.StopBGM();
        }

    }
}
