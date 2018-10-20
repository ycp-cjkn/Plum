using System;

namespace ToBeRenamed.Dtos
{
    public class MembershipDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LibraryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public string DisplayName { get; set; }
    }
}
