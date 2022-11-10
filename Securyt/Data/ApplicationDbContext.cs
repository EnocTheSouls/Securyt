using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Securyt.Models;

namespace Securyt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Relacion muchos a muchos con la llave primaria compuesta
            modelBuilder.Entity<AutorPelicula>()
                .HasKey(x => new { x.AutorId, x.PeliculaId });
        }   
        public DbSet<Autor> Autores { get; set; }
        public DbSet<AutorPelicula> AutoresPeliculas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }  
    
    }
}

