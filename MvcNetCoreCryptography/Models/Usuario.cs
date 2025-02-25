using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MvcNetCoreCryptography.Models
{
    [Table("USERS")]
    public class Usuario
    {
        [Key]
        [Column("IDUSUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("EMAIL")]
        public string Email { get; set; }
        [Column("IMAGEN")]
        public string Imagen { get; set; }
        [Column("SALT")]
        public string Salt { get; set; }
        //UNA VENTAJA QUE TENEMOS ESTA EN QUE LOS 
        //VARBINARY O BLOB CON CONVERTIDOS A byte[]
        //AUTOMATICAMENTE CON EF
        [Column("PASS")]
        public byte[] Password { get; set; }
    }

}
