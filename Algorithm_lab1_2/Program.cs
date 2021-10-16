using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Algorithm_lab1_2
{
    class Program
    {
        public static string FilePath = @"File.csv";
        public static List<int> List = new List<int>();
        public static Random Rnd = new Random(456);

        static void Main(string[] args)
        {
            //var results = new List<string>();
            //int iterations = 2000;

            //for (int i = 0; i < iterations; i++) //задаем лист элементов
            //    List.Add(Rnd.Next());

            //for (int i = 0; i < iterations; i++)
            //    results.Add(ToCSVString(i, PerformAlgorithm(i)));

            //WriteToCSV(results); //записываем в эксель

            var graph = new Dictionary<char, List<char>>
            {
                ['A'] = new List<char> { 'B', 'C', 'E' },
                ['B'] = new List<char> { 'A', 'C', 'D', 'F' },
                ['C'] = new List<char> { 'A', 'B', 'D', 'F' },
                ['D'] = new List<char> { 'C', 'E', 'B', 'F' },
                ['E'] = new List<char> { 'A', 'D' },
                ['F'] = new List<char> { 'B', 'C', 'D' }
            };

            List<char> clique = FindSingleClique_3(graph);
            foreach (var e in clique)
                Console.WriteLine(e);
        }

        public static string ToCSVString(int i, double time)
        {
            return i.ToString() + "; " + time.ToString();
        }

        public static void WriteToCSV(IEnumerable<string> lines)
        {
            File.WriteAllLines(FilePath, lines);
        }

        static double PerformAlgorithm(int size) //вызов алгоритма и подсчет времени
        {
            var time = new Stopwatch();
            var result = new double[5];

            for (int i = 0; i < 5; i++)
            {
                var number = 12; //6
                var string1 = "поиск"; //5
                var string2 = "писк"; //5
                var l = new List<int> { 9, 6, 2, 3, 7, 5, 8, 4 }; //4

                time.Start();
                //вызываемый метод
                var ft = GetAnswer_4(l);
                var s = LongestCommonSubstring_5(string1, string2);
                var n1 = FindNNumberFibonacci1_6(number);
                var n2 = FindNNumberFibonacci2_6(number);
                time.Stop();

                result[i] = time.ElapsedTicks;
                time.Reset();
            }
            return SelectTime(result);
        }
        static double SelectTime(double[] times) //сортировка выбросов
        {
            Array.Sort(times);
            return (times[times.Length / 2]);
        }

        public static double FindNNumberFibonacci1_6(int number)
        {
            double phi1 = Math.Pow(((1 + Math.Sqrt(5)) * 0.5), number);
            double phi2 = Math.Pow(((1 - Math.Sqrt(5)) * 0.5), number);
            return Math.Round((phi1 - phi2) / Math.Sqrt(5));
        }
        public static int FindNNumberFibonacci2_6(int number)
        {
            int perv = 1;
            int vtor = 1;
            int sum;

            int j = 2;
            while (j <= number)
            {
                sum = perv + vtor;
                perv = vtor;
                vtor = sum;
                j++;
            }
            return perv;
        }

        public static string LongestCommonSubstring_5(string a, string b)
        {
            var n = a.Length;
            var m = b.Length;
            var array = new int[n, m];
            var maxValue = 0;
            var maxI = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (a[i] == b[j])
                    {
                        array[i, j] = (i == 0 || j == 0)
                            ? 1
                            : array[i - 1, j - 1] + 1;
                        if (array[i, j] > maxValue)
                        {
                            maxValue = array[i, j];
                            maxI = i;
                        }
                    }
                }
            }

            return a.Substring(maxI + 1 - maxValue, maxValue);
        }

        public static int GetAnswer_4(List<int> l)
        {
            int n = l.Count;

            var tempL = new List<int>() { 1 };
            FenwickTree.Assign(tempL, n + 1, 0);

            FenwickTree fenwickTree = new FenwickTree(tempL);

            var sorted_arr = GetSortedList(l);

            var dictionary = new Dictionary<int, int>();
            for (int i = 0; i < n; i++)
                dictionary[sorted_arr[i]] = i + 1;

            for (int i = 0; i < n; i++) //Сопоставление данных arr
                l[i] = dictionary[l[i]];

            for (int i = 0; i < n; i++)
            {
                int index = l[i]; //берем ранг элементов как индекс дерева

                int x = DoQuery(fenwickTree, index - 1); //максимальная длина в дереве до этого индекса

                int val = x + 1; //увеличивающаяся длина

                while (index <= n) //от родителя к ребенку
                {
                    fenwickTree.ft[index] = Math.Max(val, fenwickTree.ft[index]); //длина заполнения при соответствующих индексах
                    index += index & (-index); //получение индекса следующего узла в дереве
                }
            }
            return DoQuery(fenwickTree, n);
        }
        public static int DoQuery(FenwickTree tree, int index)
        {
            int ans = 0;

            while (index > 0) //от ребенка к родителю
            {
                ans = Math.Max(tree.ft[index], ans);
                index -= index & (-index); //получение индекса предыдущего узла в дереве
            }
            return ans;
        }
        public static List<int> GetSortedList(List<int> l)
        {
            var sortedList = new List<int>();
            foreach (var e in l)
            {
                sortedList.Add(e);
            }
            sortedList.Sort();
            return sortedList;
        }

        public static List<char> FindSingleClique_3(Dictionary<char, List<char>> graph)
        {
            var clique = new List<char>();
            var vertices = new List<char>();
            foreach (var e in graph)
                vertices.Add(e.Key);

            clique.Add(vertices[vertices.Count-1]);

            foreach (var v in vertices)
            {
                if (clique.Contains(v))
                    continue;
                var isNext = true;
                foreach (var u in clique)
                {
                    if (graph[v].Contains(u))
                        continue;
                    else
                        isNext = false;
                        break;
                }
                if (isNext)
                    clique.Add(v);
            }
            clique.Sort();
            return clique;
        }
    }

        public class FenwickTree
    {
        public List<int> ft = new List<int>();

        public int LSB(int x)
        {
            return x & (-x);
        }

        public int Query(int x)
        {
            int sum = 0;
            while (x > 0)
            {
                sum = sum + ft[x];
                x = x - LSB(x);
            }
            return sum;
        }

        public int Query(int start, int end)
        {
            return Query(end) - Query(start - 1);
        }

        public void Update(int pos, int value)
        {
            while (pos < ft.Count) //array[pos] += value
            {
                ft[pos] += value;
                pos += LSB(pos);
            }
        }

        public FenwickTree(List<int> array)
        {
            int n = array.Count;
            Assign(ft, n + 1, 0); //инициализация ft
            for (int i = 0; i < n; ++i)
                Update(i + 1, array[i]);
        }

        public static void Assign(List<int> l, int n, int m)
        {
            if (!(l is null))
                l.Clear();

            for (int i = 0; i <= n; i++)
                l.Add(m);
        }
    }
}
