using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicManager : MonoBehaviour {

    #region Singleton

    public static LogicManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public PlayerController localPlayerController { set; get; }
}
