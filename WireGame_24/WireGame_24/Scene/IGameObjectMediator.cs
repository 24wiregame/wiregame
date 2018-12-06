using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireGame_24.Actor;

namespace WireGame_24.Scene
{
    interface IGameObjectMediator
    {
        void AddGameObject(GameObject gameObject);
        GameObject GetPlayer();
        bool IsPlayerDead();
    }
}
