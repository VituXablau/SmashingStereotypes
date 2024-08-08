using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardController : MonoBehaviour
{   
    Animator anim;
    public GameObject card;
    
    void Start()
    {
        anim = card.GetComponent<Animator>();
    }
    public void Up()
    {
        anim.SetInteger("estado", 1);
    }
    public void Down()
    {
        anim.SetInteger("estado", 0);
    }

      public void Back()
    {
        SceneManager.LoadScene("CardsMenu");
    }
}
