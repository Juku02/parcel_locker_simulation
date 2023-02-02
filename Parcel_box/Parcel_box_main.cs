using System;
using System.Threading;
using ParcelBox.Entities.Humans;
using ParcelBox.Entities.Parcels;
using ParcelBox.Entities.MailBoxes;
using System.Linq;
using ParcelBox.SchedulersAndHandlers.Scheduler;
class MainClass
{
    static Mutex mut = new Mutex();
    static Thread InitParcelBox1;
    static Thread InitParcelBox2;
    static Thread InitParcelBox3;
static void PrepareParcel(Scheduler scheduler)
    {
        Parcel parcel = new Parcel();
        scheduler.GenerateParcel(scheduler);
        int ParcelCount = scheduler.AllGenerateParcels.Count();
        scheduler.PrintOnScreen($"Przesylka od {scheduler.AllGenerateParcels[ParcelCount - 1].Sender.Name} do {scheduler.AllGenerateParcels[ParcelCount - 1].Adressee.Name}");
        Thread.Sleep(500);
    }


    static bool ReadHumanList(Scheduler scheduler, MailBox mailBox)
    {
        mut.WaitOne();
        int HumanIterator = 0;
        int Waitinghumans = 0;
        while (true)
        {
            if (HumanIterator == mailBox.HumanList.Count)
            {
                break;
            }

            switch (mailBox.HumanList[HumanIterator].Status)
            {
                case HumanStatus.SENDING:
                    {
                        scheduler.AddToQueue(mailBox.HumanList[HumanIterator], mailBox);
                        HumanIterator++;
                        break;
                    }
                case HumanStatus.RECEIVING:
                    {
                        scheduler.AddToQueue(mailBox.HumanList[HumanIterator], mailBox);
                        HumanIterator++;
                        break;
                    }
                case HumanStatus.WAITING:
                    {
                        HumanIterator++;
                        Waitinghumans++;
                        break;
                    }
                default: break;
            }
        }
        if(Waitinghumans == mailBox.HumanList.Count)
        {
            Thread.Sleep(500);
            mut.ReleaseMutex();
            return false;
        }
        else
        {
            Thread.Sleep(500);
            mut.ReleaseMutex();
            return true;
        }
    }

    static void InitParcelBoxThread(Scheduler scheduler, MailBox mailBox)
    {
        bool first = true;
        while (true)
        {
            if (scheduler.ParcelBase.Count != 0 || first)
            {
                scheduler.FindPeople(mailBox, scheduler.AllGenerateParcels);
                ReadHumanList(scheduler, mailBox);
                scheduler.QueueHandler(mailBox);
            }
            else
            {
                break;
            }
            first = false;

        }

    }

    public static void Main()
    {
        int number;
        Scheduler scheduler = new Scheduler();
        Thread InitParcelBox1;
        Thread InitParcelBox2;
        Thread InitParcelBox3;
        var humansFirstParcelBox = scheduler.InitHumans(@"./humansFirstParcelBox.json");
        var humansSecondParcelBox = scheduler.InitHumans(@"./humansSecondParcelBox.json");
        var humansThirdParcelBox = scheduler.InitHumans(@"./humansThirdParcelBox.json");

      
        var mailBox = scheduler.InitMailBox(1, humansFirstParcelBox.Count, humansFirstParcelBox);
        var mailBox2 = scheduler.InitMailBox(2, humansSecondParcelBox.Count, humansSecondParcelBox);
        var mailBox3 = scheduler.InitMailBox(3, humansThirdParcelBox.Count, humansThirdParcelBox);

        scheduler.PrintOnScreen("Podaj ile paczek wygenerowac: ");
        number = Convert.ToInt32(Console.ReadLine());

        if(number > scheduler.AllHumans.Count/2)
        {
            scheduler.PrintOnScreen($"Za duza ilosc paczek. Podaj liczbe ktora jest mniejsza od {scheduler.AllHumans.Count / 2}");
            number = Convert.ToInt32(Console.ReadLine());
        }

        for (int i = 0; i < number; i++)
        {
            PrepareParcel(scheduler);
        }

        InitParcelBox1 = new Thread(() => InitParcelBoxThread(scheduler, mailBox));
        InitParcelBox2 = new Thread(() => InitParcelBoxThread(scheduler, mailBox2));
        InitParcelBox3 = new Thread(() => InitParcelBoxThread(scheduler, mailBox3));

        InitParcelBox1.Start();
        InitParcelBox2.Start();
        InitParcelBox3.Start();

        InitParcelBox1.Join();
        InitParcelBox2.Join();
        InitParcelBox3.Join();
    }

}
