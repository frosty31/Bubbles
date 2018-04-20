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

    public GameObject localPlayerObject { set; get; }
    public PlayerController localPlayerController { set; get; }

    public int numPlayers { set; get; }

    // public GameObject AndroidARCoreCamera;
    // public GameObject StandaloneCamera;

    private void Start()
    {
#if UNITY_STANDALONE
        // Instantiate(StandaloneCamera);
#else
        // Instantiate(AndroidARCoreCamera);
#endif
    }
}
