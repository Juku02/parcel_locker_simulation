using System;
using ParcelBox.Entities.Humans;
namespace ParcelBox.Entities.Parcels
{
    interface IParcel 
    {
        Human Adressee { get; set; }
        Human Sender { get; set; }
    }
}
