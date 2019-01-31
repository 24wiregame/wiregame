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
using WireGame_24.Util;
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
        private Motion motion;
        private Sound sound;

        private PlayerIndex index;

        /// <summary>
        /// 当たった時に行うイベント
        /// </summary>
        public OnHit OnHitEvent { get; set; }

        public Player(Vector2 position, GameDevice gameDevice, IGameObjectMediator mediator, Wire wire, PlayerIndex index)
            : base("renban_1", position, 64, 64, gameDevice)
        {
            this.index = index;
            velocity = Vector2.Zero;
            isJump = true;
            this.mediator = mediator;
            this.wire = wire;
            sound = gameDevice.GetSound();

            motion = new Motion();
            for (int i = 0; i < 16; i++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            }
            //最初はすべてのパーツ表示に設定
            motion.Initialize(new Range(0, 14), new CountDownTimer(0.05f));
        }
        public Player(Player other)
            : this(other.position, other.gameDevice, other.mediator,other.wire, other.index)
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
        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer"></param>
        public override void Draw(Renderer renderer)
        {
            
            if (!wire.IsUse())
            {
                if(!IsDead())
                {
                    renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawingRange(),Color.Red);
                }
               
            }
            else
            {
                renderer.DrawTexture("swing_1", position + gameDevice.GetDisplayModify());
            }
            if(IsDead())
            {
                renderer.DrawTexture("death", position + gameDevice.GetDisplayModify());
            }

        }


        public override void Update(GameTime gameTime)
        {
            
            if (position.Y > Screen.Height)
            {
                isDeadFlag = true;
                isfall = true;
            }
            motion.Update(gameTime);
            if ((isJump == false)&&
                IsJumpKeyPressed())
            {
                JumpStart();
            }
            float speed = 4.0f;
            
            if (!wire.IsUse())
            {
                UseGravity();
                wire.SetTarget(((GameObjectManager)mediator).GetNearTarGet(position));
            }
            
            position = position+velocity;
            
            //プレイヤーの位置を画面の中心に位置補正する
           // setDisplayModify();
            
        }
        private void UseGravity()
        {
            velocity.Y = velocity.Y + 0.4f;
        }

        private void JumpStart()
        {
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
        }

        private void hitBlock(GameObject gameObject)
        {
            Direction dir = this.CheckDirection(gameObject);
            if(dir == Direction.Top)
            {
                if(position.Y > 0.0f)
                {
                    position.Y = gameObject.getRectangle().Top - this.height;
                    //velocity.Y = 0.0f;
                    isJump = false;
                    sound.PlaySE("jump");
                }
            }
            else if(dir == Direction.Right)
            {
                position.X = gameObject.getRectangle().Right;
            }
            else if(dir == Direction.Left)
            {
                position.X = gameObject.getRectangle().Left - this.width;
            }
            else if(dir == Direction.Bottom)
            {
                position.Y = gameObject.getRectangle().Bottom;
                if(isJump)
                {
                    velocity.Y = 0.0f;
                }

            }
          //  setDisplayModify();
        }
        public void setDisplayModify()
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

        /// <summary>
        /// 衝突判定（2点間の距離と円の半径） Playerだけ64*64にする
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool InCollision(GameObject otherObj)
        {
            //じぶんと相手の位置の長さを計算（2点間の距離）
            float length = ((position + new Vector2(32, 32)) - (otherObj.GetPosition() + new Vector2(16, 16))).Length();
            //白玉画像のサイズは64なので、半径は32
            float radiusSum = 32f + 32f;
            //自分半径の和と距離を比べて、等しいかまたは小さいか（以下か）
            if (length <= radiusSum)
            {
                return true;
            }
            return false;
        }

        private bool IsJumpKeyPressed()
        {
            //コントローラー判定
            if (Input.GetKeyTrigger(index, Buttons.B))
            {
                return true;
            }
            switch (index)
            {
                case PlayerIndex.One:
                    return Input.GetKeyTrigger(Keys.Space);
                case PlayerIndex.Two:
                    return Input.GetKeyTrigger(Keys.Tab);
            }
            return false;
        }

        public PlayerIndex GetIndex()
        {
            return index;
        }

    }
}
