using AutoMapper;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetRolesForMembersHandler : IRequestHandler<GetRolesForMembers, IDictionary<int, Role>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetRolesForMembersHandler(IMapper mapper, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mapper = mapper;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IDictionary<int, Role>> Handle(GetRolesForMembers request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    rol.id,
                    rol.title,
                    rpv.privilege_alias,
                    mem.id AS membership_id
                FROM plum.roles rol
                INNER JOIN plum.memberships mem
                ON mem.role_id = rol.id
                LEFT JOIN plum.role_privileges rpv
                ON rpv.role_id = rol.id
                WHERE mem.id = ANY(@MembershipIds)
                ORDER BY rol.created_at";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var dtos = await cnn.QueryAsync<RoleDto>(sql, request);

                return dtos
                    .GroupBy(d => d.MembershipId)
                    .ToDictionary(g => g.Key, g => _mapper.Map<Role>(g));
            }
        }
    }
}
