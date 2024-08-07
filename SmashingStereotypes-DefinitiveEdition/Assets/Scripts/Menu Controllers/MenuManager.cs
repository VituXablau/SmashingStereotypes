using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject loadingGameObj, lobbyGameObj;
    [SerializeField] private TMP_InputField nameRoom_InputField;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Application.targetFrameRate = 60;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = null;
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            Loading(true);
        }
        else
        {
            Loading(false);
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Loading(false);
    }

    private void Loading(bool isLoading)
    {
        loadingGameObj.SetActive(isLoading);
        lobbyGameObj.SetActive(!isLoading);
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrWhiteSpace(nameRoom_InputField.text))
            PhotonNetwork.CreateRoom(nameRoom_InputField.text, new RoomOptions { MaxPlayers = 2 }, null);
        else
        {
            nameRoom_InputField.placeholder.GetComponent<TMP_Text>().text = "Digite um nome válido.";
        }
    }

    public void JoinRoom()
    {
        if (!string.IsNullOrWhiteSpace(nameRoom_InputField.text))
            PhotonNetwork.JoinRoom(nameRoom_InputField.text);
        else
        {
            nameRoom_InputField.placeholder.GetComponent<TMP_Text>().text = "Digite um nome válido.";
        }
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("SelectionMenu");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Loading(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PhotonNetwork.Disconnect();
    }
}
