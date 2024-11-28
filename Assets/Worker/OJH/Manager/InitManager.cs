using Photon.Pun;
using UnityEngine;

public static class InitManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initalize()
    {
        //GameObject.Instantiate(Resources.Load<DataManager>("Manager/DataManager"));
        //GameObject.Instantiate(Resources.Load<SoundManager>("Manager/SoundManager"));
        GameObject.Instantiate(Resources.Load<GameSceneManager>("Manager/GameSceneManager"));
        GameObject.Instantiate(Resources.Load<ObjectPool>("Manager/ObjectPool"));
    }
}