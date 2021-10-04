using ILoveBaku.Application.Common.Interfaces;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Base;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILoveBaku.Application.CQRS.User.Queries.GetUser
{
    public class GetUserQuery : BaseRequest<ApiResult<UserDto>>
    {
        public string Email { get; set; }
        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApiResult<UserDto>>
        {
            private readonly IIdentityService _identityService;
            public GetUserQueryHandler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<ApiResult<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var response = await _identityService.GetUserAsync(c=>(request.Email!=null?c.ContactEmail == request.Email:true)&& (request.UserId!=Guid.Empty?c.Id == request.UserId:true));
                if (response != null)
                {
                    UserDto dto = new UserDto
                    {
                        Name = response.Name,
                        Surname = response.Surname,
                        Email = response.ContactEmail,
                        Phone = response.Phone,
                        Day = response.Birthday?.Day.ToString(),
                        Month = response.Birthday?.Month.ToString(),
                        Year = response.Birthday?.Year.ToString(),
                        Gender = response.Gender,
                        UserId = response.Id
                    };

                    return ApiResult<UserDto>.CreateResponse(dto);
                }

                return ApiResult<UserDto>.CreateResponse(null);

            }
        }
    }
}
