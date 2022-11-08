using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManagerMultiplayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _player1Spawn;
    [SerializeField] private Transform _player2Spawn;
    
    void Start()
    {
        if (_playerPrefab == null)
        {
            Debug.Log("Falta la referencia la player pref");
        }
        else
        {
            
            Transform spawnPoint = (PhotonNetwork.IsMasterClient) ? _player1Spawn : _player2Spawn;
            object[] initData = new object[1];
            initData[0] = "Data instaciation";
            PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint.position, Quaternion.identity, 0, initData);    
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
