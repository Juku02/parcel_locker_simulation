using System;
namespace ParcelBox.Entities.Humans
{
    public class Human : IHuman
    {
        public string Name { get; set; }
        public HumanStatus Status { get; set; }
    }

    public enum HumanStatus
    {
        SENDING,
        RECEIVING,
        WAITING
    }
}
