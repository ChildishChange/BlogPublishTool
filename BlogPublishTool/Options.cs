using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace BlogPublishTool
{
    class Options
    {
        [Value(0, HelpText = "Markdown file path.")]
        public string MarkdownFilePath { get; set; }

        [Option('u', "upload", Required = false, Default = false, HelpText = "Upload picture and replace picture file path with URL.")]
        public bool Upload { get; set; }

        [Option('r', "replace", Required = false, HelpText = "Replace relative document path with URL in json file.")]
        public string JsonFilePath { get; set; } 

        [Option('p', "publish", Required = false, Default = false, HelpText = "Publish this blog in Markdown format.")]
        public bool PublishFlag { get; set; }
        
    }

}
