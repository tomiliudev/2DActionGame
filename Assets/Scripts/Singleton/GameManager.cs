using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameManager : SingletonMonoBehaviour<GameManager>
{
    private bool isInitialized;
    public bool IsInitialized { get { return isInitialized; } }

    public bool IsGameOver { get; set; }
    public bool IsGameClear { get; set; }

    public e_GameMode CurrentGameMode = e_GameMode.StageSelection;
    private e_StageName currentStage;
    public int CurrentStageLevel
    {
        get
        {
            int stageLevel = 0;
            var clearStageDic = PlayerPrefsUtility.LoadDict<string, int>(GameConfig.ClearStageDic);
            if (clearStageDic.ContainsKey(currentStage.ToString()))
            {
                stageLevel = clearStageDic[currentStage.ToString()];
                Debug.Log(string.Format("Stage{0}のレベルは{1}", currentStage, stageLevel));
            }
            if (stageLevel > 5) stageLevel = 5;
            return stageLevel;
        }
    }

    public int PlayerMaxHp { get { return PlayerPrefsUtility.Load(GameConfig.PlayerMaxHp, 1); } }
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

    public BaseController baseController;
    public SceneController sceneController;
    public StageUiView stageUiView;
    public PopupView popupView;
    public Treasure[] treasures;
    public Player player;
    public PolygonCollider2D cameraCollider;
    public CinemachineVirtualCamera cinemachineCamera;
    public GameObject standOnObj;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        InitializeCommon();
    }

    private void Start()
    {
    }

    private void OnActiveSceneChanged(Scene scene1, Scene scene2)
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeCommon();

        e_StageName stageName;
        if (Enum.TryParse(scene.name, out stageName) && Enum.IsDefined(typeof(e_StageName), stageName))
        {
            InitializeStage();
        }
    }

    private void OnSceneUnloaded(Scene scene)
    {
        isInitialized = false;
    }

    private void InitializeCommon()
    {
        player = FindObjectOfType<Player>();
        popupView = FindObjectOfType<PopupView>();
        baseController = FindObjectOfType<BaseController>();

        // ハートは最初からショップで購入できるよう追加しておく
        ShopItemListUtility.SaveShopItemList(e_ItemType.heart);
        ShopItemListUtility.SaveShopWeaponList(e_WeaponType.bow);
    }

    private void InitializeStage()
    {
        isInitialized = true;

        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        
        sceneController = FindObjectOfType<SceneController>();
        stageUiView = FindObjectOfType<StageUiView>();
        treasures = FindObjectsOfType<Treasure>();
        
        cameraCollider = GameObject.FindGameObjectWithTag("CameraCollider").GetComponent<PolygonCollider2D>();

        PlayerCurrentHp = PlayerMaxHp;
        beforePlayerHp = PlayerMaxHp;
        IsGameClear = false;
        IsGameOver = false;
    }


    public void LoadSceneWithData(string sceneName, BaseController.BaseInitData initData)
    {
        StartCoroutine(LoadSceneWithDataCo(sceneName, initData));
    }

    IEnumerator LoadSceneWithDataCo(string sceneName, BaseController.BaseInitData initData)
    {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        //asyncOperation.allowSceneActivation = false;
        //yield return new WaitForSeconds(0.3f);
        //asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;

        var sence = SceneManager.GetSceneByName(sceneName);
        foreach (var root in sence.GetRootGameObjects())
        {
            BaseController con = root.GetComponent<BaseController>();
            if (con != null)
            {
                con.Initialize(initData);
            }
        }
    }

    public void LoadSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadToTargetStage(e_StageName stage)
    {
        CurrentGameMode = e_GameMode.Normal;
        currentStage = stage;
        LoadSceneTo(stage.ToString());
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
        LoadToTargetStage(nextStage);
    }

    public e_StageName GetNextStage()
    {
        var nextStage = currentStage + 1;
        if (!Enum.IsDefined(typeof(e_StageName), nextStage))
        {
            nextStage = e_StageName.Stage1;
        }
        return nextStage;
    }

    public string GetPlayerHpText()
    {
        string maxHpStr = PlayerMaxHp < GameConfig.MaxHp ? PlayerMaxHp.ToString() : "MAX";
        return string.Format("{0}/{1}", PlayerCurrentHp, maxHpStr);
    }
}
