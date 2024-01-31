using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NWA.Application.DTOs;
using NWA.Application.Interfaces;
using NWA.Application.Services;
using NWA.Domain.Entities;
using NWA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IntegrationTests
{
    public class NoteTests : IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<IMapper> _mockMapper;
        public NoteTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockMapper.Setup(mapper => mapper.Map<Note>(It.IsAny<NoteDto>()))
                   .Returns((NoteDto noteDto) => new Note { Content = noteDto.Content, Tags = new List<string>() });
            _mockMapper.Setup(mapper => mapper.Map<NoteDto>(It.IsAny<Note>()))
                       .Returns((Note note) => new NoteDto { Content = note.Content, Tags = note.Tags });
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
                                _dbContext = new AppDbContext(options);
            
        }
        [Fact]
        public async Task CreateNote_WithValidData_ShouldAddNoteToDatabase()
        {
            
            var note = new Note { Content = "Valid Content" };

            
            _dbContext.Notes.Add(note);
            await _dbContext.SaveChangesAsync();

            
            var savedNote = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Content == "Valid Content");
            Assert.NotNull(savedNote);
            Assert.Equal("Valid Content", savedNote.Content);
        }
        [Fact]
        public async Task CreateNote_WithInvalidData_ShouldNotAddNoteToDatabase()
        {
            
            var note = new Note { /* missing required fields */ };

            
            await Assert.ThrowsAsync<DbUpdateException>(() =>
            {
                _dbContext.Notes.Add(note);
                return _dbContext.SaveChangesAsync();
            });
        }
        [Fact]
        public async Task UpdateNote_WithValidData_ShouldModifyExistingNote()
        {
            
            var originalNote = new Note { Content = "Original Content" };
            _dbContext.Notes.Add(originalNote);
            await _dbContext.SaveChangesAsync();

            
            var savedNote = await _dbContext.Notes.FindAsync(originalNote.Id);
            savedNote.Content = "Updated Content";
            await _dbContext.SaveChangesAsync();

            
            var updatedNote = await _dbContext.Notes.FindAsync(originalNote.Id);
            Assert.Equal("Updated Content", updatedNote.Content);
        }

        [Fact]
        public async Task UpdateNonExistentNote_ShouldNotAffectDatabase()
        {
            var nonExistentNote = new Note { Id = 999, Content = "Non-existent Content" };

            
            _dbContext.Notes.Update(nonExistentNote);

            var exception = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _dbContext.SaveChangesAsync());
            Assert.NotNull(exception);
        }

        [Fact]
        public async Task GetAllNotes_WhenNoNotes_ShouldReturnEmptyList()
        {
            var notes = await _dbContext.Notes.ToListAsync();

            var result = _mockMapper.Object.Map<IEnumerable<NoteDto>>(notes);

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllNotes_WhenNotesExist_ShouldReturnAllNotes()
        {
            _dbContext.Notes.Add(new Note { Content = "Note 1" });
            _dbContext.Notes.Add(new Note { Content = "Note 2" });
            await _dbContext.SaveChangesAsync();



            var notes = await _dbContext.Notes.ToListAsync();

            Assert.Equal(2, notes.Count());
        }

        [Fact]
        public async Task GetNoteById_WhenNoteExists_ShouldReturnNote()
        {
            var newNote = new Note { Content = "Test Note" };
            _dbContext.Notes.Add(newNote);
            await _dbContext.SaveChangesAsync();

            var note = await _dbContext.Notes.FindAsync(newNote.Id);

            var result = _mockMapper.Object.Map<NoteDto>(note);

            Assert.NotNull(result);
            Assert.Equal("Test Note", result.Content);
        }

        [Fact]
        public async Task GetNoteById_WhenNoteDoesNotExist_ShouldReturnNull()
        {
            var nonExistentId = 999;

            var note = await _dbContext.Notes.FindAsync(nonExistentId);

            Assert.Null(note);
        }


        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }
    }
        
}
