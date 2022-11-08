using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;


public enum _regionCodes
    {
        AUTO,
        CAE,
        EU,
        US,
        USW,
        SA
    }
public class ConnectCntrl : MonoBehaviourPunCallbacks
{
    [SerializeField] string _gameVersion = "1";
    [SerializeField] private string _regionCode = null;
    [SerializeField] private Text _txtBtnConnect;
    [SerializeField] private GameObject _btnConnect;
    [SerializeField] private GameObject _panelRoom;
    [SerializeField] private GameObject _panelConnect;
    [SerializeField] private Text _usernameP2;
    [SerializeField] private GameObject tank;
    [SerializeField] private Material black;
    [SerializeField] private Material white;
    [SerializeField] private Material green;
    
    
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void SetBtn(bool state, string msg)
    {
        _txtBtnConnect.text = msg;
        //_btnConnect.GetComponentInChildren<Button>().SetEnabled(state);
        _btnConnect.SetActive(state);
    }

    public void SetRegion(int index)
    {
        _regionCodes region = (_regionCodes) index;
        if (region == _regionCodes.AUTO)
        {
            _regionCode = null;
        }
        else
        {
            _regionCode = region.ToString();
        }
    }

    void ShowRoomPanel()
    {
        _panelConnect.SetActive(false);
        _panelRoom.SetActive(true);
        if (PhotonNetwork.NickName != null)
        {
            
            Debug.Log(PhotonNetwork.NickName);
            _usernameP2.text = (string)PhotonNetwork.NickName;
        }
        

        
    }
    public void SetColor(int index)
    {
        string color = GameObject.Find("DdownColor").GetComponent<Dropdown>().options[index].text;
        Debug.Log("Color: " + color);
        var propsToSet = new ExitGames.Client.Photon.Hashtable() {{"color",color}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);
    }
    public void SetReady()
    {
        var propsToSet = new ExitGames.Client.Photon.Hashtable() {{"ready",true}};
        PhotonNetwork.LocalPlayer.SetCustomProperties(propsToSet);
    }
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = _regionCode;

    }


    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        SetBtn(true, "LETS BATTLE");
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        SetBtn(false, "WAITING PLAYERS");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is ready.");
            ShowRoomPanel();
            /*PhotonNetwork.LoadLevel("Game");*/
        }
        //PhotonNetwork.LoadLevel("Game");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " Se ha unido al cuarto: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is full");
            ShowRoomPanel();
            /*PhotonNetwork.LoadLevel("Game");*/
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changeProps)
    {
        if (changeProps.ContainsKey("color"))
        {
            //cambiar color al avatar
            Renderer tmp = tank.GetComponentInChildren<Renderer>();
            int tmpNum = 1;
            Debug.Log("Cambie de color");
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                string color = (string) player.CustomProperties["color"];
                Debug.Log(player.NickName + "It is the color ... " + color);
                if (color == "WHITE" && tmpNum ==1)
                {
                    tmp.material = white;
                }
                else if (color == "BLACK" && tmpNum ==1)
                {
                    tmp.material = black;
                }
                else if (color == "GREEN" && tmpNum ==1)
                {
                    tmp.material = green;
                }

                tmpNum++;
            }
        }

        if (changeProps.ContainsKey("ready"))
        {
            int playersReady = 0;
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                bool ready = (bool) player.CustomProperties["ready"];
                Debug.Log(player.NickName + "is ready? ... " + ready);
                if (ready)
                {
                    playersReady++;
                }

                if (playersReady == 2)
                {
                    PhotonNetwork.LoadLevel("Game");
                }
            }
            
        }
    }


    #endregion
}
