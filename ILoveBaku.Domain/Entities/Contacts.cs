using System;

namespace ILoveBaku.Domain.Entities
{
    public partial class Contacts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte ContactsStatusesId { get; set; }
    }
}
