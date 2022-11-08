using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PowerUp : MonoBehaviour, IOnEventCallback
{
    // Start is called before the first frame update
    private const byte CureEventCode = 1;
    [SerializeField] private GameObject _powerUP;
    [SerializeField] private GameObject _spawnPoint;
    private bool generate = true;


    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "mine" && generate)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GenerateCure();
                generate = false;
            }
        }
    }
     

    private void GenerateCure()
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };
        PhotonNetwork.RaiseEvent(CureEventCode, null, eventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == CureEventCode)
        {
            Debug.Log("Generar Cura");
            GameObject.Instantiate(_powerUP, _spawnPoint.transform.position, _spawnPoint.transform.rotation);
        }
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
}
