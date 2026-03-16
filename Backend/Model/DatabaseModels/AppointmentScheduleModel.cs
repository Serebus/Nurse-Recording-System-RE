namespace NurseRecordingSystem.Model.DatabaseModels
{
    public class AppointmentScheduleModel
    {
        public int ScheduleId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Description { get; set; } = null!;
        public int PatientId { get; set; }

    }
}
