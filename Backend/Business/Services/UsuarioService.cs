using TeaShop.Data;
using TeaShop.Models;

namespace TeaShop.Business;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
       _repository = repository;
    }

    public IEnumerable<UsuarioDtoOut> GetAllUsuarios()
    {
        return _repository.GetAll();
    }

    public UsuarioDtoOut GetUsuario(int usuarioId)
    {
        return _repository.Get(usuarioId);
    }

    public void AñadirSaldo(int usuarioId, decimal cantidad)
    {
        if (cantidad <= 0)
            throw new ArgumentException("La cantidad a recargar debe ser mayor que 0.");
        var usuario = _repository.Get(usuarioId);
        if (!usuario.EsSocio)
            throw new InvalidOperationException("Solo los socios pueden recargar saldo.");

        _repository.AñadirSaldo(usuarioId, cantidad);
    }

    public void DeleteUsuario(int usuarioId)
    {
           
        _repository.Get(usuarioId);
        _repository.DeleteUsuario(usuarioId);
    }
    public void AddUsuario(UsuarioDtoIn usuarioDTO)
    {
        if (string.IsNullOrWhiteSpace(usuarioDTO.Nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.");

        var usuario = new Usuario
        {
            Nombre = usuarioDTO.Nombre,
            EsSocio = usuarioDTO.EsSocio,
            Saldo = 0
        };

        _repository.AddUsuario(usuarioDTO);
    }

    public void UpdateUsuario(int usuarioId, UsuarioDtoIn usuarioDTO)
    {
        var usuario = _repository.Get(usuarioId);
        if (usuario == null)
            throw new KeyNotFoundException("Usuario no encontrado.");

        if (string.IsNullOrWhiteSpace(usuarioDTO.Nombre))
            throw new ArgumentException("El nombre del usuario no puede estar vacío.");

        usuario.Nombre = usuarioDTO.Nombre;
        usuario.EsSocio = usuarioDTO.EsSocio;


        _repository.UpdateUsuario(usuarioId, usuarioDTO);
    }
}
