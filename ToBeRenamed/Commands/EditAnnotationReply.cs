using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class EditAnnotationReply : IRequest<ReplyDto>
    {
        public int UserId { get; }
        public int AnnotationId { get; }
        public string Text { get; }
        public string NewText { get; }

        public EditAnnotationReply(int userId, int annotationId, string text, string new_Text)
        {
            UserId = userId;
            AnnotationId = annotationId;
            Text = text;
            NewText = new_Text;
        }
    }
}
