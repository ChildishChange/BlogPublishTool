﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaWeblogClient;
using CommandLine;

//init
namespace BlogPublishTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
             .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
             .WithNotParsed<Options>((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {

        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            opts.
            
        }


    }
}
