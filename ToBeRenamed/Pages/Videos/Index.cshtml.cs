using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public IEnumerable<AnnotationDto> Annotations { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            Video = await _mediator.Send(new GetVideoById(Id));
            Annotations = await _mediator.Send(new GetAnnotationsByVideoId(Id));

            var count = Annotations.Count();
            for (int i = 0; i < count; i++)
            {
                var timestampNumber = Annotations.ElementAt(i).Timestamp;
                var totalSeconds = Convert.ToInt32(Math.Floor(timestampNumber));
                var minutes = (totalSeconds / 60 < 10) ? "0" + Convert.ToString(totalSeconds / 60) : Convert.ToString(totalSeconds / 60);
                var seconds = (totalSeconds % 60 < 10) ? "0" + Convert.ToString(totalSeconds % 60) : Convert.ToString(totalSeconds % 60);

                var timeStamp = minutes+ ":" + seconds;
                Annotations.ElementAt(i).TimestampDisplay = timeStamp;
            }
            
            return Page();
        }
    }
}