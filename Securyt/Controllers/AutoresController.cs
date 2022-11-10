using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Securyt.Data;
using Securyt.DTOs;
using Securyt.Models;

namespace Securyt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController: Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AutoresController(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CrearAutorDTO crearAutorDTO)
        {
            var existeAutor = await _context.Autores.AnyAsync(x => x.Nombre == crearAutorDTO.Nombre);
            if (existeAutor)
            {
                return BadRequest(crearAutorDTO);
            }
            var autor = _mapper.Map<Autor>(crearAutorDTO);
            _context.Add(autor);
            await _context.SaveChangesAsync();

            var autorDTO = _mapper.Map<AutorDTO>(autor); 
            return Ok(autorDTO);
        }
        [HttpGet("primero")]
        public async Task<ActionResult<AutorDTO>> PrimeroAutor()
        {
            var autor = await _context.Autores.FirstOrDefaultAsync();
            return _mapper.Map<AutorDTO>(autor); 
        }
        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutoresPeliculasDTO>> Get(int id)
        {
            //var autor = await _context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            //var augtores2 = await _context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();
            var autor = await _context.Autores
                .Include(x => x.AutorPelicula) //Tabla Autores peliculas
                .ThenInclude(x => x.Pelicula) //Tabla Peiculas
                .FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return _mapper.Map<AutoresPeliculasDTO>(autor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CrearAutorDTO crearAutorDTO, int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            var autor = _mapper.Map<Autor>(crearAutorDTO);
            autor.Id = id;
            _context.Update(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Autores.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            _context.Remove(new Autor { Id = id });
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
