using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlogPublishTool
{
    public class MdHandler
    {
        public static List<string> RegexParser(string blogFilePath, string matchRule)
        {
            var parseList = new List<string>();
            var blogContent = File.ReadAllText(blogFilePath);
            var parses = Regex.Matches(blogContent, matchRule, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            foreach (Match match in parses)
            {
                parseList.Add(match.Groups[1].Value);
            }
            return parseList;
        }
        
        public static string ReplaceContentWithUrl(string resBlogFilePath, Dictionary<string, string> contentUrlDic)
        {
            var blogContent = new StringBuilder(File.ReadAllText(resBlogFilePath)); 
            foreach (var pathUrlPair in contentUrlDic)
            {
                blogContent = blogContent.Replace(pathUrlPair.Key, pathUrlPair.Value);
            }
            return blogContent.ToString();
        }

        public static void WriteFile(string blogFilePath, string outDirPath, string blogPlatform, string blogContent)
        {
            var fileInfo = new FileInfo(blogFilePath);
            File.WriteAllText(Path.Combine(outDirPath, fileInfo.Name.Replace(fileInfo.Extension, blogPlatform + fileInfo.Extension)), blogContent);
        }
    }
}
