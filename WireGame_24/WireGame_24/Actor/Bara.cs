using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using WireGame_24.Actor.Interface;
namespace WireGame_24.Actor
{
    class Bara : GameObject
    {
        public Bara(string name,Vector2 position, GameDevice gameDevice)
            : base(name, position, 32, 32, gameDevice)
        { }

        public Bara(Bara other)
            : this(other.name,other.position, other.gameDevice)
        {
        }

        public override object Clone()
        {
            return new Bara(this);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Hit(GameObject gameObject)
        {
            var deadcomponet = gameObject as IApplicableDead;
            if(deadcomponet == null)
            {
                return;
            }
            deadcomponet.Die();
        }

    }
}
