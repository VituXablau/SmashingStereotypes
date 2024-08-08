using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviourPunCallbacks
{
    //Hud da tela de vitória
    [SerializeField] private GameObject[] imagesWin, imagesLose;
    [SerializeField] private TextMeshProUGUI titleText, shadowTitleText;
    [SerializeField] private Button leftRoomButton;

    void Start()
    {
        //Criando uma string para o nome do vencedor
        string playerWin = null;

        //Verificando quem foi o vencedor
        if(GameManager.Instance.p1Lives >= 0)
            playerWin = GameManager.Instance.p1Name;
        else if(GameManager.Instance.p2Lives >= 0)
            playerWin = GameManager.Instance.p2Name;

        //Armazenando o nome dos dois jogadores
        string player1 = GameManager.Instance.p1Name;
        string player2 = GameManager.Instance.p2Name;

        //Exibindo o nome do vencedor na tela
        switch (playerWin)
        {
            case "BRA":
                titleText.text = "MIRELLA VENCE!";
                shadowTitleText.text = "MIRELLA VENCE!";
                break;
            case "CHN":
                titleText.text = "XIUYING VENCE!";
                shadowTitleText.text = "XIUYING VENCE!";
                break;
            case "IND":
                titleText.text = "DEEPAK VENCE!";
                shadowTitleText.text = "DEEPAK VENCE!";
                break;
            case "MEX":
                titleText.text = "ERNESTO VENCE!";
                shadowTitleText.text = "ERNESTO VENCE!";
                break;
        }

        //Verificando se o jogador 1 foi o vencedor
        if (GameManager.Instance.p1Lives >= 0 && GameManager.Instance.p2Lives < 0)
        {
            foreach (GameObject p1 in imagesWin)
            {
                //Exibindo a imagem correta do jogador 1
                if (p1.name == player1)
                {
                    p1.SetActive(true);
                }
            }
            foreach (GameObject p2 in imagesLose)
            {
                //Exibindo a imagem correta do jogador 1
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                }
            }
        }
        //Verificando se o jogador 1 foi o vencedor
        else if (GameManager.Instance.p1Lives < 0 && GameManager.Instance.p2Lives >= 0)
        {
            foreach (GameObject p2 in imagesWin)
            {
                //Exibindo a imagem correta do jogador 2
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                }
            }
            foreach (GameObject p1 in imagesLose)
            {
                //Exibindo a imagem correta do jogador 2
                if (p1.name == player1)
                {
                    p1.SetActive(true);
                }
            }
        }
        else
        {
            Debug.Log("Nenhum jogador ganhou ou perdeu. Verifique os valores das vidas.");
        }
    }
    //Método que sai da sala e limpa o Nickname dos jogadores
    public void LeaveRoom()
    {
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");
        Debug.Log("Saindo");
    }
}
