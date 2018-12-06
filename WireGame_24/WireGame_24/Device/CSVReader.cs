using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireGame_24.Device
{
    class CSVReader
    {
        private List<string[]> stringData;
        public CSVReader()
        {
            stringData = new List<string[]>();
        }
        public void Clear()
        {
            stringData.Clear();
        }

        public void Read(string filename, string path = "./")
        {
            Clear();
            try
            {
                using (var sr = new System.IO.StreamReader(@"Content/" + path + filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var values = line.Split(',');
                        stringData.Add(values);
#if DEBUG
                        foreach (var v in values)
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
                System.Console.WriteLine(e.Message);
            }
        }
        public List<string[]> GetData()
        {
            return stringData;
        }
        public string[][] GetArrayData()
        {
            int count = stringData.Count;
            string[][] array = new string[count][];
            for (int i = 0; i < count; i++)
            {
                array[i] = stringData[i];
            }
            return array;
        }
        public int[][] GetIntData()
        {
            int count = stringData.Count;
            int[][] array = new int[count][];

            for (int i = 0; i < count; i++)
            {
                int count2 = stringData[i].Length;
                array[i] = new int[count2];
                for (int j = 0; j < count2; j++)
                {
                    array[i][j] = int.Parse(stringData[i][j]);
                }
            }
            return array;
        }
        public String[,] GetStringMatrix()
        {
            var data = GetArrayData();
            int y = data.Count();
            int x = data[0].Count();

            string[,] array = new string[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    array[j, i] = data[j][i];
                }
            }
            return array;
        }

        public int[,] GetIntMatrix()
        {
            var data = GetIntData();
            int y = data.Count();
            int x = data[0].Count();

            int[,] array = new int[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    array[j, i] = data[j][i];
                }
            }
            return array;
        }
    }
}
