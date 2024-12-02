using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public static GameSceneManager Instance;

    public const string RoomName = "TestRoom";

    private Coroutine _spawnUnitroutine;

    [SerializeField] private EGameState _gameState;

    [SerializeField] private int _playerCount;                          // 시작하기 위한 참여 플레이어 수

    [SerializeField] private Vector3[] _spawnerPos;                     // 스포너 위치

    [SerializeField] private int[] _unitCounts;

    private Coroutine _waitRoutine;

    private bool _isLoad;
    public bool IsLoad { get { return _isLoad;} set { _isLoad = value; } }

    public int[] UnitCounts { get { return _unitCounts; } private set { } }

    [SerializeField] private int[] _curUnitCounts;

    public int[] CurUnitCounts { get { return _curUnitCounts; } set { _curUnitCounts = value; } }

    [SerializeField] private int _originPlayerCount;
    public int OriginPlayerCount { get { return _originPlayerCount; }  private set { } }

    [SerializeField] private int _curPlayerCount;
    public int CurPlayerCount { get { return _curPlayerCount; } set { _curPlayerCount = value; } }

    private bool _isSetCam;
    public bool IsSetCam { get { return _isSetCam; } private set { } }

    [SerializeField] private AudioSource _audio;

    // 게임 끝났는지 여부 
    [SerializeField] private bool _isFinish;

    public bool IsFinish { get { return _isFinish; } set { _isFinish = value; } }


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}
    //
    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
    //
    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 1)
    //    {
    //        Debug.Log("씬전환3");
    //        PhotonNetwork.LocalPlayer.SetLoad(true);
    //    }
    //}

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (_isLoad == false)
            {
                //게임 소리 켜주기.
                _audio.Play();
                // Player가 게임이 끝나고나서 따로 로비씬으로 이동하기위해, MastrePC Scene따라가게 하지 않기위해서 게임씬쪽에서 false 처리 해주기.
                PhotonNetwork.AutomaticallySyncScene = false;
                Debug.Log("씬전환3");
                PhotonNetwork.LocalPlayer.SetLoad(true);
                _isLoad = true;
            }
        }
        else
        {
            _audio.Stop();
        }

    }

    private bool CheckAllLoad()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.GetLoad() == false)
            {
                return false;
            }
        }

        return true;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("씬전환4");
        if (changedProps.ContainsKey(CustomProperty.LOAD))
        {
            Debug.Log($"{targetPlayer.NickName}이 로딩이 완료되었다.");
            bool allLoaded = CheckAllLoad();
            Debug.Log($"모든 플레이어가 로딩 완료되었는가: {allLoaded}");
            if (allLoaded)
            {
                GameReady();
            }
        }
    }

    private void GameReady()
    {
        if(_waitRoutine != null)
        {
            StopCoroutine(_waitRoutine);
        }
        StartCoroutine(WaitPlayerEnter());
    }

    IEnumerator WaitPlayerEnter()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        while (PhotonNetwork.PlayerList.Length < _playerCount)
        {
            Debug.Log($"현재 플레이어 수 : {PhotonNetwork.PlayerList.Length}");
            yield return delay;
        }
        Debug.Log("모든 플레이어가 접속했습니다. 게임시작 준비 중");
        StartCoroutine(StartDelayRoutine());
    }


    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        GameStart();
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");

        //테스트용 게임 시작 부분
        SetPlayerController();
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        //방장만 진행할 수 있는 코드
        //PhotonNetwork.Instantiate("Manager/ObjectPool", Vector3.zero, Quaternion.identity);
    }
    /// <summary>
    /// PlayerController를 클라이언트마다 다르게 생성하는 메서드
    /// </summary>
    private void SetPlayerController()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal)
        {
            switch (PhotonNetwork.LocalPlayer.GetPlayerNumber())
            {
                case 0:
                    GameObject playerController_zealot = PhotonNetwork.Instantiate("Prefabs/PlayerController_Zealot", _spawnerPos[0], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[0] + new Vector3(0, 0, -10);
                    _isSetCam = true;
                    break;
                case 1:
                    GameObject playerController_DarkTempler = PhotonNetwork.Instantiate("Prefabs/PlayerController_DarkTempler", _spawnerPos[1], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[1] + new Vector3(0, 0, -10);
                    _isSetCam = true;
                    break;
                case 2:
                    GameObject playerController_Zergling = PhotonNetwork.Instantiate("Prefabs/PlayerController_Zergling", _spawnerPos[2], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[2] + new Vector3(0, 0, -10);
                    _isSetCam = true;
                    break;
                case 3:
                    GameObject playerController_Ultrarisk = PhotonNetwork.Instantiate("Prefabs/PlayerController_Ultrarisk", _spawnerPos[3], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[3] + new Vector3(0, 0, -10);
                    _isSetCam = true;
                    break;
                default:
                    break;
            }
        }

    }
}
