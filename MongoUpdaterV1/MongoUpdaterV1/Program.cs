using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MongoUpdaterV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(" Welcome to MongoUpdater V1");
            var directory = "./testeClone3/";
            var githubLink = "https://github.com/viavarejo-internal/comunicacao-scripts.git";

            if (Directory.Exists(directory))
            {
                Console.WriteLine("\n Deleting old files ");
                DeleteFiles(directory);
            }

            Console.WriteLine("\n Cloning repository from " + githubLink);
            CloneRepository(directory, githubLink);

            Console.WriteLine("\n Loading files");
            var filesPath = GetAllFiles(directory);

            Console.WriteLine("\n Deleting files ");
            DeleteFiles(directory);

            Console.WriteLine("\n MongoUpdater now finished, thanks for using");
        }

        private static List<string> GetAllFiles(string directory)
        {
            var filesPath = Directory.GetDirectories(directory,"atlas*");

            var foldersRelease = Directory.GetDirectories(filesPath.First(), "release*");

            return GetFilesFullPath(foldersRelease);            
        }

        private static List<string> GetFilesFullPath(string[] foldersRelease)
        {
            var result = new List<string>();
            foreach (var folder in foldersRelease)
            {
                var files = Directory.GetFiles(folder);
                result.AddRange(files);
            }
            return result;
        }

        private static void CloneRepository(string directory, string githubLink)
        {
            var cloneOptions = new CloneOptions();
            cloneOptions.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = "0b60d95ece00ca56a5edd67497d5c02ce220828e", Password = string.Empty };
            Repository.Clone(githubLink, directory, cloneOptions);
        }

        private static void DeleteFiles(string path)
        {
            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };

            foreach (var info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                info.Attributes = FileAttributes.Normal;
            }

            directory.Delete(true);
        }
    }
}
