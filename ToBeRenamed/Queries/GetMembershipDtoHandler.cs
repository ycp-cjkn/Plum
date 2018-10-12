using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetMembershipDtoHandler : IRequestHandler<GetMembershipDto, MembershipDto>
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public GetMembershipDtoHandler(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        
        public async Task<MembershipDto> Handle(GetMembershipDto request, CancellationToken cancellationToken)
        {
            int userId = request.UserId;
            int libraryId = request.LibraryId;
            
            const string sql = @"
                SELECT id, user_id, library_id, created_at, deleted_at, display_name
                FROM plum.memberships
                WHERE user_id = @userId AND library_id = @libraryId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<MembershipDto>(sql, new { userId, libraryId })).Single();
            }
        }
    }
}