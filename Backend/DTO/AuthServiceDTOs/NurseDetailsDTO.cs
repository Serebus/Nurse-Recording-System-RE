﻿namespace NurseRecordingSystem.DTO.AuthServiceDTOs
{
    public class NurseDetailsDTO
    {
        public int NurseId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
    }
}
