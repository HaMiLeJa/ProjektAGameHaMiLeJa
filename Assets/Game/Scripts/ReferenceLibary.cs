using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceLibary : MonoBehaviour
{
    // Player
    public static GameObject Player;
    public static Rigidbody RigidbodyPl;
    public static PlayerSuperDash SuperDash;
    public static ShadowDash ShadowDashPl;
    public static PlayerBoost Dash;
    public static DownDash DownDashPl;
    public static PlayerMovement PlayerMov;
    public static HexMovements HexMov;

    // Hex


    // Missions
    public static MissionManager MissionMng;
    public static MissionItemSpawner ItemSpawner;
    public static MissionLibary MissLib;

    //Audio
    //public static AudioClipsHexes audioClipHexes;

    public static UIManager UIMng;
    public static CollectableManager ColMng;
    public static GameManager GameMng;
    public static ScoreManager ScoreMng;
    public static WindconditionManager WinconMng;

    private void Awake()
    {
        //Player
        Player = GameObject.FindGameObjectWithTag("Player");
        RigidbodyPl = Player.GetComponent<Rigidbody>();
        SuperDash = Player.GetComponent<PlayerSuperDash>();
        ShadowDashPl = Player.GetComponent<ShadowDash>();
        Dash = Player.GetComponent<PlayerBoost>();
        DownDashPl = Player.GetComponent<DownDash>();
        PlayerMov = Player.GetComponent<PlayerMovement>();
        HexMov = Player.GetComponent<HexMovements>();


        //Hex
        MissionMng = FindObjectOfType<MissionManager>();
        ItemSpawner = FindObjectOfType<MissionItemSpawner>();
        MissLib = FindObjectOfType<MissionLibary>();
        //Audio
        //audioClipHexes = AudioManager.Instance.gameObject.GetComponent<AudioClipsHexes>();

        UIMng = FindObjectOfType<UIManager>();
        ColMng = FindObjectOfType<CollectableManager>();
        GameMng = FindObjectOfType<GameManager>();
        ScoreMng = FindObjectOfType<ScoreManager>();
        WinconMng = FindObjectOfType<WindconditionManager>();
    }

}
