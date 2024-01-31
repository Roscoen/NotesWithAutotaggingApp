using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NWA.Application.DTOs;
using NWA.Application.Interfaces;
using NWA.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly ITagService _tagService;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public NoteService(ITagService tagService, IAppDbContext context, IMapper mapper)
        {
            _tagService = tagService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<NoteDto> CreateNoteAsync(NoteDto noteDto)
        {
            var note = _mapper.Map<Note>(noteDto);
            note.Tags = _tagService.AssignTagsToContent(noteDto.Content);

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return _mapper.Map<NoteDto>(note);
        }

        public async Task<NoteDto> GetNoteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) return null;

            return _mapper.Map<NoteDto>(note);
        }
        public async Task<IEnumerable<NoteDto>> GetAllNotesAsync()
        {
            var notes = await _context.Notes.ToListAsync();
            return _mapper.Map<IEnumerable<NoteDto>>(notes);
        }
        public async Task UpdateNoteAsync(int id, UpdateNoteDto updateNoteDto)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null) throw new Exception("Note not found");

            note.Content = updateNoteDto.Content;
            note.Tags = _tagService.AssignTagsToContent(updateNoteDto.Content);

            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
                throw new KeyNotFoundException("Note not found.");

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }
    }
}
