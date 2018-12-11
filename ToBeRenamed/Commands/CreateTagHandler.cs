using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Commands
{
    public class CreateTagHandler : IRequestHandler<CreateTag, TagDto>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateTagHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<TagDto> Handle(CreateTag request, CancellationToken cancellationToken)
        {
            const string sql = @"
            WITH TAG AS(
               INSERT INTO plum.tags (text)
               VALUES (@Tag)
               RETURNING id, text)
            SELECT TAG.id 
            FROM TAG
            INNER JOIN plum.video_tags VTAGS
            ON VTAGS.tag_id = TAG.id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<TagDto>(sql, request)).Single();
            }
        }
    }
}
