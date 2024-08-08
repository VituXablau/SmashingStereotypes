using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CardScreenManager: MonoBehaviour
{
     public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

       public void Mirella()
    {
        SceneManager.LoadScene("MirellaCard");
    }

       public void Xiuying()
    {
        SceneManager.LoadScene("XiuyingCard");
    }

        public void Deepak()
    {
        SceneManager.LoadScene("DeepakCard");
    }

        public void Ernesto()
    {
        SceneManager.LoadScene("ErnestoCard");
    }

}
