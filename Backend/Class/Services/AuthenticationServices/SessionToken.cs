﻿﻿﻿using Microsoft.AspNetCore.Http.HttpResults;
using Dapper;
using Microsoft.Data.SqlClient;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;
using System.Data;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class SessionTokenService : ISessionTokenService
    {
        private readonly string? _connectionString;

        // Dependency Injection of IConfiguration
        public SessionTokenService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Creates a new session token and deactivates any old ones.
        /// </summary>
        public async Task<SessionTokenDTO?> CreateSessionAsync(int authId)
        {
            await using var connection = new SqlConnection(_connectionString);
            try
            {
                return await connection.QueryFirstOrDefaultAsync<SessionTokenDTO>(
                    "dbo.usp_CreateSessionToken",
                    new { authId },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error creating session token.", ex);
            }
        }

        /// <summary>
        /// Updates an existing active token with a new value and expiry.
        /// </summary>
        public async Task<SessionTokenDTO?> RefreshSessionTokenAsync(int authId)
        {
            await using var connection = new SqlConnection(_connectionString);
            try
            {
                return await connection.QueryFirstOrDefaultAsync<SessionTokenDTO>(
                    "dbo.usp_UpdateSessionToken",
                    new { authId },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error refreshing session token.", ex);
            }
        }

        /// <summary>
        /// Deactivates the user's session token based on the token value.
        /// </summary>
        /// <returns>True if the session was successfully ended, false otherwise.</returns>
        public async Task<bool> EndSessionAsync(byte[] tokenBytes)
        {
            await using var connection = new SqlConnection(_connectionString);
            try
            {
                int rowsAffected = await connection.ExecuteAsync(
                    "dbo.usp_EndSessionToken",
                    new { Token = tokenBytes },
                    commandType: CommandType.StoredProcedure);
                
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error ending session.", ex);
            }
        }

        /// <summary>
        /// Validates if an active, non-expired session exists for a user.
        /// </summary>
        public async Task<bool> ValidateTokenAsync(int authId) // <-- Signature changed
        {
            await using var connection = new SqlConnection(_connectionString);
            try
            {
                return await connection.ExecuteScalarAsync<bool>(
                    "dbo.usp_ValidateSessionToken",
                    new { authId },
                    commandType: CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error validating session.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing validation result.", ex);
            }
        }
    }
}