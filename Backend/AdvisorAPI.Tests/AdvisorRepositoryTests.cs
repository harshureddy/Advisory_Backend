using Xunit;
using AdvisorAPI.Models;
using AdvisorAPI.Repositories;
using AdvisorAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AdvisorAPI.Tests
{
    public class AdvisorRepositoryTests
    {
        private AdvisorContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AdvisorContext>()
                .UseInMemoryDatabase(databaseName: "AdvisorDB_" + System.Guid.NewGuid().ToString()) // Use unique database name per test
                .Options;

            return new AdvisorContext(options);
        }

        [Fact]
        public void CreateAdvisor_ShouldAddAdvisor()
        {
            using (var context = GetInMemoryDbContext())
            {
                var repository = new AdvisorRepository(context);
                var newAdvisor = new Advisor { Id = 3, Name = "Alice Johnson", SIN = "123123123", HealthStatus = "Red" };
                var createdAdvisor = repository.Create(newAdvisor);
                Assert.Equal(newAdvisor.Name, createdAdvisor.Name);
                Assert.Single(context.Advisors); // Ensure there's only one advisor
            }
        }

        [Fact]
        public void GetAdvisor_ShouldReturnAdvisor()
        {
            using (var context = GetInMemoryDbContext())
            {
                var repository = new AdvisorRepository(context);
                var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
                context.Advisors.Add(advisor);
                context.SaveChanges();
                var retrievedAdvisor = repository.Get(1);
                Assert.NotNull(retrievedAdvisor);
                Assert.Equal(advisor.Name, retrievedAdvisor.Name);
            }
        }

        [Fact]
        public void ListAdvisors_ShouldReturnAllAdvisors()
        {
            using (var context = GetInMemoryDbContext())
            {
                var repository = new AdvisorRepository(context);
                var advisors = new[]
                {
                    new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" },
                    new Advisor { Id = 2, Name = "Jane Smith", SIN = "987654321", HealthStatus = "Yellow" }
                };
                context.Advisors.AddRange(advisors);
                context.SaveChanges();
                var retrievedAdvisors = repository.List();
                Assert.Equal(2, retrievedAdvisors.Count());
            }
        }

        [Fact]
        public void DeleteAdvisor_ShouldRemoveAdvisor()
        {
            using (var context = GetInMemoryDbContext())
            {
                var repository = new AdvisorRepository(context);
                var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
                context.Advisors.Add(advisor);
                context.SaveChanges();
                repository.Delete(1);
                Assert.Empty(context.Advisors);
                Assert.Null(repository.Get(1));
            }
        }

        [Fact]
        public void UpdateAdvisor_ShouldModifyAdvisor()
        {
            using (var context = GetInMemoryDbContext())
            {
                var repository = new AdvisorRepository(context);
                var advisor = new Advisor { Id = 1, Name = "John Doe", SIN = "123456789", HealthStatus = "Green" };
                context.Advisors.Add(advisor);
                context.SaveChanges();
                advisor.Name = "John Doe Updated";
                repository.Update(advisor);
                var updatedAdvisor = repository.Get(1);
                Assert.Equal("John Doe Updated", updatedAdvisor.Name);
            }
        }
    }
}
