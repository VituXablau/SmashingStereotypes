using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pre_BRA_CHN : MonoBehaviour
{
    public GameObject BRA, CHN, DIA, VS, BG, WS;
    public AudioSource audioSource;
    public AudioClip vsMusic;
    public TextMeshProUGUI dialog;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        VS.SetActive(false);
        WS.SetActive(false);
        StartCoroutine(Cutscene());
    }

    IEnumerator Cutscene(float waitTime = 5f)
    {

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Default");
        CHN.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Heh, o Brasil tá arrasando nesse jogo de vôlei. E aquela menina ali tirando selfie parece fofa. Como será que eu chamo atenção dela?";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Smug");
        CHN.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Ei, japa! Você vende pastel de frango?";

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        CHN.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Quê?! Do que você me chamou?! Eu sou chinesa, sua baranga dançarina de carnaval!";



        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Droga... Esse não foi meu melhor flerte...";

        yield return new WaitForSeconds(waitTime / 2f);
        DIA.SetActive(false);
        BRA.SetActive(false);
        CHN.SetActive(false);
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
