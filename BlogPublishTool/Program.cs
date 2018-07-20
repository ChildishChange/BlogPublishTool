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
            //login here
            //除了登录是直接调用bloghandler，其他都是调用mdhandler，mdhandler
            

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
                //replace blog urls here
            }

            if(opts.PublishFlag)
            {
                ////publish blog here
                blogHandler.PublishBlog(opts.MarkdownFilePath);
            }


            //Console.ReadKey();
            
        }
        

    }
}
