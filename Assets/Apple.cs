using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Knife"))
        {
            anim.SetTrigger("Death");
            GameManager.Instance.IncreaseApples();
        }
    }
}
