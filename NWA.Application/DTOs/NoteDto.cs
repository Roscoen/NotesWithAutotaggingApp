using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<string> Tags { get; set; }
    }
}
