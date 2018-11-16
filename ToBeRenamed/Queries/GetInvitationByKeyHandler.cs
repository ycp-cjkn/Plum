using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetInvitationByKeyHandler : IRequestHandler<GetInvitationByKey, InvitationDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetInvitationByKeyHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<InvitationDto> Handle(GetInvitationByKey request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    inv.id AS id,
                    inv.url_key as url_key,
                    inv.created_at as created_at,
                    mem.display_name as display_name,
                    usr.display_name as full_name,
                    rol.title as role_title,
                    lib.title as library_title,
                    rol.id as role_id,
                    mem.id as membership_id,
                    lib.id as library_id
                FROM plum.invitations inv
                INNER JOIN plum.memberships mem
                ON mem.id = inv.membership_id
                INNER JOIN plum.users usr
                ON usr.id = mem.user_id
                INNER JOIN plum.roles rol
                ON rol.id = inv.role_id
                INNER JOIN plum.libraries lib
                ON lib.id = rol.library_id
                WHERE
                    inv.deleted_at IS NULL
                    AND (inv.expires_at > NOW() OR inv.expires_at IS NULL)
                    AND inv.url_key = @Key";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<InvitationDto>(sql, request)).Single();
            }
        }
    }
}