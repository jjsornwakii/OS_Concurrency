using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Problem01
    {
        static int MAX_DATA_SIZE = 1000000000;
        static int PREFERED_DATA_SIZE = 0;
        static int number_of_th = 0;
        static byte[] Data_Global = new byte[PREFERED_DATA_SIZE];
        static long Sum_Global = 0;
        static int G_index = 0;


        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("C:\\Users\\jj\\source\\repos\\OS_LAB\\OS_LAB\\Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();


            number_of_th = Environment.ProcessorCount;
            PREFERED_DATA_SIZE = MAX_DATA_SIZE + number_of_th - (MAX_DATA_SIZE % number_of_th);


            try
            {
                Data_Global = (byte[])bf.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                Console.WriteLine("Read Failed:" + se.Message);
                returnData = 1;
            }
            finally
            {
                fs.Close();
            }

            return returnData;
        }
        static void sum(int start, int end)
        {
            int Sum_Local = 0;

            for (int i = start; i < end; i++)
            {
                byte curData = Data_Global[i];

                if ((curData & 1) == 0)
                    Sum_Local -= curData; 

                else if (curData % 3 == 0)
                    Sum_Local += (curData * 2);

                else if (curData % 5 == 0)
                    Sum_Local += (curData / 2);
                
                else if (curData % 7 == 0)
                Sum_Local += (curData / 3);

                Data_Global[i] = 0;
            }
            Sum_Global += Sum_Local;
        }





        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int i, y;

            /* Read data from file */
            Console.Write("Data read...");
            y = ReadData();
            if (y == 0)
            {
                Console.WriteLine("Complete.");
            }
            else
            {
                Console.WriteLine("Read Failed!");
            }

            /* Start */
            Console.Write("\n\nWorking...");
            sw.Start();


            // สร้าง thread ตามจำนวน thread ของอุปกรณ์
            Thread[] th = new Thread[number_of_th];
            int partition = PREFERED_DATA_SIZE / number_of_th;


            
            //ให้แต่ละ thread ทำ func sum ของช่วงตัวเอง
            for ( i = 0; i < th.Length; i++)
            {
                int start = i * partition;
                int end = Math.Min((i + 1) * partition, MAX_DATA_SIZE);
                th[i] = new Thread(() => sum(start, end));
            }

            // run all thread
            for ( i = 0; i < th.Length; i++)
                th[i].Start(); 
            

            // waiting for one thread end and join another
            for ( i = 0; i < th.Length; i++)
                th[i].Join(); 
            

            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
