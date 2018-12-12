using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using Microsoft.Xna.Framework.Input;


namespace WireGame_24.Scene
{
    /// <summary>
    /// エンディングクラス
    /// </summary>
    class Ending : IScene
    {
        private bool isEndFlag;//終了フラグ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Ending( )
        {
            isEndFlag = false;     
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
            renderer.End();
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            isEndFlag = false;
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
        public void Shutdown()
        {
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            if (Input.GetKeyTrigger(Keys.Space))
            {
                isEndFlag = true;
            }
        }
    }
}
