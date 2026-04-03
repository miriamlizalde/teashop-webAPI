using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TeaShop.Data;
using TeaShop.Models;

namespace TeaShop.Business;

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
     private readonly IUsuarioRepository _repository;

        public AuthService(IConfiguration configuration, IUsuarioRepository repository)
        {
        _configuration = configuration;
        _repository = repository;
        }

    public string Login(LoginDtoIn loginDtoIn)
    {
        var usuario = _repository.GetUsuarioFromCredentials(loginDtoIn);
        return GenerateToken(usuario);
    }

    public string Register(UsuarioDtoIn usuarioDtoIn)
    {
        var usuario = _repository.AddUsuarioFromCredentials(usuarioDtoIn);
        return GenerateToken(usuario);
    }

    public string GenerateToken(UsuarioDtoOut usuarioDtoOut)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _configuration["JWT:ValidIssuer"],
            Audience = _configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioDtoOut.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuarioDtoOut.Nombre),
                new Claim(ClaimTypes.Email, usuarioDtoOut.Email),
                new Claim(ClaimTypes.Role, usuarioDtoOut.Rol)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public bool HasAccessToResource(int requestedUsuarioId, ClaimsPrincipal user)
        {
            if (user.IsInRole(Roles.Admin)) return true;

            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int usuarioId))
            {return false;}

            return usuarioId == requestedUsuarioId;
        }
    }

