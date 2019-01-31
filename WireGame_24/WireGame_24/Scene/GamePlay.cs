using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Actor;
using WireGame_24.Scene;
using WireGame_24.Def;
using WireGame_24.Util;
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
        private Player player;         //プレイヤー1
        private Player player2;　      //プレイヤー2
        private Wire wire;        //ワイヤー
        private GameDevice gameDevice;    //ゲームデバイス
        private Goal goal;       //ゴール
        private Timer timer;
        private TimerUI timerUI;//スコア
        private Sound sound;
        private CountDownTimer down;
        private CountDownTimer start;
        private TimerUI Stime;
        private Wire wire2;        //ワイヤー2





        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GamePlay()
        {
            isEndFlag = false;
            gameObjectManager = new GameObjectManager();
            var gameDevice = GameDevice.Instance();
            sound = gameDevice.GetSound();

            //player = new Player(new Vector2(32 * 2, 32 * 12), GameDevice.Instance());
        }
  

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">描画オブジェクト</param>
        public void Draw(Renderer renderer)
        {
            renderer.Begin();
             renderer.DrawTexture("Sky" ,Vector2.Zero);
           
            map.Draw(renderer);
            wire.Draw(renderer);
            player.Draw(renderer);

            if (GameData.playerNumber == 2)
            {
                player2.Draw(renderer);
                wire2.Draw(renderer);
                Stime.Draw4(renderer);
            }

            gameObjectManager.Draw(renderer);
            wire.Draw(renderer);
            
            timerUI.Draw(renderer);
            //Stime.Draw4(renderer);

            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            wire = new Wire(PlayerIndex.One);
            player = new Player(new Vector2(32 * 2, 32 * 12),
               GameDevice.Instance(), gameObjectManager,wire, PlayerIndex.One);


            wire.SetPlayer(player);
            gameObjectManager.Initialize();
            //シーン終了フラグを初期化
            isEndFlag = false;
            map = new Map(GameDevice.Instance());
            map.Load("stage_1.csv");
            gameObjectManager.Add(map);
            gameObjectManager.Add(player);

            timer = new CountUpTimer(100);
            timerUI = new TimerUI(timer);
            down = new CountDownTimer(2);
            start = new CountDownTimer(4);
            Stime = new TimerUI(start);
            if (GameData.playerNumber == 2)
            {
                wire2 = new Wire(PlayerIndex.Two);
                player2 = new Player(new Vector2(32 * 1, 32 * 4),
                GameDevice.Instance(), gameObjectManager, wire2, PlayerIndex.Two);
                wire2.SetPlayer(player2);
                gameObjectManager.Add(player2);
            }

            sound.PlaySE("start");
           

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
            if (timerUI.GetScore() >= 0)
            {
                //nextScene = Scene.Ending;
            }

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
            sound.PlayBGM("gameplay");

            //wire.Update(gameTime);
            //start.Update(gameTime);
            //if (start.IsTime())


                if (Input.GetKeyTrigger(Keys.D1))
                {
                    isEndFlag = true;
                }
                if (player.Isfall())
                {
                    if (down.Rate() == 0.0f)
                    {
                        sound.PlaySE("Down3");
                    }
                    down.Update(gameTime);
                    if (down.IsTime())
                    {
                        player = new Player(new Vector2(32 * 2, 32 * 12),
                        GameDevice.Instance(), gameObjectManager, wire, PlayerIndex.One);
                        wire.SetPlayer(player);
                        gameObjectManager.Add(map);
                        gameObjectManager.Add(player);
                        down.Initialize();
                        sound.PlaySE("start");
                        //isEndFlag = true;
                    }
            }
                if (player.IsGoalFlag())
                {
                    sound.StopBGM();
                    timer.ShutDown();
                    isEndFlag = true;
                    sound.PlaySE("end");
                    return;
                }
           //二人プレイ
            if (GameData.playerNumber == 2)
            {
                start.Update(gameTime);
                if (start.IsTime())
                {
                    if (player2.Isfall())
                    {
                        sound.PlaySE("Down3");
                        isEndFlag = true;
                    }
                    if (player.IsDead())
                    {
                        isEndFlag = true;
                    }
                    if (player2.IsGoalFlag())
                    {
                        sound.PlaySE("end");
                        isEndFlag = true;
                    }
                }
            }
            //更新処理
            if (GameData.playerNumber == 1)
            {
                player.setDisplayModify();
            }
            if (GameData.playerNumber == 2)
            {
                player2.Update(gameTime);
                wire2.Update(gameTime);
                if (player2.IsDead())
                {
                    return;
                }
                map.Hit(player2);
                if (player.GetPosition().X > player2.GetPosition().X)
                {
                    player.setDisplayModify();
                    if (player2.GetPosition().X <= player.GetPosition().X - Screen.Width / 1.2f)
                    {
                        player2.Die();
                        isEndFlag = true;
                    }
                }
                if (player2.GetPosition().X > player.GetPosition().X)
                {
                    player2.setDisplayModify();
                    if (player.GetPosition().X <= player2.GetPosition().X - Screen.Width / 1.2f)
                    {
                        player.Die();
                        isEndFlag = true;
                    }
                }
                if (player.GetPosition().X == player2.GetPosition().X)
                {
                    player.setDisplayModify();
                }
            }
        
                
                timer.Update(gameTime);
                //更新処理
                map.Update(gameTime);
                player.Update(gameTime);
                wire.Update(gameTime);
                map.Hit(player);

                if (Input.GetKeyTrigger(Microsoft.Xna.Framework.Input.Keys.Z))
                {

                    isEndFlag = true;
                    timer.ShutDown();
                    //}
                }
        }
        public TimerUI returnScore()
        {
            return timerUI ;
        }
       
    }
}
