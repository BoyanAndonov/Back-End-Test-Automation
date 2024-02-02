using System;
using TownsApplication.Data.Models;
using Xunit;

namespace TownApplication.IntegrationTests
{
    public class TownControllerIntegrationTests
    {
        private readonly TownController _controller;

        public TownControllerIntegrationTests()
        {
            _controller = new TownController();
            _controller.ResetDatabase();
        }

        [Fact]
        public void AddTown_ValidInput_ShouldAddTown()
        {
            // Arrange
            string townName = "TestTown";
            int population = 1000;

            // Act
            _controller.AddTown(townName, population);

            // Assert
            var addedTown = _controller.GetTownByName(townName);
            Assert.NotNull(addedTown);
            Assert.Equal(townName, addedTown.Name);
            Assert.Equal(population, addedTown.Population);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AB")]
        public void AddTown_InvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            int validPopulation = 1000;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, validPopulation));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddTown_InvalidPopulation_ShouldThrowArgumentException(int invalidPopulation)
        {
            // Arrange
            string validName = "ValidTown";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.AddTown(validName, invalidPopulation));
        }

        [Fact]
        public void AddTown_DuplicateTownName_DoesNotAddDuplicateTown()
        {
            // Arrange
            string townName = "DuplicateTown";
            int population = 1000;

            // Act
            _controller.AddTown(townName, population);

            // Try to add the same town again
            _controller.AddTown(townName, population);

            // Assert
            var towns = _controller.ListTowns();
            Assert.Single(towns); // Only one town should exist
        }

        [Fact]
        public void UpdateTown_ShouldUpdatePopulation()
        {
            // Arrange
            string townName = "UpdateTown";
            int initialPopulation = 1000;
            int updatedPopulation = 1500;

            // Act
            _controller.AddTown(townName, initialPopulation);
            _controller.UpdateTown(_controller.GetTownByName(townName).Id, updatedPopulation);

            // Assert
            var updatedTown = _controller.GetTownByName(townName);
            Assert.NotNull(updatedTown);
            Assert.Equal(updatedPopulation, updatedTown.Population);
        }

        [Fact]
        public void DeleteTownShouldDeleteTown()
        {
            // Arrange
            string townName = "DeleteTown";
            int population = 1000;

            // Act
            _controller.AddTown(townName, population);
            var townId = _controller.GetTownByName(townName).Id;
            _controller.DeleteTown(townId);

            // Assert
            var deletedTown = _controller.GetTownById(townId);
            Assert.Null(deletedTown); // The town should not exist after deletion
        }

        [Fact]
        public void ListTowns_ShouldReturnTowns()
        {
            // Arrange
            _controller.AddTown("Town1", 1000);
            _controller.AddTown("Town2", 1500);
            _controller.AddTown("Town3", 2000);

            // Act
            var towns = _controller.ListTowns();

            // Assert
            Assert.Equal(3, towns.Count); // Three towns should be in the list

            // Check if each town is present in the list
            Assert.Contains(towns, t => t.Name == "Town1");
            Assert.Contains(towns, t => t.Name == "Town2");
            Assert.Contains(towns, t => t.Name == "Town3");
        }
    }
}
