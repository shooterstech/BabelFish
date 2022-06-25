using System.Reflection;
using BabelFish.DataModel.Definitions;

namespace BabelFish.Helpers
{
    public class DefinitionCacheHelper
    {
        private const string DEFINITION_FILE_EXT = ".json";
        private const string DEFINITION_DIR = "DEFINITIONS";
        private const int DEFINITION_EXPIRE = 14;

        private List<DefinitionCache> definitionsCached = new List<DefinitionCache>();

        private bool prefDefintionsCacheIgnore = false;

        private TimeSpan prefCacheExpireTime = TimeSpan.FromDays(DEFINITION_EXPIRE);

        private string currentFilePath = string.Empty;

        private SetName? currentSetName = null;

        private Definition.DefinitionType? currentType { get; set; } = null;

        public DefinitionCacheHelper()
        {
            // Populate user defaults
            if (!SettingsHelper.SettingIsNullOrEmpty("Definitions_CacheIgnore"))
                prefDefintionsCacheIgnore = SettingsHelper.UserSettings["Definitions_CacheIgnore"];

            if (!SettingsHelper.SettingIsNullOrEmpty("Definitions_CacheExpireTime"))
                prefCacheExpireTime = TimeSpan.FromDays(SettingsHelper.UserSettings["Definitions_CacheExpireTime"]);

            //Default directory to UserSettings
            if (!SettingsHelper.SettingIsNullOrEmpty("Definitions_CacheStorageDirctory"))
                currentFilePath = $"{SettingsHelper.UserSettings["Definitions_CacheStorageDirctory"].TrimEnd('\\')}\\{DEFINITION_DIR}\\";
            else
            {
                string userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string subDir = "\\Documents\\My Matches";
                // 1st default to Orion user directory, 2nd iuser temp directory, fail over to installation directory
                if (FileHelper.DirectoryExists($"{userDir}{subDir}"))
                    currentFilePath = $"{userDir}{subDir}\\{DEFINITION_DIR}";
                else if (FileHelper.DirectoryExists($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\appdata\\local\\temp") )
                    currentFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\appdata\\local\\temp\\{DEFINITION_DIR}";
                else
                    currentFilePath = $"{System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\{DEFINITION_DIR}";
            }

            // Load exisitng file system Definitions into memory
            if ( !prefDefintionsCacheIgnore)
                LoadDefinitionsInMemory();
        }

        #region Properties
        public string LastException { get; private set; } = string.Empty;

        public string DefaultDirectory { get { return currentFilePath; } }

        public SetName? CurrentSetName { get { return currentSetName; } }

        public Definition.DefinitionType? CurrentType { get { return currentType; } }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Retrieve Defintion from Cache
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <returns>DefinitionCache object or null if not available</returns>
        public DefinitionCache? GetCachedDefinition(Definition.DefinitionType definitionType, SetName setName)
        {
            LastException = string.Empty;
            SetCurrent(definitionType, setName);

            DefinitionCache returnDefinitionCache = null;
            try
            {
                if (!prefDefintionsCacheIgnore)
                    returnDefinitionCache = GetDefinitionFromCache();
            }
            catch (Exception ex)
            {
                LastException = $"Error getting cached definition: {ex.Message}";
            }

            return returnDefinitionCache;
        }

        /// <summary>
        /// Age of Definition Cache in Days
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <returns>INTEGER (0+ days); -1 denotes error in LastException</returns>
        public int DefinitionCacheAgeInDays(Definition.DefinitionType definitionType, SetName setName)
        {
            LastException = string.Empty;
            SetCurrent(definitionType, setName);
            try
            {
                return (int)DefinitionCacheAge().TotalDays;
            }
            catch (Exception ex)
            {
                LastException = $"Error getting Definition Cache Age: {ex.Message}";
                return -1;
            }
        }

        /// <summary>
        /// Save Definition to all cache sources
        /// </summary>
        /// <param name="setDefinition"></param>
        /// <returns>true or false with exception</returns>
        public bool SaveDefinitionToCache(string saveDefinition, Definition.DefinitionType definitionType, SetName setName)
        {
            LastException = string.Empty;
            SetCurrent(definitionType, setName);
            bool returnSave = false;
            try
            {
                DefinitionCache convertDefinition = new DefinitionCache();
                convertDefinition.SetName = this.currentSetName;
                convertDefinition.DefinitionType = this.currentType;
                convertDefinition.DefinitionJSON = saveDefinition;
                convertDefinition.LastUpdated = DateTime.Now;
                if (WriteDefinitionToMemory(convertDefinition) &&
                        WriteDefinitionToFileSystem(convertDefinition))
                    return true;
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }
            return returnSave;
        }

        /// <summary>
        /// Retrieve non-expired Definition from all sources (memory/file system)
        /// </summary>
        /// <returns>DefinitionCache object or NULL if not found/expired</returns>
        private DefinitionCache? GetDefinitionFromCache()
        {
            DefinitionCache returnDefinitionCache = null;
            try
            {
                // Check Memory
                returnDefinitionCache = GetDefinitionFromMemory();
                if (returnDefinitionCache == null && LastException == String.Empty)
                {
                    // Check File System
                    returnDefinitionCache = GetDefinitionCacheFile(currentSetName.FileName);
                    if (returnDefinitionCache != null)
                        WriteDefinitionToMemory(returnDefinitionCache);
                }

                // Check Version on expired Definition Cache
                if (returnDefinitionCache != null && DefinitionCacheExpired(returnDefinitionCache))
                {
                    string stringVersion = JsonHelper.FieldValueFromJson(returnDefinitionCache.DefinitionJSON, "Version");
                    //When .M is 0, it means to pull the latest version.
                    int cacheVersionM = int.Parse(stringVersion.Split('.')[1]);
                    float cacheVersion = float.Parse(stringVersion);
                    float serverVersion = GetDefinitionVersionFromServer(currentType, currentSetName);
                    
                    if (cacheVersionM == 0 ||
                            serverVersion > cacheVersion)
                        returnDefinitionCache = null; //force fresh download
                }
            }
            catch (Exception ex)
            {
                LastException = $"Error retrieving Defintion Cache: {ex.Message}";
            }

            return returnDefinitionCache;
        }

        /// <summary>
        /// Retrieve Definition from memory
        /// </summary>
        /// <param name="getType"></param>
        /// <param name="getSetName"></param>
        /// <returns>DefinitionCache object or null if not found</returns>
        private DefinitionCache? GetDefinitionFromMemory()
        {
            LastException = string.Empty;
            DefinitionCache returnDefinition = null;
            try
            {
                returnDefinition = definitionsCached.Where(x => x.DefinitionType.ToString() == this.currentType.ToString() && 
                                                            x.SetName.ToString() == this.currentSetName.ToString()).FirstOrDefault();
            }
            catch (Exception ex)
            {
                LastException = ex.Message;
            }

            return returnDefinition;
        }

        /// <summary>
        /// Save Definiton Cache in Memory
        /// </summary>
        /// <param name="writeDefinition"></param>
        /// <returns>true or false</returns>
        private bool WriteDefinitionToMemory(DefinitionCache writeDefinition)
        {
            List<DefinitionCache> updateCache = new List<DefinitionCache>();
            try
            {
                foreach (DefinitionCache copyDefinition in definitionsCached)
                    updateCache.Add(copyDefinition.DeepCopy(copyDefinition));

                definitionsCached.Clear();
                foreach (DefinitionCache item in updateCache)
                {
                    if (item.SetName.ToString() == writeDefinition.SetName.ToString())
                        definitionsCached.Add(item.DeepCopy(writeDefinition));
                    else
                        definitionsCached.Add(item.DeepCopy(item));
                }
                return true;
            }
            catch (Exception ex)
            {
                LastException = $"Error updating Definition Cache Memoery: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Save Definiton Cache in File System
        /// </summary>
        /// <param name="writeDefinition"></param>
        /// <returns>true or false</returns>
        private bool WriteDefinitionToFileSystem(DefinitionCache writeDefinition)
        {
            bool returnWriteSuccess = false;
            try
            {
                if ( EnsureWriteDirectory() )
                    returnWriteSuccess = FileHelper.FileContentsWrite(writeDefinition.DefinitionJSON, writeDefinition.SetName.FileName, currentFilePath);
            }
            catch (Exception ex)
            {
                LastException = $"Error writing Definition to file system: {ex.Message}";
            }

            return returnWriteSuccess;
        }

        /// <summary>
        /// Creates directory to write if !exists
        /// </summary>
        /// <param name="DirectoryPath"></param>
        private bool EnsureWriteDirectory()
        {
            try
            {
                if (!FileHelper.DirectoryExists($"{currentFilePath}"))
                    FileHelper.DirectoryCreate($"{currentFilePath}");
                return true;
            }
            catch (Exception ex)
            {
                LastException = $"Error creating Definition Cache directory: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// Age of given Definition Cache
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <returns>Local Definition Age of type Timespan. Convert with .TotalDays</returns>
        private TimeSpan DefinitionCacheAge(Definition.DefinitionType? definitionType = null, SetName? setName = null)
        {
            SetCurrent(definitionType, setName);
            LastException = string.Empty;
            TimeSpan returnDefinitionCacheAge = TimeSpan.Zero;
            try
            {
                var checkLastUpdated = DefinitionCacheLastUpdated();
                if ( checkLastUpdated != null )
                    returnDefinitionCacheAge = DateTime.Now.Subtract(checkLastUpdated);
            }
            catch (Exception ex)
            {
                LastException = $"Error: {ex.Message}";
            }

            return returnDefinitionCacheAge;
        }

        /// <summary>
        /// LastUpdated date for Current Type/SetName
        /// </summary>
        /// <returns>DateTime</returns>
        private DateTime DefinitionCacheLastUpdated(Definition.DefinitionType? definitionType = null, SetName? setName = null)
        {
            SetCurrent(definitionType, setName);
            LastException = string.Empty;
            DateTime returnDateTime = new DateTime();
            DefinitionCache? checkDefinition = null;
            try
            {
                checkDefinition = definitionsCached.Where(x => x.DefinitionType.ToString() == this.currentType.ToString() && x.SetName.ToString() == this.currentSetName.ToString()).FirstOrDefault();
                if (checkDefinition != null)
                    returnDateTime = checkDefinition.LastUpdated;
            }
            catch (Exception ex)
            {
                LastException = $"Error: {ex.Message}";
            }
            return returnDateTime;
        }

        /// <summary>
        /// Check if Definition has expired based on preference/default setting
        /// </summary>
        /// <param name="definitionCache"></param>
        /// <returns>true or false</returns>
        private bool DefinitionCacheExpired(DefinitionCache definitionCache)
        {
            if (definitionCache.LastUpdated.Subtract(DateTime.Now) > prefCacheExpireTime)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Load Definitions from file system into memory
        /// </summary>
        private void LoadDefinitionsInMemory()
        {
            foreach (string definitionFile in ListDefinitionFileNames())
                definitionsCached.Add(GetDefinitionCacheFile(definitionFile));
        }

        /// <summary>
        /// List of filenames in the definition cache filesystem folder
        /// </summary>
        /// <returns>String list of file names</returns>
        private List<string> ListDefinitionFileNames()
        {
            List<string> returnDefinitionFileNames = new List<string>();
            try
            {
                returnDefinitionFileNames = FileHelper.ListFilesInDirectory(currentFilePath);
            }
            catch (Exception ex)
            {
                LastException = $"Error fetching definition file list: {ex.Message}";
            }

            return returnDefinitionFileNames;
        }

        /// <summary>
        /// Fetch and parse definition file from file system
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>DefinitionCache object generated from json</returns>
        private DefinitionCache GetDefinitionCacheFile(string fileName)
        {
            DefinitionCache returnDefinitionCache = null;
            try
            {
                if (File.Exists($"{currentFilePath}\\{fileName}"))
                {
                    returnDefinitionCache = new DefinitionCache();
                    returnDefinitionCache.DefinitionJSON = FileHelper.FileContentsGet(fileName, currentFilePath);
                    returnDefinitionCache.LastUpdated = FileHelper.FileInfo(fileName, currentFilePath).LastWriteTime;

                    returnDefinitionCache.SetName = SetName.Parse(JsonHelper.FieldValueFromJson(returnDefinitionCache.DefinitionJSON, "SetName"));

                    // Need to match Enum.Description returned in json
                    returnDefinitionCache.DefinitionType = EnumHelper.ParseEnumByDescription<Definition.DefinitionType>(JsonHelper.FieldValueFromJson(returnDefinitionCache.DefinitionJSON, "Type"));
                }
            }
            catch (Exception ex)
            {
                LastException = $"Error loading definition file: {ex.Message}";
            }
            return returnDefinitionCache;
        }

        private float GetDefinitionVersionFromServer(Definition.DefinitionType? definitionType, SetName? setName)
        {
            float returnVersion = -1;

            if ( definitionType!=null && setName != null)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                string url = $"https://api-stage.orionscoringsystem.com/production/definition/{definitionType}/{setName}?version=true";
                try
                {
                    HttpRequestMessage msg;
                    msg = new HttpRequestMessage(HttpMethod.Get, url);
                    msg.Headers.Host = msg.RequestUri.Host;
                    msg.Headers.Add("x-api-key", SettingsHelper.UserSettings[AuthEnums.XApiKey.ToString()]);
                    response = httpClient.SendAsync(msg).Result;
                    if (float.TryParse(JsonHelper.FieldValueFromJson(JsonHelper.ToJsonString(response), "Version"), out float tryFloat))
                        returnVersion = tryFloat;
                }
                catch (Exception ex)
                {
                    LastException = $"Error retrieving Definition Version from server: {ex.Message}";
                }

            }
            else
            {
                LastException = $"Error getting Definition Version. DefinitionType and SetName cannot be null.";
            }

            return returnVersion;
        }

        /// <summary>
        /// Set local Type and SetName so we don't have to pass them around everywhere
        /// </summary>
        /// <param name="setType"></param>
        /// <param name="setSetName"></param>
        private void SetCurrent(Definition.DefinitionType? setType, SetName? setSetName)
        {
            if ( setType != null )
                currentType = setType;
            if ( setSetName != null )
                currentSetName = setSetName;
        }

        #endregion Methods
    }
}