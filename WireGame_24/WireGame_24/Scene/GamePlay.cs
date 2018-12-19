﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Actor;
using WireGame_24.Scene;
namespace WireGame_24.Scene
{
    /// <summary>
    /// ゲームプレイクラス
    /// </summary>
    class GamePlay : IScene
    {
        private GameObjectManager gameObjectManager;
        private bool isEndFlag;//終了フラグ
        private Map map;
        private Player player;
        private Player player2;
        private TarGet tarGet;            //ターゲット
        private Wire wire;        //ワイヤー
        private GameDevice gameDevice;    //ゲームデバイス

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GamePlay()
        {
            isEndFlag = false;
            gameObjectManager = new GameObjectManager();
            var gameDevice = GameDevice.Instance();
            //player = new Player(new Vector2(32 * 2, 32 * 12), GameDevice.Instance());
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
            // renderer.DrawTexture("black", Vector2.Zero);
            map.Draw(renderer);
            player.Draw(renderer);
            //player.Draw(renderer);
            if (GameData.playerNumber == 2)
            {
                player2.Draw(renderer);
            }
            gameObjectManager.Draw(renderer);
            wire.Draw(renderer);
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            wire = new Wire();
            player = new Player(new Vector2(32 * 2, 32 * 12),
               GameDevice.Instance(), gameObjectManager,wire);
            if (GameData.playerNumber == 2)
            {
                player2 = new Player(new Vector2(32 * 1, 32 * 12),
                GameDevice.Instance(), gameObjectManager, wire);
            }
            wire.SetPlayer(player);
            gameObjectManager.Initialize();
            //シーン終了フラグを初期化
            isEndFlag = false;
            map = new Map(GameDevice.Instance());
            map.Load("stage" + GameData.stageNumber.ToString().PadLeft(3, '0') + ".csv");
            gameObjectManager.Add(map);
            gameObjectManager.Add(player);
           
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
            //wire.Update(gameTime);
            if (Input.GetKeyTrigger(Keys.D1))
            {
                isEndFlag = true;
            }
            if(player.Isfall())
            {
                isEndFlag = true;
            }
            //更新処理
            map.Update(gameTime);
            wire.Update(gameTime);
            player.Update(gameTime);
            
            if (player.IsDead())
            {
                return;
            }
            map.Hit(player);
            if (GameData.playerNumber == 2)
            {
                player2.Update(gameTime);
                if (player2.IsDead())
                {
                    return;
                }
                map.Hit(player2);
            }
        }
    }
}
