using System;
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
             .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
             .WithNotParsed((errs) => HandleParseError(errs));
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {

            Console.ReadKey();
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            //login here


            if(opts.Upload)
            {
                //upload pic here
            }
            
            if(!string.IsNullOrWhiteSpace(opts.JsonFilePath))
            {
                //replace blog urls here
            }

            if(opts.PublishFlag)
            {
                ////publish blog here
            }


            Console.ReadKey();
            
        }


    }
}
