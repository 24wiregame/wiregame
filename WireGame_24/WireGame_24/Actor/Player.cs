using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WireGame_24.Device;
using Microsoft.Xna.Framework.Input;
using WireGame_24.Def;
using WireGame_24.Scene;
using WireGame_24.Actor.Interface;
namespace WireGame_24.Actor
{
    delegate void OnHit();

    class Player : GameObject, IApplicableDead
    {
        private IGameObjectMediator mediator;
        private Vector2 v;
        private Vector2 velocity;
        private bool isJump;
        private bool isfall;
        private Vector2 originVelocty;
        private float gravity;
        private Wire wire;
        private bool isGoalFlag;

        private Sound sound;

        /// <summary>
        /// 当たった時に行うイベント
        /// </summary>
        public OnHit OnHitEvent { get; set; }

        public Player(Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator, Wire wire)
            : base("IMG_0921", position, 64, 64, gameDevice)
        {
            velocity = Vector2.Zero;
            isJump = true;
            this.mediator = mediator;
            this.wire = wire;
            sound = gameDevice.GetSound();
        }
        public Player(Player other)
            : this(other.position, other.gameDevice, other.mediator,other.wire)
        {

        }
        public override object Clone()
        {
            return new Player(this);
        }
        public override void Hit(GameObject gameObject)
        {
            if (gameObject is Bara)
            {
                hitBlock(gameObject);
            }
            if (gameObject is Block )
            {
                hitBlock(gameObject);
            }
            if (gameObject is Cushion)
            {
                hitBlock(gameObject);
            }
            OnHitEvent.Invoke();
            if (gameObject is Goal)
            {
                isGoalFlag = true;
            }
            
        }


        public override void Update(GameTime gameTime)
        {
            if (position.Y > Screen.Height)
            {
                isDeadFlag = true;
                isfall = true;
            }

            if((isJump == false)&&
                (Input.GetKeyTrigger(Keys.Space)||
                Input.GetKeyTrigger(PlayerIndex.One, Buttons.B)))
            {
                sound.PlaySE("jump");
                JumpStart();
            }
            float speed = 4.0f;
            
            if (!wire.IsUse())
            {
                UseGravity();
                wire.SetTarget(((GameObjectManager)mediator).GetNearTarGet(position));
            }
            //UpdateWireVelocity();
            // float inputVelocity = Input.Velocity().X;
            //velocity.X = Input.Velocity().X * speed +
            // Input.Velocity(PlayerIndex.One).X * speed;
            position = position+velocity;
            Console.WriteLine("Velocity:"+velocity);
            
            //プレイヤーの位置を画面の中心に位置補正する
            setDisplayModify();
            
        }
        private void UseGravity()
        {
            velocity.Y = velocity.Y + 0.4f;
        }

        private void JumpStart()
        {
            velocity.Y = -8.0f;
            isJump = true;
        }

        private void UpdateWireVelocity()
        {
            if (originVelocty.LengthSquared() <= 0.1f)
            {
               originVelocty = Vector2.Zero;
            }
            //ワイヤーを離した時の反動(調節可)
            velocity += originVelocty;
            originVelocty.X *= 0.99f;
            originVelocty.Y *= 0.05f;
            
            // velocity = wire.GetVelocity() * new Vector2(0.99f, 0.99f);
            Console.WriteLine("ああああああああああああああ" + velocity);

        }

        private void hitBlock(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);
            if(dir == Direction.Top)
            {
                if(position.Y > 0.0f)
                {
                    position.Y = gameObject.getRectangle().Top - this.height;
                    velocity.Y = 0.0f;
                    isJump = false;
                    sound.PlaySE("jump");
                }
                    Console.WriteLine("HitTop");
            }
            else if(dir == Direction.Right)
            {
                position.X = gameObject.getRectangle().Right;
                Console.WriteLine("HitRight");
            }
            else if(dir == Direction.Left)
            {
                position.X = gameObject.getRectangle().Left - this.width;
                Console.WriteLine("HitLeft");
            }
            else if(dir == Direction.Bottom)
            {
                position.Y = gameObject.getRectangle().Bottom;
                if(isJump)
                {
                    velocity.Y = 0.0f;
                    sound.PlaySE("jump");
                }
                Console.WriteLine("HitBottom");

            }
            setDisplayModify();
        }
        private void setDisplayModify()
        {
            gameDevice.SetDisplayModify(new Vector2(-position.X + (Screen.Width / 2 -
                width / 2), 0.0f));
            if(position.X < (Screen.Width / 2-width / 2) )
            {
                gameDevice.SetDisplayModify(Vector2.Zero);
            }
        }
        private void setSlideModify(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);

        }
        //死んだときの動きの大きさ

        public void Die()
        {
            isDeadFlag = true;
            velocity = new Vector2(0,-12);
        }
        public bool Isfall()
        {
            return isfall;
        }
        public void SetVelocity(Vector2 velocity)
        {
            if (IsDead()) { return; }
            this.velocity = velocity;
        }

        public void Initialize()
        {
            gravity = 0.5f;
            isJump = false;
            originVelocty = Vector2.Zero;
        }
        public void SetPositionX(float positionX)
        {
            position.X = positionX;
        }
        public void SetPositionY(float positionY)
        {
            position.Y = positionY;
        }
        public Vector2 GetVeloity()
        {
            return velocity;
        }
        public void SetJump(bool flg)
        {
            isJump = flg;
        }
        public void SetSpeed(Vector2 velocity)
        {
            originVelocty = velocity;
        }
        public Vector2 GetOriginVl()
        {
            return originVelocty;
        }
        public bool IsGoalFlag()
        {
            return isGoalFlag;
        }
        
    }
}
