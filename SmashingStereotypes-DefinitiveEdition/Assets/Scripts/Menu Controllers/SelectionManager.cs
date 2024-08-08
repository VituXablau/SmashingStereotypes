using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //Armazenando os botões da cena
    [SerializeField] private Button braButton, chnButton, indButton, mexButton, startButton;
    //Armazenando os jogadores
    private Player player1, player2;

    //Desativando o botão de jogar quando entra na cena
    private void Start()
    {
        startButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Verificando se existem dois jogadores na cena, se sim então atribui eles às variáveis de jogadores
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            player1 = PhotonNetwork.PlayerList[0];
            player2 = PhotonNetwork.PlayerList[1];
        }
        //Verificando se os jogadores foram preenchidos
        if (player1 != null && player2 != null)
        {
            //Verificanso se os seus Nicknames estão preenchidos e se está no jogo do criador da sala, se sim ativa o botão de jogar
            if ((!String.IsNullOrEmpty(player1.NickName) && !String.IsNullOrEmpty(player2.NickName)) && PhotonNetwork.IsMasterClient)
            {
                startButton.gameObject.SetActive(true);
            }
        }
        //Se tiver apenas um jogador na sala, então o botão de jogar fica desativado
        if (PhotonNetwork.PlayerList.Length < 2)
            startButton.gameObject.SetActive(false);
    }
    //Método dos botões de seleção
    public void SelectCharacter(string characterID)
    {
        //Definindo o nome que está armazenado no botão de seleção do jogador como o seu Nickname
        PhotonNetwork.NickName = characterID;
    }
    //Método que começa o jogo
    public void StartGame()
    {
        //Aleatorizando a arena de batalha com base nos personagens escolhidos
        int indexMap = UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length);

        PhotonNetwork.LoadLevel(PhotonNetwork.PlayerList[indexMap].NickName);
    }
    //Método do botão de voltar para o lobby
    public void BackToLobby()
    {
        //Inicia a coroutine que volta para o lobby
        StartCoroutine(DisconnectAndBackLobby());
    }
    //Coroutine que volta para o lobby  
    private IEnumerator DisconnectAndBackLobby()
    {
        //Se estiver conectado, então ele desconecta
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();

            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }
        //Voltando para o lobby
        SceneManager.LoadScene("Lobby");
    }
}
