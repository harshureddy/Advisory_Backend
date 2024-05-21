using Xunit;
using Moq;
using AdvisorAPI.Models;
using AdvisorAPI.Controllers;
using AdvisorAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AdvisorAPI.Tests
{
    public class AdvisorControllerTests
    {
        private readonly Mock<IAdvisorRepository> _mockRepo;
        private readonly AdvisorController _controller;

        public AdvisorControllerTests()
        {
            _mockRepo = new Mock<IAdvisorRepository>();
            _controller = new AdvisorController(_mockRepo.Object);
        }

        [Fact]
        public void GetAdvisor_ReturnsAdvisor_WhenAdvisorExists()
        {
            // Arrange
            var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
            _mockRepo.Setup(repo => repo.Get(1)).Returns(advisor);

            // Act
            var result = _controller.GetAdvisor(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Advisor>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnAdvisor = Assert.IsType<Advisor>(returnValue.Value);
            Assert.Equal(advisor.Name, returnAdvisor.Name);
        }

        [Fact]
        public void GetAdvisor_ReturnsNotFound_WhenAdvisorDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get(1)).Returns((Advisor)null);

            // Act
            var result = _controller.GetAdvisor(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CreateAdvisor_ReturnsCreatedAdvisor()
        {
            // Arrange
            var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
            _mockRepo.Setup(repo => repo.Create(It.IsAny<Advisor>())).Returns((Advisor a) => a);

            // Act
            var result = _controller.CreateAdvisor(advisor);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Advisor>>(result);
            var returnValue = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnAdvisor = Assert.IsType<Advisor>(returnValue.Value);
            Assert.Equal(advisor.Name, returnAdvisor.Name);
        }

        [Fact]
        public void DeleteAdvisor_ReturnsNoContent()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.Get(1)).Returns(new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" });
            _mockRepo.Setup(repo => repo.Delete(1));

            // Act
            var result = _controller.DeleteAdvisor(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateAdvisor_ReturnsNoContent()
        {
            // Arrange
            var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
            _mockRepo.Setup(repo => repo.Get(1)).Returns(advisor);
            _mockRepo.Setup(repo => repo.Update(advisor));

            // Act
            var result = _controller.UpdateAdvisor(1, advisor);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ListAdvisors_ReturnsAllAdvisors()
        {
            // Arrange
            var advisors = new List<Advisor>
            {
                new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" },
                new Advisor { Id = 2, Name = "Jane Smith", SIN = "987654321", HealthStatus = "Yellow" }
            };
            _mockRepo.Setup(repo => repo.List()).Returns(advisors);

            // Act
            var result = _controller.ListAdvisors();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Advisor>>>(result);
            var returnValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnAdvisors = Assert.IsType<List<Advisor>>(returnValue.Value);
            Assert.Equal(2, returnAdvisors.Count);
        }
    }
}
