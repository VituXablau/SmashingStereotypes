using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_BRA_MEX : MonoBehaviour
{
    public GameObject BRA, MEX, DIA;
    public TextMeshProUGUI dialog;

    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Parando pra pensar, esse balde de pipoca é grande de mais pra um. Tem certeza que não quer dividir?";
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {


        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Bem, você não me esfaqueou durante nossa luta imaginária, então acho que estava errado sobre brasileiros.";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "Eba, pipoca! Eu retiro o que eu disse!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "...Espera... se você é mexicano, como conseguiu um colete a prova de balas em Paris?";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Eu sou acima da lei.";


        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Uhhh, pipoca?...";


        yield return new WaitForSeconds(waitTime);

        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");



    }
}
