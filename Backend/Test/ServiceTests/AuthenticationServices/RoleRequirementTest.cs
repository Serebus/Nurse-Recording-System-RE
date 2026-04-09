using NurseRecordingSystem.Authorization;
using Xunit;

namespace NurseRecordingSystem.Test.ServiceTests.AuthenticationServices
{
    public class RoleRequirementTest
    {
        [Fact]
        public void Constructor_SingleRole_SetsAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Nurse");

            // Assert
            Assert.Single(requirement.AllowedRoles);
            Assert.Contains("Nurse", requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_MultipleRoles_SetsAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Nurse", "User", "Guest");

            // Assert
            Assert.Equal(3, requirement.AllowedRoles.Count());
            Assert.Contains("Nurse", requirement.AllowedRoles);
            Assert.Contains("User", requirement.AllowedRoles);
            Assert.Contains("Guest", requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_NoRoles_SetsEmptyAllowedRoles()
        {
            // Arrange & Act
            var requirement = new RoleRequirement();

            // Assert
            Assert.Empty(requirement.AllowedRoles);
        }

        [Fact]
        public void Constructor_DuplicateRoles_IncludesDuplicates()
        {
            // Arrange & Act
            var requirement = new RoleRequirement("Nurse", "Nurse");

            // Assert
            Assert.Equal(2, requirement.AllowedRoles.Count());
            Assert.All(requirement.AllowedRoles, role => Assert.Equal("Nurse", role));
        }
    }
}
