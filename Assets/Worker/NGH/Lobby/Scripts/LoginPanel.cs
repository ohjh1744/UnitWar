using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginPanel : UIBInder
{
    private void Awake()
    {
        BindAll();

        AddEvent("LoginButton", EventType.Click, Login);
        AddEvent("SignUpButton", EventType.Click, ShowSignUpPanel);
        AddEvent("ResetPasswordButton", EventType.Click, ShowResetPasswordPanel);
    }

    // 로그인을 진행
    private void Login(PointerEventData eventData)
    {
        SoundManager.Instance.PlayUI("mousedown2");
        string email = GetUI<TMP_InputField>("EmailInputField").text; //emailInputField.text;
        string password = GetUI<TMP_InputField>("PasswordInputField").text;

        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    ShowErrorPopup("로그인 요청이 취소되었습니다.");
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    string errorMessage = GetFirebaseErrorMessage(task.Exception);
                    ShowErrorPopup(errorMessage);
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    return;
                }

                AuthResult result = task.Result;
                Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
                CheckUserInfo();
            });
    }

    // 회원가입 창 활성화
    private void ShowSignUpPanel(PointerEventData eventData)
    {
        GetUI("SignUpPanel").SetActive(true);
    }

    // 비밀번호 재설정 창 활성화
    private void ShowResetPasswordPanel(PointerEventData eventData)
    {
        GetUI("ResetPasswordPanel").SetActive(true);
    }

    // 유저의 정보를 확인하고, 필요 설정이 있다면 진행.
    // 다른 설정이 필요없다면 네트워크와 연결.
    private void CheckUserInfo()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        Debug.Log($"Display Name : {user.DisplayName}");
        Debug.Log($"Email : {user.Email}");
        Debug.Log($"Email verified : {user.IsEmailVerified}");
        Debug.Log($"User ID : {user.UserId}");

        if (user.IsEmailVerified == false)
        {
            GetUI("VerifyPanel").SetActive(true); //verifyPanel.SetActive(true);
        }
        else if (user.DisplayName == "")
        {
            GetUI("NickNamePanel").SetActive(true); //nickNamePanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // 오류가 있다면 팝업창에 메시지를 띄움
    private void ShowErrorPopup(string message)
    {
        GetUI<TMP_Text>("ErrorPopupText").text = message; //errorPopupText.text = message;
        GetUI("ErrorPopup").SetActive(true);
        StartCoroutine(CloseErrorPopupCoroutine());
    }

    // 팝업창을 자동으로 닫는 코루틴
    private IEnumerator CloseErrorPopupCoroutine()
    {
        yield return new WaitForSeconds(1f);
        CloseErrorPopup();
    }

    // 팝업창을 자동으로 닫는 함수
    private void CloseErrorPopup()
    {
        GetUI("ErrorPopup").SetActive(false);
    }

    private bool IsValidEmail(string email)
    {
        // 간단한 이메일 형식 검증
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private string GetFirebaseErrorMessage(AggregateException exception)
    {
        // Firebase에서 반환된 에러 메시지 파싱
        foreach (var e in exception.InnerExceptions)
        {
            if (e is FirebaseException firebaseException)
            {
                switch ((AuthError)firebaseException.ErrorCode)
                {
                    case AuthError.MissingEmail:
                        return "이메일을 입력해주세요.";
                    case AuthError.MissingPassword:
                        return "비밀번호를 입력해주세요.";
                    case AuthError.InvalidEmail:
                        return "유효하지 않은 이메일 형식입니다.";
                    case AuthError.WrongPassword:
                        return "잘못된 비밀번호입니다.";
                    case AuthError.UserNotFound:
                        return "존재하지 않는 계정입니다.";
                    default:
                        return "로그인 중 알 수 없는 오류가 발생했습니다.";
                }
            }
        }
        return "네트워크 오류가 발생했습니다.";
    }
}
