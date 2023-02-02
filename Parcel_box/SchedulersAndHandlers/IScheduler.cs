using ParcelBox.Entities.Humans;
using ParcelBox.Entities.MailBoxes;
using ParcelBox.Entities.Parcels;
using System.Collections.Generic;

namespace ParcelBox.SchedulersAndHandlers.Scheduler
{
    public interface IScheduler
    {
        void PrintOnScreen(string Text);
        List<Human> InitHumans(string path);
        MailBox InitMailBox(int id, int capasity, List<Human> humans);
        Parcel CreateParcel(Human Adressee, Human Sender);
        void QueueHandler(MailBox mailBox);
        void AddToQueue(Human person, MailBox mailBox);
        MailBox FindMailBox(Human human);
        void FindPeople(MailBox mailBox, List<Parcel> parcels);
        void GenerateParcel(Scheduler scheduler);
    }
}
