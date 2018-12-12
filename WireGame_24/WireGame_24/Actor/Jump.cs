using WireGame_24.Actor.Interface;
using WireGame_24.Device;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGame_24.Actor
{
    class Jump : GameObject
    {
        public Jump(Vector2 position, GameDevice gameDevice)
           : base("Bara", position, 32, 32, gameDevice)
        { }

        public Jump(Jump other)
            : this(other.position, other.gameDevice)
        {
        }

        public override object Clone()
        {
            return new Jump(this);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Hit(GameObject gameObject)
        {
            var player = gameObject as Player;
            if (player == null)
            {
                return;
            }
            player.SetVelocity(new Vector2(10, -10));
        }
    }
}
