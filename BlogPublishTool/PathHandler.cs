using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlogPublishTool
{
    public class PathHandler
    {
        public static string GetAbsPath(string path)
        {
            if (path == null) return null;
            try
            {
                var absPath = !Path.IsPathRooted(path) ? Path.Combine(Directory.GetCurrentDirectory(), path) : path;

                var fileInfo = new FileInfo(absPath);

                if (fileInfo.Attributes == FileAttributes.Directory)
                {
                    var directoryInfo = new DirectoryInfo(absPath);
                    if (directoryInfo.Exists)
                    {
                        return absPath;
                    }

                    Console.WriteLine("Not exists! Please check the directory path: {0}!", path);
                }
                else
                {
                    if (fileInfo.Exists)
                    {
                        return absPath;
                    }

                    Console.WriteLine("Not exists! Please check the file path: {0}!", path);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Please check the path: {1}!", ex.Message, path);
            }
            return null;
        }

        public static List<string> GetAllMarkDown(string path)
        {
            var markDownList = new List<string>();

            if (new FileInfo(path).Attributes == FileAttributes.Directory)
            {
                markDownList = GetAllMarkDown(path, markDownList);
            }
            else
            {
                markDownList.Add(path);
            }
            return markDownList;
        }

        public static List<string> GetAllMarkDown(string path, List<string> markDownList)
        {
            var directoryInfo = new DirectoryInfo(path);

            var fileInfos = directoryInfo.GetFiles();
            var directoryInfos = directoryInfo.GetDirectories();

            markDownList.AddRange(from file in fileInfos where file.Extension == ".md" select file.FullName);

            foreach(var dir in directoryInfos)
            {
                GetAllMarkDown(dir.FullName, markDownList);
            }
            
            return markDownList;
        }
    }
}
