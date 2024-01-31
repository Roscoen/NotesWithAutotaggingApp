using NWA.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Interfaces
{
    public interface INoteService
    {
        Task<NoteDto> CreateNoteAsync( NoteDto noteDto);
        Task<NoteDto> GetNoteAsync(int id);
        Task<IEnumerable<NoteDto>> GetAllNotesAsync();
        Task UpdateNoteAsync(int id, UpdateNoteDto updateNoteDto);
        Task DeleteNoteAsync(int id);
    }
}
