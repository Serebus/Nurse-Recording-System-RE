
using Microsoft.Extensions.Configuration;
using NurseRecordingSystem.Class.Services.UserServices.Users;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AdminServicesTests.AdminUsersTest
{
    public class DeleteUserTest
    {
        [Fact]
        public void DeleteUser_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new DeleteUser(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_ConnectionFactoryThrows_ThrowsInvalidOperationException()
        {
            // Arrange
            var service = new DeleteUser(() => throw new InvalidOperationException("Simulated connection failure"));
            int userId = 1;
            string deletedBy = "Admin1";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => service.SoftDeleteUserAsync(userId, deletedBy));
            Assert.Contains("Simulated connection failure", exception.Message);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_NullDeletedBy_ThrowsArgumentNullException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=0.0.0.0;Database=Dummy;Connection Timeout=1;ConnectRetryCount=0;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new DeleteUser(config);
            int userId = 1;
            string? deletedBy = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SoftDeleteUserAsync(userId, deletedBy!));
        }
    }
}
