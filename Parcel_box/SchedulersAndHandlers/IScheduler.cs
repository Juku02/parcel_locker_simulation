using ParcelBox.Entities.Humans;
using ParcelBox.Entities.MailBoxes;
using ParcelBox.Entities.Parcels;

namespace ParcelBox.SchedulersAndHandlers
{
    public interface IScheduler
    {
        MailBox InitMailBoxes(string path);
        Human InitHuman(string path);
        Parcel InitParcel(object AdressHuman, object SenderHuman);

        int 
    }
}
