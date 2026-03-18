﻿namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class UserDetailsDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
