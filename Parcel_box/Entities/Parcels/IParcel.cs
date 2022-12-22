using System;

namespace ParcelBox.Entities.Parcels
{
    interface IParcel 
    {
        string Adressee { get; set; }
        string Sender { get; set; }
        ParcelStatus Status { get; set; }
    }
}
