using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Securyt.DTOs;
using Securyt.Models;

namespace Securyt.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<CrearAutorDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutoresPeliculasDTO>()
                .ForMember(x => x.Peliculas, opciones => opciones.MapFrom(MapAutorDTOPeliculas));
        }

      

        private List<PeliculasDTO> MapAutorDTOPeliculas(Autor autor, AutorDTO autorDTO)
        {
            var lista = new List<PeliculasDTO>();
            if (autor.AutorPelicula == null)
            {
                return lista;
            }

            foreach (var autorLibro in autor.AutorPelicula)
            {
                lista.Add(new PeliculasDTO()
                {
                    Id = autorLibro.PeliculaId,
                    Titulo = autorLibro.Pelicula.Titulo
                });
            }
            return lista;
        }
    }
}
