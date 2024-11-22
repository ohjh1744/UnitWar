using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainPanel : UIBInder
{
    //[SerializeField] GameObject menuPanel;
    //[SerializeField] GameObject createRoomPanel;
    //[SerializeField] TMP_InputField roomNameInputField;
    //[SerializeField] TMP_InputField maxPlayerInputField;

    private void Awake()
    {
        BindAll();

        AddEvent("CreateRoomButton", EventType.Click, CreateRoomMenu);
        AddEvent("RandomMatchingButton", EventType.Click, RandomMatching);
        AddEvent("CreateRoomConfirmButton", EventType.Click, CreateRoomConfirm);
        AddEvent("CreateRoomCancelButton", EventType.Click, CreateRoomCancel);
        AddEvent("LobbyButton", EventType.Click, JoinLobby);
        AddEvent("LogoutButton", EventType.Click, Logout);
        AddEvent("DeleteUserButton", EventType.Click, DeleteUser);
        AddEvent("DeleteUserButton", EventType.Click, DeleteUser);
    }

    private void OnEnable()
    {
        GetUI("CreateRoomPanel").SetActive(false); //createRoomPanel.SetActive(false);
    }

    private void CreateRoomMenu(PointerEventData eventData)
    {
        GetUI("CreateRoomPanel").SetActive(true);

        GetUI<TMP_InputField>("RoomNameInputField").text = $"Room {Random.Range(1000, 10000)}"; //roomNameInputField.text = $"Room {Random.Range(1000, 10000)}";
        GetUI<TMP_InputField>("MaxPlayerInputField").text = "8"; //maxPlayerInputField.text = "8";
    }

    private void CreateRoomConfirm(PointerEventData eventData)
    {
        string roomName = GetUI<TMP_InputField>("RoomNameInputField").text;
        if (roomName == "")
        {
            Debug.LogWarning("방 이름을 지정해야 방을 생성할 수 있습니다.");
            return;
        }

        int maxPlayer = int.Parse(GetUI<TMP_InputField>("MaxPlayerInputField").text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;

        PhotonNetwork.CreateRoom(roomName, options);
    }

    private void CreateRoomCancel(PointerEventData eventData)
    {
        GetUI("CreateRoomPanel").SetActive(false);
    }

    private void RandomMatching(PointerEventData eventData)
    {
        Debug.Log("랜덤 매칭 요청");

        // 비어 있는 방이 없으면 들어가지 않는 방식
        // PhotonNetwork.JoinRandomRoom();

        // 비어 있는 방이 없으면 새로 방을 만들어서 들어가는 방식
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions() { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName : name, roomOptions : options);
    }

    private void JoinLobby(PointerEventData eventData)
    {
        Debug.Log("로비 입장 요청");
        PhotonNetwork.JoinLobby();
    }

    private void Logout(PointerEventData eventData)
    {
        Debug.Log("로그아웃 요청");
        PhotonNetwork.Disconnect();
    }

    private void DeleteUser(PointerEventData eventData)
    {
        GetUI("UserDeletePanel").SetActive(true);
    }

    private void DeleteUserCancel(PointerEventData eventData)
    {
        GetUI("UserDeletePanel").SetActive(false);
    }

    private void DeleteUserConfirm(PointerEventData eventData, string email, string password)
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;

        if (user == null)
        {
            Debug.LogError("No user is currently logged in.");
            return;
        }

        user.DeleteAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User deleted successfully.");
                PhotonNetwork.Disconnect();
            });
    }
}
