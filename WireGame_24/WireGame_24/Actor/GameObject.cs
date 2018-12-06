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
        Top, Bottom, Left, Right
    };

    abstract class GameObject : ICloneable
    {
        protected string name;
        protected Vector2 position;
        protected int width;
        protected int height;
        protected bool isDeadFlag = false;
        protected GameDevice gameDevice;

        public GameObject(string name, Vector2 position, int width, int height, GameDevice gameDevice)
        {
            this.name = name;
            this.position = position;
            this.width = width;
            this.height = height;
            this.gameDevice = gameDevice;


        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public int GetWidth()
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

        public virtual void Draw(Renderer renderer)
        {
            renderer.DrawTexture(name, position);
        }

        public bool IsDead()
        {
            return isDeadFlag;
        }

        public Rectangle GetRectangle()
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
            return this.GetRectangle().Intersects(otherObj.GetRectangle());
        }

        public Direction CheckDirection(GameObject otherObj)
        {
            Point thisCenter = this.GetRectangle().Center;
            Point otherCenter =
                otherObj.GetRectangle().Center;

            //向きのベクトル
            Vector2 dir =
                new Vector2(thisCenter.X, thisCenter.Y) -
                new Vector2(otherCenter.X, otherCenter.Y);

            //当たっている側面をリターンする
            //x成分とy成分のどちらが多いか
            if (Math.Abs(dir.X) > Math.Abs(dir.Y))
            {
                if (dir.X > 0)
                {
                    return Direction.Right;  //x方向に+と言えば右
                }
                return Direction.Left; //反対方向

            }
            if (dir.Y > 0)
            {
                return Direction.Bottom; //プログラム的には↓らしい。
            }
            return Direction.Top;
        }
    }
}
