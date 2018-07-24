using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogPublishTool
{
    class PathHandler
    {
        public static string GetAbsPath(string path)
        {
            if (path != null)
            {
                try
                {
                    string absPath = string.Empty;
                    //判断是否是根目录
                    if (!Path.IsPathRooted(path))
                    {
                        absPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                    }
                    else
                    {
                        absPath = path;
                    }

                    FileInfo fileInfo = new FileInfo(absPath);

                    //是个文件夹
                    if (fileInfo.Attributes == FileAttributes.Directory)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(absPath);
                        if (directoryInfo.Exists)
                        {
                            return absPath;
                        }
                        else
                        {
                            Console.WriteLine("Not exists! Please check the directory path: {0}!", path);
                        }
                    }
                    else
                    {
                        if (fileInfo.Exists)
                        {
                            return absPath;
                        }
                        else
                        {
                            Console.WriteLine("Not exists! Please check the file path: {0}!", path);
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} Please check the path: {1}!", ex.Message, path);
                }
            }
            return null;
        }
        public static List<string> GetAllMarkDown(string path, List<string> markDownList)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            
            foreach(FileInfo file in fileInfos)
            {
                if(file.Extension == ".md")
                {
                    markDownList.Add(file.FullName);
                }
            }

            foreach(DirectoryInfo dir in directoryInfos)
            {
                GetAllMarkDown(dir.FullName, markDownList);
            }


            //能到这一步的一定是真实存在的路径
            //先判断是文件还是路径
            //如果是文件，那就很方便，如果是路径，就GG
            return markDownList;
        }
    }
}
