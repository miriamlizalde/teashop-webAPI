using TeaShop.Models;

namespace TeaShop.Business
{
    public interface IUsuarioService
    {
        IEnumerable<UsuarioDtoOut> GetAllUsuarios();
        UsuarioDtoOut GetUsuario(int usuarioId);

        void AñadirSaldo(int usuarioId, decimal cantidad);
        void AddUsuario(UsuarioDtoIn usuarioDTO);
        void UpdateUsuario(int usuarioId, UsuarioDtoIn usuarioDTO);
        void DeleteUsuario(int usuarioId);
    }
}