using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.Application.Common.Models;
using ILoveBaku.Application.CQRS.Country.Queries.GetCitiesByCountryId;
using ILoveBaku.Application.CQRS.Country.Queries.GetCountries;
using ILoveBaku.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.API.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<ApiResult<List<Countries>>>> GetCountries()
        {
            return await Mediator.Send(new GetCountriesQuery());
        }

        [HttpGet("{countryId}/cities")]
        public async Task<ActionResult<ApiResult<List<Regions>>>> GetCitiesByCountryId(int countryId)
        {
            return await Mediator.Send(new GetCitiesByCountryIdQuery { CountryId = countryId });
        }
    }
}
