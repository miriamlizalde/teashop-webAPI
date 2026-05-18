using System.Security.Claims;
using TeaShop.Models;

namespace TeaShop.Business
{
    public interface IAuthService
    {
        public string Login(LoginDtoIn loginDtoIn);
        public string Register(UsuarioDtoIn usuarioDtoIn);
        public string GenerateToken(UsuarioDtoOut usuarioDtoOut);
        public bool HasAccessToResource(int requestedUsuarioId, ClaimsPrincipal user);
    }
}
