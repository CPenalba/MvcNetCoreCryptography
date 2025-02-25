using Microsoft.EntityFrameworkCore;
using MvcNetCoreCryptography.Data;
using MvcNetCoreCryptography.Helpers;
using MvcNetCoreCryptography.Models;

namespace MvcNetCoreCryptography.Repositories
{
    public class RepositoryUsuarios
    {
        private UsuariosContext context;

        public RepositoryUsuarios(UsuariosContext context)
        {
            this.context = context;
        }

        private async Task<int> GetMaxIdUser()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync
                    (x => x.IdUsuario) + 1;
            }
        }

        public async Task RegisterUserAsync(string nombre,
            string email,
            string password, string imagen)
        {
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUser();
            user.Nombre = nombre;
            user.Email = email;
            user.Imagen = imagen;
            //CADA USUARIO TENDRA UN SALT DIFERENTE
            user.Salt = HelperCryptography.GenerateSalt();
            //ALMACENAMOS EL PASSWORD CIFRADO A byte[]
            user.Password =
                HelperCryptography.EncryptPassword(password, user.Salt);
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
        }

        //NECESITAMOS UN METODO PARA HACER UN LOGIN DE USUARIO
        //Y DEVOLVEMOS AL USUARIO SI HEMOS COMPROBADO TODO CORRECTAMENTE
        //COMO TENEMOS CIFRADO, DEBEMOS HACER EL LOGIN PIDIENDO
        //DATOS UNICOS (email, username, nif)
        public async Task<Usuario> LogInUserAsync
            (string email, string password)
        {
            //BUSCAMOS AL USUARIO POR EL DATO UNICO (email)
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            Usuario user = await consulta.FirstOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            else
            {
                //RECUPERAMOS EL SALT DEL USUARIO DE BBDD
                string salt = user.Salt;
                //CONVERTIMOS EL PASSWORD QUE NOS HAN DADO Y EL SALT
                //A byte[]
                byte[] temp =
                    HelperCryptography.EncryptPassword(password, salt);
                //RECUPERAMOS EL PASSWORD DE BYTE[] DE BBDD
                byte[] passBytes = user.Password;
                //POR ULTIMO, REALIZAMOS LA COMPARACION  DE ARRAYS
                bool response =
                    HelperCryptography.CompararArrays(temp, passBytes);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
