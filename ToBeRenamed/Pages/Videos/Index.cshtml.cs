using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Videos
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        
        public VideoDto Video { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Video = await _mediator.Send(new GetVideoById(Id));
            
            return Page();
        }
    }
}