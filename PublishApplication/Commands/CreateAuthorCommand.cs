using MediatR;
using PubApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PublishApplication.Commands
{
    public record CreateAuthorCommand(AuthorDto author) : IRequest<AuthorDto>;
}
