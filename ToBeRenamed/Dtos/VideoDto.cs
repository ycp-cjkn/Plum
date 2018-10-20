using System;

namespace ToBeRenamed.Dtos
{
    public class VideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int VideoUrlId { get; set; }
        public int LibraryId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
