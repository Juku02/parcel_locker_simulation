using System;
namespace ParcelBox.Entities.Humans
{
    public class Human : IHuman
    {
        public string Name { get; set; }
        public HumanStatus Status { get; set; }
        public int WaitingTime { get; set; }
    }

    public enum HumanStatus
    {
        SENDING,
        WAITING,
        RECEIVING
    }
}
