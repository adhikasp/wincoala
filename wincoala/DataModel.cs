using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wincoala
{
    public class BearMetadata
    {
        [PrimaryKey]
        public String Name { get; set; }
        public String Description { get; set; }
        public String Languages { get; set; } 
        public List<String> LanguagesAsList
        {
            get { return Languages.Split(',').ToList<string>(); }
        }
        public void setLanguagesFromList(List<String> languages)
        {
            Languages = string.Join(",", languages);
        }
    }

    public class BearListResponse
    {
        public String name { get; set; }
        public String desc { get; set; }
        public List<String> languages { get; set; } 
    }

    public class LintRequest
    {
        public String bears;
        public String file_data;
    }

    public class LintResponse
    {
        public Dictionary<String, List<Result>> results;
        public String success;
    }

    public class Result
    {
        public String message { get; set; }
        public String origin { get; set; }
        public int severity { get; set; }
        // K = diff unit
        // V = the diff itself
        public Dictionary<String, String> diffs { get; set; }
        public List<SourceRange> affected_code { get; set; }
        // Special attribute to embed information from affected_code
        // (only have line number of file) with the text itself.
        public List<String> snippets { get; set; }
    }

    // Data structure taken directly from
    // https://github.com/coala/coala/blob/master/coalib/results/SourceRange.py#L9
    public class SourceRange
    {
        public SourcePosition start { get; set; }
        public SourcePosition end { get; set; }
    }

    public class SourcePosition
    {
        SourcePosition() 
        {
            this.column = -1;
        }
        public int line { get; set; }
        // Nullable because not all bears can return column number
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Populate)]
        public int column { get; set; }
    }

}
