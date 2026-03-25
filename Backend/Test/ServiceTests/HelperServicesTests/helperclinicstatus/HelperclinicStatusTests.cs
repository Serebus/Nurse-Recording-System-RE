using NurseRecordingSystem.Class.Services.ClinicStatusServices;
using NurseRecordingSystem.DTO.HelperServiceDTOs.HelperClinicStatusDTOs;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.HelperServicesTests.helperclinicstatus
{
    public class HelperclinicStatusTests
    {
        [Fact]
        public void ViewClinicStatus_ConfigurationNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:DefaultConnection"] = null })
                .Build();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new ViewClinicStatus(config));
            Assert.Contains("Connection string 'DefaultConnection' not found.", exception.Message);
        }

        [Fact]
        public async Task ViewAllAsync_InvalidConnection_ThrowsSqlException()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?> {
                {"ConnectionStrings:DefaultConnection", "Server=DESKTOP-UM7KJD2\\SQLEXPRESS;Database=NurseRecordingSystem;Trusted_Connection=True;TrustServerCertificate=True;"}
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var service = new ViewClinicStatus(config);

            // Act & Assert
            await Assert.ThrowsAsync<Microsoft.Data.SqlClient.SqlException>(() => service.ViewAllAsync());
        }
    }
}
