using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class GameSceneManager : MonoBehaviourPunCallbacks
{
    public static GameSceneManager Instance;

    public const string RoomName = "TestRoom";

    private Coroutine _spawnUnitroutine;

    [SerializeField] private EGameState _gameState;

    [SerializeField] private int _playerCount;                          // 시작하기 위한 참여 플레이어 수

    [SerializeField] private Vector3[] _spawnerPos;                     // 스포너 위치

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
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

    public override void OnJoinedRoom()
    {
        StartCoroutine(WaitPlayerEnter());
    }


    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("게임 시작");

        //테스트용 게임 시작 부분
        SetPlayerController();
        if (PhotonNetwork.IsMasterClient == false)
        {
            return;
        }
        //방장만 진행할 수 있는 코드
        PhotonNetwork.Instantiate("Manager/ObjectPool", Vector3.zero, Quaternion.identity);
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
                    Camera.main.transform.position = _spawnerPos[0];
                    break;
                case 1:
                    GameObject playerController_DarkTempler = PhotonNetwork.Instantiate("Prefabs/PlayerController_DarkTempler", _spawnerPos[1], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[1];
                    break;
                case 2:
                    GameObject playerController_Zergling = PhotonNetwork.Instantiate("Prefabs/PlayerController_Zergling", _spawnerPos[2], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[2];
                    break;
                case 3:
                    GameObject playerController_Ultrarisk = PhotonNetwork.Instantiate("Prefabs/PlayerController_Ultrarisk", _spawnerPos[3], Quaternion.identity);
                    Camera.main.transform.position = _spawnerPos[3];
                    break;
                default:
                    break;
            }
        }

    }
}
