using System.Collections.Generic;

using ParcelBox.Entities.Humans;
namespace ParcelBox.Entities.MailBoxes
{
    public class MailBox : IMailBox
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public List<Human> Queue { get; set; }
        public List<Human> HumanList { get; set; }
    }
}
