using AutoMapper;
using estoque_tek.Domains.Interfaces;
using estoque_tek.Domains.Models;
using estoque_tek.Domains.Types;
using estoque_tek.Models;
using estoque_tek.Web.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace estoque_tek.Web.Controllers
{
    [ApiController]
    [Route("v1/Contractor")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IContractorRepository contractorRepository;

        public ProductController(
            IMapper mapper,
            IProductRepository productRepository,
            IContractorRepository contractorRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.contractorRepository = contractorRepository;
        }

        [HttpGet]
        [Route("{contractorId}/Products/Count")]
        [Authorize(Roles = "user")]
        public async Task<ActionResult> Count(string contractorId)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            return Ok(await this.productRepository.CountAsync(contractorId));
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        [Route("{contractorId}/Products")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductOutputModel))]
        public async Task<ActionResult> Get(string contractorId, string productName, string category, int? page)
        {
            var contractor = await contractorRepository.GetOneAsync(contractorId);

            if (contractor == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ContractorNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var users = await productRepository.GetAllAsync(contractorId, productName, category, page);
            return Ok(users);
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        [Route("Product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductOutputModel))]
        public async Task<ActionResult> GetOneUser(string productId)
        {
            var product = await productRepository.GetOneAsync(productId);

            if (product == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ProductNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [Route("Product")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductInputModel))]
        public async Task<ActionResult> CreateContractor([FromForm] ProductInputModel productInputModel)
        {
            Product product = this.mapper.Map<Product>(productInputModel);

            await productRepository.CreateAsync(product);

            return CreatedAtAction(nameof(GetOneUser), new { ProductId = product.ProductId.ToString() }, product);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        [Route("Product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductInputModel))]
        public async Task<ActionResult> Update(string productId, [FromForm] ProductInputModel productInputModel)
        {
            var product = await this.productRepository.GetOneAsync(productId);

            if (product == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ProductNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            var existingProduct = mapper.Map<Product>(productInputModel);

            existingProduct.ProductId = productId;

            await productRepository.UpdateAsync(existingProduct);

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("Product/{productId}")]
        public async Task<ActionResult> Delete(string productId)
        {
            var product = await productRepository.GetOneAsync(productId);

            if (product == null)
            {
                var notFound = Result.BuildNotFoundResult(ErrorCodeType.ProductNotFound);
                return StatusCode((int)notFound.StatusCode, notFound);
            }

            await productRepository.DeleteAync(productId);
            return NoContent();
        }
    }
}
