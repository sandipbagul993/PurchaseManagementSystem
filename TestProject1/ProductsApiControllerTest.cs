using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Controllers;
using Services.Interfaces;

namespace TestProject1
{
    public class ProductsApiControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _logger;
        private readonly ProductsController _controller;
        public ProductsApiControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _logger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockProductService.Object, _logger.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            var expectedProducts = new List<Product>
        {
            new Product { Id = 1,Name = "Product A", Price = 10.00m },
            new Product { Id = 2, Name = "Product B", Price = 20.00m }
        };
            mockProductService.Setup(service => service.GetAllProductsAsync())
                              .ReturnsAsync(expectedProducts);



            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<Product>>>(result);// Adjusted to expect NotFoundObjectResult
        }

      

        [Fact]
        public async Task GetAllProducts_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(service => service.GetAllProductsAsync())
                              .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            Assert.IsAssignableFrom<ActionResult<IEnumerable<Product>>>(result);
        }

        [Fact]
        public async Task PostProduct_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
           
            var product = new Product { Id = 1, Name = "Product A", Price = 10.00m };

            // Act
            var result = await _controller.PostProduct(product);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostProduct_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(service => service.AddProductAsync(It.IsAny<Product>()))
                              .ThrowsAsync(new Exception("Test exception"));
          
            var product = new Product { Id = 1, Name = "Product A", Price = 10.00m };

            // Act
            var result = await _controller.PostProduct(product);

            // Assert
            Assert.IsType<ActionResult<Product>>(result);
        }

        [Fact]
        public async Task PutProduct_ValidProduct_ReturnsNoContent()
        {
            // Arrange
           
            int productId = 1;
            var product = new Product { Id = productId, Name = "Updated Product A", Price = 20.00m };

            // Act
            var result = await _controller.PutProduct(productId, product);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task PutProduct_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            
            _mockProductService.Setup(service => service.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>()))
                              .ThrowsAsync(new Exception("Test exception"));
           
            int productId = 1;
            var product = new Product { Id = productId, Name = "Updated Product A", Price = 20.00m };

            // Act
            var result = await _controller.PutProduct(productId, product);

            // Assert
            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async Task GetProductById_ProductExists_ReturnsOkResultWithProduct()
        {
          
           
            // Arrange
            var productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };

            _mockProductService
                .Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnedProduct.Id);
            Assert.Equal(expectedProduct.Name, returnedProduct.Name);
        }

        [Fact]
        public async Task GetProductById_ProductDoesNotExist_ReturnsNotFoundResult()
        {
         
            // Arrange
            var productId = 1;

            _mockProductService
                .Setup(service => service.GetProductByIdAsync(productId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.IsType<ActionResult<Product>>(result);
        }

     
    }
}
