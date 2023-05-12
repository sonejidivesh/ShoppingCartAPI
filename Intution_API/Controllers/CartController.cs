﻿using Intution_API.DTOModel;
using Intution_API.Models;
using Intution_API.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace Intution_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async  Task<IActionResult> GetCartItem(int customerId)
        {
            return Ok(await _unitOfWork.Carts.GetAll(customerId));
        }


        [HttpPost]
        public async Task<IActionResult> Add(CartDTO cart)
        {
            if (cart == null)
                return BadRequest("Cart cannot be empty");

            var product = await _unitOfWork.Products.GetValueAsync(cart.ProductId);

            Cart cartdetails = new Cart()
            {
                ProductId = cart.ProductId,
                Price =product.Price * cart.Qty,
                Qty= cart.Qty,
                CustomerId= cart.CustomerId
            };

            await _unitOfWork.Carts.Add(cartdetails);
           await _unitOfWork.CompleteAsync();
            return Ok();

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (_unitOfWork.Carts.DeleteCartItem(id))
            {
                await _unitOfWork.CompleteAsync();
                return NoContent();

            }

            return NotFound();
        }


    }
}
