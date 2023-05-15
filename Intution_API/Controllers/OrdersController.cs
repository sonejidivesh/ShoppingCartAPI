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

        /// <summary>
        /// This function gets All orders
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>It return a json format of the list</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int customerId ,int pageSize = 10, int pageIndex = 1 )
        {
            IEnumerable<Order> orders = await _unitOfWork.Orders.GetAllOrderBasedonCutomerIdAsync(customerId , pageSize, pageIndex);
            return Ok(orders);
        }



        /// <summary>
        /// This function gets single order based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Return single object with status OK</returns>
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


        /// <summary>
        /// This function post the customers cart into the ordering service
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="deliveryAddress"></param>
        /// <returns>returns status of update  </returns>
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(int customerId, string deliveryAddress)
        {
            List<Cart> carts = await _unitOfWork.Carts.GetAll(customerId);
            OrderDTO orderD = new OrderDTO();
            orderD.DeliveryAddress = deliveryAddress;
            orderD.Price = GetCartPrice(carts);
            orderD.Product = new List<ProductDTO>();
            orderD.CustomerId = customerId;
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

        /// <summary>
        /// This function generates the total cost of the cart by running through the cart 
        /// </summary>
        /// <param name="carts"></param>
        /// <returns> returns the total price of the cart</returns>
        private double GetCartPrice(List<Cart> carts)
        {
            double price = 0;
            foreach (var cart in carts)
            {
                price += cart.Price;
            }

            return price;
        }
        /// <summary>
        /// This function post the order 
        /// </summary>
        /// <param name="ordersDetails"></param>
        /// <returns>returns status code with the json object</returns>
        [HttpPost]
        public async Task<IActionResult> PostOrder(OrderDTO ordersDetails)
        {
            Order order = new Order()
            {
                Id = ordersDetails.Id,
                DeliveryAddress = ordersDetails.DeliveryAddress,
                IsActive = true,
                Price = ordersDetails.Price,
                CustomerId = ordersDetails.CustomerId,
 
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
        /// <summary>
        /// Update the product qty when order is placed
        /// </summary>
        /// <param name="productDetail"></param>
        /// <returns>returns boolean </returns>

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

        /// <summary>
        /// This function updates the order address
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>

        [HttpPatch]
        public async Task<IActionResult> UPdateoOrederAddress(OrderDTO orderDetails)
        {

            Order order = new Order()
            {
                Id = orderDetails.Id,
                DeliveryAddress = orderDetails.DeliveryAddress,
                CustomerId = orderDetails.CustomerId

            };

            if (await _unitOfWork.Orders.UpdateAddress(order) is not null)
            {

                await _unitOfWork.CompleteAsync();
                return Ok(order);
            }


            return NotFound();


        }

        /// <summary>
        /// This functions Delete the order item 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns> returns the status code of No Content and Not found</returns>

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
        /// <summary>
        /// This function delete's the entier order
        /// </summary>
        /// <param name="ordersDetails"></param>
        /// <returns></returns>

        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(OrderDTO ordersDetails)
        {

            Order order = new Order()
            {
                Id = ordersDetails.Id,
                DeliveryAddress = ordersDetails.DeliveryAddress,
                IsActive = false,
                Price = ordersDetails.Price,
                CustomerId = ordersDetails.CustomerId,

            };
            if (await _unitOfWork.Orders.Delete(order))
            {
                return Ok();

            }

            return NotFound();
        }


    }
}
