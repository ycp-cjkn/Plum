using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class DeleteAnnotationReply : IRequest<ReplyDto>
    {
        public int UserId { get; }
        public int AnnotationId { get; }
        public string Text { get; }

        public DeleteAnnotationReply(int userId, int annotationId, string text)
        {
            UserId = userId;
            AnnotationId = annotationId;
            Text = text;
        }
    }
}
