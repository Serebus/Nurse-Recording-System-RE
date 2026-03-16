﻿using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.Model.DTO.NurseServicesDTOs.PatientRecordsDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class ViewPatientRecord : IViewPatientRecord
    {
        private readonly IDbConnection _dbConnection;

        public ViewPatientRecord(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ViewPatientRecordResponseDTO> GetPatientRecordAsync(int patientRecordId)
        {
            const string storedProc = "dbo.nsp_ViewPatientRecord";

            try
            {
                var record = await _dbConnection.QueryFirstOrDefaultAsync<ViewPatientRecordResponseDTO>(
                    storedProc,
                    new { patientRecordId },
                    commandType: CommandType.StoredProcedure);

                if (record != null)
                    return record;

                throw new KeyNotFoundException($"Patient record with ID {patientRecordId} not found or is archived.");
            }
            catch (SqlException sqlEx) when (sqlEx.Number == 50001) // Custom SP error number
            {
                throw new KeyNotFoundException(sqlEx.Message, sqlEx);
            }
            catch (Exception ex) when (!(ex is KeyNotFoundException))
            {
                throw new Exception($"Error retrieving patient record {patientRecordId}.", ex);
            }
        }
    }
}