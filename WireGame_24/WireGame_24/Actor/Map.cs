using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireGame_24.Device;
using WireGame_24.Util;
using Microsoft.Xna.Framework;

namespace WireGame_24.Actor
{
    class Map
    {
        private List<List<GameObject>> mapList;
        private List<TarGetBlock> tarGetBlocks;
        private GameDevice gameDevice;

        public Map(GameDevice gameDevice)
        {
            mapList = new List<List<GameObject>>();
            tarGetBlocks = new List<TarGetBlock>();
            this.gameDevice = gameDevice;
        }

        private List<GameObject>addBlock(int lineCnt , string[]line)
        {
            Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();
            //スペース
            objectList.Add("0", new Space(Vector2.Zero, gameDevice));
      　　　//ブロック
            objectList.Add("1", new Block("SB_5",Vector2.Zero, gameDevice));
            objectList.Add("14", new Block("SB_6", Vector2.Zero, gameDevice));
            objectList.Add("15", new Block("SB_3", Vector2.Zero, gameDevice));
            objectList.Add("16", new Block("SB_4", Vector2.Zero, gameDevice));
            objectList.Add("19", new Block("SB_6", Vector2.Zero, gameDevice));
            objectList.Add("20", new Block("JB2", Vector2.Zero, gameDevice));
            //バラ（DeathBlock）
            objectList.Add("2", new Bara("DeathBall",Vector2.Zero, gameDevice));
            objectList.Add("7", new Bara("DeathBall", Vector2.Zero, gameDevice));
            //objectList.Add("12", new Bara("DB_3", Vector2.Zero, gameDevice));
            //objectList.Add("13", new Bara("DB_4", Vector2.Zero, gameDevice));
            //クッション（吸収ブロック)
            objectList.Add("3", new Cushion("CB_1",Vector2.Zero, gameDevice));
            objectList.Add("8", new Cushion("CB_2", Vector2.Zero, gameDevice));
            objectList.Add("9", new Cushion("CB_3", Vector2.Zero, gameDevice));
            objectList.Add("10", new Cushion("CB_4", Vector2.Zero, gameDevice));
            //ジャンプ（ジャンプ台)
            objectList.Add("4", new Jump2("JB7",Vector2.Zero, gameDevice));
            objectList.Add("11", new Jump2("JB2", Vector2.Zero, gameDevice));
            objectList.Add("17", new Jump("JB_5", Vector2.Zero, gameDevice));
            //ターゲットブロック
            objectList.Add("5", new TarGetBlock(Vector2.Zero, gameDevice));
            //ゴール
            objectList.Add("6", new Goal(Vector2.Zero, gameDevice));

            List<GameObject> workList = new List<GameObject>();
            int colCnt = 0;
            foreach(var s in line)
            {
                try
                {
                    GameObject work = (GameObject)objectList[s].Clone();
                    if(work is TarGetBlock)
                    {
                        tarGetBlocks.Add(work as TarGetBlock);
                    }

                    work.SetPosition(new Vector2(colCnt * work.GetHeight(),
                        lineCnt * work.GetWidht()));
                    workList.Add(work);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                colCnt += 1;

            }
            return workList;
        }
        public void Load(string filename, string path = "./")
        {
            CSVReader csvReader = new CSVReader();
            csvReader.Read(filename, path);
            var data = csvReader.GetData();
            for(int lineCnt = 0; lineCnt < data.Count(); lineCnt++)
            {
                mapList.Add(addBlock(lineCnt, data[lineCnt]));
            }
        }
        public void Unload()
        {
            mapList.Clear();
        }
        public void Update(GameTime gameTime)
        {
            foreach (var list in mapList)
            {
                foreach(var obj in list)
                {
                    if(obj is Space)
                    {
                        continue;
                    }
                    obj.Update(gameTime);
                }
                
            }
            
        }
        public void Hit(GameObject gameObject)
        {
            //もしgameObjectが死んでいたらマップの判定を無視する
            if (gameObject.IsDead())
            {
                return;
            }
            //GameObjectのPositionをもとにした矩形の左上のPointを取得
            Point work = gameObject.getRectangle().Location;
            //左上のPointがあるマップの添え字となるx,yを取得
            int x = work.X / 32;
            int y = work.Y / 32;
            //if(x < 1)
            //{
            //    x = 1;
            //}
            //if(y < 1)
            //{
            //    y = 1;
            //}
            x = Math.Max(x, 1);
            y = Math.Max(y, 1);

            Range yRange = new Range(0, mapList.Count() - 1);
            Range xRange = new Range(0, mapList[0].Count() - 1);
            if (gameObject.GetHeight() == 32 &&
                gameObject.GetWidht()  == 32)
            {
                CallHit32x32(x, y, xRange, yRange, gameObject);
                return;
            }

            if (gameObject.GetHeight() == 64 &&
                gameObject.GetWidht()  == 64)
            {
                CallHit64x64(x, y, xRange, yRange, gameObject);
                return;
            }
        }
        public void Draw(Renderer renderer)
        {
            foreach (var list in mapList)
            {
                foreach (var obj in list)
                {
                    if(obj is Space)
                    {
                        continue;
                    }
                    obj.Draw(renderer);
                }
            }
        }




        public TarGetBlock GetNearTarget(Vector2 position)
        {
            float distance = float.MaxValue;
            TarGetBlock nearTarget = null;
            foreach (var tl in tarGetBlocks)
            {
                float length = (position - tl.GetPosition()).LengthSquared();
            
               
                if(distance > length)
                {
                    distance = length;
                    nearTarget = tl;
                }
                
            }
            
            return nearTarget;
        }

        /// <summary>
        /// 32x32の時に呼ぶヒット処理
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xRange"></param>
        /// <param name="yRange"></param>
        /// <param name="gameObject"></param>
        private void CallHit32x32(int x, int y, Range xRange, Range yRange, GameObject gameObject)
        {
            //row=行,col=列
            //(y-1 <= row <= y+1)
            for (int row = y - 1; row <= (y + 1) && yRange.IsWithin(row); row++)
            {
                //念のため,xRangeを更新
                xRange = new Range(0, mapList[row].Count() - 1);
                for (int col = x - 1; col <= (x + 1) && xRange.IsWithin(col); col++)
                {
                    //(x-1 <= col <= x+1)
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                    {
                        continue;
                    }
                    //mapのgameObjectを取得
                    GameObject obj = mapList[row][col];
                    //添え字の値が範囲外だったらcontinue
                    if (xRange.IsOutOfRange(col) || xRange.IsOutOfRange(row)) continue;
                    //spaceだったらcontinue
                    if (obj is Space) continue;

                    //当たっているか判定
                    if (obj.IsCollision(gameObject))
                    {
                        //当たっていたのでHitメソッドを呼ぶ
                        gameObject.Hit(obj);
                        obj.Hit(gameObject);
                    }
                }
            }
        }

        //64x64の時に呼ぶヒット処理
        private void CallHit64x64(int x, int y, Range xRange, Range yRange, GameObject gameObject)
        {
            //row=行,col=列
            //(y-1 <= row <= y+1)
            for (int row = y - 2; row <= (y + 2) /*&& yRange.IsWithin(row)*/; row += 2)
            {
                if (yRange.IsOutOfRange(row))
                {
                    continue;
                }
                //念のため,xRangeを更新
                xRange = new Range(0, mapList[row].Count() - 2);
                for (int col = x - 2; col <= (x + 2)/* && xRange.IsWithin(col)*/; col +=2)
                {
                    //(x-1 <= col <= x+1)
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                    {
                        continue;
                    }
                    //mapのgameObjectを取得
                    GameObject obj = mapList[row][col];
                    //添え字の値が範囲外だったらcontinue
                    if (xRange.IsOutOfRange(col) || xRange.IsOutOfRange(row)) continue;
                    //spaceだったらcontinue
                    if (obj is Space) continue;

                    //当たっているか判定
                    if (obj.IsCollision(gameObject))
                    {
                        //当たっていたのでHitメソッドを呼ぶ
                        gameObject.Hit(obj);
                        obj.Hit(gameObject);
                    }
                }
            }
        }

    }
}
