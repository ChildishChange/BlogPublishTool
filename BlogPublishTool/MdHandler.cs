using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    class MdHandler
    {
        public static List<string> RegexParser(string blogFilePath, string MatchRule)
        {
            List<string> parseList = new List<string>();
            string blogContent = File.ReadAllText(blogFilePath);
            var parses = Regex.Matches(blogContent, MatchRule, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            foreach (Match match in parses)
            {
                parseList.Add(match.Groups[1].Value);
            }
            return parseList;
        }
        
        public static string ReplaceContentWithUrl(string resBlogFilePath, Dictionary<string, string> contentUrlDic)
        {
            StringBuilder blogContent = new StringBuilder(File.ReadAllText(resBlogFilePath)); 
            foreach (KeyValuePair<string,string> pathUrlPair in contentUrlDic)
            {
                blogContent = blogContent.Replace(pathUrlPair.Key, pathUrlPair.Value);
            }
            return blogContent.ToString();
        }

        public static void WriteFile(string blogFilePath, string outBlogFilePath, string blogPlatform, string blogContent)
        {
            FileInfo fileInfo = new FileInfo(blogFilePath);
            File.WriteAllText(Path.Combine(outBlogFilePath,fileInfo.Name.Replace(fileInfo.Extension,"")+blogPlatform+fileInfo.Extension), blogContent);
        }
    }
}
