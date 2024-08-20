using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Pre_IND_MEX : MonoBehaviour
{
    public GameObject IND, MEX, DIA, VS, BG, WS;
    int random;
    public AudioSource audioSource;

    public AudioClip vsMusic;
    public TextMeshProUGUI dialog;

    void Start()
    {
        vsMusic = Resources.Load<AudioClip>("Music_Vs");
        audioSource = this.GetComponent<AudioSource>();
        VS.SetActive(false);
        WS.SetActive(false);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Default");
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "TIRO É O ÚNICO ESPORTE QUE PRESTA!!! VAI MÉXICOOOOO!!!!!";
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Embarassed");
        MEX.GetComponent<Animator>().SetTrigger("Happy");
        dialog.text = "IÉAAAAAAAA!!";

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Right");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Dá pra torcer mais baixo? Aqui não é cinco de maio.";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Ou o quê? Você vai passar por cima de mim com uma vaca?";

        yield return new WaitForSeconds(waitTime / 2f);

        DIA.SetActive(false);
        MEX.SetActive(false);
        IND.SetActive(false);
        BG.SetActive(false);
        VS.SetActive(true);
        WS.SetActive(true);
        WS.GetComponent<Animator>().SetTrigger("Blink");

        audioSource.PlayOneShot(vsMusic);

        yield return new WaitForSeconds(waitTime);

        WS.GetComponent<Animator>().SetTrigger("Whiten");

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(SelectionManager.nextMap);




    }
}
