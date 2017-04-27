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
        public String bears;
        public String file_data;
    }

    public struct LintResponse
    {
        public Dictionary<String, List<Result>> results;
        public String success;
    }

    public struct Result
    {
        public String message; 
        public String origin;
        public int severity;
        // K = diff unit
        // V = the diff itself
        public Dictionary<String, String> diffs; 
    }

}
