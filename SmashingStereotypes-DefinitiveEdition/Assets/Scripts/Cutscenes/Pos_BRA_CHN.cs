using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_BRA_CHN : MonoBehaviour
{
    public GameObject BRA, CHN, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 3f)
    {

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        CHN.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Epa, epa, foi mal! Eu só tava tentando pegar seu número!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        CHN.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Meu... Meu número? Tá. Eu deixo você me seguir. Se você prometer nunca mais me chamar de \"japa\" de novo.";

        yield return new WaitForSeconds(waitTime);

        
        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        CHN.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Eu prometo!!!";

        yield return new WaitForSeconds(waitTime);


       PhotonNetwork.NickName = "";
        SceneManager.LoadScene("Lobby");



    }
}
