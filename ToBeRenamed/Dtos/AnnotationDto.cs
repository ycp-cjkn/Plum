using System;

namespace ToBeRenamed.Dtos
{
    public class AnnotationDto
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string DisplayName { get; set; }
        public double Timestamp { get; set; }

        public string TimestampDisplay
        {
            get
            {
                var totalSeconds = Convert.ToInt32(Math.Floor(Timestamp));

                var minutes = totalSeconds / 60 < 10
                    ? "0" + Convert.ToString(totalSeconds / 60)
                    : Convert.ToString(totalSeconds / 60);

                var seconds = totalSeconds % 60 < 10
                    ? "0" + Convert.ToString(totalSeconds % 60)
                    : Convert.ToString(totalSeconds % 60);

                return minutes + ":" + seconds;
            }
        }
    }
}
