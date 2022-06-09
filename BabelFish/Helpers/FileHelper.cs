using System.Reflection;
using System.Configuration;
using BabelFish.DataModel;
using BabelFish.DataModel.Definitions;
using System.Collections.Specialized;

namespace BabelFish.Helpers
{
    static class FileHelper
    {
        private static string BaseFilePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string LastException { get; set; } = string.Empty;

        static FileHelper() { }

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="altPath"></param>
        /// <returns></returns>
        public static bool FileExists(string fileName, string altPath = "")
        {
            LastException = string.Empty;
            try
            {
                return File.Exists($"{(String.IsNullOrEmpty(altPath) ? BaseFilePath : altPath)}\\{fileName}");
            }
            catch(Exception ex)
            {
                LastException = $"File Exists Error: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Check if Directory exists
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string directoryPath)
        {
            LastException = string.Empty;
            try
            {
                return Directory.Exists(directoryPath);
            }
            catch (Exception ex)
            {
                LastException = $"Directory Exists Error: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool DirectoryCreate(string directoryPath)
        {
            LastException = string.Empty;
            try
            {
                if ( !DirectoryExists(directoryPath) )
                    Directory.CreateDirectory(directoryPath);
                return true;
            }
            catch (Exception ex)
            {
                LastException = $"Directory Create Error: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// List of files in a directory
        ///     If no directory, uses BaseFilePath
        /// </summary>
        /// <param name="directoryToList"></param>
        /// <returns></returns>
        public static List<string> ListFilesInDirectory(string directoryToList, bool returnFullPath = false)
        {
            LastException = string.Empty;
            List<string> returnList = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(directoryToList))
                    directoryToList = $"{BaseFilePath}";
                if (Directory.Exists(directoryToList))
                {
                    string[] fileList = Directory.GetFiles(directoryToList);
                    if (fileList.Length > 0)
                    {
                        foreach (string fileName in fileList)
                        {
                            if (returnFullPath)
                                returnList.Add(fileName);
                            else
                                returnList.Add(Path.GetFileName(fileName));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LastException = $"Directory File List Error: {ex.Message}";
            }

            return returnList;
        }

        /// <summary>
        /// Retrieve contents of file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="altPath"></param>
        /// <returns></returns>
        public static string FileContentsGet(string fileName, string altPath = "")
        {
            string returnFileContents = string.Empty;
            LastException = string.Empty;
            try
            {
                if ( FileExists(fileName, altPath))
                {
                   returnFileContents = System.IO.File.ReadAllText(BuildFileNamePath(fileName, (String.IsNullOrEmpty(altPath) ? BaseFilePath : altPath)));
                }
            }
            catch (Exception ex)
            {
                LastException = $"File Contents Error: {ex.Message}";
            }

            return returnFileContents;
        }

        /// <summary>
        /// Write contents to file
        /// </summary>
        /// <param name="fileContent"></param>
        /// <param name="fileName"></param>
        /// <param name="altPath"></param>
        /// <returns></returns>
        public static bool FileContentsWrite(string fileContent, string fileName, string altPath = "")
        {
            bool returnFileContentsWrite = false;
            LastException = string.Empty;
            try
            {
                System.IO.File.WriteAllText(BuildFileNamePath(fileName, (String.IsNullOrEmpty(altPath) ? BaseFilePath : altPath)), fileContent);
                returnFileContentsWrite = true;
            }
            catch (Exception ex)
            {
                LastException = $"File Contents Error: {ex.Message}";
            }

            return returnFileContentsWrite;
        }

        /// <summary>
        /// Get File Information
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="altPath"></param>
        /// <returns></returns>
        public static System.IO.FileInfo FileInfo(string fileName, string altPath = "")
        {
            return new System.IO.FileInfo(BuildFileNamePath(fileName, (String.IsNullOrEmpty(altPath) ? BaseFilePath : altPath)));
        }

        /// <summary>
        /// Utility function to craft full file path with filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="altPath"></param>
        /// <returns>string Path/Filename</returns>
        private static string BuildFileNamePath(string fileName, string altPath = "")
        {
            return $"{(String.IsNullOrEmpty(altPath) ? BaseFilePath : altPath)}\\{fileName}";
        }
    }
}
