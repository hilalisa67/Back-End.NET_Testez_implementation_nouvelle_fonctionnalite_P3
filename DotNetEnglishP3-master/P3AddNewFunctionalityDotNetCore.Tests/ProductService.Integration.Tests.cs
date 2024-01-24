using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
 using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests;

public class AdminController
{
    private readonly P3Referential _context;

    public AdminController(P3Referential context)
    {
        _context = context;
    }

    public void AddProduct(Product product)
    {
        _context.Product.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product product)
    {
        _context.Product.Update(product);
        _context.SaveChanges();
    }

    public void DeleteProduct(int id)
    {
        var product = _context.Product.Find(id);
        if (product == null) return;
        _context.Product.Remove(product);
        _context.SaveChanges();
    }
}

public class ClientController
{
    private readonly P3Referential _context;
    private readonly Cart _cart;

    public ClientController(P3Referential context, Cart cart)
    {
        _context = context;
        _cart = cart;
    }

    public List<Product> GetProducts()
    {
        return _context.Product.ToList();
    }

    public void AddToCart(int productId, int quantity)
    {
        var product = _context.Product.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            _cart.AddItem(product, quantity);
        }
    }

    public Cart GetCart()
    {
        return _cart;
    }

    public void ValidateCart()
    {
        var order = new Order
            {Name = "John Do", Address = "7 rue des lilas", City = "Strasbourg", Country = "France", Zip = "67000"};
        foreach (var line in _cart.Lines)
        {
            order.OrderLine.Add(new OrderLine {Product = line.Product, Quantity = line.Quantity});
        }

        _context.Order.Add(order);
        _context.SaveChanges();
    }
}

public class ProductServiceIntegrationTests: IDisposable
{
    private readonly P3Referential _context;
    private readonly AdminController _adminController;
    private readonly ClientController _clientController;

    public ProductServiceIntegrationTests()
    {
        var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
        var configuration = configBuilder.Build();

        var options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(configuration.GetConnectionString("P3Referential"))
            .Options;

        _context = new P3Referential(options, configuration);
        _adminController = new AdminController(_context);
        _clientController = new ClientController(_context, new Cart());
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Theory]
    [InlineData("Test Product 1", 10.0, 3, "TestProductUpdatesAreReflectedOnClientSide", "Add data")]
    [InlineData("Test Product 2", 20.0, 5, "TestProductUpdatesAreReflectedOnClientSide", "Add data")]
    [InlineData("Test Product 3", 30.0, 7, "TestProductUpdatesAreReflectedOnClientSide", "Add data")]
    public Task TestProductUpdatesAreReflectedOnClientSide(string name, double price, int quantity, string description,
        string details)
    {
        // Arrange
        var product = new Product
        {
            Name = name,
            Price = price,
            Quantity = quantity,
            Description = description,
            Details = details
        };

        // Act
        _adminController.AddProduct(product);
        var clientProducts = _clientController.GetProducts();

        // Assert
        Assert.Contains(clientProducts, p => p.Name == product.Name && p.Price == product.Price);

        // Act
        product = clientProducts.First(p => p.Name == product.Name);
        product.Price = 20.0;
        product.Details = "Update data";
        _adminController.UpdateProduct(product);
        clientProducts = _clientController.GetProducts();

        // Assert
        Assert.Contains(clientProducts, p => p.Name == product.Name && p.Price == product.Price);

        // Act
        _adminController.DeleteProduct(product.Id);
        clientProducts = _clientController.GetProducts();

        // Assert
        Assert.DoesNotContain(clientProducts, p => p.Id == product.Id);
        return Task.CompletedTask;
    }
}