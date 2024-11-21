using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class SignUpPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInputField;
    [SerializeField] TMP_InputField passwordInputField;
    [SerializeField] TMP_InputField passwordConfirmInputField;
    private bool existsEmail = false;

    public void SignUp()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirm = passwordConfirmInputField.text;

        if(email.IsNullOrEmpty())
        {
            Debug.LogWarning("이메일을 입력해주세요.");
            return;
        }

        if (password != confirm)
        {
            Debug.LogWarning("패스워드가 일치하지 않습니다.");
            return;
        }

        /*CheckEmailExists(email, (exists) =>
        {
            if (exists)
            {
                Debug.LogWarning("이미 존재하는 계정입니다.");
                return;
            }
            else
            {
                Debug.Log("사용 가능한 이메일입니다.");
                
            }
        });*/

        CreateUser(email, password);
    }
    
    private void CreateUser(string email, string password)
    {
        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                // Firebase user has been created.
                Firebase.Auth.AuthResult result = task.Result;
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    result.User.DisplayName, result.User.UserId);
                gameObject.SetActive(false);
            });
    }

    public void CheckEmailExists(string email, System.Action<bool> callback)
    {
        Firebase.Auth.FirebaseAuth.DefaultInstance.FetchProvidersForEmailAsync(email)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("FetchProvidersForEmailAsync was canceled.");
                    return false;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("FetchProvidersForEmailAsync encountered an error: " + task.Exception);
                    return false;
                }

                // 이메일 제공자가 있다면 이미 등록된 이메일
                var providers = task.Result;
                return true;
            });
    }
}
