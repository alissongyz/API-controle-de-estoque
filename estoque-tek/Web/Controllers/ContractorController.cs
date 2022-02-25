using AutoMapper;
using estoque_tek.Domains.Interfaces;
using estoque_tek.Domains.Models;
using estoque_tek.Domains.Types;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace estoque_tek.Web.Controllers
{
    [ApiController]
    [Route("v1/contractors")]
    public class ContractorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IContractorRepository contractorRepository;

        public ContractorController (
            IMapper mapper,
            IContractorRepository contractorRepository)
        {
            this.mapper = mapper;
            this.contractorRepository = contractorRepository;
        }

        [HttpGet]
        [Route("Count")]
        public async Task<IActionResult> Count()
        {
            return Ok(await this.contractorRepository.CountAsync());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractorOutputModel))]
        public async Task<ActionResult> GetAllContractors(string displayName, string cnpj, int? page)
        {
            var contractors = await contractorRepository.GetAllAsync(displayName, cnpj, page);
            return Ok(contractors);
        }

        [HttpGet]
        [Route("{contractorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractorOutputModel))]
        public async Task<ActionResult> GetOneContractor(string contractorId)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotExisting);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            return Ok(contractor);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractorInputModel))]
        public async Task<ActionResult> CreateContractor([FromForm] ContractorInputModel contractorInputModel)
        {
            Contractor contractor = this.mapper.Map<Contractor>(contractorInputModel);

            await contractorRepository.CreateAsync(contractor);

            return CreatedAtAction(nameof(GetOneContractor), new { ContractorId = contractor.ContractorId.ToString() }, contractor);
        }

        [HttpPut]
        [Route("{contractorId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ContractorInputModel))]
        public async Task<ActionResult> UpdateContractor(string contractorId, [FromForm] ContractorInputModel contractorInputModel)
        {
            var contractor = await this.contractorRepository.GetOneAsync(contractorId);
            
            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotExisting);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var existingContractor = mapper.Map<Contractor>(contractorInputModel);

            existingContractor.ContractorId = contractorId;

            await contractorRepository.UpdateAsync(existingContractor);

            return NoContent();
        }

        [HttpDelete]
        [Route("{contractorId}")]
        public async Task<ActionResult> DeleteContractor(string contractorId)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotExisting);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            await contractorRepository.DeleteAync(contractorId);
            return NoContent();
        }
    }
}
