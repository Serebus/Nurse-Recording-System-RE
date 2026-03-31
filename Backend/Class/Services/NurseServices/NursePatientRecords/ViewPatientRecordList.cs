﻿using Microsoft.Data.SqlClient;
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
            var records = new List<PatientRecordListItemDTO>();
            const string storedProc = "dbo.nsp_ViewPatientRecordList";

            await using (var connection = new SqlConnection(_connectionString))
            await using (var cmd = new SqlCommand(storedProc, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nurseId", nurseId.HasValue ? (object)nurseId.Value : DBNull.Value);

                try
                {
                    await connection.OpenAsync();
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            records.Add(new PatientRecordListItemDTO
                            {
                                PatientRecordId = reader.GetInt32(reader.GetOrdinal("patientRecordId")),
                                NursingDiagnosis = reader.IsDBNull(reader.GetOrdinal("nursingDiagnosis")) ? null : reader.GetString(reader.GetOrdinal("nursingDiagnosis")),
                                NursingIntervention = reader.IsDBNull(reader.GetOrdinal("nursingIntervention")) ? null : reader.GetString(reader.GetOrdinal("nursingIntervention")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("createdOn")),
                                CreatedBy = reader.GetString(reader.GetOrdinal("createdBy"))
                            });
                        }
                    }
                    return records;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error retrieving the list of patient records.", ex);
                }
            }
        }
    }
}