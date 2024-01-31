using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWA.Application.Services
{
    public interface ITagService
    {
        List<string> AssignTagsToContent(string content);
    }
}
