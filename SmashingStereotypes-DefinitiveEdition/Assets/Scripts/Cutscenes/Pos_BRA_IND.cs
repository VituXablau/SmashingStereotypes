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

    // Start is called before the first frame update
    void Start()
    {
         DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Que droga de luta imaginária foi aquela?";
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 3f)
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
        dialog.text = "Eu prefiro vôlei...";
        IND.transform.localScale = new Vector3(1, 1, 1);


        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Tá, eu posso forçar meus estagiários a criarem um grupo de vôlei.";
    


        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Okay, foi mal a precipitação mas eu odeio startup.";
             
        yield return new WaitForSeconds(waitTime);

         PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");



    }
}
