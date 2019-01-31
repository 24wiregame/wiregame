using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireGame_24.Device;
using Microsoft.Xna.Framework;

namespace WireGame_24.Actor
{
    class Goal : GameObject
    {
        private Sound sound;//SE
 
        public Goal(Vector2 position, GameDevice gameDevice) : base("GB_1", position, 32, 32, gameDevice)
        {
        }
        public Goal(Goal other) : this(other.position, other.gameDevice)
        {
        }
        public override object Clone()
        {
            return new Goal(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
            
        }
        //public override void Draw(Renderer renderer)
        //{
        //}
    }
}
