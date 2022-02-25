using AutoMapper;
using estoque_tek.Domains.Interfaces;
using estoque_tek.Domains.Models;
using estoque_tek.Domains.Types;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace estoque_tek.Web.Controllers
{
    [ApiController]
    [Route("v1/contractor")]
    public class UserController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUsersRepository usersRepository;
        private readonly IContractorRepository contractorRepository;

        public UserController(
            IMapper mapper,
            IUsersRepository usersRepository,
            IContractorRepository contractorRepository)
        {
            this.mapper = mapper;
            this.usersRepository = usersRepository;
            this.contractorRepository = contractorRepository;
        }

        [HttpGet]
        [Route("{contractorId}/Users/Count")]
        public async Task<ActionResult> Count(string contractorId)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            return Ok(await this.usersRepository.CountAsync(contractorId));
        }

        [HttpGet]
        [Route("{contractorId}/Users")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputModel))]
        public async Task<ActionResult> Get(string contractorId, string userName, int? page)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var users = await usersRepository.GetAllAsync(contractorId, userName, page);
            return Ok(users);
        }

        [HttpGet]
        [Route("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputModel))]
        public async Task<ActionResult> GetOneUser(string userId)
        {
            var user = await usersRepository.GetOneAsync(userId);

            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.UserNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInputModel))]
        public async Task<ActionResult> CreateContractor([FromForm] UserInputModel userInputModel)
        {
            User user = this.mapper.Map<User>(userInputModel);

            await usersRepository.CreateAsync(user);

            return CreatedAtAction(nameof(GetOneUser), new { UserId = user.UserId.ToString() }, user);
        }

        [HttpPut]
        [Route("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInputModel))]
        public async Task<ActionResult> Update(string userId, [FromForm] UserInputModel userInputModel)
        {
            var user = await this.usersRepository.GetOneAsync(userId);

            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.UserNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var existingUser = mapper.Map<User>(userInputModel);

            existingUser.ContractorId = userId;

            await usersRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpDelete]
        [Route("user/{userId}")]
        public async Task<ActionResult> Delete(string userId)
        {
            var user = await usersRepository.GetOneAsync(userId);

            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.UserNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            await usersRepository.DeleteAync(userId);
            return NoContent();
        }
    }
}
