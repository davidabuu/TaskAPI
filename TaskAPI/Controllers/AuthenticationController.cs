using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskAPI.Data;
using TaskAPI.DTO_s;
using TaskAPI.Model;

namespace TaskAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly UserManager<Model.ApplicationUsers> _userManager;

    private readonly ApiDbContext _context;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IConfiguration _configuration;
    public AuthenticationController(UserManager<Model.ApplicationUsers> userManager, IConfiguration configuration , ApiDbContext context, TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register( [FromBody] CreateUserDto registrationDto)
    {
        if(ModelState.IsValid)
        {
            var user_Exists = await _userManager.FindByEmailAsync(registrationDto.Email!);
            if(user_Exists != null) {

                return BadRequest( new Model.AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email Already Exists"
                    }                }
                    );
            }
            //Create A New User
            var new_User = new Model.ApplicationUsers()
            {
               Email = registrationDto.Email,
               UserName = registrationDto.Email,
               FirstName = registrationDto.FirstName,
               LastName = registrationDto.LastName,

            };
            
            var isCreated = await _userManager.CreateAsync(new_User, registrationDto.Password!);
            if (isCreated.Succeeded)
            {
                var currentUser = await _userManager.FindByEmailAsync(new_User.Email!);
                 if(currentUser != null) { 
                    var addUserRole = await _userManager.AddToRoleAsync(currentUser, registrationDto.Role!);
                    if (addUserRole.Succeeded)
                    {
                        //Generate Token
                        var token = await GenerateToken(new_User);
                        
                        return Ok(token);
                    }
                   
                }
                return BadRequest(new Model.AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Server Error"
                    }
                }
                 );
            }
            return BadRequest(new Model.AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                    {
                        isCreated.ToString()
                    }
            }
            );


        }
        return BadRequest();
    }
    private async Task<AuthResult> GenerateToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value!);

        // Token Descriptorc
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email !),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),

            }),
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value!)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            Token = RandomStringGeneration(23),
            AddedDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
            IsRevoked = false,
            IsUsed = false,
            UserId = user.Id
        };
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return new AuthResult()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            Result = true

        };

    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Login loginRequest)
    {
        if (ModelState.IsValid)
        {
            // If The User Exists
            var user_exist = await _userManager.FindByEmailAsync(loginRequest.Email!);
            if (user_exist == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                {
                    "User Do Not Exits"
                },
                    Result = false

                });
            }
            var isCorrect = await _userManager.CheckPasswordAsync(user_exist, loginRequest.Password!);
            if (!isCorrect)

                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                {
                    "Password failed"
                },
                    Result = false

                });
            var jwtToken = await GenerateToken(user_exist);
            return Ok(jwtToken);


        }
        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
                {
                    "Invalid Payload"
                },
            Result = false

        });
    }
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var result = await VerifyAndGenertateToken(tokenRequest);
            if (result == null)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
            {
                "Inavlid Tokens"
            },
                    Result = false
                });
            }
            return Ok(result);
        }
        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
            {
                "Inavlid Paramter"
            },
            Result = false
        });
    }
    private async Task<AuthResult> VerifyAndGenertateToken(TokenRequest tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        try
        {
            _tokenValidationParameters.ValidateLifetime = false;// For testing
            var tokenVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken securityToken)
            {
                var result = securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                if (result == false)
                    return null;

            }
            var utcExpriryDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)!.Value);

            var expiryDate = UnitTimeStampToDateTime(utcExpriryDate);
            if (expiryDate > DateTime.Now)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Expire Token"
                    }
                };
            }
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            if (storedToken == null)
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid Token"
                    }
                };

            if (storedToken.IsUsed || storedToken.IsRevoked)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid Token"
                    }
                };
            }
            var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value;
            if (storedToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid Token"
                    }
                };
            }
            if (storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Expired Token"
                    }
                };
            }
            storedToken.IsUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId!);
            return await GenerateToken(dbUser!);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new AuthResult()
            {
                Result = false,
                Errors = new List<string>()
                    {
                        "Server Error"
                    }
            };
        }
    }

    private DateTime UnitTimeStampToDateTime(long utcTimeStamp)
    {
        var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTimeVal = dateTimeVal.AddSeconds(utcTimeStamp).ToUniversalTime();
        return dateTimeVal;
    }
    private string RandomStringGeneration(int length)
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXY123456789abucdefghijklmnoipqrus";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
