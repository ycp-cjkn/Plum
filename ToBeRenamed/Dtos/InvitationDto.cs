using System;

namespace ToBeRenamed.Dtos
{
    public class InvitationDto
    {
        public int Id { get; set; }
        public string UrlKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string FullName { get; set; }
        public string LibraryTitle { get; set; }
        public string RoleTitle { get; set; }
        public int MembershipId { get; set; }
        public int RoleId { get; set; }
        public int LibraryId { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
