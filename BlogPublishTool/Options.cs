using CommandLine;

/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    [Verb("publish", HelpText = "Publish the blog file to the specified blog platform.")]
    public class PublishOptions
    {   
        [Option("cnblogs", Required = true, HelpText = "Publish the blog on cnblogs.")]
        public string CnblogsFilePath { get; set; }
    }

    [Verb("uploadpic", HelpText = "Upload and replace picture with URL.")]
    public class UploadPicOptions
    {
        [Option("input", Required = true, HelpText = "Input path, file or directory.")]
        public string InputPath { get; set; }

        [Option("test", Required = false, Default = false, HelpText = "Not upload, only show all picture need to replace.")]
        public bool TestFlag { get; set; }
    }

    [Verb("replace", HelpText = "Replace link of picture in the blog")]
    public class ReplaceOptions
    {
        //deal with single file
        [Option("picture", Required = false, Default = false, HelpText = "Upload and replace picture with URL.")]
        public bool PictureFlag { get; set; }
        
        [Option("cnblogs", Required = false, HelpText = "Replace the link with blog on cnblogs.")]
        public string CnblogsFilePath { get; set; }

        [Option("csdn", Required = false, HelpText = "Replace the link with the blog on csdn.")]
        public string CsdnFilePath { get; set; }

        [Option("config", Required = false, HelpText = "Config json file path.")]
        public string LinkJsonPath { get; set; }

        //deal with a folder
        [Option("input", Required = false, HelpText = "Input path, file or directory.")]
        public string InputPath { get; set; }

        [Option("output", Required = false, HelpText = "Output path, file or directory.")]
        public string OutputPath { get; set; }
    }
}
