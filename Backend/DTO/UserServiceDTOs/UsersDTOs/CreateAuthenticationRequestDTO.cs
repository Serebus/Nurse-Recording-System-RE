﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NurseRecordingSystem.DTO.UserServiceDTOs.UsersDTOs
{
    public class CreateAuthenticationRequestDTO
    {
        [Required]
        [DefaultValue("Password123!")]
        public string Password { get; set; } = null!;
        
        [EmailAddress]
        [Required]
        [DefaultValue("user@example.com")]
        public string Email { get; set; } = null!;

        [Required]
        [DefaultValue("Juan")]
        public string FirstName { get; set; } = null!;

        [DefaultValue("M.")]
        public string? MiddleName { get; set; }

        [Required]
        [DefaultValue("123 Main St, Cebu City")]
        public string Address { get; set; } = null!;

        [Required]
        [DefaultValue("Dela Cruz")]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [DefaultValue("09123456789")]
        public string? EmergencyContact { get; set; }

        [DefaultValue("https://facebook.com/juandelacruz")]
        public string? Facebook { get; set; }
    }
}
