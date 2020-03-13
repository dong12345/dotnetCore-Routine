using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.api.DtoModel;
using Routine.api.Entities;
using Routine.api.helpers;
using Routine.api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.api.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanyCollectionsController:ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRespository _companyRespository;

        public CompanyCollectionsController(IMapper mapper,ICompanyRespository companyRespository)
        {
            this._mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            this._companyRespository = companyRespository??throw new ArgumentNullException(nameof(companyRespository));
        }

        /// <summary>
        /// 根据公司id集合获得公司集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}",Name =nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection(
            [FromRoute] 
            [ModelBinder(BinderType =typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var entities = await _companyRespository.GetCompaniesAsync(ids);
            if (ids.Count() != entities.Count())
            {
                return NotFound();
            }
            var dtoReturns = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            return Ok(dtoReturns);
        }

        /// <summary>
        /// 添加公司集合
        /// </summary>
        /// <param name="companycollection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ICollection<CompanyDto>>> AddCompanyCollections(ICollection<CompanyAddDto> companycollection)
        {
            var companyEntities = _mapper.Map<ICollection<Company>>(companycollection);
            foreach (var item in companyEntities)
            {
                _companyRespository.AddCompany(item);
            }

            await _companyRespository.SaveAsync();
            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var idsString = string.Join(",", dtosToReturn.Select(x => x.Id));
            return CreatedAtRoute(nameof(GetCompanyCollection), new { ids = idsString }, dtosToReturn);
        }
    }
}
