using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Pre_BRA_MEX : MonoBehaviour
{
    public GameObject BRA, MEX, DIA, VS, BG, WS;
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
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "É meio difícil andar com esse baldão de pipoca, mas não dá pra ver ginástica sem pipoca. É muito chato. Imagina fazer ginástica.";
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {


        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Kick");
        dialog.text = "!!!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Ei, olha por onde anda, seu caminhão de taco!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Popcorn");
        dialog.text = "Que bom que eu trouxe um colete aprova de balas pra não ser esfaqueado por brasileiro!";


        yield return new WaitForSeconds(waitTime / 2f);

        DIA.SetActive(false);
        MEX.SetActive(false);
        BRA.SetActive(false);
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
