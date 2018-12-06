using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using WireGame_24.Scene;

namespace WireGame_24.Actor
{
    class GameObjectManager:IGameObjectMediator
    {
        private List<GameObject> gameObjectList;
        private List<GameObject> addgameObjects;

        //private Map map;

        public GameObjectManager()
        {
            Initialize();
        }
        public void Initialize()
        {
            if(gameObjectList!=null)
            {
                gameObjectList.Clear();
            }
            else
            {
                gameObjectList.Clear();
            }
            
            if(addgameObjects!=null)
            {
                addgameObjects.Clear();
            }
            else
            {
                addgameObjects = new List<GameObject>();
            }
           
        }
        //public void Add(GameObject gameObject)
        //{
        //    if(gameObject==null)
        //    {
        //        return;
        //    }
        //    addgameObjects.Add(gameObject);
        //}
        //public void Add(Map map)
        //{
        //    if(map==null)
        //    {
        //        return;
        //    }
        //    this.map = map;
        //}

        //private void hitToMap()
        //{
        //    if(map==null)
        //    {
        //        return;
        //    }
        //    foreach(var obj in gameObjectList)
        //    {
        //        map.Hit(obj);
        //    }
        //}
        private void hitToGameObject()
        {
            foreach(var c1 in gameObjectList)
            {
                foreach(var c2 in gameObjectList)
                {
                    if(c1.Equals(c2)||c1.IsDead()||c2.IsDead())
                    {
                        continue;
                    }

                    
                }
            }
        }
        private void removeDeadCharacters()
        {
            gameObjectList.RemoveAll(c => c.IsDead());
        }
        public void Update(GameTime gameTime)
        {
            foreach(var c in gameObjectList)
            {
                c.Update(gameTime);
            }
            foreach(var c in addgameObjects)
            {
                gameObjectList.Add(c);
            }
            addgameObjects.Clear();

            //hitToMap();
            hitToGameObject();

            removeDeadCharacters();
        }

        public void AddGameObject(GameObject gameObject)
        {
            if(gameObject==null)
            {
                return;
            }
            addgameObjects.Add(gameObject);
        }
        public GameObject GetPlayer()
        {
            GameObject find = gameObjectList.Find(c => c is Player);
            if(find!=null &&find.IsDead())
            {
                return find;
            }
            return null;
        }
        public bool IsPlayerDead()
        {
            GameObject find = gameObjectList.Find(c => c is Player);
            return (find == null || find.IsDead());
        }

    }
}
