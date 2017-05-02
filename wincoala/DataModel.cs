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
        public String message; 
        public String origin;
        public int severity;
        // K = diff unit
        // V = the diff itself
        public Dictionary<String, String> diffs; 
    }

}
