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

    IEnumerator Cutscene(float waitTime = 3f)
    {

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Se você fizer propaganda da minha startup talvez eu considere não derrubar a sua conta.";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Uhh, aceito?";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "...Chat, o que é startup?...";
        CHN.transform.localScale = new Vector3(1, 1, 1);
       
        yield return new WaitForSeconds(waitTime);

        PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");

    }
}
