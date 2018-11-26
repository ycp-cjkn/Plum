using AutoMapper;
using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Extensions;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetSignedInMemberHandler : IRequestHandler<GetSignedInMember, Member>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetSignedInMemberHandler(IMapper mapper, IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mapper = mapper;
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Member> Handle(GetSignedInMember request, CancellationToken cancellationToken)
        {
            var member = await GetSignedInMember(request);

            var roles = await _mediator.Send(new GetRolesForMembers(new[] { member.Id }), cancellationToken);

            member.Role = roles[member.Id];

            return member;
        }

        private async Task<Member> GetSignedInMember(GetSignedInMember request)
        {
            var nameIdentifier = request.User.GetNameIdentifier();

            const string sql = @"
                SELECT
	                mem.id,
	                mem.user_id,
	                mem.library_id,
                    mem.display_name,
                    usr.display_name AS full_name,
                    mem.created_at,
	                usr.id = lib.created_by AS is_creator
                FROM plum.users usr
                INNER JOIN plum.memberships mem
                ON usr.id = mem.user_id
                INNER JOIN plum.libraries lib
                ON lib.id = mem.library_id
                WHERE usr.google_claim_nameidentifier = @nameIdentifier";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var dtos = await cnn.QueryAsync<MemberDto>(sql, new { nameIdentifier, request.LibraryId });
                return _mapper.Map<Member>(dtos.Single());
            }
        }
    }
}
