using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private Button braButton, chnButton, indButton, mexButton, startButton;

    private Player player1, player2;

    private void Start()
    {
        startButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            player1 = PhotonNetwork.PlayerList[0];
            player2 = PhotonNetwork.PlayerList[1];
        }

        if (player1 != null && player2 != null)
        {
            if ((!String.IsNullOrEmpty(player1.NickName) && !String.IsNullOrEmpty(player2.NickName)) && PhotonNetwork.IsMasterClient)
            {
                startButton.gameObject.SetActive(true);
            }
        }

        if (PhotonNetwork.PlayerList.Length < 2)
            startButton.gameObject.SetActive(false);
    }

    public void SelectCharacter(string characterID)
    {
        PhotonNetwork.NickName = characterID;
    }

    public void StartGame()
    {
        int indexMap = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);

        PhotonNetwork.LoadLevel(PhotonNetwork.PlayerList[indexMap].NickName);
    }

    public void BackToLobby()
    {
        StartCoroutine(DisconnectAndBackLobby());
    }

    private IEnumerator DisconnectAndBackLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();

            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }

        SceneManager.LoadScene("Lobby");
    }
}
