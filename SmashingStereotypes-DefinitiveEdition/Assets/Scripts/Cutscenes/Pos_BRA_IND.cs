using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_BRA_IND : MonoBehaviour
{
    public GameObject BRA, IND, DIA;
    public TextMeshProUGUI dialog;

    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Que droga de luta imaginária foi aquela?";
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Que droga de luta imaginária foi aquela?";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");

        dialog.text = "Que droga de tentativa de dar em cima de mim foi aquela?!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Quê?! Dar em cima?! Que nojo! Eu só tava tentando te chamar pra o time de futebol do meu startup?! Me disseram que Brasileiros eram amigáveis!";


        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Ok, eu admito que presumi que você tava dando em cima de mim por que indianos tem essa fama... Desculpa";
        IND.transform.localScale = new Vector3(1, 1, 1);


        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Ok, desculpe por achar que só por ser Brasileira, você é especialista em futebol.";



        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Default");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "Ha, na verdade eu amo futebol.";

        yield return new WaitForSeconds(waitTime);

        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");



    }
}
