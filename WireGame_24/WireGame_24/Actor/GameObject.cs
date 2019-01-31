using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using WireGame_24.Device;

namespace WireGame_24.Actor
{
    enum Direction
    {
        Top,Bottom,Left,Right
    };

    abstract class GameObject : ICloneable
    {
        protected string name;
        protected Vector2 position;
        protected int width;
        protected int height;
        protected bool isDeadFlag = false;
        protected GameDevice gameDevice;
        protected Sound sound;

        public GameObject(string name, Vector2 position, int width,
          int height, GameDevice gameDevice)
        {
            this.name = name;
            this.position = position;
            this.width = width;
            this.height = height;
            this.gameDevice = gameDevice;
            sound = gameDevice.GetSound();
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public int GetWidht()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public abstract object Clone();
        public abstract void Update(GameTime gameTime);
        public abstract void Hit(GameObject gameObject);

        public virtual void Draw ( Renderer renderer)
        {
            renderer.DrawTexture(name, position + gameDevice.GetDisplayModify());
        }

        public bool IsDead()
        {
            return isDeadFlag;
        }

        public Rectangle getRectangle()
        {
            Rectangle area = new Rectangle();

            area.X = (int)position.X;
            area.Y = (int)position.Y;
            area.Height = height;
            area.Width = width;

            return area;

        }

        public bool IsCollision(GameObject otherObj)
        {
            return this.getRectangle().Intersects(otherObj.getRectangle());
        }

        public Direction CheckDirection(GameObject otherObj)
        {
            Point thisCenter = this.getRectangle().Center;
            Point otherCenter = otherObj.getRectangle().Center;

            Vector2 dir =
                new Vector2(thisCenter.X, thisCenter.Y) -
                new Vector2(otherCenter.X, otherCenter.Y);
            
            if (Math.Abs(dir.X ) > Math.Abs(dir.Y))
            {
                if(dir.X > 0)
                {
                    return Direction.Right;
                }
                return Direction.Left;
            }

            if(dir.Y  > 0)
            {
                return Direction.Bottom;
            }
            return Direction.Top;

        }
        
         /// <summary>
        /// 衝突判定（2点間の距離と円の半径）
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool InCollision(GameObject other)
        {
            //じぶんと相手の位置の長さを計算（2点間の距離）
            float length = ((position + new Vector2(16, 16)) - (other.GetPosition()+new Vector2(32,32))).Length();
            //白玉画像のサイズは64なので、半径は32
            float radiusSum = 16f + 16f;
            //自分半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            if (length <= radiusSum)
            {
                return true;
            }
            return false;
        }

    }


}
