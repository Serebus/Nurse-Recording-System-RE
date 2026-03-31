﻿﻿﻿using Microsoft.Data.SqlClient;
using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INursePatientRecords;
using NurseRecordingSystem.DTO.NurseServiceDTOs.NursePatientRecordDTOs;

namespace NurseRecordingSystem.Class.Services.NurseServices.PatientRecords
{
    public class ViewPatientRecordList : IViewPatientRecordList
    {
        private readonly string? _connectionString;

        public ViewPatientRecordList(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public async Task<List<PatientRecordListItemDTO>> GetPatientRecordListAsync(int? nurseId = null)
        {
            const string storedProc = "dbo.nsp_ViewPatientRecordList";

            await using var connection = new SqlConnection(_connectionString);
            try
            {
                var records = await connection.QueryAsync<PatientRecordListItemDTO>(
                    storedProc,
                    new { nurseId },
                    commandType: System.Data.CommandType.StoredProcedure);
                return records.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving the list of patient records.", ex);
            }
        }
    }
}