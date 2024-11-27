using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitManager
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initalize()
    {
        //GameObject.Instantiate(Resources.Load<DataManager>("Manager/DataManager"));
        //GameObject.Instantiate(Resources.Load<SoundManager>("Manager/SoundManager"));
    }


}