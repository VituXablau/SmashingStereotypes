using System.Security.Cryptography;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] imagesWin, imagesLose;
    [SerializeField] private TextMeshProUGUI titleText, shadowTitleText;
    [SerializeField] private Button leftRoomButton;

    void Start()
    {
        string playerWin = null;

        if(GameManager.Instance.p1Lives > 0)
            playerWin = GameManager.Instance.p1Name;
        else if(GameManager.Instance.p2Lives > 0)
            playerWin = GameManager.Instance.p2Name;
        
        string player1 = GameManager.Instance.p1Name;
        string player2 = GameManager.Instance.p2Name;

        // Log para verificar os nomes dos jogadores
        Debug.Log($"Player 1: {player1}, Player 2: {player2}");

        // Determina o texto do título
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

        // Log para verificar os valores das vidas
        Debug.Log($"p1Lives: {GameManager.Instance.p1Lives}, p2Lives: {GameManager.Instance.p2Lives}");

        // Condições para definir a vitória e a derrota
        if (GameManager.Instance.p1Lives > 0 && GameManager.Instance.p2Lives < 0)
        {
            foreach (GameObject p1 in imagesWin)
            {
                if (p1.name == player1)
                {
                    p1.SetActive(true);
                    Debug.Log($"{player1} ganhou!");
                }
            }
            foreach (GameObject p2 in imagesLose)
            {
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                    Debug.Log($"{player2} perdeu!");
                }
            }
        }
        else if (GameManager.Instance.p1Lives < 0 && GameManager.Instance.p2Lives > 0)
        {
            foreach (GameObject p2 in imagesWin)
            {
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                    Debug.Log($"{player2} ganhou!");
                }
            }
            foreach (GameObject p1 in imagesLose)
            {
                if (p1.name == player1)
                {
                    p1.SetActive(true);
                    Debug.Log($"{player1} perdeu!");
                }
            }
        }
        else
        {
            Debug.Log("Nenhum jogador ganhou ou perdeu. Verifique os valores das vidas.");
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");
        Debug.Log("Saindo");
    }
}
