using System.ComponentModel.DataAnnotations;

namespace Securyt.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength: 20, ErrorMessage = "El campo {0} no debe de tener mas de {0} caracteres  ")]
        public string Nombre { get; set; }
        //Relacion muchos a muchos
        public List<AutorPelicula> AutorPelicula { get; set; }
    }
}
