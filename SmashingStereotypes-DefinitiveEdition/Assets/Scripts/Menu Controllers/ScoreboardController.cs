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

        if(GameManager.Instance.p1Lives >= 0)
            playerWin = GameManager.Instance.p1Name;
        else if(GameManager.Instance.p2Lives >= 0)
            playerWin = GameManager.Instance.p2Name;
        
        string player1 = GameManager.Instance.p1Name;
        string player2 = GameManager.Instance.p2Name;

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

        if (GameManager.Instance.p1Lives >= 0 && GameManager.Instance.p2Lives < 0)
        {
            foreach (GameObject p1 in imagesWin)
            {
                if (p1.name == player1)
                {
                    p1.SetActive(true);
                }
            }
            foreach (GameObject p2 in imagesLose)
            {
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                }
            }
        }
        else if (GameManager.Instance.p1Lives < 0 && GameManager.Instance.p2Lives >= 0)
        {
            foreach (GameObject p2 in imagesWin)
            {
                if (p2.name == player2)
                {
                    p2.SetActive(true);
                }
            }
            foreach (GameObject p1 in imagesLose)
            {
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

    public void LeaveRoom()
    {
        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");
        Debug.Log("Saindo");
    }
}
