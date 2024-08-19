using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Pos_MEX_CHN : MonoBehaviour
{
  public GameObject MEX, CHN, DIA;
  public TextMeshProUGUI dialog;

  // Start is called before the first frame update
  void Start()
  {
    DIA.GetComponent<Animator>().SetTrigger("Left");
    MEX.GetComponent<Animator>().SetTrigger("Neutral");
    CHN.GetComponent<Animator>().SetTrigger("Sad");
    dialog.text = "Vo-você... realmente acha... que eu sou um projeto de muralha?...";
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
    MEX.GetComponent<Animator>().SetTrigger("Embarassed");
    dialog.text = "Não sei, você acha que eu sou um vendedor de taco?";

    yield return new WaitForSeconds(waitTime);

    CHN.GetComponent<Animator>().SetTrigger("Sad");
    DIA.GetComponent<Animator>().SetTrigger("Left");
    dialog.text = "Ok, justo, eu fui trouxa de gravar aquele vídeo. Desculpa.";

    yield return new WaitForSeconds(waitTime);

    MEX.GetComponent<Animator>().SetTrigger("Default");
     CHN.GetComponent<Animator>().SetTrigger("Embarassed");
    DIA.GetComponent<Animator>().SetTrigger("Right");
    dialog.text = "Tá, talvez você não seja tão muralha assim.";

    yield return new WaitForSeconds(waitTime);

    PhotonNetwork.NickName = "";
    SceneManager.LoadScene("Lobby");

  }
}
