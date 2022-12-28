using System;
using System.Collections.Generic;
using ParcelBox.Entities.Parcels;
using ParcelBox.Entities.Humans;
namespace ParcelBox.Entities.MailBoxes
{
    interface IMailBox
    {
        int Id { get; set; }
        int Capacity { get; set; }
        List<Human> Queue { get; set; }
        List<Parcel> Parcels { get; set; }
    }
}
