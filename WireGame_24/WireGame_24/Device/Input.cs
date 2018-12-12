using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGame_24.Device
{
    /// <summary>
    /// キーボード入力、マウス入力の管理クラス
    /// </summary>
    static class Input
    {
        //移動量
        private static Vector2 velocity = Vector2.Zero;
        private static Vector2 padvelocity = Vector2.Zero;
        //キーボード
        private static KeyboardState currentKey;//現在のキーの状態
        private static KeyboardState previousKey;//1フレーム前のキーの状態
        //マウス
        private static MouseState currentMouse;//現在のマウスの状態
        private static MouseState previousMouse;//1フレーム前のマウスの状態
        private static List<PlayerIndex> playerIndex = new List<PlayerIndex>()
        {
            PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four

        };
        private static Dictionary<PlayerIndex, GamePadState> currentGamePads =
            new Dictionary<PlayerIndex, GamePadState>()
            {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One) },
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two) },
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three) },
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four) },


            };
        private static Dictionary<PlayerIndex, GamePadState> previousGamePads =
           new Dictionary<PlayerIndex, GamePadState>()
           {
                {PlayerIndex.One, GamePad.GetState(PlayerIndex.One) },
                {PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two) },
                {PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three) },
                {PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four) },


           };
        /// <summary>
        /// 更新
        /// </summary>
        public static void Update()
        {
            //キーボード
            previousKey = currentKey;
            currentKey = Keyboard.GetState();
            //マウス
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
            for( int i = 0; i < currentGamePads.Count; i++)
            {
                if(currentGamePads[playerIndex[i]].IsConnected == false)
                {
                    continue;
                }
                previousGamePads[playerIndex[i]] =
                    currentGamePads[playerIndex[i]];
                currentGamePads[playerIndex[i]] = 
                    GamePad.GetState(playerIndex[i]);
            }
            //更新
            UpdateVelocity();
        }

        //キーボード関連
        public static Vector2 Velocity()
        {
            return velocity;
        }

        /// <summary>
        /// キー入力による移動量の更新
        /// </summary>
        private static void UpdateVelocity()
        {
            //毎ループ初期化
            velocity = Vector2.Zero;

            //右
            if( currentKey.IsKeyDown(Keys.Right))
            {
                velocity.X += 1.0f;
            }
            //左
            if( currentKey.IsKeyDown(Keys.Left))
            {
                velocity.X -= 1.0f;
            }
            //上
            if( currentKey.IsKeyDown(Keys.Up))
            {
                velocity.Y -= 1.0f;
            }
            //下
            if( currentKey.IsKeyDown(Keys.Down))
            {
                velocity.Y += 1.0f;
            }

            //正規化
            if( velocity.Length() != 0)
            {
                velocity.Normalize();
            }
        }

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="key">チェックしたいキー</param>
        /// <returns>現在キーが押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsKeyDown(Keys key)
        {
            return currentKey.IsKeyDown(key) && !previousKey.IsKeyDown(key);
        }

        /// <summary>
        /// キーが押された瞬間か？
        /// </summary>
        /// <param name="key">チェックしたいキー</param>
        /// <returns>押された瞬間ならtrue</returns>
        public static bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }

        /// <summary>
        /// キーが押されているか？
        /// </summary>
        /// <param name="key">調べたいキー</param>
        /// <returns>キーが押されていたらtrue</returns>
        public static bool GetKeyState(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }

        //マウス関連
        /// <summary>
        /// マウスの左ボンタンが押された瞬間か？
        /// </summary>
        /// <returns>現在押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsMouseLButtonDown()
        {
            return currentMouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// マウスの左ボタンが離された瞬間か？
        /// </summary>
        /// <returns>現在はなされていて、1フレーム前に押されていたらtrue</returns>
        public static bool IsMouseLButtonUp()
        {
            return currentMouse.LeftButton == ButtonState.Released &&
                previousMouse.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの左ボタンが押されているか？
        /// </summary>
        /// <returns>左ボタンが押されていたらtrue</returns>
        public static bool IsMouseLButton()
        {
            return currentMouse.LeftButton == ButtonState.Pressed;
        }

        /// マウスの右ボンタンが押された瞬間か？
        /// </summary>
        /// <returns>現在押されていて、1フレーム前に押されていなければtrue</returns>
        public static bool IsMouseRButtonDown()
        {
            return currentMouse.RightButton == ButtonState.Pressed &&
                previousMouse.RightButton == ButtonState.Released;
        }

        /// <summary>
        /// マウスの右ボタンが離された瞬間か？
        /// </summary>
        /// <returns>現在はなされていて、1フレーム前に押されていたらtrue</returns>
        public static bool IsMouseRButtonUp()
        {
            return currentMouse.RightButton == ButtonState.Released &&
                previousMouse.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの左ボタンが押されているか？
        /// </summary>
        /// <returns>左ボタンが押されていたらtrue</returns>
        public static bool IsMouseRButton()
        {
            return currentMouse.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// マウスの位置を返す
        /// </summary>
        public static Vector2 MousePosition
        {
            //プロパティでGetterを作成
            get
            {
                return new Vector2(currentMouse.X, currentMouse.Y);
            }
        }

        /// <summary>
        /// マウスのスクロールホイールの変化量
        /// </summary>
        /// <returns>1フレーム前と現在のホイール量の差分</returns>
        public static int GetMouseWheel()
        {
            return previousMouse.ScrollWheelValue - currentMouse.ScrollWheelValue;
        }
        public static bool IsKeyDown(PlayerIndex index, Buttons button)
        {
            if( currentGamePads[index].IsConnected == false)
            {
                return false;
            }
            return currentGamePads[index].IsButtonDown(button) &&
                !previousGamePads[index].IsButtonDown(button);
        }
        public static bool GetKeyTrigger(PlayerIndex index, Buttons button)
        {
            if(currentGamePads[index].IsConnected == false)
            {
                return false;
            }
            return IsKeyDown(index, button);
        }
        public static bool GetKeyState(PlayerIndex index, Buttons button)
        {
            if( currentGamePads[index].IsConnected == false)
            {
                return false;
            }
            return currentGamePads[index].IsButtonDown(button);
        }
        public static Vector2 Velocity(PlayerIndex index)
        {
            if(currentGamePads[index].IsConnected == false)
            {
                return Vector2.Zero;
            }
            padvelocity = Vector2.Zero;
            if(currentGamePads[index].IsButtonDown(Buttons.DPadRight))
            {
                padvelocity.X += 1.0f;
            }
            if (currentGamePads[index].IsButtonDown(Buttons.DPadLeft))
            {
                padvelocity.X -= 1.0f;
            }
            if (currentGamePads[index].IsButtonDown(Buttons.DPadUp))
            {
                padvelocity.Y -= 1.0f;
            }
            if (currentGamePads[index].IsButtonDown(Buttons.DPadDown))
            {
                padvelocity.Y += 1.0f;
            }
            if(padvelocity.Length() !=0)
            {
                padvelocity.Normalize();
            }
            return padvelocity;
        }
        public static bool GetKeyRelease(Keys key)
        {
            return !currentKey.IsKeyDown(key) && previousKey.IsKeyDown(key);
        }
        
            











    }
}
