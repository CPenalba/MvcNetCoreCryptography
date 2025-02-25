using Microsoft.EntityFrameworkCore;
using MvcNetCoreCryptography.Models;

namespace MvcNetCoreCryptography.Data
{
    public class UsuariosContext: DbContext
    {
        public UsuariosContext(DbContextOptions<UsuariosContext> options):base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
