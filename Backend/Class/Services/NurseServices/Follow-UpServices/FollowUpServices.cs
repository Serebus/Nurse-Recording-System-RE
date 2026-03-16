using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.IFollowUps;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.FollowUpDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.FollowUps
{
    public class CreateFollowUp : ICreateFollowUp
    {
        private readonly IDbConnection _dbConnection;
        public CreateFollowUp(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task<int> CreateFollowUpAsync(CreateFollowUpDTO dto)
        {
            const string storedProc = "dbo.usp_CreateFollowUp";
            return await _dbConnection.QuerySingleAsync<int>(
                storedProc,
                new { dto.PatientId, dto.RecordId, dto.FollowUpDate, dto.Description, dto.CreatedBy },
                commandType: CommandType.StoredProcedure);
        }
    }

    public class UpdateFollowUp : IUpdateFollowUp
    {
        private readonly IDbConnection _dbConnection;
        public UpdateFollowUp(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task UpdateFollowUpAsync(int followUpId, UpdateFollowUpDTO dto)
        {
            const string storedProc = "dbo.usp_UpdateFollowUp";
            try
            {
                await _dbConnection.ExecuteAsync(
                    storedProc,
                    new { FollowUpId = followUpId, dto.FollowUpDate, dto.Description, dto.UpdatedBy },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new KeyNotFoundException(ex.Message);
            }
        }
    }

    public class DeleteFollowUp : IDeleteFollowUp
    {
        private readonly IDbConnection _dbConnection;
        public DeleteFollowUp(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public async Task DeleteFollowUpAsync(int followUpId, string deletedBy)
        {
            const string storedProc = "dbo.usp_DeleteFollowUp";
            try
            {
                await _dbConnection.ExecuteAsync(
                    storedProc,
                    new { FollowUpId = followUpId, DeletedBy = deletedBy },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex) when (ex.Number == 50001)
            {
                throw new KeyNotFoundException(ex.Message);
            }
        }
    }
}