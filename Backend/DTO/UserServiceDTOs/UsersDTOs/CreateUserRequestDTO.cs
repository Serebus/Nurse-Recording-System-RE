using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs
{
    public class CreateUserRequestDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string EmergencyContact { get; set; } = string.Empty;

        public string? Facebook { get; set; }

        [Required]
        public int AuthId { get; set; }
    }
}
