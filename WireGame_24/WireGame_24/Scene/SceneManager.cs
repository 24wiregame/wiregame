using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;

namespace WireGame_24.Scene
{
    /// <summary>
    /// シーン管理者
    /// </summary>
    class SceneManager
    {
        //シーン管理用ディクショナリ
        private Dictionary<Scene, IScene> scenes = new Dictionary<Scene, IScene>();
        //現在のシーン
        private IScene currentScene = null;



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SceneManager()
        { }

        /// <summary>
        /// シーンの追加
        /// </summary>
        /// <param name="name">シーン名</param>
        /// <param name="scene">具体的なシーンオブジェクト</param>
        public void Add( Scene name, IScene scene)
        {
            //すでにシーン名が登録されていたら
            if( scenes.ContainsKey(name))
            {
                //何もしない
                return;
            }
            //シーンの追加
            scenes.Add(name, scene);
        }

        /// <summary>
        /// シーンの変更
        /// </summary>
        /// <param name="name"></param>
        public void Change( Scene name)
        {
            //何かシーンが登録されていたら
            if( currentScene != null )
            {
                //現在のシーンの終了処理
                currentScene.Shutdown();
            }

            //ディクショナリから次のシーンを取り出し、
            //現在のシーンに設定
            currentScene = scenes[name];

            //シーンの初期化
            currentScene.Initialize();
        }

        /// <summary>
        /// シーンの更新
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update( GameTime gameTime)
        {
            //シーンが登録されていないか？
            if( currentScene == null)
            {
                //何もしない
                return;
            }

            //現在のシーンの更新
            currentScene.Update(gameTime);

            //現在のシーンが終了しているか？
            if( currentScene.IsEnd())
            {
                //次のシーンを取り出し、シーン切り替え
                Change(currentScene.Next());
            }
        }

        /// <summary>
        /// シーンの描画
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw( Renderer renderer)
        {
            //現在のシーンがまだないか？
            if( currentScene == null)
            {
                //何もしない
                return;
            }
            //現在のシーンを描画
            currentScene.Draw(renderer);
        }
    }
}
