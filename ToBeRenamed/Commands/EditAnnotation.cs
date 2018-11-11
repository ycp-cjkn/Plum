using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class EditAnnotation : IRequest
    {
        public int UserId { get; }
        public string Comment { get; }
        public int AnnotationId { get; }

        public EditAnnotation(int userId, string comment, int annotationId)
        {
            UserId = userId;
            Comment = comment;
            AnnotationId = annotationId;
        }
    }
}
