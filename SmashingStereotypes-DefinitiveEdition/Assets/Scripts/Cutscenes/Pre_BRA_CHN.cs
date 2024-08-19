using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pre_BRA_CHN : MonoBehaviour
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
        dialog.text = "Quê?! Do que você me chamou?! \"japa\"? Eu sou chinesa, sua baranga dançarina de carnaval!";



        yield return new WaitForSeconds(waitTime);

        DIA.GetComponent<Animator>().SetTrigger("Left");
        BRA.GetComponent<Animator>().SetTrigger("Embarassed");
        dialog.text = "Droga... Esse não foi meu melhor flerte...";

        yield return new WaitForSeconds(waitTime);



    }
}
