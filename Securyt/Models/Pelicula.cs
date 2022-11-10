using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Securyt.Models
{
    public class Pelicula
    {
        public int Id { get; set; } 
        public string Titulo { get; set; }  
        public DateTime FechaEstreno { get; set; }
        //Relacion muchos a muchos
        public List<AutorPelicula> AutorPelicula { get; set; }
    }
}

