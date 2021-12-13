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


    //Audio
    //public static AudioClipsHexes audioClipHexes;

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


        //Audio
        //audioClipHexes = AudioManager.Instance.gameObject.GetComponent<AudioClipsHexes>();

    }

}
