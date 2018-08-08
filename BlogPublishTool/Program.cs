using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;

namespace BlogPublishTool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<UploadPicOptions, ReplaceOptions>(args)
                .MapResult(
                  (UploadPicOptions opts)=> RunUploadPicOptions(opts),
                  (ReplaceOptions opts) => RunReplaceOptions(opts),
//                  (PublishOptions opts) => RunPublishOptions(opts),
                  errs => 1);
        }

        public static int RunUploadPicOptions(UploadPicOptions opts)
        {
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath,false);
            
            if(!string.IsNullOrWhiteSpace(opts.InputPath))
            {
                var markDownList = PathHandler.GetAllMarkDown(opts.InputPath);

                var blogHandler = new BlogHandler();

                foreach (var markDownPath in markDownList)
                {
                    blogHandler.UploadPicture(markDownPath, opts.TestFlag);
                }
            }
            return 0;
        }

        public static int RunReplaceOptions(ReplaceOptions opts)
        {
            opts.LinkJsonPath = PathHandler.GetAbsPath(opts.LinkJsonPath, false);
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath, false);

            opts.OutputPath = PathHandler.GetAbsPath(opts.OutputPath, true);

            if (!string.IsNullOrWhiteSpace(opts.InputPath) &&
                !string.IsNullOrWhiteSpace(opts.LinkJsonPath))
            {
                
                if (string.IsNullOrWhiteSpace(opts.OutputPath))
                {
                    opts.OutputPath = opts.InputPath;
                }

                if (!File.Exists(opts.LinkJsonPath))
                {
                    Console.WriteLine("[ERROR]Json must be a file! Please check path : " + opts.LinkJsonPath);
                    return 0;
                }

                List<string> markDownList = new List<string>();
                
                if ((new FileInfo(opts.InputPath).Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    var directoryInfo = new DirectoryInfo(opts.InputPath);

                    var fileInfos = directoryInfo.GetFiles();
                    var directoryInfos = directoryInfo.GetDirectories();

                    markDownList.AddRange(from file in fileInfos where file.Extension.ToLower() == ".md" select file.FullName);

                    foreach (var dir in directoryInfos)
                    {
                        if (dir.Name == "output")
                            continue;
                        PathHandler.GetAllMarkDown(dir.FullName, markDownList);
                    }
                }
                else
                {

                    if (!opts.InputPath.ToLower().EndsWith(".md"))
                    {
                        Console.WriteLine("[ERROR]Not a markdown file! Please check path : " + opts.InputPath);
                        return 0;
                    }
                    markDownList.Add(opts.InputPath);
                }

                foreach (var markDownPath in markDownList)
                {
                    BlogHandler.ReplaceBlogUrl(markDownPath, opts.InputPath, opts.OutputPath, opts.LinkJsonPath, "cnblogs");
                    BlogHandler.ReplaceBlogUrl(markDownPath, opts.InputPath, opts.OutputPath, opts.LinkJsonPath, "csdn");
                }
            }
            
            return 0;
        }

        public static int RunPublishOptions(PublishOptions opts)
        {
            opts.InputPath = PathHandler.GetAbsPath(opts.InputPath,false);
            opts.LinkJsonPath = PathHandler.GetAbsPath(opts.LinkJsonPath,false);

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
                    else
                    {
                        Console.WriteLine("[INFO]START BLOG PUBLISHING");
                        Console.WriteLine($"[INFO]{markDownPath} is a csdn blog, cannot be published now.");
                        Console.WriteLine("[INFO]END PUBLISH BLOG");
                    }
                }
            }
            return 0;
        }
    }
}
