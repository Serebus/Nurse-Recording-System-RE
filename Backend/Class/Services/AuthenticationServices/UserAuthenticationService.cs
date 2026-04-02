using Microsoft.Data.SqlClient;
using Dapper;
using NurseRecordingSystem.Class.Services.HelperServices.HelperAuthentication;
using NurseRecordingSystem.Contracts.RepositoryContracts.User;
using NurseRecordingSystem.Contracts.ServiceContracts.Auth;
using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Class.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly string? _connectionString;
        private readonly IUserRepository _userRepository;


        //Dependency Injection of IConfiguration and IUserRepository
        public UserAuthenticationService(IConfiguration configuration, IUserRepository userRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _userRepository = userRepository
                ?? throw new ArgumentNullException(nameof(userRepository),"UserAuth Service cannot be null");
        }

        //User Method: Login
        #region Login

        public async Task<LoginResponseDTO?> AuthenticateAsync(LoginRequestDTO request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "LoginRequest cannot be Null");
            }

            await using var connection = new SqlConnection(_connectionString);
            try
            {
                // Dapper Row maps fields dynamically
                var userRow = await connection.QueryFirstOrDefaultAsync(
                    "dbo.ausp_LoginUserAuth",
                    new { email = request.Email },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                if (userRow != null && PasswordHelper.VerifyPasswordHash(request.Password, (byte[])userRow.passwordHash, (byte[])userRow.passwordSalt))
                {
                    string userRole = userRow.role.ToString();
                    var response = new LoginResponseDTO
                    {
                        AuthId = (int)userRow.authId,
                        UserName = userRow.userName.ToString(),
                        Email = userRow.email.ToString(),
                        Role = userRole,
                        IsAuthenticated = true
                    };

                    if (userRole == "User")
                    {
                        response.UserDetails = new UserDetailsDTO
                        {
                            UserId = (int)userRow.userId,
                            FirstName = userRow.User_firstName.ToString(),
                            MiddleName = userRow.User_middleName?.ToString(),
                            LastName = userRow.User_lastName.ToString(),
                            ContactNumber = userRow.User_contactNumber.ToString(),
                            Address = userRow.User_address?.ToString()
                        };
                    }
                    else if (userRole == "Nurse")
                    {
                        response.NurseDetails = new NurseDetailsDTO
                        {
                            NurseId = (int)userRow.nurseId,
                            FirstName = userRow.Nurse_firstName.ToString(),
                            MiddleName = userRow.Nurse_middleName?.ToString(),
                            LastName = userRow.Nurse_lastName.ToString(),
                            ContactNumber = userRow.Nurse_contactNumber?.ToString()
                        };
                    }

                    return response;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Database ERROR occurred during login", ex);
            }
        }
        #endregion


        //User Function: To Determine the role of the user
        public async Task<int> DetermineRoleAync(LoginResponseDTO response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response), "LoginResponse cannot be Null");
            }
            var user = await _userRepository.GetUserByUsernameAsync(response.UserName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }
            return user.Role;
        }

        //User Method: Logout (fishballs need session tokens)
        public async Task LogoutAsync()
        {
            // Implement logout logic if needed (e.g., invalidate tokens, clear session data)
            await Task.CompletedTask;
        }
    }
}
