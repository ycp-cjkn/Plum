using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetPrivilegesForMemberHandler : IRequestHandler<GetPrivilegesForMember, ISet<Privilege>>
    {
        private readonly IMapper _mapper;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetPrivilegesForMemberHandler(IMapper mapper, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mapper = mapper;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<ISet<Privilege>> Handle(GetPrivilegesForMember request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT rpv.privilege_alias
                FROM plum.role_privileges rpv
                INNER JOIN plum.memberships mem
                ON mem.role_id = rpv.role_id
                WHERE mem.id = @MembershipId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var aliases = await cnn.QueryAsync<string>(sql, request);
                return _mapper.Map<ISet<Privilege>>(aliases);
            }
        }
    }
}
