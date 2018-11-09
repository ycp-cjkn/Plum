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
    public class GetMembersOfLibraryHandler : IRequestHandler<GetMembersOfLibrary, IEnumerable<Member>>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetMembersOfLibraryHandler(IMapper mapper, IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mapper = mapper;
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<Member>> Handle(GetMembersOfLibrary request, CancellationToken cancellationToken)
        {
            var dtos = await GetMembersOfLibrary(request);
            var members = _mapper.Map<IEnumerable<Member>>(dtos).ToArray();
            var memberIds = members.Select(m => m.Id).ToList();
            var roles = await _mediator.Send(new GetRolesForMembers(memberIds), cancellationToken);

            foreach (var member in members)
            {
                member.Role = roles[member.Id];
            }

            return members;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersOfLibrary(GetMembersOfLibrary request)
        {
            const string sql = @"
                SELECT
	                mem.id,
	                usr.id AS user_id,
	                lib.id AS library_id,
                    mem.display_name,
                    usr.display_name AS full_name,
                    mem.created_at,
	                usr.id = lib.created_by AS is_creator
                FROM plum.users usr
                INNER JOIN plum.memberships mem
                ON usr.id = mem.user_id
                INNER JOIN plum.libraries lib
                ON lib.id = mem.library_id
                WHERE mem.library_id = @LibraryId
                ORDER BY mem.created_at";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<MemberDto>(sql, new { request.LibraryId });
            }
        }
    }
}