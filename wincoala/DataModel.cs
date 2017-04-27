using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wincoala
{
    public struct BearMetadata
    {
        public String name;
        public String desc;
        public List<String> languages;
    }

    public struct LintRequest
    {
        public List<String> bears;
        public String fileData;
    }
}
