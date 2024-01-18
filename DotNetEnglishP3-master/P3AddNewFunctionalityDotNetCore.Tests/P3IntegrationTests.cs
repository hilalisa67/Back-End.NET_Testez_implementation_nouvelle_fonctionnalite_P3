using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        if (product != null)
        {
            _context.Product.Remove(product);
            _context.SaveChanges();
        }
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

public class P3IntegrationTests
{
    private readonly DbContextOptions<P3Referential> _options;
    private readonly IConfiguration _configuration;

    public P3IntegrationTests()
    {
        var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables();
        _configuration = configBuilder.Build();

        _options = new DbContextOptionsBuilder<P3Referential>()
            .UseSqlServer(_configuration.GetConnectionString("P3Referential"))
            .Options;
    }


    [Fact]
    public void TestProductUpdatesAreReflectedOnClientSide()
    {
        // Arrange
        using var context = new P3Referential(_options, _configuration);
        using var transaction = context.Database.BeginTransaction();

        var adminController = new AdminController(context);
        var clientController = new ClientController(context, new Cart());
        var product = new Product
        {
            Name = "Test Product", Price = 10.0, Quantity = 3, Description = "TestProductUpdatesAreReflectedOnClientSide",
            Details = "TestProductUpdatesAreReflectedOnClientSide"
        };

        // Act
        adminController.AddProduct(product);
        var clientProducts = clientController.GetProducts();

        // Assert
        Assert.Contains(clientProducts, p => p.Name == product.Name && p.Price == product.Price);

        // Act
        product = clientProducts.First(p => p.Name == product.Name);
        product.Price = 20.0;
        adminController.UpdateProduct(product);
        clientProducts = clientController.GetProducts();

        // Assert
        Assert.Contains(clientProducts, p => p.Name == product.Name && p.Price == product.Price);

        // Act
        adminController.DeleteProduct(product.Id);
        clientProducts = clientController.GetProducts();

        // Assert
        Assert.DoesNotContain(clientProducts, p => p.Id == product.Id);

        transaction.Rollback();
    }


    [Fact]
    public void TestDeleteNonExistentProduct()
    {
        // Arrange
        using var context = new P3Referential(_options, _configuration);
        using var transaction = context.Database.BeginTransaction();

        var adminController = new AdminController(context);

        // Act
        adminController.DeleteProduct(9999); // ID non existant

        // Assert
        // Aucune exception ne doit être levée

        transaction.Rollback();
    }

    [Fact]
    public void TestAddProductToCart()
    {
        // Arrange
        using var context = new P3Referential(_options, _configuration);
        using var transaction = context.Database.BeginTransaction();

        var adminController = new AdminController(context);
        var clientController = new ClientController(context, new Cart());
        var product = new Product
        {
            Name = "Test Product", Price = 10.0, Quantity = 3, Description = "Test Description",
            Details = "TestAddProductToCart"
        };
        adminController.AddProduct(product);

        // Act
        clientController.AddToCart(product.Id, 1);

        // Assert
        var cart = clientController.GetCart();
        Assert.Contains(cart.Lines,
            c => c.Product.Name == product.Name && c.Product.Price == product.Price && c.Quantity == 1);

        // Act
        clientController.ValidateCart();
        var lastOrder = context.Order.OrderByDescending(o => o.Id).Include(order => order.OrderLine).FirstOrDefault();

        // Assert
        // vérifier que le produit a bien été ajouté à la commande
        Assert.Contains(lastOrder.OrderLine, o => o.ProductId == product.Id);

        transaction.Rollback();
    }
}