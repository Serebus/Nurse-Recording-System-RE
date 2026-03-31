﻿﻿﻿using Microsoft.Data.SqlClient;
using Dapper;
using NurseRecordingSystem.Contracts.ServiceContracts.INurseServices.INurseUsers;
using NurseRecordingSystem.Model.DTO.UserServiceDTOs.UsersDTOs; // Assuming ViewUserDTO

namespace NurseRecordingSystem.Class.Services.UserServices.Users
{
    public class ViewAllUsers : IViewAllUsers
    {
        private readonly string? _connectionString;

        public ViewAllUsers(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Handles the business logic of receiving all user data from nsp_ViewAllUsers.sp
        public async Task<List<ViewUserDTO>> GetAllUsersAsync(bool? isActive = null) // Optional parameter
        {
            // SQL command and connection setup
            const string storedProc = "dbo.nsp_ViewAllUsers";
            await using var connection = new SqlConnection(_connectionString);
            
            try
            {
                // Dapper directly resolves 'null' parameter values gracefully 
                var users = await connection.QueryAsync<ViewUserDTO>(
                    storedProc,
                    new { IsActive = isActive },
                    commandType: System.Data.CommandType.StoredProcedure);
                return users.ToList();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"A database error occurred while retrieving users: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while processing user data.", ex);
            }
        }
    }
}