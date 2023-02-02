using System;
using ParcelBox.Entities.Parcels;
using ParcelBox.Entities.Humans;
using ParcelBox.Configurations;
using System.Collections.Generic;

namespace Tests.Test
{
    public class TestJSON
    {
        public void Test()
        {
            List<Parcel> inParcelList = new List<Parcel>();

            Configuration conf = new Configuration();

            //Human human1, human2 = new Human();

            List<Parcel> parcelList = conf.ReadFromJSON<Parcel>(@"./parcel.json");

            foreach (Parcel parcel in parcelList)
            {
                Console.WriteLine($"Adressee: {parcel.Adressee}");
                Console.WriteLine($"Sender: {parcel.Sender}");
               
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    inParcelList.Add(new Parcel()
            //    {   
            //        Adressee = human1,
            //        Sender = human2,
            //        Status = ParcelStatus.DELIVERED
            //    });
            //}

            //int result = conf.WriteToJSON<Parcel>(inParcelList);
        }
    }
}
