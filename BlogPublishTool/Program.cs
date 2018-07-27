using System.IO;
using CommandLine;

namespace BlogPublishTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<UploadPicOptions, ReplaceOptions, PublishOptions>(args)
                .MapResult(
                  (UploadPicOptions opts)=> RunUploadPicOptions(opts),
                  (ReplaceOptions opts) => RunReplaceOptions(opts),
                  (PublishOptions opts) => RunPublishOptions(opts),
                  errs => 1);
        }

        public static int RunUploadPicOptions(UploadPicOptions opts)
        {
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath);
            
            if(!string.IsNullOrWhiteSpace(opts.InputPath))
            {
                var markDownList = PathHandler.GetAllMarkDown(opts.InputPath);

                var blogHandler = new BlogHandler();
                if (opts.TestFlag)
                {
                    foreach (var markDownPath in markDownList)
                    {
                        blogHandler.UploadPicture(markDownPath, true);
                    }
                }
                else
                {
                    foreach (var markDownPath in markDownList)
                    {
                        blogHandler.UploadPicture(markDownPath, false);

                    }
                }
            }
            return 0;

        }

        public static int RunReplaceOptions(ReplaceOptions opts)
        {
            opts.LinkJsonPath = PathHandler.GetAbsPath(opts.LinkJsonPath);
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath);
            opts.OutputPath = PathHandler.GetAbsPath(opts.OutputPath);

            if (!string.IsNullOrWhiteSpace(opts.InputPath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                var markDownList = PathHandler.GetAllMarkDown(opts.InputPath);

                if (!string.IsNullOrWhiteSpace(opts.OutputPath))
                {
                    foreach (var markDownPath in markDownList)
                    {
                        BlogHandler.ReplaceBlogUrl(markDownPath, opts.OutputPath, opts.LinkJsonPath, "cnblogs");
                        BlogHandler.ReplaceBlogUrl(markDownPath, opts.OutputPath, opts.LinkJsonPath, "csdn");
                        
                    }
                }
                else
                {
                    foreach (var markDownPath in markDownList)
                    {
                        BlogHandler.ReplaceBlogUrl(markDownPath, new FileInfo(markDownPath).DirectoryName, opts.LinkJsonPath, "cnblogs");
                        BlogHandler.ReplaceBlogUrl(markDownPath, new FileInfo(markDownPath).DirectoryName, opts.LinkJsonPath, "csdn");
                    }
                }
                
            }

            return 0;
        }

        public static int RunPublishOptions(PublishOptions opts)
        {
            
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath);
            opts.LinkJsonPath = PathHandler.GetAbsPath(opts.LinkJsonPath);

            if (!string.IsNullOrWhiteSpace(opts.InputPath) &&
                !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                var blogHandler = new BlogHandler();

                var markDownList = PathHandler.GetAllMarkDown(opts.InputPath);

                foreach (var markDownPath in markDownList)
                {
                    if(!markDownPath.EndsWith("-csdn.md"))
                    {
                        blogHandler.PublishBlog(markDownPath,opts.LinkJsonPath);
                    }
                }
            }
            
            return 0;
        }

        
    }
}
