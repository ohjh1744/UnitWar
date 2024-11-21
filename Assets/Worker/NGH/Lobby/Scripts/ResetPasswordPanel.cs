using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResetPasswordPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputFiedl;

    public void SendResetEmail()
    {
        string email = emailInputFiedl.text;
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
