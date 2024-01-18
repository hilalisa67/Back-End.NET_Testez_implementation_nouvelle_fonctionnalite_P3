using Microsoft.Extensions.Localization;
using Moq;

using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private readonly ProductService _service;


        public ProductServiceTests()
        {
            var mockCart = new Mock<ICart>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            _service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object,
                mockLocalizer.Object);
        }
        
        [Fact]
        public void CreateProduct_ReturnsMissingName_WhenNameIsEmpty()
        {
            // Arrange
            var product = new ProductViewModel {Name = ""};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingName", errors);
        }

        [Fact]
        public void CreateProduct_ReturnsMissingPrice_WhenPriceIsEmpty()
        {
            // Arrange
            var product = new ProductViewModel {Price = ""};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingPrice", errors);
        }
        
        [Fact]
        public void CreateProduct_ReturnsPriceNotANumber_WhenPriceIsNotANumber()
        {
            // Arrange
            var product = new ProductViewModel {Price = "abc"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotANumber", errors);
        }

        [Fact]
        public void CreateProduct_ReturnsPriceNotGreaterThanZero_WhenPriceIsZero()
        {
            // Arrange
            var product = new ProductViewModel {Price = "0"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotGreaterThanZero", errors);
        }

        [Fact]
        public void CreateProduct_ReturnsMissingQuantity_WhenQuantityIsEmpty()
        {
            // Arrange
            var product = new ProductViewModel {Stock = ""};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingQuantity", errors);
        }

        [Fact]
        public void CreateProduct_ReturnsQuantityNotAnInteger_WhenQuantityIsNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel {Stock = "abc"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotAnInteger", errors);
        }

        [Fact]
        public void CreateProduct_ReturnsQuantityNotGreaterThanZero_WhenQuantityIsZero()
        {
            // Arrange
            var product = new ProductViewModel {Stock = "0"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotGreaterThanZero", errors);
        }
    }
}