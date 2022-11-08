using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Mines : MonoBehaviourPun
{
    [SerializeField] private GameObject mine;

        // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && Input.GetKeyUp(KeyCode.M))
        {
            photonView.RPC("SetMine",RpcTarget.AllBuffered);
            
        }
        
    }
    [PunRPC]
    void SetMine()
    {
        Vector3 tmp= this.transform.position - new Vector3(3,0,0);
        Quaternion tmp2= this.transform.rotation;
        Debug.Log("Colocando mina");
        GameObject.Instantiate(mine,tmp, tmp2);
    }
}
