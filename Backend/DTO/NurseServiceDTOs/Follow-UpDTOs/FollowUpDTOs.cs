using System.ComponentModel.DataAnnotations;

namespace NurseRecordingSystem.Model.DTO.NurseServicesDTOs.FollowUpDTOs
{
    public class CreateFollowUpDTO
    {
        [Required] public int PatientId { get; set; }
        [Required] public int RecordId { get; set; }
        [Required] public DateTime FollowUpDate { get; set; }
        public string? Description { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class UpdateFollowUpDTO
    {
        [Required] public DateTime FollowUpDate { get; set; }
        public string? Description { get; set; }
        [Required] [MaxLength(50)] public string UpdatedBy { get; set; } = string.Empty;
    }
}