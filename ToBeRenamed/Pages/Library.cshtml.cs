using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages
{
    public class LibraryModel : PageModel
    {
        private readonly IMediator _mediator;

        public LibraryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public LibraryDto Library;

        public async Task OnGetAsync(int id)
        {
            Library = await _mediator.Send(new GetLibraryDtoById(id));
        }
    }
}