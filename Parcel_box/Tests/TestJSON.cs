using System;
using ParcelBox.Entities.Parcels;
using ParcelBox.Configurations;
using System.Collections.Generic;

namespace Tests.Test
{
    public class TestJSON
    {
        public void Test()
        {
            List<Parcel> parcelList = new List<Parcel>();

            Configuration conf = new Configuration();

            List<Parcel> parcelData = conf.readFromJSON<Parcel>(@"./parcel.json");

            Console.WriteLine($"Adressee: {parcelData[0].Adressee}");
            Console.WriteLine($"Sender: {parcelData[0].Sender}");
            Console.WriteLine($"Status: {parcelData[0].Status}");

            for (int i = 0; i < 3; i++)
            {
                parcelList.Add(new Parcel()
                {
                    Adressee = "dupa" + i,
                    Sender = "pizda" + i,
                    Status = ParcelStatus.DELIVERED
                });
            }

            int result = conf.writeToJSON<Parcel>(parcelList);
        }
    }
}
