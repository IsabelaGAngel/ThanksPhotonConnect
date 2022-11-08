using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class IFNickname : MonoBehaviour
{
    private const string _playerNamePrefKey = "PlayerName";
    // Start is called before the first frame update
    void Start()
    {
        string _defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(_playerNamePrefKey))
            {
                _defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
                _inputField.text = _defaultName;
            }
        }

        PhotonNetwork.NickName = _defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("PlayerName is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(_playerNamePrefKey, value);
    }
}
