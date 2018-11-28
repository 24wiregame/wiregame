using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGame_24
{

    class CSVReader
    {
        private List<string[]> stringData;

        //コンストラクタ
        public CSVReader()
        {


        }
        public void Clear()
        {
            this.stringData = null;
        }
        public List<string[]> GetData()
        {
            return stringData;
        }
        public string[][] GetArrayData()
        {
            return stringData.ToArray();
        }
        public string[][] GetArrayDate()
        {
            return stringData.ToArray();
        }
        public int[][] GetIntdata()
        {
            //ジャグ配列を取得し、空のint型の二次元配列を取得
            var data = GetArrayData();
            int row = data.Count();// 行の数の取得
            int[][] intData = new int[row][];//行列の数文配列を生成
            for (int i = 0; i < row; i++)
            {
                int col = data[i].Count();//列数の取得
            }
            //整数に変換しコピーする
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < intData[y].Count(); x++)
                {
                    intData[y][x] = int.Parse(data[y][x]);
                }
            }
            return intData;
        }
        public string[,] GetStringMatrix()
        {
            var data = GetArrayData();
            int row = data.Count();//行の取得
            int col = data[0].Count();//行の数がどの行でも同じとし、数を取得

            string[,] result = new string[row, col];//多次元配列を生成
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    result[y, x] = data[y][x];
                }

            }
            return result;

        }
        public int[,] GetIntMatrix()
        {
            var data = GetIntdata();
            int row = data.Count();
            int col = data[0].Count();

            int[,] result = new int[row, col];//多次元配列を生成
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    result[y, x] = data[y][x];
                }
            }
            return result;
        }
        //CSVファイルの読み込み
        public void Read(string filename, string path = "./")
        {
            //例外処理
            try
            {
                //csvファイルを開く
                using (var sr = new System.IO.StreamReader(@"Content/" + path + filename))
                {
                    //ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        //1行読み込む
                        var line = sr.ReadLine();
                        //カンマごとに分けて配列に格納する

                        var valuses = line.Split(',');//文字のカンマ
                                                      //リストに読み込んだ1行を追加
#if DEBUG
                        //出力する
                        foreach (var v in valuses)
                        {
                            System.Console.Write("{0}", v);
                        }
                        System.Console.WriteLine();
#endif
                    }
                }
            }

            catch (System.Exception e)
            {
                //ファイルオープンが失敗したとき
                System.Console.WriteLine(e.Message);
            }


        }
    }
}
        

