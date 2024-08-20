using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_IND_MEX : MonoBehaviour
{
    public GameObject IND, MEX, DIA;
    public TextMeshProUGUI dialog;

    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Okay, foi mal por me exaltar. Eu odeio quando me mandam calar a boca.";
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Que tal a gente ir ali tomar uma cerveja?";

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Default");
        MEX.GetComponent<Animator>().SetTrigger("");
        MEX.transform.localScale = new Vector3(-1, 1, 1);
        dialog.text = "Eu tenho compromisso mais tarde, mas acho que dá pra dar uma passada rápida ali na esquina...";
        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "Oba!!!";

        yield return new WaitForSeconds(waitTime);

        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");

    }
}
