using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
                var displayTimestamp = generateTimestampDisplay(Annotations.ElementAt(i).Timestamp);
                Annotations.ElementAt(i).TimestampDisplay = displayTimestamp;
            }
            
            return Page();
        }

        public async Task<PartialViewResult> OnPostCreateAnnotation(int videoId, string comment, string timestamp)
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));
            
            var annotation =
                _mediator.Send(new CreateAnnotation(userDto.Id, comment, videoId, Double.Parse(timestamp))).GetAwaiter().GetResult();
            annotation.TimestampDisplay = generateTimestampDisplay(annotation.Timestamp);
            
            return new PartialViewResult
            {
                ViewName = "partials/_Annotation",
                ViewData = new ViewDataDictionary<AnnotationDto>(ViewData, annotation)
            };
        }

        private string generateTimestampDisplay(double timestampNumber)
        {
            
            var totalSeconds = Convert.ToInt32(Math.Floor(timestampNumber));
            var minutes = (totalSeconds / 60 < 10) ? "0" + Convert.ToString(totalSeconds / 60) : Convert.ToString(totalSeconds / 60);
            var seconds = (totalSeconds % 60 < 10) ? "0" + Convert.ToString(totalSeconds % 60) : Convert.ToString(totalSeconds % 60);

            return minutes+ ":" + seconds;
        }
    }
}