using NWA.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NWA.Application.Services
{
    public class TagService : ITagService
    {
        public List<string> AssignTagsToContent(string content)
        {
            var tags = new List<string>();

            
            var phoneRegex = new Regex(@"\b(\+?\d{1,3}[-\s.]?)?\(?\d{2,3}\)?[-\s.]?\d{3,4}[-\s.]?\d{3,4}\b");
            if (phoneRegex.IsMatch(content))
            {
                tags.Add("PHONE");
            }

            
            var emailRegex = new Regex(@"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b");
            if (emailRegex.IsMatch(content))
            {
                tags.Add("EMAIL");
            }

            return tags;
        }
    }
}
