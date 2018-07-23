using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetaWeblogClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// 
/// </summary>
namespace BlogPublishTool
{
    public class BlogHandler
    {
        private static BlogConnectionInfo connectionInfo;
        
        private const string blogUrl = "https://www.cnblogs.com/";
        private const string metaWeblogUrl = "https://rpc.cnblogs.com/metaweblog/";
        
        /// <summary>
        /// 
        /// </summary>
        public BlogHandler()
        {
            Console.WriteLine("Please input your blog ID:");
            string BlogId = Console.ReadLine();

            Console.WriteLine("Please input your user name:");
            string UserName = Console.ReadLine();

            Console.WriteLine("Please input your password:");
            string PassWord = GetPassword();
            
            connectionInfo = new BlogConnectionInfo(
                blogUrl + BlogId,
                metaWeblogUrl + BlogId,
                BlogId,
                UserName,
                PassWord);
        }

         /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        private static string GetPassword()
        {
            var password = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.Write('\n');
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Remove(password.Length - 1,1);
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password.Append(i.KeyChar);
                    Console.Write("*");
                }
            }
            return password.ToString();
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        public void UploadPicture(string blogFilePath)
        {
            Client blogClient = new Client(connectionInfo);
            Console.WriteLine("======>START UPLOAD PICTURE<======");

            const string MatchRule = @"!\[.*?\]\((.*?)\)";
            var pictureList = MdHandler.RegexParser(blogFilePath, MatchRule);

            Dictionary<string, string> pictureUrlDic = new Dictionary<string, string>();
            
            foreach(string picturePath in pictureList)
            {
                if (picturePath.StartsWith("http"))
                {
                    Console.WriteLine($"Jump picture:{picturePath}.");
                    continue;
                }

                //upload picture
                try
                {
                    string pictureAbsPath = Path.Combine(new FileInfo(blogFilePath).DirectoryName, picturePath);
                    if (File.Exists(pictureAbsPath))
                    {
                        var pictureUrl = blogClient.NewMediaObject(picturePath, "image / jpeg", File.ReadAllBytes(pictureAbsPath));

                        if (!pictureUrlDic.ContainsKey(picturePath))
                        {
                            pictureUrlDic.Add(picturePath, pictureUrl.URL);
                        }
                        Console.WriteLine($"{picturePath} uploaded");
                    }
                    else
                    {
                        Console.WriteLine($"No such file:{picturePath}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            string blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, pictureUrlDic);
            MdHandler.WriteFile(blogFilePath, "cnblogs", blogContent);
            Console.WriteLine("======>END UPLOAD PICTURE<======");
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="blogFilePath"></param>
        /// <param name="jsonFilePath"></param>
        public static void ReplaceBlogUrl(string blogFilePath, string jsonFilePath, string blogPlatform)
        {
            Console.WriteLine("======>START REPLACE BLOG URL<======");

            const string MatchRule = @"\[.*?\]\((.*?\.md)\)";
            var linkList = MdHandler.RegexParser(blogFilePath, MatchRule);
            Dictionary<string, string> blogUrlDic = new Dictionary<string, string>();
            
            JArray blogJson = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(jsonFilePath));

            Dictionary<string, JObject> blogJsonDic = new Dictionary<string, JObject>();

            foreach (JObject blog in blogJson)
            {
                blogJsonDic.Add(blog.Properties().First().Name, blog);
            }

            foreach (string link in linkList)
            {
                if (link.StartsWith("http"))
                {
                    Console.WriteLine($"Jump Link:{link}.");
                    continue;
                }

                //upload picture
                try
                {
                    string blogUrl = blogJsonDic[link][link][blogPlatform].ToString();
                    if (!blogUrlDic.ContainsKey(link))
                    {
                        blogUrlDic.Add(link, blogUrl);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            string blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, blogUrlDic);

            //添加写入文件的部分
            MdHandler.WriteFile(blogFilePath, blogPlatform, blogContent);
            
            Console.WriteLine("======>END REPLACE BLOG URL<======");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogFilePath"></param>
        public void PublishBlog(string blogFilePath)
        {
            Client blogClient = new Client(connectionInfo);
            Console.WriteLine("======>START PUBLISH BLOG<======");

            Console.WriteLine("Please input title of this blog:");
            string blogTitle = Console.ReadLine();
            
            string blogContent = File.ReadAllText(blogFilePath);

            var postID = blogClient.NewPost(blogTitle, blogContent, new List<string>(){"[Markdown]"}, true, DateTime.Now);

            Console.WriteLine("Blog published here: "+ connectionInfo.BlogURL+"/p/"+postID+".html");
            Console.WriteLine("======>END PUBLISH BLOG<======");
        }

    }
}
