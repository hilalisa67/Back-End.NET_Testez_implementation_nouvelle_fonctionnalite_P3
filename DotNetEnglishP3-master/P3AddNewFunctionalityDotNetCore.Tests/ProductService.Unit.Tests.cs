using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _service;

        public ProductServiceUnitTests()
        {
            var mockCart = new Mock<ICart>();
            var mockProductRepository = new Mock<IProductRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            _service = new ProductService(mockCart.Object, mockProductRepository.Object, mockOrderRepository.Object,
                mockLocalizer.Object);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateProduct_ReturnsError_WhenNameIsEmpty(string name,
            string expectedErrorMessage = "Veuillez saisir un nom")
        {
            // Arrange
            var product = new ProductViewModel {Name = name};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateProduct_ReturnsMissingPrice_WhenPriceIsEmpty(string price,
            string expectedErrorMessage = "Veuillez saisir un prix")
        {
            // Arrange
            var product = new ProductViewModel {Price = price};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("1.2.3")]
        [InlineData("1,2,3")]
        public void CreateProduct_ReturnsPriceNotANumber_WhenPriceIsNotANumber(string price,
            string expectedErrorMessage = "La valeur saisie pour le prix doit être un nombre positif")
        {
            // Arrange
            var product = new ProductViewModel {Price = price};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("0,0")]
        [InlineData("-1")]
        public void CreateProduct_ReturnsPriceNotGreaterThanZero_WhenPriceIsZero(string price,
            string expectedErrorMessage = "Le prix doit être supérieur à zéro")
        {
            // Arrange
            var product = new ProductViewModel {Price = price};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }


        [Theory]
        [InlineData("abc")]
        [InlineData("1,2")]
        [InlineData("1.2")]
        public void CreateProduct_ReturnsQuantityNotAnInteger_WhenQuantityIsNotAnInteger(string quantity,
            string expectedErrorMessage = "Seuls les nombres entiers sont autorisés pour le stock")
        {
            // Arrange
            var product = new ProductViewModel {Stock = quantity};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("-1")]
        [InlineData("0.2")]
        [InlineData("0,2")]
        public void CreateProduct_ReturnsQuantityNotGreaterThanZero_WhenQuantityIsZero(string quantity,
            string expectedErrorMessage = "La quantité doit être supérieure à zéro")
        {
            // Arrange
            var product = new ProductViewModel {Stock = quantity};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateProduct_ReturnsMissingStock_WhenStockIsEmpty(string stock,
            string expectedErrorMessage = "Veuillez saisir un stock")
        {
            // Arrange
            var product = new ProductViewModel {Stock = stock};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == expectedErrorMessage);
        }
    }
}