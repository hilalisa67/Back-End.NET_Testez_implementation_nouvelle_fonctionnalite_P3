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
            Assert.Contains(errors, error => error.ErrorMessage == "Veuillez saisir un nom");
        }

        [Fact]
        public void CreateProduct_ReturnsMissingPrice_WhenPriceIsEmpty()
        {
            // Arrange
            var product = new ProductViewModel {Price = ""};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "Veuillez saisir un prix");
        }


        [Fact]
        public void CreateProduct_ReturnsPriceNotANumber_WhenPriceIsNotANumber()
        {
            // Arrange
            var product = new ProductViewModel {Price = "abc"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "La valeur saisie pour le prix doit être un nombre positif");
        }

        [Fact]
        public void CreateProduct_ReturnsPriceNotGreaterThanZero_WhenPriceIsZero()
        {
            // Arrange
            var product = new ProductViewModel {Price = "0"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "Le prix doit être supérieur à zéro");
        }

        [Fact]
        public void CreateProduct_ReturnsMissingQuantity_WhenQuantityIsEmpty()
        {
            // Arrange
            var product = new ProductViewModel {Stock = ""};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "Veuillez saisir un stock");
        }

        [Fact]
        public void CreateProduct_ReturnsQuantityNotAnInteger_WhenQuantityIsNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel {Stock = "abc"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "Seuls les nombres entiers sont autorisés pour le stock");
        }

        [Fact]
        public void CreateProduct_ReturnsQuantityNotGreaterThanZero_WhenQuantityIsZero()
        {
            // Arrange
            var product = new ProductViewModel {Stock = "0"};

            // Act
            var errors = _service.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(errors, error => error.ErrorMessage == "La quantité doit être supérieure à zéro");
        }
    }
}