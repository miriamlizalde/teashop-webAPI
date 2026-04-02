using Microsoft.EntityFrameworkCore;
using TeaShop.Models;

namespace TeaShop.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TeashopContext _context;

        public UsuarioRepository(TeashopContext context)
        {
            _context = context;
        }
    

    public void AddUsuario(UsuarioDtoIn usuario)
    {
        var newUsuario = new Usuario
        {
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Password = usuario.Password,
            Rol = Roles.Usuario,
            EsSocio = usuario.EsSocio,
            FechaRegistro = DateTime.Now,
            Saldo = 0
        };
        _context.Usuarios.Add(newUsuario);
        SaveChanges();
    }

    public IEnumerable<UsuarioDtoOut> GetAll()
    {
        return _context.Usuarios.Select(u => new UsuarioDtoOut
        {
            UsuarioId = u.UsuarioId,
            Nombre = u.Nombre,
            Email = u.Email,
            Rol = u.Rol,
            Saldo = u.Saldo,
            EsSocio = u.EsSocio
        }).ToList();
    }
    
    public UsuarioDtoOut Get(int usuarioId)
    {
        var usuario = _context.Usuarios.Find(usuarioId);
        if (usuario == null) throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        return new UsuarioDtoOut
        {
            UsuarioId = usuario.UsuarioId,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Saldo = usuario.Saldo,
            EsSocio = usuario.EsSocio
        };
    }

    public void UpdateUsuario( int usuarioId, UsuarioDtoIn usuDtoIn)
    {
        var usuario = _context.Usuarios.Find(usuarioId);
        if (usuario == null) throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        usuario.Nombre = usuDtoIn.Nombre;
        usuario.Email = usuDtoIn.Email;
        usuario.EsSocio = usuDtoIn.EsSocio;
        if (!string.IsNullOrWhiteSpace(usuDtoIn.Password))
            usuario.Password = usuDtoIn.Password;

        _context.Entry(usuario).State = EntityState.Modified;
        SaveChanges();
    }

    public void DeleteUsuario(int usuarioId)
    {
        var usuario = _context.Usuarios.Find(usuarioId);
        if (usuario == null) throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        _context.Usuarios.Remove(usuario);
        SaveChanges();
    }

    public UsuarioDtoOut AddUsuarioFromCredentials(UsuarioDtoIn usuarioDtoIn)
    {
        bool existe = _context.Usuarios.Any(u => u.Email == usuarioDtoIn.Email);
        if (existe)
            throw new InvalidOperationException($"Ya existe un usuario con el email {usuarioDtoIn.Email}.");
        var newUsuario = new Usuario
        {
            Nombre = usuarioDtoIn.Nombre,
            Email = usuarioDtoIn.Email,
            Password = usuarioDtoIn.Password,
            Rol = Roles.Usuario,
            EsSocio = usuarioDtoIn.EsSocio,
            FechaRegistro = DateTime.Now,
            Saldo = 0
        };
        _context.Usuarios.Add(newUsuario);
        SaveChanges();

        return new UsuarioDtoOut
        {
            UsuarioId = newUsuario.UsuarioId,
            Nombre = newUsuario.Nombre,
            Email = newUsuario.Email,
            Rol = newUsuario.Rol,
            Saldo = newUsuario.Saldo,
            EsSocio = newUsuario.EsSocio
        };
    }

    public UsuarioDtoOut GetUsuarioFromCredentials(LoginDtoIn loginDtoIn)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == loginDtoIn.Email && u.Password == loginDtoIn.Password);
        if (usuario == null)
            throw new KeyNotFoundException("Credenciales inválidas.");

        return new UsuarioDtoOut
        {
            UsuarioId = usuario.UsuarioId,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Saldo = usuario.Saldo,
            EsSocio = usuario.EsSocio
        };
    }

    public void AñadirSaldo(int usuarioId, decimal cantidad)
    {
        var usuario = _context.Usuarios.Find(usuarioId);
        if (usuario == null) throw new KeyNotFoundException($"Usuario con ID {usuarioId} no encontrado.");

        usuario.Saldo += cantidad;
        SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    }
}        