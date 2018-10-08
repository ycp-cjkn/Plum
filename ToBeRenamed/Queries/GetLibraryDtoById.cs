using MediatR;
using ToBeRenamed.Dtos;
namespace ToBeRenamed.Queries
{
    public class GetLibraryDtoById : IRequest<LibraryDto>
    {
        public int Id { get; }

        public GetLibraryDtoById(int id)
        {
            Id = id;
        }
    }
}
