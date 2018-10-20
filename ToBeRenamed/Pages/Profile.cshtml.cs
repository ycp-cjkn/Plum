using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IMediator _mediator;
        public UserDto UserDto { get; set; }

        public ProfileModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id == null)
            {
                UserDto = await _mediator.Send(new GetSignedInUserDto(User));
            }
            else
            {
                UserDto = await _mediator.Send(new GetUserByUserId(id.Value));
            }
        }
    }
}