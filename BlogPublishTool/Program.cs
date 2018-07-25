using System;
using System.Collections.Generic;
using System.IO;
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
                  (UploadPicOptions opts)=> RunUploadPicOptions(opts),
                  errs => 1);
        }

        private static int RunReplaceOptions(ReplaceOptions opts)
        {
            
            //默认是文件
            opts.CnblogsFilePath = PathHandler.GetAbsPath(opts.CnblogsFilePath);
            opts.CsdnFilePath = PathHandler.GetAbsPath(opts.CsdnFilePath);
            opts.LinkJsonPath = PathHandler.GetAbsPath(opts.LinkJsonPath);

            //可以是文件也可以是文件夹
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath);
            opts.OutputPath = PathHandler.GetAbsPath(opts.OutputPath);
            
            //cnblogs && config
            if (!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                BlogHandler.ReplaceBlogUrl(opts.CnblogsFilePath, new FileInfo(opts.CnblogsFilePath).DirectoryName, opts.LinkJsonPath, "-cnblogs");
            }

            //csdn && config
            if (!string.IsNullOrWhiteSpace(opts.CsdnFilePath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                BlogHandler.ReplaceBlogUrl(opts.CsdnFilePath, new FileInfo(opts.CsdnFilePath).DirectoryName, opts.LinkJsonPath, "-csdn");
            }
            
            //cnblogs && picture
            if (!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
                opts.PictureFlag)
            {
                BlogHandler blogHandler = new BlogHandler();

                blogHandler.UploadPicture(opts.CnblogsFilePath, false);
            }

            //csdn && picture
            if (!string.IsNullOrWhiteSpace(opts.CnblogsFilePath) &&
                opts.PictureFlag)
            {
                Console.WriteLine("Sorry, replace picture function only supports cnblogs.");
            }
            
            //input output config
            if(!string.IsNullOrWhiteSpace(opts.InputPath) &&
               !string.IsNullOrWhiteSpace(opts.OutputPath) &&
               !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                List<string> markDownList = new List<string>();

                if(new FileInfo(opts.InputPath).Attributes == FileAttributes.Directory)
                {
                    markDownList = PathHandler.GetAllMarkDown(opts.InputPath, markDownList);
                }
                else
                {
                    markDownList.Add(opts.InputPath);
                }
                
                foreach (string markDownPath in markDownList)
                {
                    BlogHandler.ReplaceBlogUrl(markDownPath, opts.OutputPath, opts.LinkJsonPath, "-cnblogs");
                    BlogHandler.ReplaceBlogUrl(markDownPath, opts.OutputPath, opts.LinkJsonPath, "-csdn");
                }
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

        private static int RunUploadPicOptions(UploadPicOptions opts)
        {
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath);

            List<string> markDownList = new List<string>();

            if (new FileInfo(opts.InputPath).Attributes == FileAttributes.Directory)
            {
                markDownList = PathHandler.GetAllMarkDown(opts.InputPath, markDownList);
            }
            else
            {
                markDownList.Add(opts.InputPath);
            }

            BlogHandler blogHandler = new BlogHandler();
            if (opts.TestFlag)
            {
                foreach (string markDownPath in markDownList)
                {
                    blogHandler.UploadPicture(markDownPath, true);
                }
            }
            else
            {
                foreach (string markDownPath in markDownList)
                {
                    blogHandler.UploadPicture(markDownPath, false);
                    blogHandler.PublishBlog(markDownPath);
                    blogHandler.PublishBlog(markDownPath);
                }
            }

            return 0;
        }
        
    }
}
