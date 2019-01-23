using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using WireGame_24.Util;
using WireGame_24.Def;

namespace WireGame_24.Scene
{
    /// <summary>
    /// 画像データ、音データ読み込み用シーン
    /// </summary>
    class LoadScene : IScene
    {
        private Renderer renderer;//描画オブジェクト

        //読み込み用オブジェクト
        private TextureLoader textureLoader;
        private BGMLoader bgmLoader;
        private SELoader seLoader;

        private int totalResouceNum;//全リソース数
        private bool isEndFlag;//終了フラグ
        private Timer timer;//演出用タイマー


        #region テクスチャ用
        /// <summary>
        /// テクスチャ読み込み用２次元配列の取得
        /// </summary>
        /// <returns></returns>
        private string[,] textureMatrix()
        {
            //テクスチャディレクトリのデフォルトパス
            string path = "./Texture/";

            //読み込み対象データ
            string[,] data = new string[,]
            {
                { "black", path},
                { "block", path},
                { "block2", path},
                { "button1", path},
                { "button2", path},
                { "door1", path},
                { "door2", path},
                { "number", path},
                { "player", path},
                { "puddle", path},
                { "Bara", path},
                {"kariT", path },
                {"cursor",path },
                {"stage",path },
                {"01or02",path },
                {"number",path },
                {"score",path },
                {"sikaku",path },
                {"red",path },
                {"SB_1",path },
                {"SB_2",path },
                {"SB_3",path },
                {"SB_4",path },
                {"SB_5",path },
                {"SB_6",path },
                {"TG_green",path },
                {"TG_yellow",path },
                {"TG_black",path },
                {"DB",path },
                {"DB_1",path },
                {"DB_2",path },
                {"DB_3",path },
                {"DB_4",path },
                {"JB_1",path },
                {"JB_2",path },
                {"JB_3",path },
                {"JB_4",path },
                {"JB_5",path },
                {"JB_6",path },
                {"JB_7",path },
                {"CB_1",path },
                {"CB_2",path },
                {"CB_3",path },
                {"CB_4",path },
                {"GB_1",path },
                {"back_1",path },
                {"DeathBall",path },
                {"IMG_0921",path },
                {"renban",path },
                
                // numberの画像はLoadシーンで使うので、Game1クラスで読み込む
                //{ "number", path } 

                //必要に応じて自分で追加
            };

            return data;
        }
        #endregion テクスチャ用

        #region BGM用
        /// <summary>
        /// BGM読み込み用２次元配列の取得
        /// </summary>
        /// <returns></returns>
        private string[,] BGMMatrix()
        {
            //テクスチャディレクトリのデフォルトパス
            string path = "./Sound/";

            //BGM(MP3)読み込み対象データ
            string[,] data = new string[,]
            {
                { "title4", path},
                {"gameplay",path },
                {"Ending2",path },
                

                //必要に応じて自分で追加
            };

            return data;
        }
        #endregion BGM用

        #region SE用
        /// <summary>
        /// SE読み込み用２次元配列の取得
        /// </summary>
        /// <returns></returns>
        private string[,] SEMatrix()
        {
            //テクスチャディレクトリのデフォルトパス
            string  path = "./Sound/";

            //BGM(MP3)読み込み対象データ
            string[,] data = new string[,]
            {
                { "click", path },
                {"end",path },
                {"jump",path },
                {"play4",path},
                {"toge1",path},
                {"Down3",path },
                {"start",path }

                //必要に応じて自分で追加
            };

            return data;
        }
        #endregion SE用

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LoadScene()
        {
            //描画オブジェクトを取得
            renderer = GameDevice.Instance().GetRenderer();

            //読み込む対象を取得し、実体生成
            textureLoader = new TextureLoader(textureMatrix());
            bgmLoader = new BGMLoader(BGMMatrix());
            seLoader = new SELoader(SEMatrix());
            isEndFlag = false;

            timer = new CountDownTimer(0.1f);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(Renderer renderer)
        {
            //描画開始
            renderer.Begin();

            renderer.DrawTexture("load", new Vector2(20, 20));

            //現在読み込んでいる数を取得
            int currentCount =
                textureLoader.CurrentCount() +
                bgmLoader.CurrentCount() +
                seLoader.CurrentCount();

            //読み込むモノがあれば描画
            if(totalResouceNum != 0)
            {
                //読み込んだ割合
                float rate = (float)currentCount / totalResouceNum;
                //数字で描画
                renderer.DrawNumber(
                    "number",
                    new Vector2(20, 100),
                    (int)( rate * 100.0f));

                //バーで描画
                renderer.DrawTexture(
                    "fade",
                    new Vector2(0, 500),
                    null,
                    0.0f,
                    Vector2.Zero,
                    new Vector2(rate * Screen.Width, 20));
            }

            //終了
            //すべてのデータを読み込んだか？
            if( textureLoader.IsEnd() && bgmLoader.IsEnd() && seLoader.IsEnd())
            {
                isEndFlag = true;
            }

            //描画終了
            renderer.End();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //終了フラグを継続に設定
            isEndFlag = false;
            //テクスチャ読み込みオブジェクトを初期化
            textureLoader.Initialize();
            //BGM読み込みオブジェクトを初期化
            bgmLoader.Initialize();
            //SE読み込みオブジェクトを初期化
            seLoader.Initialize();
            //全リソース数を計算
            totalResouceNum =
                textureLoader.RegistMAXNum() +
                bgmLoader.RegistMAXNum() +
                seLoader.RegistMAXNum();
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
        /// 次のシーン
        /// </summary>
        /// <returns></returns>
        public Scene Next()
        {
            return Scene.Title;
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Shutdown()
        {
            
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            //演出確認用（大量のデータがあるときは設定時間を０に）
            //一定時間ごとに読み込み
            timer.Update(gameTime);
            if (timer.IsTime() == false)
            {
                return;
            }
            timer.Initialize();

            //テクスチャから順々に読み込みを行う
            if ( textureLoader.IsEnd() == false)
            {
                textureLoader.Update(gameTime);
            }
            else if( bgmLoader.IsEnd() == false)
            {
                bgmLoader.Update(gameTime);
            }
            else if( seLoader.IsEnd() == false)
            {
                seLoader.Update(gameTime);
            }            
        }
    }
}
