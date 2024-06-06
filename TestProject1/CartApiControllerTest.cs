using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Controllers;
using Services.Interfaces;
using Services.ViewModels;

namespace TestProject1
{
    public class CartApiControllerTest
    {



        [Fact]
        public async Task AddToCart_AddsProductToCart_ReturnsOk()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            mockCartService.Setup(service => service.AddToCartAsync(It.IsAny<AddToCartRequest>()))
                           .ReturnsAsync(true);
            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);
            var addToCartRequest = new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 2
            };

            // Act
            var result = await controller.AddToCart(addToCartRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Changed to OkObjectResult
            Assert.NotNull(okResult.Value);  // Optionally, you can check the value if needed
        }

        [Fact]
        public async Task AddToCart_NullRequest_ReturnsBadRequest()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.AddToCart(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task AddToCart_FailsToAddProduct_ReturnsServerError()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            mockCartService.Setup(service => service.AddToCartAsync(It.IsAny<AddToCartRequest>()))
                           .ReturnsAsync(false);

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);
            var addToCartRequest = new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 2
            };

            // Act
            var result = await controller.AddToCart(addToCartRequest);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);  // Expecting ObjectResult
            Assert.Equal(500, objectResult.StatusCode);  // Ensure it's a server error
            Assert.NotNull(objectResult.Value);
        }

        [Fact]
        public async Task CalculateTotalCost_ReturnsCorrectTotal()
        {
            var calculate = new CreatePurchaseOrderRequest();
            // Arrange
            var mockCartService = new Mock<ICartService>();
            decimal expectedTotalCost = 100.00m;
            mockCartService.Setup(service => service.CalculateTotalCostAsync(calculate))
                           .ReturnsAsync(expectedTotalCost);

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.CalculateTotal(calculate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedTotalCost, okResult.Value);
        }

        [Fact]
        public async Task CalculateTotalCost_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var calculate = new CreatePurchaseOrderRequest();
            // Arrange
            var mockCartService = new Mock<ICartService>();
            mockCartService.Setup(service => service.CalculateTotalCostAsync(calculate))
                           .ThrowsAsync(new System.Exception("Test exception"));

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.CalculateTotal(calculate);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal Server while calculating the total cost.", objectResult.Value); // Adjust the expected error message here
        }

     
     
        [Fact]
        public async Task GetOrderItems_ReturnsOrderItems()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();

            var expectedOrderItems = new List<OrderItem>()
            {
                new OrderItem { Id= 1, OrderId= 1, Quantity= 2 }
            };

            var getOrderItems = new AddToCartRequest
            {
                Id = 1,
                OrderId = 1,
                Quantity = 2
            };
            mockCartService.Setup(service => service.GetOrderItemsAsync(getOrderItems))
                           .ReturnsAsync(expectedOrderItems);

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.GetOrderItems(getOrderItems);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(expectedOrderItems, okResult.Value);
        }

     

        [Fact]
        public async Task GetOrderItems_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            var getOrderItems = new AddToCartRequest
            {
                Id = 1,
                OrderId = 1,
                Quantity = 2
            };
            // Arrange
            var mockCartService = new Mock<ICartService>();

            mockCartService.Setup(service => service.GetOrderItemsAsync(getOrderItems))
                           .ThrowsAsync(new System.Exception("Test exception"));

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.GetOrderItems(getOrderItems);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while retrieving the order items.", statusCodeResult.Value);
        }

        [Fact]
        public async Task RemoveFromCart_ProductExists_ReturnsOk()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            int OrderItemId = 1;
            mockCartService.Setup(service => service.RemoveFromCartAsync(OrderItemId))
                           .ReturnsAsync(true);

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.RemoveFromCart(OrderItemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Product removed from cart.", okResult.Value);
        }

        [Fact]
        public async Task RemoveFromCart_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            int OrderItemId = 99; // Assume product ID 99 does not exist
            mockCartService.Setup(service => service.RemoveFromCartAsync(OrderItemId))
                           .ReturnsAsync(false);

            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.RemoveFromCart(OrderItemId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Product not found in cart.", notFoundResult.Value);
        }

        [Fact]
        public async Task RemoveFromCart_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var mockCartService = new Mock<ICartService>();
            int OrderItemId = 1;
            mockCartService.Setup(service => service.RemoveFromCartAsync(OrderItemId))
                           .ThrowsAsync(new System.Exception("Test exception"));
            var _logger = new Mock<ILogger<CartItemsController>>();
            var controller = new CartItemsController(mockCartService.Object, _logger.Object);

            // Act
            var result = await controller.RemoveFromCart(OrderItemId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while removing the product from the cart.", statusCodeResult.Value);
        }
    }

}
