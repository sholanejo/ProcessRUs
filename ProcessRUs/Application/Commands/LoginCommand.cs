using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProcessRUs.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProcessRUs.Application.Commands
{
    public class LoginResult
    {
        public bool Status { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }



    public class LoginCommand : IRequest<LoginResult>
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }




    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<LoginCommandHandler> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }





        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return new LoginResult
                    {
                        Message = "Login Successful",
                        Status = true,
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    };
                }

                return new LoginResult
                {
                    Message = "Incorrrect Username and or Password",
                    Status = false,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while logging in user");
                return new LoginResult
                {
                    Message = "An error occured while logging in",
                    Status = false,
                };
            }
        }
    }
}