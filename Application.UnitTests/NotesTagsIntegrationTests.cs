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

namespace Application.UnitTests
{
    public class NotesTagsIntegrationTests :IDisposable
    {
        private readonly AppDbContext _dbContext;
        private readonly TagService _tagService;
        private readonly NoteService _noteService;
        private readonly Mock<IMapper> _mockMapper;
        public NotesTagsIntegrationTests()
        {
            _mockMapper = new Mock<IMapper>();
            _tagService = new TagService();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;
            _dbContext = new AppDbContext(options);

            _mockMapper.Setup(mapper => mapper.Map<Note>(It.IsAny<NoteDto>()))
                   .Returns((NoteDto noteDto) => new Note { Content = noteDto.Content, Tags = new List<string>() });
            _mockMapper.Setup(mapper => mapper.Map<NoteDto>(It.IsAny<Note>()))
                       .Returns((Note note) => new NoteDto { Content = note.Content, Tags = note.Tags });

            _noteService = new NoteService(_tagService, _dbContext,  _mockMapper.Object);
        }

    
        [Fact]
        public async Task CreateNote_WithPhoneNumber_ShouldAddPhoneTagAndSaveToDatabase()
        {
            
            var noteDto = new NoteDto { Content = "Test 516354184" };

            
            var createdNoteDto = await _noteService.CreateNoteAsync(noteDto);

            
            Assert.NotNull(createdNoteDto);
            Assert.Contains("PHONE", createdNoteDto.Tags);

            
            var savedNote = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Content == noteDto.Content);
            Assert.NotNull(savedNote);
            Assert.Contains("PHONE", savedNote.Tags);
        }
        [Fact]
        public async Task CreateNote_WithEmail_ShouldAddEmailTagAndSaveToDatabase()
        {
            var noteDto = new NoteDto { Content = "Test example@wp.pl" };


            var createdNoteDto = await _noteService.CreateNoteAsync(noteDto);


            Assert.NotNull(createdNoteDto);
            Assert.Contains("EMAIL", createdNoteDto.Tags);


            var savedNote = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Content == noteDto.Content);
            Assert.NotNull(savedNote);
            Assert.Contains("EMAIL", savedNote.Tags);
        }

        [Fact]
        public async Task CreateNote_WithPhoneAndEmail_ShouldAddBothTagsAndSaveToDatabase()
        {
            var noteDto = new NoteDto { Content = "Test 736456484 example@wp.pl" };


            var createdNoteDto = await _noteService.CreateNoteAsync(noteDto);


            Assert.NotNull(createdNoteDto);
            Assert.Contains("EMAIL", createdNoteDto.Tags);
            Assert.Contains("PHONE", createdNoteDto.Tags);


            var savedNote = await _dbContext.Notes.FirstOrDefaultAsync(n => n.Content == noteDto.Content);
            Assert.NotNull(savedNote);
            Assert.Contains("EMAIL", savedNote.Tags);
            Assert.Contains("PHONE", savedNote.Tags);
        }
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }

    }
}
