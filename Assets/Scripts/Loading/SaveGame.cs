using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour
{
    public GameObject SaveGameUi;
    public GameObject LoadingMessage;
    public GameObject SaveFileList;

    public const string SaveGamePath = "/Saves/";
    private string _fullGameSavePath;

    private bool _menuOpened;
    private bool _loadingSaveFiles;
    private bool _firstFrameSinceLoaded;
    //private Task _loadSaveFilesTask;
    private List<string> _saveFileNames;

    void Start()
    {
        _fullGameSavePath = Application.dataPath + SaveGamePath;
        Directory.CreateDirectory(_fullGameSavePath);
        LoadingMessage.SetActive(false);
    }

    void Update()
    {
        if (!_menuOpened)
            return;
        /*
        if (_loadingSaveFiles && _loadSaveFilesTask.IsCompleted)
        {
            LoadingMessage.SetActive(false);
            _loadingSaveFiles = false;
            _firstFrameSinceLoaded = true;
        }
        */

        //if (!_loadingSaveFiles)
            ProcessFileSelect();
    }

    private void ProcessFileSelect()
    {
        if (_firstFrameSinceLoaded)
        {
            //SaveFileList.gameObject.SetActive(true);
            _firstFrameSinceLoaded = false;
        }
        
        if (Input.GetButtonDown(GameConstants.Controls.Action) || ControllerInputProcessor.PressedAction())
        {
            CreateSaveFile();
        }
        else if (Input.GetButtonDown(GameConstants.Controls.Aim) || ControllerInputProcessor.PressedAim())
        {
            CreateSaveFile();
        }
        // TODO: Process selecting file for overwrite/load/etc.
        // throw new System.NotImplementedException();
    }

    private void CreateSaveFile(string filename = null)
    {
        var playerStatus = FindObjectOfType<PlayerStatus>();
        var playerInventory = FindObjectOfType<PlayerInventory>();
        var sceneInfo = new SceneLoadData
        {
            TargetScene = SceneManager.GetActiveScene().name,
            LoadPosition = playerStatus.gameObject.transform.position,
            LoadRotation = playerStatus.gameObject.transform.eulerAngles,
        };

        var saver = FindObjectOfType<DataSaver>();
        saver.SaveGameStateFromScene(playerStatus, playerInventory, sceneInfo);
        var data = saver.GetGameState();

        var roomStr = SceneManager.GetActiveScene().name;
        var dateStr = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
        var newFilename = roomStr + " - " + dateStr + ".sav";

        var fullFilePath = _fullGameSavePath + newFilename;

        var dataJson = JsonConvert.SerializeObject(data);
        File.WriteAllText(fullFilePath, dataJson);
        if (!string.IsNullOrEmpty(filename))
        {
            var oldFilePath = _fullGameSavePath + filename;
            File.Delete(oldFilePath);
        }

        Close();
    }

    public void Open()
    {
        var playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
            playerStatus.HasSaveUiOpen = true;

        _loadingSaveFiles = true;
        LoadingMessage.SetActive(true);
        //SaveFileList.gameObject.SetActive(false);
        SaveGameUi.SetActive(true);

        //_loadSaveFilesTask = Task.Run(RefreshSaveFileList);

        _menuOpened = true;
    }

    private void Close()
    {
        _menuOpened = false;

        var playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus != null)
            playerStatus.HasSaveUiOpen = false;
        SaveGameUi.SetActive(false);
    }

    private void RefreshSaveFileList()
    {
        _saveFileNames = GetSaveFilesByMostRecentFirst(_fullGameSavePath);
        var saveFileDisplayStr = new StringBuilder("Create New Save File\r\n\r\n");
        foreach (var saveFileName in _saveFileNames)
        {
            var cleanFileName = CleanFileName(saveFileName);
            saveFileDisplayStr.AppendLine(cleanFileName);
        }
        
        var displayStr = saveFileDisplayStr.ToString();
    }

    // HACK: God forgive me this is hacky.
    public static List<string> GetSaveFilesByMostRecentFirst(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var files = Directory.GetFiles(folderPath).Where(f => f.EndsWith(".sav"));

        var filenamesWithLastModified = new List<Tuple<string, DateTime>>();
        foreach (var file in files)
        {
            var lastModified = File.GetLastWriteTimeUtc(file);
            filenamesWithLastModified.Add(new Tuple<string, DateTime>(file, lastModified));
        }

        var saveFileNames = filenamesWithLastModified.OrderByDescending(f => f.Item2).Select(f => f.Item1).ToList();
        return saveFileNames;
    }

    public static string CleanFileName(string saveFileName)
    {
        var saveFileLastSlashIndex = saveFileName.LastIndexOfAny(new[] { '\\', '/' });
        var fileExtensionStartIndex = saveFileName.LastIndexOf('.');
        var cleanFileName = saveFileName.Substring(saveFileLastSlashIndex + 1, fileExtensionStartIndex - saveFileLastSlashIndex - 1);
        return cleanFileName;
    }
}
