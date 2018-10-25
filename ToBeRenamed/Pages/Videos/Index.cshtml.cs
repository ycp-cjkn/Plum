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
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var video = await _mediator.Send(new GetVideoById(Id));

            Title = video.Title;
            Description = video.Description;
            
            return Page();
        }
    }
}