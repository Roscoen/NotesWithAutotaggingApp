using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NWA.Application.DTOs;
using NWA.Application.Interfaces;

namespace NotesWithAutotagging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _notesService;

        public NotesController(INoteService notesService)
        {
            _notesService = notesService;
        }

        // GET: api/notes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _notesService.GetAllNotesAsync();
            return Ok(posts);
        }

        // GET: api/notes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var post = await _notesService.GetNoteAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "InternalServerError");
            }
        }

        // POST: api/notes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto createNoteDto)
        {
            var noteDto = new NoteDto
            {
                Content = createNoteDto.Content
                
            };
            var createdNoteDto = await _notesService.CreateNoteAsync(noteDto);
            return CreatedAtAction(nameof(Get), new { id = createdNoteDto.Id }, createdNoteDto);
        }

        // PUT: api/notes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                await _notesService.UpdateNoteAsync(id, updateNoteDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _notesService.DeleteNoteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "InternalServerError");
            }
        }
    }
}
