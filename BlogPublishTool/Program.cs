using System;
using CommandLine;

/// <summary>
/// 
/// 
/// </summary>
namespace BlogPublishTool
{
    class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<PublishOptions, ReplaceOptions>(args)
                .MapResult(
                  (PublishOptions opts) => RunPublishOptions(opts),
                  (ReplaceOptions opts) => RunReplaceOptions(opts),
                  errs => 1);
        }

        private static int RunReplaceOptions(ReplaceOptions opts)
        {
            //cnblogs && link
            if(!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                BlogHandler.ReplaceBlogUrl(opts.CnblogsFilePath, opts.LinkJsonPath, "cnblogs");
            }
            
            //csdn && link
            if (!string.IsNullOrWhiteSpace(opts.CsdnFilePath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                BlogHandler.ReplaceBlogUrl(opts.CsdnFilePath, opts.LinkJsonPath, "csdn");
            }
            
            //cnblogs && picture
            if (!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
                opts.PictureFlag)
            {
                BlogHandler blogHandler = new BlogHandler();

                blogHandler.UploadPicture(opts.CnblogsFilePath);
            }

            //csdn && picture
            if (!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
                opts.PictureFlag)
            {
                Console.WriteLine("Sorry, replace picture function only supports cnblogs.");
            }
            
            return 0;
        }

        private static int RunPublishOptions(PublishOptions opts)
        {
            if(!string.IsNullOrWhiteSpace(opts.CnblogsFilePath))
            {
                BlogHandler blogHandler = new BlogHandler();

                blogHandler.PublishBlog(opts.CnblogsFilePath);
            }
            return 0;
        }
        
    }
}
