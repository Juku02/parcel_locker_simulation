using System;
using System.Threading;
using System.Linq;
using ParcelBox.Configurations;
using ParcelBox.Entities.Humans;
using ParcelBox.SchedulersAndHandlers.Scheduler;
namespace ParcelBox.Tests
{
    public class TestSimpleParcelBox
    {
        public void TestParcelBox()
        {
            Scheduler scheduler = new Scheduler();
            //get list o people
            int i;
            var humans = scheduler.InitHumans(@"./humans.json");
            for (i = 0; i < humans.Capacity - 1; i++)
            {
                Console.WriteLine($"Human List:{humans[i].Name}");
            }
            //create a mail box and add every parcelBox to list
            var mailBox1 = scheduler.InitMailBox(1, 5, humans);
            var mailBox2 = scheduler.InitMailBox(2, 5, humans);
            var mailBox3 = scheduler.InitMailBox(3, 5, humans);

            for (i = 0; i < mailBox1.HumanList.Capacity - 1; i++)
            {
                Console.WriteLine($"Human List in Mail Box:{mailBox1.HumanList[i].Name}");
            }
            //create a parcel
            var parcel = scheduler.CreateParcel(humans[0], humans[1]);
            humans[0].Status = HumanStatus.SENDING;
            humans[1].Status = HumanStatus.RECEIVING;
            //add to mailbox queue
            scheduler.AddToQueue(parcel.Adressee, mailBox1);
            Console.WriteLine($"Kolejka {mailBox1.Queue.Count()}");
            //Handle the queue
            scheduler.QueueHandler(mailBox1);
            scheduler.AddToQueue(parcel.Sender, mailBox1);

            scheduler.QueueHandler(mailBox1);
            Console.WriteLine($"Kolejka {mailBox1.Queue.Count()}");
            //if sender is first in mailBox queue send or recive package
        }
    }
}
