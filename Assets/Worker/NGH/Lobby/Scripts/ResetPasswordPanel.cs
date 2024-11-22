using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResetPasswordPanel : UIBInder
{
    //[SerializeField] TMP_InputField emailInputFiedl;

    private void Awake()
    {
        BindAll();

        AddEvent("SendResetEmailButton", EventType.Click, SendResetEmail);
    }

    // 입력한 메일 주소로 비밀번호 재설정 메일 송신
    public void SendResetEmail(PointerEventData eventData)
    {
        string email = GetUI<TMP_InputField>("ResetPasswordEmailInputField").text; //emailInputFiedl.text;
        BackendManager.Auth.SendPasswordResetEmailAsync(email)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Password reset email sent successfully.");
                gameObject.SetActive(false);
            });
    }
}
