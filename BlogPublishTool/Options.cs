using CommandLine;

namespace BlogPublishTool
{
    [Verb("publish", HelpText = "Publish the blog file to the specified blog platform.")]
    public class PublishOptions
    {
        [Option("input", Required = true, HelpText = "Input path, file or directory.")]
        public string InputPath { get; set; }

        [Option("cnblogs", Required = false, HelpText = "Publish the blog on cnblogs.")]
        public bool CnblogsFlag { get; set; }

        [Option("config", Required = true, HelpText = "Config json file path.")]
        public string LinkJsonPath { get; set; }
    }
    
    [Verb("replace", HelpText = "Replace link of picture in the blog.")]
    public class ReplaceOptions
    {
        [Option("config", Required = true, HelpText = "Config json file path.")]
        public string LinkJsonPath { get; set; }

        [Option("input", Required = true, HelpText = "Input path, file or directory.")]
        public string InputPath { get; set; }

        [Option("output", Required = false, HelpText = "Output directory path.")]
        public string OutputPath { get; set; }
    }

    [Verb("uploadpic", HelpText = "Upload and replace picture with URL.")]
    public class UploadPicOptions
    {
        [Option("input", Required = true, HelpText = "Input path, file or directory.")]
        public string InputPath { get; set; }

        [Option("test", Required = false, Default = false, HelpText = "Not upload, only show all picture need to replace.")]
        public bool TestFlag { get; set; }
    }
}
