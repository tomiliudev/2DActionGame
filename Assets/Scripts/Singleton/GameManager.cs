using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum e_GameMode
{
    None,
    Normal,
    MiniGame,
}

public enum e_StageName
{
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5
}

public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{   
    private bool isInitialized;
    public bool IsInitialized { get { return isInitialized; } }

    public bool IsGameOver { get; set; }
    public bool IsGameClear { get; set; }

    public e_GameMode CurrentGameMode = e_GameMode.Normal;
    private e_StageName currentStage;

    public int PlayerMaxHp { get { return PlayerPrefs.GetInt(GameConfig.PlayerMaxHp, 1); } }
    private int playerCurrentHp;
    public int PlayerCurrentHp {
        get { return playerCurrentHp; }
        set {
            playerCurrentHp = value;
            if (playerCurrentHp < 0) playerCurrentHp = 0;
            if (playerCurrentHp > PlayerMaxHp) playerCurrentHp = PlayerMaxHp;
            if (beforePlayerHp > playerCurrentHp) stageUiView.HpHitAnim();
            if (beforePlayerHp < playerCurrentHp) stageUiView.HpPickAnim();
            beforePlayerHp = playerCurrentHp;
        }
    }
    private int beforePlayerHp;

    public StageUiView stageUiView;
    public PopupView popupView;
    public Treasure[] treasures;
    public Player player;
    public PolygonCollider2D cameraCollider;
    public CinemachineVirtualCamera cinemachineCamera;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start()
    {
        currentStage = e_StageName.Stage1;
        LoadSceneTo(currentStage.ToString());
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

        // ハートは最初からショップで購入できるよう追加しておく
        ShopItemListUtility.SaveShopItemList(e_ItemType.heart);
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        stageUiView = FindObjectOfType<StageUiView>();
        popupView = FindObjectOfType<PopupView>();
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
