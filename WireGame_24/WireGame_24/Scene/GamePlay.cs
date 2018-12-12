using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Actor;
using WireGame_24.Device;


namespace WireGame_24.Scene
{
    class GamePlay : IScene
    {
        private bool isEndFlag;           //終了フラグ
        private Player player;    　　　　//プレイヤー
        private TarGet tarGet;            //ターゲット
        private Wire wire;　　　　　　　　//ワイヤー
        private GameDevice gameDevice;    //ゲームデバイス
        private Map map;                  //マップ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GamePlay()
        {
            isEndFlag = false;
            gameDevice = GameDevice.Instance();
        }
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            renderer.DrawTexture("haikei_1", Vector2.Zero);
            player.Draw(renderer);
            wire.Draw(renderer);
            tarGet.Draw(renderer);
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            player = new Player(new Vector2(200, 500), gameDevice);
            player.Initialize();
           
            tarGet = new TarGet(new Vector2(800, 300), gameDevice);
            wire = new Wire(player, tarGet);
            //シーン終了フラグを初期化
            isEndFlag = false;
        }

        /// <summary>
        /// シーン終了か？
        /// </summary>
        /// <returns></returns>
        public bool IsEnd()
        {
            return isEndFlag;
        }

        /// <summary>
        /// 次のシーンは
        /// </summary>
        /// <returns>次のシーン名</returns>
        public Scene Next()
        {
            Scene nextScene = Scene.Ending;
            return nextScene;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="gameTime">ゲーム時間</param>
        public void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            wire.Update(gameTime);
            if (Input.IsKeyDown(Keys.Space))
            {
                isEndFlag = true;
            }
        }

    }
}
