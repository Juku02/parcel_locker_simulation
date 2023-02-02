using System;
using System.Threading;
namespace ParcelBox.Tests
{
    class MyCounter
    {
        public static int count = 0;
        public static Mutex MuTexLock = new Mutex();
    }
    class IncThread
    {
        public Thread th;
        void Go()
        {
            Console.WriteLine("IncThread is waiting for the mutex.");
            MyCounter.MuTexLock.WaitOne();
            Console.WriteLine("IncThread acquires the mutex.");
            int num = 10;
            do
            {
                Thread.Sleep(50);
                MyCounter.count++;
                Console.WriteLine("In IncThread, MyCounter.count is " + MyCounter.count);
                num--;
            } while (num > 0);
            Console.WriteLine("IncThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();
        }
        public IncThread()
        {
            th = new Thread(this.Go);
            th.Start();
        }
    }
    class DecThread
    {
        public Thread th;
        public DecThread()
        {
            th = new Thread(new ThreadStart(this.Go));
            th.Start();
        }
        void Go()
        {
            Console.WriteLine("DecThread is waiting for the mutex.");
            MyCounter.MuTexLock.WaitOne();
            Console.WriteLine("DecThread acquires the mutex.");
            int num = 10;
            do
            {
                Thread.Sleep(50);
                MyCounter.count--;
                Console.WriteLine("In DecThread, MyCounter.count is " + MyCounter.count);
                num--;
            } while (num > 0);
            Console.WriteLine("DecThread releases the mutex.");
            MyCounter.MuTexLock.ReleaseMutex();
        }
    }
    class TestThread
    {
        static void Test()
        {
            IncThread myt1 = new IncThread();
            DecThread myt2 = new DecThread();
            myt1.th.Join();
            myt2.th.Join();
            Console.Read();
        }

    }
}

