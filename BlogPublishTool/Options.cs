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
        [Option('i', "input",  HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        [Option('l',"length", HelpText = "The maximum number of bytes to process.")]
        public int MaximumLength { get; set; }

        [Option('v',  HelpText = "Print details during execution.")]
        public bool Verbose { get; set; }
        
    }
}
