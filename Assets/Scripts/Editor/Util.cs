using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ZhanGuoWuxia.Editor
{
    public static class FileTool
    {
        private static readonly string[] EmptyResult = new string[0];

        public static string GetFolderName(string folderPath)
        {
            var dirInfo = new DirectoryInfo(folderPath);
            return dirInfo.Name;
        }

        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string AsFormatFileSize(this long bytes)
        {
            if (bytes < 0)
            {
                return "-" + AsFormatFileSize(-bytes);
            }

            if (bytes == 0)
                return "0 bytes";

            var unit = 1024;
            if (bytes < unit) { return $"{bytes} B"; }

            var exp = (int)(Math.Log(bytes) / Math.Log(unit));
            var suffixIndex = Math.Clamp(exp, 0, SizeSuffixes.Length - 1);
            return string.Format("{0:F2} {1}", bytes / Math.Pow(unit, exp), SizeSuffixes[suffixIndex]);
        }

        public static string NormalizePath(this string path)
        {
            return path.Replace('\\', '/');
        }

        public static string WithExtension(this string path, string extension)
        {
            var ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext))
            {
                return path + extension;
            }

            if (ext == extension)
                return path;

            return Path.ChangeExtension(path, extension);
        }
    }
}
