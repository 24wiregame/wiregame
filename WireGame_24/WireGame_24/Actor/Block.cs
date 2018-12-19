using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using WireGame_24.Device;

namespace WireGame_24.Actor
{
    class Block : GameObject
    {
        public Block(Vector2 position, GameDevice gameDevice)
            :base("block", position,32,32,gameDevice)
        { }

        public Block(Block other)
            :this(other.position, other.gameDevice)
        {
        }

        public override object Clone()
        {
            return new Block(this);
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
            player.SetVelocity(new Vector2(0, -10));
        }
    }
}
