using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Problem01
{
    class Program
    {
        static byte[] Data_Global = new byte[1000000000];
        static long Sum_Global = 0;
        static int G_index = 0;

        static int ReadData()
        {
            int returnData = 0;
            FileStream fs = new FileStream("C:\\Users\\jj\\source\\repos\\OS_LAB\\OS_LAB\\Problem01.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

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

        static void sum1()
        {
            long sum_local = 0;
            for (int i = 0; i < 500000000; i++)
            {
                if (Data_Global[i] % 2 == 0)
                {
                    sum_local -= Data_Global[i]; 
                }
                else if (Data_Global[i] % 3 == 0)
                {
                    sum_local += (Data_Global[i] * 2); 
                }
                else if (Data_Global[i] % 5 == 0)
                {
                    sum_local += (Data_Global[i] / 2); 
                }
                else if (Data_Global[i] % 7 == 0)
                {
                    sum_local += (Data_Global[i] / 3); 
                }
                Data_Global[i] = 0;
                //G_index++;
            }

            Sum_Global += sum_local;
        }

        static void sum2()
        {
            long sum_local = 0;
            for (int i = 500000000; i < 1000000000; i++)
            {
                if (Data_Global[i] % 2 == 0)
                {
                    sum_local -= Data_Global[i];
                }
                else if (Data_Global[i] % 3 == 0)
                {
                    sum_local += (Data_Global[i] * 2);
                }
                else if (Data_Global[i] % 5 == 0)
                {
                    sum_local += (Data_Global[i] / 2);
                }
                else if (Data_Global[i] % 7 == 0)
                {
                    sum_local += (Data_Global[i] / 3);
                }
                Data_Global[i] = 0;
                //G_index++;
            }
            Sum_Global += sum_local;
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
            //for (i = 0; i < 1000000000; i++)

            Thread th1 = new Thread(sum1);
            Thread th2 = new Thread(sum2);

            th1.Start();
            th2.Start();

            th1.Join();
            th2.Join();

            sw.Stop();
            Console.WriteLine("Done.");

            /* Result */
            Console.WriteLine("Summation result: {0}", Sum_Global);
            Console.WriteLine("Time used: " + sw.ElapsedMilliseconds.ToString() + "ms");
        }
    }
}
