using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class DeleteAnnotationReply : IRequest
    {
        public int UserId { get; }
        public int ReplyId { get; }

        public DeleteAnnotationReply(int userId, int replyId)
        {
            UserId = userId;
            ReplyId = replyId;
        }
    }
}
