namespace ToBeRenamed.Dtos
{
    public class MemberDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LibraryId { get; set; }
        public string DisplayName { get; set; }
        public bool IsCreator { get; set; }
    }
}
