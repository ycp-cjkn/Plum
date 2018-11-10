using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateAnnotationHandler : IRequestHandler<CreateAnnotation, AnnotationDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateAnnotationHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AnnotationDto> Handle(CreateAnnotation request, CancellationToken cancellationToken)
        {
            const string sql = @"
                WITH ANN AS (
                    INSERT INTO plum.annotations (user_id, comment, timestamp, video_id)
                    VALUES (@UserId, @Comment, @Timestamp, @VideoId)
                    RETURNING id, comment, timestamp, user_id, video_id
                )
                SELECT ANN.id, ANN.comment, ANN.timestamp, ANN.user_id, MEM.display_name FROM ANN
                INNER JOIN plum.memberships MEM
                ON MEM.user_id = ANN.user_id
                INNER JOIN plum.videos VID
                ON MEM.library_id = VID.library_id AND VID.id = ANN.video_id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<AnnotationDto>(sql, request)).SingleOrDefault();
            }
        }
    }
}
