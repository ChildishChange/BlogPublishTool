using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

/// <summary>
/// 
/// 
/// </summary>
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
            BlogHandler blogHandler = null;
            if (opts.UploadFlag||opts.PublishFlag)
            {
                blogHandler = new BlogHandler();
            }

            
            if(opts.UploadFlag)
            {
                blogHandler.UploadPicture(opts.MarkdownFilePath);
            }
            
            if(!string.IsNullOrWhiteSpace(opts.JsonFilePath))
            {
                BlogHandler.ReplaceBlogUrl(opts.MarkdownFilePath, opts.JsonFilePath);
            }

            if(opts.PublishFlag)
            {
                blogHandler.PublishBlog(opts.MarkdownFilePath);
            }
        }
    }
}
