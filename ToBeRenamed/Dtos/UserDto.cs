using System;

namespace ToBeRenamed.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public string GoogleClaimNameIdentifier { get; set; }
    }
}
