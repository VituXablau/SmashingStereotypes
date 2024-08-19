using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_IND_CHN : MonoBehaviour
{

    public GameObject IND, CHN, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Okay, talvez essa não foi a melhor das minhas ideias...";
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 5f)
    {

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "É. Foi terrível. Mas eu confesso que reagi de forma exagerada também.";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Ok, amigos?";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "...Uh, não?!";
        CHN.transform.localScale = new Vector3(1, 1, 1);
       
        yield return new WaitForSeconds(waitTime);

        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");

    }
}
