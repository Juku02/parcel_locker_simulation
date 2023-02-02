using ParcelBox.Entities.MailBoxes;
using ParcelBox.Entities.Parcels;
using ParcelBox.Entities.Humans;
using ParcelBox.Configurations;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;
namespace ParcelBox.SchedulersAndHandlers.Scheduler
{ 
    public class Scheduler : IScheduler
    {
        private Mutex mut = new Mutex();
        private Configuration configuration = new Configuration();
        public List<MailBox> AllMailBoxes = new List<MailBox>();
        public List<Parcel> AllGenerateParcels = new List<Parcel>();
        public List<Parcel> ParcelBase= new List<Parcel>();
        public List<Human> AllHumans = new List<Human>();
        public List<Human> UseHumans = new List<Human>();

        public void PrintOnScreen(string Text)
        {
            mut.WaitOne();
            Console.WriteLine(Text);
            Thread.Sleep(500);
            mut.ReleaseMutex();
        }

        public List<Human> InitHumans(string path)
        {
            List<Human> humans = new List<Human>();
           
            humans.AddRange(configuration.ReadFromJSON<Human>(path));
            AllHumans.AddRange(humans);
            return humans;

        }

        public MailBox InitMailBox(int id, int capasity, List<Human> humans)
        {
            List<Human> queue = new List<Human>();

            MailBox mailBox = new MailBox()
            {
                Id = id,
                Capacity = capasity,
                Queue = queue,
                HumanList = humans
            };
            AllMailBoxes.Add(mailBox);
            return mailBox;
        }

        public Parcel CreateParcel(Human Adressee, Human Sender)
        {
            Parcel parcel = new Parcel()
            {
                Adressee = Adressee,
                Sender = Sender
            };
            return parcel;
        }

        private void DeleteFromQueue(List<Human> list)
        {
            list.Remove(list.First());
        }

        private void DeleteFromMailBox(List<Parcel> list, Parcel parcel, MailBox mailBox)
        { 
            var index = list.IndexOf(parcel);
            foreach(var human in mailBox.HumanList)
            {
                if(human.Name == parcel.Adressee.Name)
                {
                    human.Status = HumanStatus.WAITING;
                }
            }
            PrintOnScreen($"Paczka od {parcel.Sender.Name} zostala odebrana w paczkomacie {mailBox.Id}");
            list.RemoveAt(index);
        }

        public MailBox FindMailBox(Human human)
        {

            foreach (var mailBox in AllMailBoxes)
            {
                foreach (var people in mailBox.HumanList)
                {
                    if (people == human)
                    {
                        return mailBox;
                    }
                }
            }
            return null;
        }
       public void FindPeople(MailBox mailBox, List<Parcel> parcels)
        {
            mut.WaitOne();
            foreach (var Parcel in parcels)
            {
                foreach (var people in mailBox.HumanList)
                {
                    if (people.Name == Parcel.Sender.Name)
                    {
                        people.Status = Parcel.Sender.Status;
                    }
                    if (people.Name == Parcel.Adressee.Name)
                    {
                        people.Status = Parcel.Adressee.Status;
                    }
                }
            }
            Thread.Sleep(500);
            mut.ReleaseMutex();
        }

        private void SendParcel(Parcel parcel, MailBox mailBox)
        {
            ParcelBase.Add(parcel);
            parcel.Adressee.Status = HumanStatus.RECEIVING;
            parcel.Sender.Status = HumanStatus.WAITING;
            FindPeople(mailBox, ParcelBase);
            PrintOnScreen($"Paczka do {parcel.Adressee.Name} od {parcel.Sender.Name} zostala wyslana");
        }

        private Parcel FindParcel(Human human, List<Parcel> parcelList)
        {
            switch(human.Status)
            {
                case HumanStatus.SENDING:
                {
                        foreach (var Parcel in parcelList)
                        {
                            if(Parcel.Sender.Name == human.Name)
                            {
                              
                                return Parcel;
                            }
                        }
                        break;
                }
                case HumanStatus.RECEIVING:
                {
                        foreach (var Parcel in parcelList)
                        {
                            if (Parcel.Adressee.Name == human.Name)
                            {
                                return Parcel;
                            }
                        }
                        break;
                }
                default: return null;
            }
           return null;
        }

        public void QueueHandler(MailBox mailBox)
        {
            mut.WaitOne();
            if (mailBox.Queue.Count != 0)
            {
                switch (mailBox.Queue[0].Status)
                {
                    case HumanStatus.SENDING:
                        var Sending = FindParcel(mailBox.Queue[0], AllGenerateParcels);
                        var adressMail = FindMailBox(Sending.Adressee);
                        SendParcel(Sending, adressMail);
                        DeleteFromQueue(mailBox.Queue);
                        break;
                    case HumanStatus.RECEIVING:
                        var Receiving = FindParcel(mailBox.Queue[0], ParcelBase);
                        DeleteFromMailBox(ParcelBase, Receiving, mailBox);
                        DeleteFromQueue(mailBox.Queue);
                        break;
                    default:
                        break;

                }
            }
            Thread.Sleep(500);
            mut.ReleaseMutex();
        }

        public void AddToQueue(Human person, MailBox mailBox)
        {

            if (mailBox.Queue.Count == mailBox.Capacity)
            {
               PrintOnScreen($"{mailBox.Id} Kolejka jest pelna");
            }
            else if ((mailBox.Queue.Contains(person)))
            {
                return;
            }
            else
            {
                mailBox.Queue.Add(person);
               PrintOnScreen($"Dodano {person.Name} do kolejki paczkomatu {mailBox.Id}");
            }

        }

        public void GenerateParcel(Scheduler scheduler)
        {
           
            Random random = new Random();
            Parcel parcel = new Parcel();
            int AdresseeIndex, SenderIndex;
            MailBox SenderParcelBox, AdresseeParcelBox = new MailBox();
            while (true)
            {
                AdresseeIndex = random.Next(0, scheduler.AllHumans.Count);
                SenderIndex = random.Next(0, scheduler.AllHumans.Count);
                if (AdresseeIndex != SenderIndex)
                {
                    if (!(UseHumans.Contains(scheduler.AllHumans[AdresseeIndex])) && !(UseHumans.Contains(scheduler.AllHumans[SenderIndex])))
                    {

                        parcel = scheduler.CreateParcel(scheduler.AllHumans[AdresseeIndex], scheduler.AllHumans[SenderIndex]);
                        AdresseeParcelBox = scheduler.FindMailBox(parcel.Adressee);
                        SenderParcelBox = scheduler.FindMailBox(parcel.Sender);

                        if (AdresseeParcelBox.Id != SenderParcelBox.Id)
                        {
                            UseHumans.Add(parcel.Adressee);
                            UseHumans.Add(parcel.Sender);
                          
                            AllGenerateParcels.Add(parcel);
                            parcel.Sender.Status = HumanStatus.SENDING;
                          
                            Thread.Sleep(500);
                            return;
                        }
                    }
                }
            }
        }
    }
}
