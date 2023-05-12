using Intution_API.Data;
using Intution_API.DTOModel;
using Intution_API.Models;
using Intution_API.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Intution_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        //private static List<Product> _products = new List<Product>()
    

        private readonly IUnitOfWork _unitOfWork;
        public ProductController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts(int pageSize =10, int pageIndex = 1)
        {

            IEnumerable<Product> products = await _unitOfWork.Products.GetAllAsync(pageSize,pageIndex);
            return Ok(products);

        }


        [HttpGet("{id}")]
        public async Task< IActionResult> GetProductById(int id)
        {
            Product? product = await _unitOfWork.Products.GetValueAsync(id);
            if (product is not null) {

                return Ok(product);
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> PostProductById(ProductDTO productDetail)
        {
            Product product = new Product()
            {
                Id = productDetail.Id,
                ProductName = productDetail.ProductName,
                Description = productDetail.Description,
                Qty = productDetail.Qty,
                Price = productDetail.Price,
                IsActive = productDetail.IsActive

            };
            
            await _unitOfWork.Products.Add(product);
            await _unitOfWork.CompleteAsync();
            return Ok(product);

        }

        [HttpDelete]
        public async  Task<IActionResult> DeleteProduct(ProductDTO productDetail)
        {
            Product product = new Product()
            {
                Id = productDetail.Id,
                ProductName = productDetail.ProductName,
                Description = productDetail.Description,
                Qty = productDetail.Qty,
                Price = productDetail.Price,
                IsActive = productDetail.IsActive

            };


            if ( await _unitOfWork.Products.Delete(product))
            {
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            
            return NotFound();
        }


        [HttpPatch]
       public async Task< IActionResult> UpdateProduct(ProductDTO productDetail)
        {
            Product product = new Product()
            {
                Id = productDetail.Id,
                ProductName = productDetail.ProductName,
                Description = productDetail.Description,
                Qty = productDetail.Qty,
                Price = productDetail.Price,
                IsActive = productDetail.IsActive

            };

            if (await _unitOfWork.Products.Update(product))
            {
                await _unitOfWork.CompleteAsync();
                return NoContent();
            }
            return NotFound();

        }

    }
}
