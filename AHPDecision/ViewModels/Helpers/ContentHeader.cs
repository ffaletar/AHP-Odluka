using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AHPDecision.ViewModels.Helpers
{
    public class ContentHeader
    {
        public string MainTitle { get; private set; }
        public string SmallTitle { get; private set; }
        public List<Tuple<string,string, string>> Path { get; private set; }

        public ContentHeader(string mainTitle, string smallTitle, List<Tuple<string,string,string>> path)
        {
            this.MainTitle = mainTitle;
            this.SmallTitle = smallTitle;
            this.Path = path;
        }
    }
}