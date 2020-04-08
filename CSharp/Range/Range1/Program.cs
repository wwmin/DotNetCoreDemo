using System;
using System.Collections.Generic;
using System.Linq;

namespace Range1
{
    //以下 .NET 类型同时支持索引和范围：String、Span<T> 和 ReadOnlySpan<T>。 List<T> 支持索引，但不支持范围
    //Array 具有更多的微妙行为。 单个维度数组同时支持索引和范围。 多维数组则不支持。 多维数组的索引器具有多个参数，而不是一个参数。 交错数组（也称为数组的数组）同时支持范围和索引器。
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Range!");
            //TestSpan();
            //TestRange();
            TestRange2();
            //TestRange3();
            Console.ReadKey();
        }
        #region Test Span
        static void TestSpan()
        {
            //var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var arr = Enumerable.Range(1, 10).ToArray();
            Span<int> slice = arr.AsSpan().Slice(2, 4);
            Console.WriteLine(string.Join(",", slice.ToArray()));
            //foreach (var number in slice)
            //{
            //    Console.WriteLine(number);
            //}
        }
        #endregion
        #region Test Range
        static void TestRange()
        {
            //int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
            int[] someArray = Enumerable.Range(1, 5).ToArray();
            int[] subArray1 = someArray[0..2];//{1,2}
            Console.WriteLine(string.Join(",", subArray1));
            int[] subArray2 = someArray[1..^0];//{2,3,4,5}
            Console.WriteLine(string.Join(",", subArray2));
            int[] subArray3 = someArray[1..];
            Console.WriteLine(string.Join(",", subArray3));

            List<int> otherArray = new List<int> { 1, 2, 3, 4, 5 };
            int[] other1 = otherArray.ToArray()[1..2];

            //创建Range对象
            //var arr = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var arr = Enumerable.Range(1, 10).ToArray();
            Index middle = 4;
            Index threeFromEnd = ^3;//Hat运算符^
            Range range = middle..threeFromEnd;
            int[] slice = arr[range];
            Console.WriteLine(string.Join(",", slice));

            //使用Index类型和^操作符
            Index index = ^2;
            var number = arr[index];
            Console.WriteLine(number);//9

            //使用Range和Index
            var arrTwin = arr[0..^0];//arrTwin是arr的完成拷贝
            foreach (var i in arrTwin)
            {
                Console.WriteLine(i);
            }

            var arrTwin2 = arr[..];//arr[0..^0];的简化版, 还等同于 arr[0..10];
            foreach (var i in arrTwin2)
            {
                Console.WriteLine(i);
            }

            int[] arrTwin3 = arr[..];//完成的复制,数组
            int[] allButFirst = arr[1..];//不包含第一个元素的数组
            int[] empty = arr[^0..];//空数组
            int[] onlyLastItem = arr[^1..];//只包含最后一个元素的数组
            int[] last4Items = arr[^4..];//包含四个元素的数组
            int lastItem = arr[^1];//最后一个元素,类型是int

        }
        #endregion
        #region Test Range2
        static void TestRange2()
        {
            var jagged = new int[10][]{
        new int[10] { 0,  1, 2, 3, 4, 5, 6, 7, 8, 9},
        new int[10] { 10,11,12,13,14,15,16,17,18,19},
        new int[10] { 20,21,22,23,24,25,26,27,28,29},
        new int[10] { 30,31,32,33,34,35,36,37,38,39},
        new int[10] { 40,41,42,43,44,45,46,47,48,49},
        new int[10] { 50,51,52,53,54,55,56,57,58,59},
        new int[10] { 60,61,62,63,64,65,66,67,68,69},
        new int[10] { 70,71,72,73,74,75,76,77,78,79},
        new int[10] { 80,81,82,83,84,85,86,87,88,89},
        new int[10] { 90,91,92,93,94,95,96,97,98,99},
    };
            var selectedRows = jagged[3..^3];
            foreach (var row in selectedRows)
            {
                var selectedColumns = row[2..^2];
                foreach (var cell in selectedColumns)
                {
                    Console.WriteLine($"{cell}, ");
                }
                Console.WriteLine();
            }
        }
        #endregion
        #region Test Range3
        static void TestRange3()
        {
            (int min, int max, double average) MovingAverage(int[] subSequence, Range range) => (
                subSequence[range].Min(),
                subSequence[range].Max(),
                subSequence[range].Average()
                );
            int[] Sequence(int count) => Enumerable.Range(0, count).Select(x => (int)(Math.Sqrt(x) * 100)).ToArray();
            int[] sequence = Sequence(1000);
            for (int start = 0; start < sequence.Length; start += 100)
            {
                Range r = start..(start + 10);
                var (min, max, average) = MovingAverage(sequence, r);
                Console.WriteLine($"From {r.Start} to {r.End}: \tMin: {min},\t{max},\tAverage: {average}");
            }

            for (int start = 0; start < sequence.Length; start += 100)
            {
                Range r = ^(start + 10)..^start;
                var (min, max, average) = MovingAverage(sequence, r);
                Console.WriteLine($"From {r.Start} to {r.End}:  \tMin: {min},\tMax: {max},\tAverage: {average}");
            }
        }
        #endregion
    }
}
