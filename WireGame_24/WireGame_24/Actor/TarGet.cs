using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;

namespace WireGame_24.Actor
{
    class TarGet
    {
        private Vector2 position;

        public TarGet(Vector2 position, GameDevice gameDevice)
        {
            this.position = position;
        }
        public void Draw(Renderer renderer)
        {
            renderer.DrawTexture("bullet3", position);
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
