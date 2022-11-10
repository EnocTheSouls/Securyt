namespace Securyt.Models
{
    public class AutorPelicula
    {
        //Llave compuesta configurada en el Db Context
        public int PeliculaId { get; set;}
        public int AutorId { get; set;}
        //Relacion muchos a muchos
        public Autor Autor { get; set;}
        public Pelicula Pelicula { get; set;}

    }
}
