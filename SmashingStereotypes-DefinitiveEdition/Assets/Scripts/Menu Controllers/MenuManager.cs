using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviourPunCallbacks
{
    //Criando objetos para as telas de carregamento e lobby, além de criar um input field que retornará o nome da sala
    [SerializeField] private GameObject loadingGameObj, lobbyGameObj;
    [SerializeField] private TMP_InputField nameRoom_InputField;

    private void Start()
    {
        //Definindo as configurações do jogo
        PhotonNetwork.AutomaticallySyncScene = true;
        Application.targetFrameRate = 60;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
        //Destruindo o singleton do Gamemanager, para ele não ser duplicado quando o jogo reiniciar
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = null;
        }
        //Conectando no Photon
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
    //Se conectou, então entra para o lobby
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    //Se está no lobby, então desliga a tela de carregamento e inicia a de lobby
    public override void OnJoinedLobby()
    {
        Loading(false);
    }

    private void Loading(bool isLoading)
    {
        loadingGameObj.SetActive(isLoading);
        lobbyGameObj.SetActive(!isLoading);
    }
    //Método que cria a sala
    public void CreateRoom()
    {
        //Verificando se o Inputfield está preenchido
        if (!string.IsNullOrWhiteSpace(nameRoom_InputField.text))
            //Criando a sala para no máximo dois jogadores
            PhotonNetwork.CreateRoom(nameRoom_InputField.text, new RoomOptions { MaxPlayers = 2 }, null);
        else
        {
            //Avisando que o nome é inválido
            nameRoom_InputField.placeholder.GetComponent<TMP_Text>().text = "Digite um nome válido.";
        }
    }
    //Método que entra na sala
    public void JoinRoom()
    {
        //Verificando se o Inputfield está preenchido
        if (!string.IsNullOrWhiteSpace(nameRoom_InputField.text))
            //Entrando na sala para no máximo dois jogadores
            PhotonNetwork.JoinRoom(nameRoom_InputField.text);
        else
        {
            //Avisando que o nome é inválido
            nameRoom_InputField.placeholder.GetComponent<TMP_Text>().text = "Digite um nome válido.";
        }
    }
    //Se entrou na sala, então muda a cena para a tela de seleção
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("SelectionMenu");
    }
    //Se desconectou, então exibe a tela de carregamento e reconecta
    public override void OnDisconnected(DisconnectCause cause)
    {
        Loading(true);
        PhotonNetwork.ConnectUsingSettings();
    }
    //Método que desconecta e retorna para o menu inicial
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PhotonNetwork.Disconnect();
    }
}
