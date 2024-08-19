using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Pre_MEX_CHN : MonoBehaviour
{
    public GameObject MEX, CHN, DIA, VS, BG, WS;
    int random;
    public AudioSource audioSource;

    public AudioClip vsMusic;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        vsMusic = Resources.Load<AudioClip>("Music_Vs");


        audioSource = this.GetComponent<AudioSource>();


        VS.SetActive(false);
        WS.SetActive(false);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        MEX.GetComponent<Animator>().SetTrigger("Angry");
        CHN.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Olha o carro do taco, galera! Será que ele faz amostra grátis? XD";
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
        CHN.GetComponent<Animator>().SetTrigger("Angry");
        MEX.GetComponent<Animator>().SetTrigger("Angry");
        CHN.transform.localScale = new Vector3(-1, 1, 1);
        dialog.text = "O que você quer, projeto de muralha?!";

        yield return new WaitForSeconds(waitTime / 2f);
        DIA.SetActive(false);
        MEX.SetActive(false);
        CHN.SetActive(false);
        BG.SetActive(false);
        VS.SetActive(true);
        WS.SetActive(true);
        WS.GetComponent<Animator>().SetTrigger("Blink");

        audioSource.PlayOneShot(vsMusic);

        yield return new WaitForSeconds(waitTime);

        WS.GetComponent<Animator>().SetTrigger("Whiten");

          PhotonNetwork.LoadLevel(SelectionManager.nextMap);



    }
}
