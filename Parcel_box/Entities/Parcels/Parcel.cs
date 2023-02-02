using System;
using System.Text.Json.Serialization;
using ParcelBox.Entities.Humans;
namespace ParcelBox.Entities.Parcels
{

    public class Parcel : IParcel
    {
        public Human Adressee { get; set; }
        public Human Sender { get; set; }
    }
}
