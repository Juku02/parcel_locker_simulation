using System;
using System.Text.Json.Serialization;
namespace ParcelBox.Entities.Parcels
{

    public class Parcel : IParcel
    {
        public string Adressee  { get; set; }
        public string Sender { get; set; }
        public ParcelStatus Status { get; set; }

    }

    public enum ParcelStatus
    {
        PREPARING,
        SENDING,
        DELIVERED,
    }
}
