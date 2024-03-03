using Microsoft.EntityFrameworkCore;
using ProductConsoleAPI.Business;
using ProductConsoleAPI.Business.Contracts;
using ProductConsoleAPI.Data.Models;
using ProductConsoleAPI.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace ProductConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
    {
        private TestProductsDbContext dbContext;
        private IProductsManager productsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestProductsDbContext();
            this.productsManager = new ProductsManager(new ProductsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddProductAsync_ShouldAddNewProduct()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            await productsManager.AddAsync(newProduct);

            var dbProduct = await this.dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == newProduct.ProductCode);

            Assert.NotNull(dbProduct);
            Assert.AreEqual(newProduct.ProductName, dbProduct.ProductName);
            Assert.AreEqual(newProduct.Description, dbProduct.Description);
            Assert.AreEqual(newProduct.Price, dbProduct.Price);
            Assert.AreEqual(newProduct.Quantity, dbProduct.Quantity);
            Assert.AreEqual(newProduct.OriginCountry, dbProduct.OriginCountry);
            Assert.AreEqual(newProduct.ProductCode, dbProduct.ProductCode);
        }

        //Negative test
        [Test]
        public async Task AddProductAsync_TryToAddProductWithInvalidCredentials_ShouldThrowException()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = -1m,
                Quantity = 100,
                Description = "Anything for description"
            };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await productsManager.AddAsync(newProduct));
            var actual = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductCode == newProduct.ProductCode);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid product!"));

        }

        [Test]
        public async Task DeleteProductAsync_WithValidProductCode_ShouldRemoveProductFromDb()
        {
            // Arrange
            var productToDelete = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProductToDelete",
                ProductCode = "XYZ123",
                Price = 2.50m,
                Quantity = 50,
                Description = "Product to be deleted"
            };
            await dbContext.Products.AddAsync(productToDelete);
            await dbContext.SaveChangesAsync();

            // Act
            await productsManager.DeleteAsync(productToDelete.ProductCode);

            // Assert
            var deletedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == productToDelete.ProductCode);
            Assert.IsNull(deletedProduct);
        }

        [Test]
        public async Task DeleteProductAsync_TryToDeleteWithNullOrWhiteSpaceProductCode_ShouldThrowException()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));

            // Act and Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await productsManager.DeleteAsync(null));
            Assert.That(ex?.Message, Is.EqualTo("Product code cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenProductsExist_ShouldReturnAllProducts()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));

            // Act and Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await productsManager.DeleteAsync(null));
            Assert.That(ex?.Message, Is.EqualTo("Product code cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenNoProductsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));

            // Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetAllAsync());
            Assert.That(ex?.Message, Is.EqualTo("No product found."));
        }

        [Test]
        public async Task SearchByOriginCountry_WithExistingOriginCountry_ShouldReturnMatchingProducts()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var originCountry = "Bulgaria";  // Replace with an existing origin country in your test data

            // Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.SearchByOriginCountry(originCountry));
            Assert.That(ex?.Message, Is.EqualTo("No product found with the given country of origin."));
        }

        [Test]
        public async Task SearchByOriginCountryAsync_WithNonExistingOriginCountry_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var nonExistingOriginCountry = "NonExistingCountry";  // Replace with a non-existing origin country

            // Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.SearchByOriginCountry(nonExistingOriginCountry));
            Assert.That(ex?.Message, Is.EqualTo("No product found with the given country of origin."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidProductCode_ShouldReturnProduct()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var validProductCode = "ABC123";  // Replace with an actual valid product code in your test data

            // Act and Assert
            var product = await productsManager.GetSpecificAsync(validProductCode);
            Assert.NotNull(product);
            // Add additional assertions based on the expected properties of the product
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidProductCode_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var invalidProductCode = "InvalidProductCode";  // Replace with an invalid product code

            // Act and Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetSpecificAsync(invalidProductCode));
            Assert.That(ex?.Message, Is.EqualTo($"No product found with product code: {invalidProductCode}"));
        }
    

        [Test]
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
        {
            // Arrange
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var existingProductCode = "ABC123";  // Replace with an existing product code in your test data

            // Get the existing product
            var existingProduct = await productsManager.GetSpecificAsync(existingProductCode);

            // Update some properties of the existing product
            existingProduct.ProductName = "UpdatedProductName";
            existingProduct.Quantity += 10;

            // Act
            await productsManager.UpdateAsync(existingProduct);

            // Assert
            // Retrieve the updated product
            var updatedProduct = await productsManager.GetSpecificAsync(existingProductCode);

            // Assert that the properties are updated
            Assert.NotNull(updatedProduct);
            Assert.AreEqual("UpdatedProductName", updatedProduct.ProductName);
            Assert.AreEqual(existingProduct.Quantity + 10, updatedProduct.Quantity);
            // Add additional assertions based on the expected properties of the updated product
        }

        [Test]
        public async Task UpdateAsync_WithInvalidProduct_ShouldThrowValidationException()
        {
            
            var productsManager = new ProductsManager(new ProductsRepository(new TestProductsDbContext()));
            var invalidProduct = new Product();  // Replace with an invalid product (e.g., missing required properties)

            // Act and Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await productsManager.UpdateAsync(invalidProduct));
            Assert.That(ex?.Message, Is.EqualTo("Invalid product!"));
        }
    }
}
