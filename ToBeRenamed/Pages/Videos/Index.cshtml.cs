using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
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

        public VideoDto Video { get; set; }

        public IEnumerable<AnnotationDto> Annotations { get; set; }

        public async Task OnGetAsync(int id)
        {
            Video = await _mediator.Send(new GetVideoById(id));
            Annotations = await _mediator.Send(new GetAnnotationsByVideoId(id));
        }

        public async Task<PartialViewResult> OnPostCreateAnnotation(int videoId, string comment, string timestamp)
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));

            var createAnnotation = new CreateAnnotation(userDto.Id, comment, videoId, double.Parse(timestamp));
            var annotation = await _mediator.Send(createAnnotation);

            // TODO - Use membership display name instead of users table display name
            annotation.DisplayName = userDto.DisplayName;

            return new PartialViewResult
            {
                ViewName = "_Annotation",
                ViewData = new ViewDataDictionary<AnnotationDto>(ViewData, annotation)
            };
        }
    }
}