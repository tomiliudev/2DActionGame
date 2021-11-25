using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public const string playerMaxHpStr = "playerMaxHp";
    
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

    public StageUiView stageUiView;
    public Treasure[] treasures;
    public Player player;
    public PolygonCollider2D cameraCollider;


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
        treasures = FindObjectsOfType<Treasure>();
        player = FindObjectOfType<Player>();
        cameraCollider = GameObject.FindGameObjectWithTag("CameraCollider").GetComponent<PolygonCollider2D>();

        PlayerCurrentHp = PlayerMaxHp;
        beforePlayerHp = PlayerMaxHp;
        IsGameClear = false;
        IsGameOver = false;
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
