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

        //TBD
        //[Option("csdn", Required = false, Default = false, HelpText = "Publish the blog on csdn.")]
        //public string CsdnFilePath { get; }
    }

    [Verb("replace", HelpText = "Replace link of picture in the blog")]
    public class ReplaceOptions
    {
        [Option("picture", Required = false, Default = false, HelpText = "Upload and replace picture with URL.")]
        public bool PictureFlag { get; set; }
        
        [Option("link", Required = false, HelpText = "Replace link with URL.")]
        public string LinkJsonPath { get; set; }
        
        [Option("cnblogs", Required = false, HelpText = "Replace the link with blog on cnblogs.")]
        public string CnblogsFilePath { get; set; }

        [Option("csdn", Required = false, HelpText = "Replace the link with the blog on csdn.")]
        public string CsdnFilePath { get; set; }
    }
}
