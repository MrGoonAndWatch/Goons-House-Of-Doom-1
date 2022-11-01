using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroMenu : MonoBehaviour
{
    private double _remainingIntroVideoInSec;
    public GameObject TitleUi;
    public GameObject MenuUi;
    public VideoPlayer VideoPlayer;
    public TextMeshProUGUI[] LoadSaveFileSlots;
    private string[] _saveFileSlots;

    private bool _cutsceneFinished;

    private float _timeTilGameStartSeconds;
    private bool _startingGame;
    private bool _startingHordeMode;

    // Start is called before the first frame update
    void Start()
    {
        if (VideoPlayer == null)
        {
            _cutsceneFinished = true;
            if(MenuUi != null)
                MenuUi.SetActive(true);
            if(TitleUi != null)
                TitleUi.SetActive(true);
        }
        else
        {
            _remainingIntroVideoInSec = VideoPlayer.length;
            MenuUi.SetActive(false);
            TitleUi.SetActive(false);
        }

        LoadSaveFileList();
    }

    private void LoadSaveFileList()
    {
        if (LoadSaveFileSlots == null || LoadSaveFileSlots.Length == 0)
            return;

        _saveFileSlots = new string[LoadSaveFileSlots.Length];
        var folderPath = Application.dataPath + SaveGame.SaveGamePath;
        var saveFiles = SaveGame.GetSaveFilesByMostRecentFirst(folderPath).Take(8).ToArray();

        for (var i = 0; i < saveFiles.Length; i++)
        {
            var fullFilePath = saveFiles[i];
            LoadSaveFileSlots[i].text = SaveGame.CleanFileName(fullFilePath);
            _saveFileSlots[i] = fullFilePath;
        }
    }


    public void PlayGame(int firstScene)
    {
        var clipLength = SoundManager.PlayGameStartSfx();
        _timeTilGameStartSeconds = clipLength;
        _startingGame = true;
    }

    public void PlayHordeMode()
    {
        var clipLength = SoundManager.PlayGameStartSfx();
        _timeTilGameStartSeconds = clipLength;
        _startingHordeMode = true;
    }

    public void QuitGame()
    {
        // TODO: Going back to title screen breaks horribly!
        //var hordeModeObj = FindObjectOfType<HordeModeManager>();
        //if (hordeModeObj == null)
            Application.Quit();
        //else
        //    SceneManager.LoadScene(SceneNames.TitleScreen);
    }

//Everything below this point is a public void that the UI can call on

    //Change graphics and resolution

    public void ChangeQuality(TMP_Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }

    //Bools and ints to track next screen resolution
    private int screenwidth,screenheight;
    private bool isFullscreen;

    private void RefreshResolution()
    {
      if(isFullscreen)
      {
        Screen.SetResolution(screenwidth,screenheight,FullScreenMode.FullScreenWindow);
      }
      else
      {
        Screen.SetResolution(screenwidth,screenheight,FullScreenMode.Windowed);
      }
    }

    public void ChangeResolution(TMP_Dropdown dropdown)
    {
        switch(dropdown.value)
        {
          case 0:
          screenwidth = 1280;
          screenheight = 720;
          RefreshResolution();
          break;
          case 1:
          screenwidth = 1920;
          screenheight = 1080;
          RefreshResolution();
          break;
          case 2:
          screenwidth = 2560;
          screenheight = 1440;
          RefreshResolution();
          break;
          case 3:
          screenwidth = 3840;
          screenheight = 2160;
          RefreshResolution();
          break;

        }
    }

    //Set Windowed mode

    public void ToggleFullscreen(Toggle toggle)
    {
      if(toggle.isOn)
      {
        isFullscreen = true;
      }
      else isFullscreen = false;
      RefreshResolution();
    }



    //Audio Sliders

    public void ChangeMasterAudio(Slider audioSlider)
    {
        SoundManager.SetMainVolume(audioSlider.value);
    }

    public void ChangeStingAudio(Slider audioSlider)
    {
        SoundManager.SetSfxVolume(audioSlider.value);
    }

    public void ChangeBGMAudio(Slider audioSlider)
    {
        SoundManager.SetBgmVolume(audioSlider.value);
    }

    public void ChangeVoiceAudio(Slider audioSlider)
    {
        SoundManager.SetVoiceVolume(audioSlider.value);
    }
    
    private void Update()
    {
        ProcessVideoLogic();

        WaitForGameToStart();
        WaitForHordeModeToStart();
    }

    private void ProcessVideoLogic()
    {
        if (_cutsceneFinished)
            return;

        _remainingIntroVideoInSec -= Time.deltaTime;

        if (Input.GetButtonDown(GameConstants.Controls.Action))
        {
            _remainingIntroVideoInSec = 0;
        }

        if (_remainingIntroVideoInSec <= 0)
        {
            VideoPlayer.frame = (long)VideoPlayer.frameCount - 1;
            MenuUi.SetActive(true);
            TitleUi.SetActive(true);
            _cutsceneFinished = true;
        }
    }

    private void WaitForGameToStart()
    {
        if (!_startingGame)
            return;

        _timeTilGameStartSeconds -= Time.deltaTime;

        if (_timeTilGameStartSeconds <= 0)
        {
            SceneManager.LoadScene(SceneNames.GameStartup);
        }
    }

    private void WaitForHordeModeToStart()
    {
        if (!_startingHordeMode)
            return;

        _timeTilGameStartSeconds -= Time.deltaTime;

        if (_timeTilGameStartSeconds <= 0)
        {
            SceneManager.LoadScene(SceneNames.HordeMode);
        }
    }

    public void LoadSaveFile(int fileSlot)
    {
        if (fileSlot >= _saveFileSlots.Length)
            return;

        var targetFilePath = _saveFileSlots[fileSlot];
        if (string.IsNullOrEmpty(targetFilePath) || !File.Exists(targetFilePath))
            return;

        var saveData = File.ReadAllText(targetFilePath);
        var gameState = JsonConvert.DeserializeObject<DataSaver.GameState>(saveData);

        var loadGameData = FindObjectOfType<LoadGameData>();
        loadGameData.SetGameState(gameState);

        SceneManager.LoadScene(SceneNames.GameStartup);
    }
}
