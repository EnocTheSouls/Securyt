using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Securyt.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Securyt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
      
        public CuentasController(UserManager<IdentityUser> userManager, 
            IConfiguration configuration, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<AutenticacionRespuesta>> Registrar(CredencialAdmin credencialAdmin)
        {
            var administrador = new IdentityUser { UserName = credencialAdmin.Email, Email = credencialAdmin.Email };
            var resultado = await _userManager.CreateAsync(administrador, credencialAdmin.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialAdmin);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<AutenticacionRespuesta>> Login(CredencialAdmin credencialAdmin)
        {
            var resultado = await _signInManager.PasswordSignInAsync(credencialAdmin.Email,
                credencialAdmin.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialAdmin);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }
        }
        [HttpGet("renovarToken")]
        public async Task<ActionResult<AutenticacionRespuesta>> Renovar()
        {
            var emailClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            var credencialAdmin= new CredencialAdmin()
            {
                Email = emailClaim.Value
            };
            return await ConstruirToken(credencialAdmin);
        }
        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(AgregarRolDTO agregarRol)
        {
            var usaurio = await _userManager.FindByEmailAsync(agregarRol.Email);
            if (usaurio != null)
            {
                await _userManager.AddClaimAsync(usaurio, new Claim("esAdmin", "1"));
                return NoContent();
            }
            return BadRequest();
        }


        [HttpPost("RemoverAdmin")]
        //Acceso solo a usaurios Logeados
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
        public async Task<ActionResult> RemoverAdmin(AgregarRolDTO agregarRol)
        {
            var usaurio = await _userManager.FindByEmailAsync(agregarRol.Email);
            if (usaurio != null)
            {
                await _userManager.RemoveClaimAsync(usaurio, new Claim("esAdmin", "1"));
                return NoContent();
            }
            return BadRequest();
        }

        private async Task<AutenticacionRespuesta> ConstruirToken(CredencialAdmin credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
            };

            //Agregar claims
            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email);
            //Obtener claims de la base de datos
            var claimsDB = await _userManager.GetClaimsAsync(usuario);
            //Anexamos los claims
            claims.AddRange(claimsDB);


            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveJWT"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new AutenticacionRespuesta
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
            };
        }

    }
    
}
