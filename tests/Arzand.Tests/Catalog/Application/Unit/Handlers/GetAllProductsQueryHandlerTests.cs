using System;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Application.Queries;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Shared.Domain;
using AutoMapper;
using Moq;

namespace Arzand.Tests.Catalog.Application.Handlers;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllProductsQueryHandler _handler;

    public GetAllProductsQueryHandlerTests()
    {
        _productRepoMock = new Mock<IProductRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllProductsQueryHandler(_productRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMappedProducts()
    {
        // Arrange
        var categoryId = 10;
        var brandId = Guid.NewGuid();
        var products = new List<Product> 
        { 
            Product.Create("Test Product", "desc", categoryId, brandId, []) 
        };
        _productRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        var productDtos = new List<ProductDto>
        {
            new ProductDto { Id = products[0].Id, Name = "Test Product", Description = "desc", CategoryId = categoryId, BrandId = brandId}
        };

        _mapperMock.Setup(m => m.Map<List<ProductDto>>(products)).Returns(productDtos);

        // Act
        var result = await _handler.Handle(new GetAllProductsQuery(), default);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Product", result[0].Name);
    }
}