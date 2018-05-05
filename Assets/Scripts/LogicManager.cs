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

    public GameObject ARKitCamera;
    public GameObject StandaloneCamera;
    public GameObject Floor;

    private void Start()
    {
#if UNITY_STANDALONE
        Instantiate(StandaloneCamera);
        Instantiate(Floor);
#else
        Instantiate(AndroidARCoreCamera);
        GameObject arkitManager = Instantiate(Resources("ARKitManager"));
        Transform hitCubeParent = arkitManager.transform.Find("HitCubeParent");
        Instantiate(Floor, hitCubeParent);
#endif
    }
}
