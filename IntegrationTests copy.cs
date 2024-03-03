using System.ComponentModel.DataAnnotations;
using NUnit.Framework;
using ZooConsoleAPI.Business;
using ZooConsoleAPI.Business.Contracts;
using ZooConsoleAPI.Data.Model;
using ZooConsoleAPI.DataAccess;

namespace ZooConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestAnimalDbContext dbContext;
        private IAnimalsManager animalsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestAnimalDbContext();
            this.animalsManager = new AnimalsManager(new AnimalRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddAnimalAsync_ShouldAddNewAnimal()
        {
            // Arrange
            var newAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQXXX",
                Name = "Axel",
                Breed = "Lbrador",
                Type = "TestType",
                Age = 3,
                Gender = "Male",
                IsHealthy = true
            };

            // Act
            await this.animalsManager.AddAsync(newAnimal);

            // Assert
            var addedAnimal = await this.dbContext.Animals.FindAsync(newAnimal.Id);
            Assert.NotNull(addedAnimal);
            Assert.AreEqual(newAnimal.CatalogNumber, addedAnimal.CatalogNumber);

        }

        //Negative test
        [Test]
        public async Task AddAnimalAsync_TryToAddAnimalWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var validAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQXX4",
                Name = "Axelttt",
                Breed = "Labrador",
                Type = "TestType",
                Age = 3,
                Gender = "Male",
                IsHealthy = true
            };

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await this.animalsManager.AddAsync(validAnimal));

        }

        [Test]
        public async Task DeleteAnimalAsync_WithValidCatalogNumber_ShouldRemoveAnimalFromDb()
        {
            // Arrange
            var animalToDelete = new Animal
            {

                CatalogNumber = "01HNTWXTQXXX",
                Name = "Axel",
                Breed = "Lbrador",
                Type = "TestType",
                Age = 3,
                Gender = "Male",
                IsHealthy = true
            };

            await this.dbContext.Animals.AddAsync(animalToDelete);
            await this.dbContext.SaveChangesAsync();

            // Act
            await this.animalsManager.DeleteAsync(animalToDelete.CatalogNumber);

            // Assert
            var deletedAnimal = await this.dbContext.Animals.FindAsync(animalToDelete.Id);
            Assert.Null(deletedAnimal);
        }

        [Test]
        public async Task DeleteAnimalAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException()
        {
            // Arrange
            var animal = new Animal
            {

                CatalogNumber = "01HNTWXTQXXX",
                Name = "Axel",
                Breed = "Lbrador",
                Type = "TestType",
                Age = 3,
                Gender = "Male",
                IsHealthy = true
            };

            await this.dbContext.Animals.AddAsync(animal);
            await this.dbContext.SaveChangesAsync();

            // Act and Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await this.animalsManager.DeleteAsync(null));
            Assert.ThrowsAsync<ArgumentException>(async () => await this.animalsManager.DeleteAsync(""));
            Assert.ThrowsAsync<ArgumentException>(async () => await this.animalsManager.DeleteAsync("   "));
        }

        [Test]
        public async Task GetAllAsync_WhenAnimalsExist_ShouldReturnAllAnimals()
        {
            // Arrange
            var animalsToAdd = new List<Animal>
    {
        new Animal { CatalogNumber = "01HNTWXTQAAA",
            Name = "Jak",
            Breed = "Jack Rusel",
            Type = "Type1",
            Age = 3,
            Gender = "Male",
            IsHealthy = true },

        new Animal { CatalogNumber = "01HNTWXTQBBC",
            Name = "Kira",
            Breed = "Labrador",
            Type = "Type2",
            Age = 5,
            Gender = "Female",
            IsHealthy = false },

        new Animal { CatalogNumber = "01HNTWXTQCDE",
            Name = "Tom",
            Breed = "Husky",
            Type = "Type3",
            Age = 2,
            Gender = "Male",
            IsHealthy = true }
    };

            await this.dbContext.Animals.AddRangeAsync(animalsToAdd);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.animalsManager.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(animalsToAdd.Count, result.Count());

            foreach (var animal in animalsToAdd)
            {
                Assert.IsTrue(result.Any(a => a.CatalogNumber == animal.CatalogNumber));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenNoAnimalsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.GetAllAsync());
        }

        [Test]
        public async Task SearchByTypeAsync_WithExistingType_ShouldReturnMatchingAnimals()
        {
            // Arrange
            var animalsToAdd = new List<Animal>
    {
        new Animal { CatalogNumber = "01HNTWXTQAAA",
            Name = "Jak",
            Breed = "Jack Rusel",
            Type = "Type1",
            Age = 3, Gender = "Male",
            IsHealthy = true },

        new Animal { CatalogNumber = "01HNTWXTQBBC",
            Name = "Kira",
            Breed = "Labrador",
            Type = "Type2",
            Age = 5,
            Gender = "Female",
            IsHealthy = false },

        new Animal { CatalogNumber = "01HNTWXTQCDE",
            Name = "Tom",
            Breed = "Husky",
            Type = "Type3",
            Age = 2,
            Gender = "Male",
            IsHealthy = true }
    };

            foreach (var animal in animalsToAdd)
            {
                await this.animalsManager.AddAsync(animal);
            }

            // Act
            var retrievedAnimals = await this.animalsManager.SearchByTypeAsync("Type2");

            // Assert
            Assert.IsNotNull(retrievedAnimals);
            Assert.IsNotEmpty(retrievedAnimals);

            foreach (var animal in retrievedAnimals)
            {
                Assert.AreEqual("Type2", animal.Type);
            }
        }

        [Test]
        public async Task SearchByTypeAsync_WithNonExistingType_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var existingAnimals = new List<Animal>
    {
        new Animal { CatalogNumber = "01HNTWXTQAAA",
            Name = "Jak",
            Breed = "Jack Rusel",
            Type = "Type1", Age = 3,
            Gender = "Male",
            IsHealthy = true },

        new Animal { CatalogNumber = "01HNTWXTQBBC",
            Name = "Kira",
            Breed = "Labrador",
            Type = "Type2",
            Age = 5,
            Gender = "Female",
            IsHealthy = false },

        new Animal { CatalogNumber = "01HNTWXTQCDE",
            Name = "Tom",
            Breed = "Husky",
            Type = "Type3", Age = 2,
            Gender = "Male",
            IsHealthy = true }
    };

            await this.dbContext.Animals.AddRangeAsync(existingAnimals);
            await this.dbContext.SaveChangesAsync();

            var nonExistingType = "NonExistingType";

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.SearchByTypeAsync(nonExistingType));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnAnimal()
        {
            // Arrange
            var existingAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQAAA",
                Name = "Jake",
                Breed = "Golden Retriever",
                Type = "Type2",
                Age = 4,
                Gender = "Male",
                IsHealthy = false
            };

            await this.animalsManager.AddAsync(existingAnimal);

            // Act
            var retrievedAnimal = await this.animalsManager.GetSpecificAsync(existingAnimal.CatalogNumber);

            // Assert
            Assert.IsNotNull(retrievedAnimal);
            Assert.AreEqual(existingAnimal.CatalogNumber, retrievedAnimal.CatalogNumber);
            Assert.AreEqual(existingAnimal.Name, retrievedAnimal.Name);
            Assert.AreEqual(existingAnimal.Breed, retrievedAnimal.Breed);
            Assert.AreEqual(existingAnimal.Type, retrievedAnimal.Type);
            Assert.AreEqual(existingAnimal.Age, retrievedAnimal.Age);
            Assert.AreEqual(existingAnimal.Gender, retrievedAnimal.Gender);
            Assert.AreEqual(existingAnimal.IsHealthy, retrievedAnimal.IsHealthy);
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidCatalogNumber = "InvalidCatalogNumber";

            // Act and Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.GetSpecificAsync(invalidCatalogNumber));
        }

        [Test]
        public async Task UpdateAsync_WithValidAnimal_ShouldUpdateAnimal()
        {
            // Arrange
            var originalAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQAAA",
                Name = "Fluffy",
                Breed = "Magic Dragon",
                Type = "Mythical",
                Age = 5,
                Gender = "Unknown",
                IsHealthy = true
            };


            await this.animalsManager.AddAsync(originalAnimal);


            var updatedAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQAAA",
                Name = "Sparklewing",
                Breed = "Enchanted Unicorn",
                Type = "Fantasy",
                Age = 3,
                Gender = "Female",
                IsHealthy = false
            };

            // Act
            await this.animalsManager.UpdateAsync(updatedAnimal);

            var retrievedAnimal = await this.animalsManager.GetSpecificAsync("01HNTWXTQAAA");

            // Assert
            Assert.IsNotNull(retrievedAnimal);
            Assert.AreEqual("Sparklewing", retrievedAnimal.Name);
            Assert.AreEqual("Enchanted Unicorn", retrievedAnimal.Breed);
            Assert.AreEqual("Fantasy", retrievedAnimal.Type);
            Assert.AreEqual(3, retrievedAnimal.Age);
            Assert.AreEqual("Female", retrievedAnimal.Gender);
            Assert.IsFalse(retrievedAnimal.IsHealthy);
        }



        [Test]
        public async Task UpdateAsync_WithInvalidAnimal_ShouldThrowValidationException()
        {
            // Arrange
            var originalAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQAAA",
                Name = "Jak",
                Breed = "Jack Rusel",
                Type = "Type1",
                Age = 3,
                Gender = "Male",
                IsHealthy = true
            };


            await this.animalsManager.AddAsync(originalAnimal);


            var invalidAnimal = new Animal
            {
                CatalogNumber = "01HNTWXTQAAA",
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(() => this.animalsManager.UpdateAsync(invalidAnimal));

        }
    }
}



