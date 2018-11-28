using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Def;

namespace WireGame_24.Device
{
    public static class Camera
    {
        //カメラの座標
        private static Vector2 position;

        //最小値
        private static Vector2 min;
        //最大値
        private static Vector2 max;

        public static void SetPosition(Vector2 pos)
        {
            position = pos;
            position = Vector2.Clamp(position, min, max);
        }
        public static void Move(Vector2 velocity)
        {
            //移動
            position += velocity;
            position = Vector2.Clamp(position, min, max);

        }
        public static void SetMoveArea(Vector2 minmum, Vector2 maximum)
        {
            min = minmum;
            max = maximum;
        }
        public static Vector2 GetPosition()
        {
            return position;
        }
        public static Vector2 GetScreenPos(Vector2 pos)
        {
            return pos - position;
        }
        public static Vector2 GetMin()
        {
            return min;
        }
        public static Vector2 Getmax()
        {
            return max;
        }
        public static Vector2 GetArrayPos()
        {
            Vector2 arrayPos = new Vector2(
                (int)position.X / Size.BlockX,
                (int)position.Y / Size.BlockY);
            return arrayPos;
        }
    }
}
