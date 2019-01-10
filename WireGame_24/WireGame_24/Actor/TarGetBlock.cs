using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Actor;
using WireGame_24.Device;
namespace WireGame_24.Actor
{
    class TarGetBlock : GameObject
    {

        public TarGetBlock(Vector2 position, GameDevice gameDevice)
           : base("TG_black", position, 32, 32, gameDevice)
        { }

        public TarGetBlock(TarGetBlock other)
            : this(other.position, other.gameDevice)
        { }

        public override object Clone()
        {
            return new TarGetBlock(this);
        }
        public void Initialize()
        {
            name = "TG_black";
        }

        
        public override void Update(GameTime gameTime)
        {
            //SetName("TG_black");
        }

        public override void Hit(GameObject gameObject)
        {
        }

        //public override void Draw(Renderer renderer)
        //{

        //}
        public void SetName(string name)
        {
            this.name = name;
        }
    }
}
