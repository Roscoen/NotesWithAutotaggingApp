using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using NotesWithAutotagging.Models;
using NWA.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Web.IntegrationTests
{
    public class NotesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public NotesControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44369")
            });
            // Konfiguracja klienta HTTP do zaakceptowania niesprawdzonych certyfikatów SSL
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        private async Task<string> GetJwtToken()
        {
            var loginData = new { Username = "dg", Password = "dg1" };
            var loginContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

            var loginResponse = await _client.PostAsync("/Auth/login", loginContent);
            loginResponse.EnsureSuccessStatusCode();

            var responseString = await loginResponse.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<LoginResponse>(responseString);

            return responseObject.Token;
        }

        [Fact]
        public async Task PostNote_ShouldCreateNoteAndReturnCreatedResponse()
        {
            var token = await GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var newNote = new CreateNoteDto { Content = "Test Note" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(newNote), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/notes", jsonContent);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
        [Fact]
        public async Task GetAllNotes_ShouldReturnListOfNotes()
        {
            var token = await GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/api/notes");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<List<NoteDto>>(responseContent);

            Assert.NotNull(notes);
            // Możesz dodać więcej asercji w zależności od oczekiwań, np. Assert.NotEmpty(notes), jeśli oczekujesz danych
        }
        [Fact]
        public async Task GetNoteById_ShouldReturnNote()
        {
            var token = await GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
            var response = await _client.GetAsync("/api/notes/4");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<NoteDto>(responseContent);

            Assert.NotNull(note);
            Assert.Equal(4, note.Id); 
        }
        [Fact]
        public async Task UpdateNote_ShouldReturnNoContent()
        {
            var token = await GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var updatedNote = new UpdateNoteDto { Content = "Updated Content" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(updatedNote), Encoding.UTF8, "application/json");

            
            var response = await _client.PutAsync("/api/notes/4", jsonContent);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }
        [Fact]
        public async Task DeleteNote_ShouldReturnNoContent()
        {
            var token = await GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
            var response = await _client.DeleteAsync("/api/notes/5");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getResponse = await _client.GetAsync("/api/notes/5");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

    }
}
