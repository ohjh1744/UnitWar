using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class DatabaseTester : MonoBehaviour
{
    [SerializeField] TMP_InputField searchInputField;
    [SerializeField] TMP_Text resultText;
    [SerializeField] TMP_Text playerLevelText;
    [SerializeField] int statIncreasePerLevel = 5; // 레벨업 시 스탯 증가량

    private DatabaseReference userDataRef;
    private PlayerData currentPlayer;

    private void Start()
    {
        string uid = BackendManager.Auth.CurrentUser.UserId;
        userDataRef = BackendManager.Database.RootReference.Child("UserData").Child(uid);

        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        userDataRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("데이터를 불러오는 중 오류 발생");
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Value == null)
            {
                CreateDefaultPlayerData();
            }
            else
            {
                currentPlayer = JsonUtility.FromJson<PlayerData>(snapshot.GetRawJsonValue());
                UpdateUI();
            }
        });
    }

    private void CreateDefaultPlayerData()
    {
        currentPlayer = new PlayerData
        {
            name = BackendManager.Auth.CurrentUser.DisplayName,
            email = BackendManager.Auth.CurrentUser.Email,
            level = 1,
            job = "Warrior", // 기본 직업 설정
            stats = new PlayerStats { strength = 10, agility = 10, intelligence = 10, luck = 10 }
        };

        SavePlayerData();
    }

    private void SavePlayerData()
    {
        string json = JsonUtility.ToJson(currentPlayer);
        userDataRef.SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("데이터 저장 중 오류 발생");
            }
        });
    }

    private void UpdateUI()
    {
        playerLevelText.text = $"레벨: {currentPlayer.level}";
    }

    public void LevelUp()
    {
        currentPlayer.level++;
        currentPlayer.stats.strength += statIncreasePerLevel;
        currentPlayer.stats.agility += statIncreasePerLevel;
        currentPlayer.stats.intelligence += statIncreasePerLevel;
        currentPlayer.stats.luck += statIncreasePerLevel;

        SavePlayerData();
        UpdateUI();
    }

    public void SearchPlayer()
    {
        string playerName = searchInputField.text;
        BackendManager.Database.RootReference.Child("UserData")
            .OrderByChild("name")
            .EqualTo(playerName)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    resultText.text = "검색 중 오류가 발생했습니다.";
                    return;
                }

                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (var userSnapshot in snapshot.Children)
                    {
                        PlayerData playerData = JsonUtility.FromJson<PlayerData>(userSnapshot.GetRawJsonValue());
                        resultText.text = $"플레이어 정보:\n이름: {playerData.name}\n" +
                                          $"레벨: {playerData.level}\n" +
                                          $"직업: {playerData.job}\n" +
                                          $"힘: {playerData.stats.strength}\n" +
                                          $"민첩: {playerData.stats.agility}\n" +
                                          $"지능: {playerData.stats.intelligence}\n" +
                                          $"행운: {playerData.stats.luck}";
                    }
                }
                else
                {
                    resultText.text = "존재하지 않는 플레이어입니다.";
                }
            });
    }
}

[Serializable]
public class PlayerData
{
    public string name;
    public string email;
    public int level;
    public string job;
    public PlayerStats stats;
}

[Serializable]
public class PlayerStats
{
    public int strength;
    public int agility;
    public int intelligence;
    public int luck;
}