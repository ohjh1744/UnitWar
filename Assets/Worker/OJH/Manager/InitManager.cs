using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InitManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initalize()
    {
        //GameObject.Instantiate(Resources.Load<DataManager>("Manager/DataManager"));
        //GameObject.Instantiate(Resources.Load<SoundManager>("Manager/SoundManager"));
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject.Instantiate(Resources.Load<GameSceneManager>("Manager/GameSceneManager"));
            GameObject.Instantiate(Resources.Load<ObjectPool>("Manager/ObjectPool"));
        }
    }


}