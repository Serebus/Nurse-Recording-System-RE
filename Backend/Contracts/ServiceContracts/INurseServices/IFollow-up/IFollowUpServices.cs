using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.FollowUpDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.IFollowUps
{
    public interface ICreateFollowUp
    {
        Task<int> CreateFollowUpAsync(CreateFollowUpDTO dto);
    }
    public interface IUpdateFollowUp
    {
        Task UpdateFollowUpAsync(int followUpId, UpdateFollowUpDTO dto);
    }
    public interface IDeleteFollowUp
    {
        Task DeleteFollowUpAsync(int followUpId, string deletedBy);
    }
}