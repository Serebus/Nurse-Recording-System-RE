namespace NurseRecordingSystem.DTO.HelperServiceDTOs.HelperMedecineStockDTOs
{
    public class ViewAllMedecineStockResponseDTO
    {
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public string MedicineDescription { get; set; } = string.Empty;
        public int NumberOfStock { get; set; }
    }
}
