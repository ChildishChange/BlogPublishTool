using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    class MdHandler
    {
        public static List<string> ParsePicture(string blogFilePath)
        {
            const string MatchRule = @"!\[.*?\]\((.*?)\)";

            List<string> pictureList = new List<string>();
            string blogContent = File.ReadAllText(blogFilePath);
            var pictures = Regex.Matches(blogContent, MatchRule, RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
            foreach (Match picture in pictures)
            {
                pictureList.Add(picture.Groups[1].Value);
            }
            return pictureList;
        }


        public static void ReplacePicWithUrl(string resBlogFilePath, Dictionary<string, string> pictureUrlDic)
        {
            StringBuilder blogContent = new StringBuilder(File.ReadAllText(resBlogFilePath)); 
            foreach (KeyValuePair<string,string> pathUrlPair in pictureUrlDic)
            {
                blogContent = blogContent.Replace(pathUrlPair.Key, pathUrlPair.Value);
            }
            //Console.WriteLine(blogContent);

            File.WriteAllText(resBlogFilePath,blogContent.ToString());
        }
    }
}
