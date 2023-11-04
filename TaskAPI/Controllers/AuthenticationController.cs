using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Configuration;
using TaskAPI.DTO_s;
using TaskAPI.Model;

namespace TaskAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly JwtConfig _jwtConfig;

    public AuthenticationController(UserManager<IdentityUser> userManager, JwtConfig jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
    {
        if(ModelState.IsValid)
        {
            var user_Exists = await _userManager.FindByEmailAsync(registrationDto.Email);   
            if(user_Exists != null) {

                return BadRequest( new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email Already Exists"
                    }
                }
                    );
            }
            //Create A New User
            var new_User = new IdentityUser
            {
               Email = registrationDto.Email,
               UserName = registrationDto.FirstName + registrationDto.LastName
               

            };
            var isCreated = await _userManager.CreateAsync(new_User, registrationDto.Password);
            

        }
        return BadRequest();
    }
}
