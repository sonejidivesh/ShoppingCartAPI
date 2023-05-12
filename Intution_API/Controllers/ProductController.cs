using Intution_API.Data;
using Intution_API.DTOModel;
using Intution_API.Models;
using Intution_API.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Xml;

namespace Intution_API.Controllers
{
    /// <summary>
    /// Thi sis the produc endpoint which has 5 endpoints GET, POST, GET{ID}, DELETE,PATCH
    /// </summary>
    

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public ProductController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// This function get a list of all products and the orders associated to it
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns> It return an action result and convert the list into json format </returns>
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts(int pageSize =10, int pageIndex = 1)
        {

            IEnumerable<Product> products = await _unitOfWork.Products.GetAllAsync(pageSize,pageIndex);
            return Ok(products);

        }


        /// <summary>
        /// This function gets a single product based on an id that 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> This returns an object of Product in json format. If record does not exist it send Not found in repsone</returns>
        [HttpGet("{id}")]
        public async Task< IActionResult> GetProductById(int id)
        {
            Product? product = await _unitOfWork.Products.GetValueAsync(id);
            if (product is not null) {

                return Ok(product);
            }

            return NotFound();

        }


        /// <summary>
        /// This inserts product into the database
        /// </summary>
        /// <param name="productDetail"></param>
        /// <returns> it return status 200 with the object so we can utlize the id generated</returns>
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


        /// <summary>
        /// This function is an endpoint to delete
        /// </summary>
        /// <param name="productDetail"></param>
        /// <returns> it return no content and not found if record does not exist</returns>
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



        /// <summary>
        /// This function is used to update the record details 
        /// </summary>
        /// <param name="productDetail"></param>
        /// <returns></returns>
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
