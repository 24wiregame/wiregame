using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGame_24.Util
{
    interface ScoreMediator
    {
        void ADDScore();//得点を追加
        void AddScore(int num);//指定された得点を追加
    }
}
