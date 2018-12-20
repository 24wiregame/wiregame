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
            objectList.Add("1", new Block(Vector2.Zero, gameDevice));
            //バラ（DeathBlock）
            objectList.Add("2", new Bara(Vector2.Zero, gameDevice));
            //クッション（吸収ブロック)
            objectList.Add("3", new Cushion(Vector2.Zero, gameDevice));
            //ジャンプ（ジャンプ台)
            objectList.Add("4", new Jump(Vector2.Zero, gameDevice));
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
            Point work = gameObject.getRectangle().Location;
            int x = work.X / 32;
            int y = work.Y / 32;
            if(x < 1)
            {
                x = 1;
            }
            if(y < 1)
            {
                y = 1;
            }
            Range yRange = new Range(0, mapList.Count() - 1);
            Range xRange = new Range(0, mapList[0].Count() - 1);

            for (int row = y - 1; row <= (y + 1) && yRange.IsWithin(row); row++)
            {
                 xRange = new Range(0, mapList[row].Count() - 1);
                for (int col = x - 1; col <= (x + 1) && xRange.IsWithin(col); col++)
                {
                    if (xRange.IsOutOfRange(col) || yRange.IsOutOfRange(row))
                    {
                        continue;
                    }
                    GameObject obj = mapList[row][col];
                    if(obj is Space)
                    {
                        continue;
                    }
                    if(obj.IsCollision(gameObject))
                    {
                        gameObject.Hit(obj);
                        obj.Hit(gameObject);
                    }
                }
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
    }
}
