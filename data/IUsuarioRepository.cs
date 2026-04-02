using TeaShop.Models;

namespace TeaShop.Data
{
    public interface IUsuarioRepository
    {
        public void AddUsuario(UsuarioDtoIn usuarioDtoIn);
        public IEnumerable<UsuarioDtoOut> GetAll();
        public UsuarioDtoOut Get(int usuarioId);
        public void UpdateUsuario(int usuarioId, UsuarioDtoIn usuarioDtoIn);
        public void DeleteUsuario(int usuarioId);
        public void SaveChanges();
        public UsuarioDtoOut AddUsuarioFromCredentials(UsuarioDtoIn UsuarioDtoIn);
        public UsuarioDtoOut GetUsuarioFromCredentials(LoginDtoIn loginDtoIn);
        public void AñadirSaldo(int usuarioId, decimal nuevoSaldo)
        ;
        
    }
}