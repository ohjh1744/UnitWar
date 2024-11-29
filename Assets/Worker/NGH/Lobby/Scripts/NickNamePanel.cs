using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NickNamePanel : UIBInder
{
    //[SerializeField] TMP_InputField nickNameInputField;

    private void Awake()
    {
        BindAll();

        AddEvent("NickNameConfirmButton", EventType.Click, Confirm); 
    }

    // 닉네임 설정
    private void Confirm(PointerEventData eventData)
    {
        string nickName = GetUI<TMP_InputField>("NickNameInputField").text; //nickNameInputField.text;
        if(nickName == "")
        {
            Debug.LogWarning("닉네임을 설정해주세요.");
            return;
        }

        FirebaseUser user = BackendManager.Auth.CurrentUser;
        UserProfile profile = new UserProfile();
        profile.DisplayName = GetUI<TMP_InputField>("NickNameInputField").text;

        user.UpdateUserProfileAsync(profile)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
                Debug.Log($"Display Name : {user.DisplayName}");
                Debug.Log($"Email : {user.Email}");
                Debug.Log($"Email verified : {user.IsEmailVerified}");
                Debug.Log($"User ID : {user.UserId}");

                PhotonNetwork.LocalPlayer.NickName = nickName;
                PhotonNetwork.ConnectUsingSettings();
                gameObject.SetActive(false);
            });
    }
}
