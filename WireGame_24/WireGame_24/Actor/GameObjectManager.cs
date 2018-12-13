using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireGame_24.Device;
using WireGame_24.Scene;
using Microsoft.Xna.Framework;
namespace WireGame_24.Actor
{
    class GameObjectManager : IGameObjectMediator
    {
        private List<GameObject> gameObjectList;
        private List<GameObject> addGameObjects;

        private Map map;

        public GameObjectManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (gameObjectList != null)
            {
                gameObjectList.Clear();
            }
            else
            {
                gameObjectList = new List<GameObject>();
            }

            if (addGameObjects != null)
            {
                addGameObjects.Clear();
            }
            else
            {
                addGameObjects = new List<GameObject>();
            }
        }

        public void Add(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }

        public void Add(Map map)
        {
            if (map == null)
            {
                return;
            }
            this.map = map;
        }
        private void hitToMap()
        {
            if (map == null) return;

            foreach (var obj in gameObjectList)
            {
                if (obj.IsDead()) continue;
            }
        }
        private void hitToGameObject()
        {
            foreach (var c1 in gameObjectList)
            {
                foreach (var c2 in gameObjectList)
                {
                    if (c1.Equals(c2) || c1.IsDead() || c2.IsDead())
                    {
                        continue;
                    }

                    if (c1.IsCollision(c2))
                    {
                        c1.Hit(c2);
                        c2.Hit(c1);
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
            foreach (var c in gameObjectList)
            {
                c.Update(gameTime);
                addGameObjects.Clear();
            }

            foreach (var c in addGameObjects)
            {
                gameObjectList.Add(c);
            }
            addGameObjects.Clear();

            hitToMap();
            hitToGameObject();

            removeDeadCharacters();
        }

        public void Draw(Renderer renderer)
        {
            foreach (var c in gameObjectList)
            {
                c.Draw(renderer);
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }

        public GameObject GetPlayer()
        {
            GameObject find = gameObjectList.Find(c => c is Player);
            if (find != null && find.IsDead())
            {
                return find;
            }
            return null;
        }
        //public bool IsPlayerDead()
        //{
        //    GameObject find = gameObjectList.Find(c => c is Player);

        //    return (find == null || find.IsDead());
        //}
    }
}

