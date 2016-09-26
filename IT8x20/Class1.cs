using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Sortings
{


    public static void Main()
    {
        //define variables
        DateTime StartTime, EndTime;
        int Num = 1000000;
        int Times = 100;
        string Lanugage = "C#";
        string ResultPath = @"C:\Users\21604419\Documents\IT8x20\A1\Code\Data\Result.csv";

        double[] TimeCollectionQuickSort = new double[Times];
        double[] TimeCollectionMergeSort = new double[Times];
        double[] TimeCollectionHeapSort = new double[Times];

        Sortings MyClass = new Sortings();

        int[] refArray = new int[Num];
        int[] listArray = new int[Num];
        List<int> RefList = new List<int>();

        //the csv filename
        string csvFile = @"C:\Users\21604419\Documents\IT8x20\A1\Code\Data\random" + Times.ToString() + "_" + Num.ToString() + ".csv";

        //if file doesn't exist, run random generator and create csv
        if (!File.Exists(csvFile))
        {
            List<int> RandomNumbers = RandomList(Num);
            refArray = RandomNumbers.ToArray();
            RefList = RandomNumbers;
            SaveArrayAsCSV(refArray, csvFile);
        }
        else
        {
            //if file exists, read csv
            string str = File.ReadAllText(@"C:\Users\21604419\Documents\IT8x20\A1\Code\Data\random" + Times.ToString() + "_" + Num.ToString() + ".csv").Replace("\r", "");
            string[] s = str.Split('\n');

            foreach (String strPart in s)
            {
                int i;
                if (Int32.TryParse(strPart, out i))
                    RefList.Add(i);
            }

            refArray = RefList.ToArray<int>();

        }

        Console.WriteLine("=========QuickSort=========");
        for (int i = 0; i < Times; i++)
        {
            List<Int32> list = new List<Int32>(RefList);
            StartTime = DateTime.Now;
            MyQuickSort(list, 0, list.Count);
            EndTime = DateTime.Now;
            TimeCollectionQuickSort[i] = (EndTime - StartTime).TotalMilliseconds;
        }

        MyClass.MathCalculate(TimeCollectionQuickSort, Num.ToString(), Times.ToString(), Lanugage, "QuickSort",ResultPath);

        //=============MergeSort====================
        Console.WriteLine("=====MergeSort=====");
        for (int i = 0; i < Times; i++)
        {
            int[] list = new int[Num];
            Array.Copy(refArray, list, Num);

            StartTime = DateTime.Now;
            MergeSort(list, 0, Num - 1);
            EndTime = DateTime.Now;
            TimeCollectionMergeSort[i] = (EndTime - StartTime).TotalMilliseconds;

        }
        MyClass.MathCalculate(TimeCollectionMergeSort, Num.ToString(), Times.ToString(), Lanugage, "MergeSort", ResultPath);

        //=============Insertion====================
        Console.WriteLine("=====HeapSort=====");
        for (int i = 0; i < Times; i++)
        {
            int[] list = new int[Num];
            Array.Copy(refArray, list, Num);

            StartTime = DateTime.Now;
            HeapSort(list);
            EndTime = DateTime.Now;
            TimeCollectionHeapSort[i] = (EndTime - StartTime).TotalMilliseconds;
            //Console.WriteLine(TimeCollectionMergeSort[i].ToString());
        }
        MyClass.MathCalculate(TimeCollectionHeapSort, Num.ToString(), Times.ToString(), Lanugage, "HeapSort", ResultPath);
        Console.Read();
    }



    static List<int> RandomList(int size)
    {
        List<int> ret = new List<int>(size);
        Random rand = new Random(1);
        for (int i = 0; i < size; i++)
        {

            ret.Add(rand.Next(size));
        }
        return ret;
    }

    static int MyPartition(List<int> list, int left, int right)
    {
        int start = left;
        int pivot = list[start];

        left++;

        right--;

        while (true)
        {
            while (left <= right && list[left] <= pivot)

                left++;

            while (left <= right && list[right] > pivot)

                right--;

            if (left > right)
            {
                list[start] = list[left - 1];
                list[left - 1] = pivot;

                return left;
            }


            int temp = list[left];
            list[left] = list[right];
            list[right] = temp;

        }
    }

    static void MyQuickSort(List<int> list, int left, int right)
    {
        if (list == null || list.Count <= 1)
            return;

        if (left < right)
        {
            int pivotIdx = MyPartition(list, left, right);
            //Console.WriteLine("MQS " + left + " " + right);
            //DumpList(list);
            MyQuickSort(list, left, pivotIdx - 1);
            MyQuickSort(list, pivotIdx, right);
        }
    }

    static void DumpList(List<int> list)
    {
        list.ForEach(delegate (int val)
        {
            Console.Write(val);
            Console.Write(", ");
        });

    }



    public static void MergeSort(int[] input, int left, int right)
    {
        if (left < right)
        {
            int middle = (left + right) / 2;

            MergeSort(input, left, middle);
            MergeSort(input, middle + 1, right);

            //Merge
            int[] leftArray = new int[middle - left + 1];
            int[] rightArray = new int[right - middle];

            Array.Copy(input, left, leftArray, 0, middle - left + 1);
            Array.Copy(input, middle + 1, rightArray, 0, right - middle);

            int i = 0;
            int j = 0;
            for (int k = left; k < right + 1; k++)
            {
                if (i == leftArray.Length)
                {
                    input[k] = rightArray[j];
                    j++;
                }
                else if (j == rightArray.Length)
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else if (leftArray[i] <= rightArray[j])
                {
                    input[k] = leftArray[i];
                    i++;
                }
                else
                {
                    input[k] = rightArray[j];
                    j++;
                }
            }
        }
    }
    /// HeapSort sort

    public static void HeapSort(int[] input)
    {
        //Build-Max-Heap
        int heapSize = input.Length;
        for (int p = (heapSize - 1) / 2; p >= 0; p--)
        {
            MaxHeapify(input, heapSize, p);
        }

        for (int i = input.Length - 1; i > 0; i--)
        {
            //Swap
            //Console.WriteLine("HeapSort {0} : {1} ", input[i], input[0]);
            int temp = input[i];
            input[i] = input[0];
            input[0] = temp;

            heapSize--;
            //Console.WriteLine(" HeapSort heapSize  :" + heapSize);
            MaxHeapify(input, heapSize, 0);
        }
    }
    private static void MaxHeapify(int[] input, int heapSize, int index)
    {
        int left = (index + 1) * 2 - 1;
        int right = (index + 1) * 2;
        int largest = index;

        if (left < heapSize && input[left] > input[index])
        {
            //Console.WriteLine(" Condition 1 :  largest : left =  " + largest + ":" + left);
            largest = left;
        }
        else
        {
            //Console.WriteLine(" Condition 2 :   largest : index =  " + largest + ":" + index);
            largest = index;
        }
        if (right < heapSize && input[right] > input[largest])
        {
            //Console.WriteLine(" Conditon 3 :  largest : right " + largest + ":" + right);
            largest = right;
        }
        if (largest != index)
        {
            //Console.WriteLine("MaxHeapify {0} : {1} ", input[index], input[largest]);
            int temp = input[index];
            input[index] = input[largest];
            input[largest] = temp;
            //Console.WriteLine(" MaxHeapify Heap Size  :" + heapSize);
            MaxHeapify(input, heapSize, largest);
        }
    }



    public void MathCalculate(double[] results, string size, string num, string language, string sortname, string path)
    {
        Console.WriteLine("Mean: " + Mean(results));
        Console.WriteLine("Variance: " + Variance(results));
        Console.WriteLine("Standard Deviation: " + StDev(results));

        string[] result = new string[7];
        result[0] = size;
        result[1] = num;
        result[2] = language;
        result[3] = sortname;
        result[4] = Mean(results).ToString();
        result[5] = Variance(results).ToString();
        result[6] = StDev(results).ToString();

        WriteResultsToFile(result, path);
    }

    /// <summary>
    /// Calculates the mean of an array of values
    /// </summary>
    /// <param name="v">the array of values to calculate their mean</param>
    /// <returns>The mean of the array of values</returns>
    public static double Mean(double[] v)
    {
        double sum = 0.0;

        for (int i = 0; i < v.Length; i++)
        {
            sum += v[i];
        }

        return sum / v.Length;
    }

    /// <summary>
    /// Calculates the variance of an array of values
    /// </summary>
    /// <param name="v">the array of values to calculate their variance</param>
    /// <returns>The variance of the array of values</returns>
    public static double Variance(double[] v)
    {
        double mean = Mean(v);
        double sum = 0.0;

        for (int i = 0; i < v.Length; i++)
        {
            sum += (v[i] - mean) * (v[i] - mean);
        }

        int denom = v.Length - 1;
        if (v.Length <= 1)
            denom = v.Length;

        return sum / denom;
    }

    /// <summary>
    /// Calculates the standard deviation of an array of values
    /// </summary>
    /// <param name="v">the array of values to calculate their standard deviation</param>
    /// <returns>The standard deviation of the array of values</returns>
    public static double StDev(double[] v)
    {
        return Math.Sqrt(Variance(v));
    }

    public static void SaveArrayAsCSV(Array arrayToSave, string fileName)
    {
        using (StreamWriter file = new StreamWriter(fileName))
        {
            WriteItemsToFile(arrayToSave, file);
        }
    }

    private static void WriteItemsToFile(Array items, TextWriter file)
    {
        foreach (object item in items)
        {
            if (item is Array)
            {
                WriteItemsToFile(item as Array, file);
                file.Write(Environment.NewLine);
            }
            else
                file.Write(item + Environment.NewLine);
        }
    }

    private static void WriteResultsToFile(string[] items, string filePath)
    {
        //before your loop
        var csv = new StringBuilder();
        //in your loop
        var DataSize = items[0].ToString();
        var LoopNum = items[1].ToString();
        var Lanugage = items[2].ToString();
        var SortName = items[3].ToString();
        var Mean = items[4].ToString();
        var Variance = items[5].ToString();
        var Stdev = items[6].ToString();


        //Suggestion made by KyleMit
        var headings = "DataSize, LoopNum, Language, SortName, Mean, Variance, Stdev";
        var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", DataSize, LoopNum, Lanugage, SortName, Mean, Variance, Stdev);


        if (!File.Exists(filePath))
        {
            csv.AppendLine(headings);
            //after your loop

        }
            csv.AppendLine(newLine);
            File.AppendAllText(filePath, csv.ToString());
        


    }

}