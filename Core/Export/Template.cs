﻿using System.Collections.Generic;

namespace Core.Export
{
    public class Template
    {
        public string Name { get; set; }
        public string Rootdir { get; set; }
        public string Headertpl { get; set; }
        public string Articletpl { get; set; }
        public string Footertpl { get; set; }
        public bool IncludeToc { get; set; }
        public string Tocheadertpl { get; set; }
        public string Toctpl { get; set; }
        public string Tocfootertpl { get; set; }
        public string Countheadertpl { get; set; }
        public string Counttpl { get; set; }
        public string Countfootertpl { get; set; }
        public string Countgrouptpl { get; set; }
        public string Countindentstr { get; set; }

        public IList<string> Images { get; set; }
    }
}
