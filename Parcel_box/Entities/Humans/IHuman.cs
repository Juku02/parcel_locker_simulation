using System;
namespace ParcelBox.Entities.Humans
{
    interface IHuman
    {
        string Name { get; set; }
        HumanStatus Status { get; set; }
    }

}
