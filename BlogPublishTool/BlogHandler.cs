using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MetaWeblogClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlogPublishTool
{
    public class BlogHandler
    {
        private static BlogConnectionInfo _connectionInfo;
        private static Dictionary<string, string> _picFileTable;

        private const string BlogUrl = "https://www.cnblogs.com/";
        private const string MetaWeblogUrl = "https://rpc.cnblogs.com/metaweblog/";

        public BlogHandler()
        {
            Console.WriteLine("Please input your blog ID:");
            var blogId = Console.ReadLine();

            Console.WriteLine("Please input your user name:");
            var userName = Console.ReadLine();

            Console.WriteLine("Please input your password:");
            var passWord = GetPassword();
            
            _connectionInfo = new BlogConnectionInfo(
                BlogUrl + blogId,
                MetaWeblogUrl + blogId,
                blogId,
                userName,
                passWord);
            InitPicFileTable();
        }

        private void InitPicFileTable()
        {
            _picFileTable = new Dictionary<string, string>
            {
                { ".bmp", "image/bmp" },
                { ".gif", "image/gif" },
                { ".ico", "image/x-icon" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" }
            };
        }
      

        public static string GetPassword()
        {
            var password = new StringBuilder();
            while (true)
            {
                var i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    Console.Write('\n');
                    break;
                }
                
                switch (i.Key)
                {
                    case ConsoleKey.Backspace when password.Length <= 0:
                        continue;
                    case ConsoleKey.Backspace:
                        password.Remove(password.Length - 1,1);
                        Console.Write("\b \b");
                        break;
                    default:
                        password.Append(i.KeyChar);
                        Console.Write("*");
                        break;
                }
            }
            return password.ToString();
        }


        public void UploadPicture(string blogFilePath, bool testFlag)
        {
            Console.WriteLine($"[INFO]START UPLOAD PICTURE IN FILE:\n{blogFilePath}");

            const string matchRule = @"!\[.*?\]\((.*?)\)";
            var pictureList = MdHandler.RegexParser(blogFilePath, matchRule);
            var pictureUrlDic = new Dictionary<string, string>();

            Client blogClient = null;
            if (!testFlag)
            {
                blogClient = new Client(_connectionInfo);
            }

            foreach (var picturePath in pictureList)
            {
                if (picturePath.StartsWith("http"))
                {
                    Console.WriteLine($"[INFO]Jump picture:{picturePath}");
                    continue;
                }
                
                try
                {
                    var pictureAbsPath = Path.Combine(new FileInfo(blogFilePath).DirectoryName, picturePath);
                    if (File.Exists(pictureAbsPath))
                    {
                        if(!testFlag)
                        {
                            var pictureUrl = blogClient.NewMediaObject(picturePath, _picFileTable[new FileInfo(picturePath).Extension.ToLower()], File.ReadAllBytes(pictureAbsPath));

                            if (!pictureUrlDic.ContainsKey(picturePath))
                            {
                                pictureUrlDic.Add(picturePath, pictureUrl.URL);
                            }
                            Console.WriteLine($"[INFO]{picturePath} uploaded");
                        }
                        else
                        {
                            Console.WriteLine($"[INFO]{picturePath} needs upload");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[ERROR]No such file:{picturePath}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR]"+e.Message);
                }
            }
            if(!testFlag)
            {
                var blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, pictureUrlDic);
                MdHandler.WriteFile(blogFilePath, new FileInfo(blogFilePath).DirectoryName, "", blogContent);
            }
            
            Console.WriteLine("[INFO]END UPLOAD PICTURE\n");
        }


        public static void ReplaceBlogUrl(string blogFilePath, string outDirPath, string jsonFilePath, string blogPlatform)
        {
            Console.WriteLine($"[INFO]START REPLACE BLOG URL:\n{blogFilePath}");
            const string matchRule = @"\[.*?\]\((.*?\.md)\)";
            var linkList = MdHandler.RegexParser(blogFilePath, matchRule);
            var blogUrlDic = new Dictionary<string, string>();
            var blogJson = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(jsonFilePath));
            var blogJsonDic = new Dictionary<string, JObject>();
            foreach (var jToken in blogJson)
            {
                var blog = (JObject) jToken;
                blogJsonDic.Add(blog.Properties().First().Name, blog);
            }

            foreach (var link in linkList)
            {
                if (link.StartsWith("http"))
                {
                    Console.WriteLine($"[INFO]Jump Link:{link}");
                    continue;
                }
                try
                {
                    var blogUrl = blogJsonDic[link][link][blogPlatform].ToString();
                    if (!blogUrlDic.ContainsKey(link))
                    {
                        Console.WriteLine($"[INFO]replace link {link} to {blogUrl}");
                        blogUrlDic.Add(link, blogUrl);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR]"+e.Message);
                }
            }
            var blogContent = MdHandler.ReplaceContentWithUrl(blogFilePath, blogUrlDic);


            if (blogPlatform == "cnblogs" || blogFilePath.EndsWith("-csdn.md"))
            {
                MdHandler.WriteFile(blogFilePath, outDirPath, "", blogContent);
            }
            else
            {
                MdHandler.WriteFile(blogFilePath, outDirPath, "-" + blogPlatform, blogContent);
            }
            Console.WriteLine("[INFO]END REPLACE BLOG URL");
        }


        public void PublishBlog(string blogFilePath, string jsonFilePath)
        {
            var blogClient = new Client(_connectionInfo);
            var blogContent = File.ReadAllText(blogFilePath);

            var fileInfo = new FileInfo(blogFilePath);
            var blogJson = (JArray)JsonConvert.DeserializeObject(File.ReadAllText(jsonFilePath));
            var blogJsonDic = new Dictionary<string, JObject>();
            foreach (var jToken in blogJson)
            {
                var blog = (JObject)jToken;
                blogJsonDic.Add(blog.Properties().First().Name, blog);
            }

            if (blogJsonDic.ContainsKey(fileInfo.Name))
            {
                var blogUrl = blogJsonDic[fileInfo.Name][fileInfo.Name]["cnblogs"].ToString();
                var postId = blogUrl.Replace(_connectionInfo.BlogURL + "/p/", "").Replace(".html","");


                Console.WriteLine("[INFO]This blogs has been published before:\n" + blogFilePath);
                Console.WriteLine("[INFO]START BLOG EDITTING");
                blogClient.EditPost(postId,
                                    blogJsonDic[fileInfo.Name][fileInfo.Name]["title"].ToString(),
                                    blogContent,
                                    new List<string> { "[Markdown]" },
                                    true);
                Console.WriteLine("[INFO]END BLOG EDITTING");
            }
            else
            {
                Console.WriteLine("[INFO]START PUBLISH BLOG\n");
                Console.WriteLine("[INFO]Please input title of this blog:\n" + blogFilePath);
                var blogTitle = Console.ReadLine();


                var postId = blogClient.NewPost(blogTitle, blogContent, new List<string> { "[Markdown]" }, true, DateTime.Now);
                var blogUrl = _connectionInfo.BlogURL + "/p/" + postId + ".html";
                Console.WriteLine("[INFO]Blog published here: " + blogUrl);


                var newBlogJsonText = "{\"" + fileInfo.Name + "\":{\"title\":\""+blogTitle+"\",\"cnblogs\":\""+blogUrl+"\",\"csdn\":\""+""+"\"}}";
                var newBlog = (JObject)JsonConvert.DeserializeObject(newBlogJsonText);


                blogJson.Add(newBlog);
                File.WriteAllText(jsonFilePath,JsonConvert.SerializeObject(blogJson));
                Console.WriteLine("[INFO]Refreshed json");

                Console.WriteLine("[INFO]END PUBLISH BLOG");

            }
        }

    }
}
