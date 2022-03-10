using AutoMapper;
using estoque_tek.Domains.Interfaces;
using estoque_tek.Domains.Models;
using estoque_tek.Domains.Services;
using estoque_tek.Domains.Types;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace estoque_tek.Web.Controllers
{
    [ApiController]
    [Route("v1/Contractor")]
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

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] LoginUserDtoInputModel loginUserDtoInputModel)
        {
            // Recuperando o usuário
            var user = await usersRepository.GetOneUserAsync(loginUserDtoInputModel.ContractorId, loginUserDtoInputModel.UserName, loginUserDtoInputModel.Password);

            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            // Gerando o Token ao acessar
            var token = TokenService.GenerateToken(user);

            // Ocultando a senha
            user.Password = "";

            // Retornando os dados
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("{contractorId}/Users/Count")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> Count(string contractorId)
        {
            // Buscando contratante
            var user = await contractorRepository.GetOneAsync(contractorId);

            // Verificando se o contratante é nulo
            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            // Retornando a quantidade de users por contratante
            return Ok(await this.usersRepository.CountAsync(contractorId));
        }

        [HttpGet]
        [Route("{contractorId}/Users")]
        [Authorize(Roles = "user")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserOutputModel))]
        public async Task<ActionResult> Get(string contractorId, string userName, int? page)
        {
            var user = await contractorRepository.GetOneAsync(contractorId);

            if (user == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var users = await usersRepository.GetAllAsync(contractorId, userName, page);
            return Ok(users);
        }

        [HttpGet]
        [Route("users/{userId}")]
        [Authorize(Roles = "user")]
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
        [Authorize(Roles = "admin")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInputModel))]
        public async Task<ActionResult> CreateContractor([FromForm] UserInputModel userInputModel)
        {
            User user = this.mapper.Map<User>(userInputModel);

            await usersRepository.CreateAsync(user);

            return CreatedAtAction(nameof(GetOneUser), new { UserId = user.UserId.ToString() }, user);
        }

        [HttpPut]
        [Route("user/{userId}")]
        [Authorize(Roles = "admin")]
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

            existingUser.UserId = userId;

            await usersRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpDelete]
        [Route("user/{userId}")]
        [Authorize(Roles = "admin")]
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
