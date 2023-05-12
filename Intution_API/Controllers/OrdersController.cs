using Intution_API.DTOModel;
using Intution_API.Models;
using Intution_API.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Intution_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly ConcurrentQueue<Product> _productQueue = new ConcurrentQueue<Product>();
        private bool _isProcessing = false;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int pageSize = 10, int pageIndex = 1)
        {
            IEnumerable<Order> orders = await _unitOfWork.Orders.GetAllAsync(pageSize, pageIndex);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order? order = await _unitOfWork.Orders.GetValueAsync(id);
            if (order is not null)
            {
                return Ok(order);
            }

            return NotFound();

        }



        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(int customerId, string deliveryAddress)
        {
            List<Cart> carts = await _unitOfWork.Carts.GetAll(customerId);
            OrderDTO orderD = new OrderDTO();
            orderD.DeliveryAddress = deliveryAddress;
            orderD.Price = GetCartPrice(carts);
            orderD.Product = new List<ProductDTO>();

            foreach (var item in carts)
            {
                ProductDTO product = new ProductDTO();
                product.Id = item.Product.Id;
                product.ProductName = item.Product.ProductName;
                product.Description = item.Product.Description;
                product.Price = item.Product.Price;
                product.Qty = item.Qty;

                if (item.Qty > item.Product.Qty) {
                return BadRequest("Product in the cart is more than the product available");
                
                }

                orderD.Product.Add(product);
            }



            var request =await PostOrder(orderD);
           
            await _unitOfWork.Carts.DeleteCart(customerId);
            await _unitOfWork.CompleteAsync();
            return request;
        }

        private double GetCartPrice(List<Cart> carts)
        {
            double price = 0;
            foreach (var cart in carts)
            {
                price += cart.Price;
            }

            return price;
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderDTO ordersDetails)
        {
            Order order = new Order()
            {
                Id = ordersDetails.Id,
                DeliveryAddress = ordersDetails.DeliveryAddress,
                IsActive = true,
                Price = ordersDetails.Price,
 
            };

            if (ordersDetails.Product.Count == 0)
            {
                return NotFound("No item in the cart!");
            }

            await _unitOfWork.Orders.Add(order);
            await _unitOfWork.CompleteAsync();


            foreach (var po in ordersDetails.Product)
            {
                if (await UpdateProductQtyFromOrder(po))
                {

                    ProductOrder productOrder = new ProductOrder()
                    {
                        OrderId = order.Id,
                        ProductId = po.Id
                    };

                    await _unitOfWork.ProductOrder.Add(productOrder);

                }
                else
                {
                    return BadRequest("Quantity Mismatch!");
                }



            }
            await _unitOfWork.CompleteAsync();

            return Ok(order);


        }


        private async Task<bool> UpdateProductQtyFromOrder(ProductDTO productDetail)
        {
            var product = await _unitOfWork.Products.GetValueAsync(productDetail.Id);
            _productQueue.Enqueue(product);
            while (_productQueue.TryDequeue(out var productQueud))
            {
                _isProcessing = await _unitOfWork.Products.UpdateQty(product, productDetail);
                if (!_isProcessing)
                {
                    _isProcessing = false;
                    return false;
                }
                
                
            }
            return true;


        }



        [HttpPatch]
        public async Task<IActionResult> UPdateoOrederAddress(OrderDTO orderDetails)
        {

            Order order = new Order()
            {
                Id = orderDetails.Id,
                DeliveryAddress = orderDetails.DeliveryAddress

            };

            if (await _unitOfWork.Orders.UpdateAddress(order) is not null)
            {

                await _unitOfWork.CompleteAsync();
                return Ok(order);
            }


            return NotFound();


        }



        [HttpDelete("DeleteItem")]

        public async Task<IActionResult> DeleteOrderItem(int orderId, int productId)
        {

            Order? od = await _unitOfWork.Orders.GetValueAsync(orderId);

            if (od is not null)
            {
                ProductOrder? productOrderDetail = od.ProductOrder.FirstOrDefault(x => x.OrderId == orderId && x.ProductId == productId);
                if (productOrderDetail is not null)
                {
                    _unitOfWork.ProductOrder.DeleteOrderItem(productOrderDetail);

                    Product? productDetails = await _unitOfWork.Products.GetValueAsync(productId);
                    if (productDetails is not null)
                    {
                        productDetails.Qty += 1;
                    }


                    await _unitOfWork.CompleteAsync();
                    return Ok();

                }
                return NotFound();

            }


            return NotFound();


        }


        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(OrderDTO ordersDetails)
        {

            Order order = new Order()
            {
                Id = ordersDetails.Id,
                DeliveryAddress = ordersDetails.DeliveryAddress,
                IsActive = false,
                Price = ordersDetails.Price

            };
            if (await _unitOfWork.Orders.Delete(order))
            {
                return Ok();

            }

            return NotFound();
        }


    }
}
