using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pre_Bra_IND : MonoBehaviour
{
    public GameObject BRA, IND, DIA;
    public TextMeshProUGUI dialog;

    // Start is called before the first frame update
    void Start()
    {
        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        IND.GetComponent<Animator>().SetTrigger("Default");
        StartCoroutine(Cutscene());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Cutscene(float waitTime = 3f)
    {

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Happy");
        IND.GetComponent<Animator>().SetTrigger("Default");
        dialog.text = "Olha só! Aquela mulher parece ser brasileira! Aposto que vai ser moleza conseguir o número dela!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Smug");

        dialog.text = "Ha, gostando do jogo de futebol? Aposto que você consegue fazer várias embaixadas!";

        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Quê?! Só porque eu sou brasileira? Mete o pé, fedorento!";
        IND.transform.localScale = new Vector3(-1, 1, 1);

        yield return new WaitForSeconds(waitTime);


        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Smug");
        dialog.text = "Fedorento?! Só porque sou indiano?! Só pra você saber, minha colônia custou-";


        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Right");
        BRA.GetComponent<Animator>().SetTrigger("Angry");
        IND.GetComponent<Animator>().SetTrigger("Angry");
        dialog.text = "Eu não perguntei, @#$!@!$!";


        yield return new WaitForSeconds(waitTime);









    }
}
