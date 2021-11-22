using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public const string playerMaxHpStr = "playerMaxHp";
    
    private StageUiView stageUiView;

    private bool isInitialized;
    public bool IsInitialized { get { return isInitialized; } }

    public bool IsGameOver { get; set; }
    public bool IsGameClear { get; set; }

    public enum e_StageName
    {
        Stage1,
        Stage2
    }
    private e_StageName currentStage;

    public int PlayerMaxHp { get { return PlayerPrefs.GetInt(playerMaxHpStr, 3); } }
    private int playerCurrentHp;
    public int PlayerCurrentHp {
        get { return playerCurrentHp; }
        set {
            playerCurrentHp = value;
            if (playerCurrentHp < 0) playerCurrentHp = 0;
            if (playerCurrentHp > PlayerMaxHp) playerCurrentHp = PlayerMaxHp;
            if (beforePlayerHp > playerCurrentHp)
            {
                beforePlayerHp = playerCurrentHp;
                stageUiView.HpHitAnim();
            }
        }
    }
    private int beforePlayerHp;

    public Treasure[] treasures;
    public Player player;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start()
    {
        LoadSceneTo(e_StageName.Stage1.ToString());
    }

    private void Update()
    {

    }

    private void OnActiveSceneChanged(Scene scene1, Scene scene2)
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        isInitialized = false;
    }

    private void Initialize()
    {
        isInitialized = true;
        stageUiView = FindObjectOfType<StageUiView>();
        PlayerCurrentHp = PlayerMaxHp;
        beforePlayerHp = PlayerMaxHp;
        IsGameClear = false;
        IsGameOver = false;
        treasures = FindObjectsOfType<Treasure>();
        player = FindObjectOfType<Player>();
    }

    public void LoadSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadToNextStage()
    {
        currentStage++;
        var nextStage = currentStage;
        if (!Enum.IsDefined(typeof(e_StageName), nextStage))
        {
            nextStage = e_StageName.Stage1;
            currentStage = e_StageName.Stage1;
        }
        LoadSceneTo(nextStage.ToString());
    }
}