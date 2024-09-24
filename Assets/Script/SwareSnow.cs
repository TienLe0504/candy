using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwareSnow : MonoBehaviour
{
    // Start is called before the first frame update
    public int positionX;
    public int positionY;
    public bool scenceTwo = false;
    public bool scenceThree = false;
    public bool status = true;
    Animator anim;
    void Start()
    {
     anim = GetComponent<Animator>();   
    }
    public void SetAnimationTwo()
    {
        anim.SetBool("scenceTwo", true);
        scenceTwo = true;
        status = true;
    }
    public void SetAnimationThree()
    {
        anim.SetBool("scenceThree", true);
        scenceThree = true;
        status = true;
    }
    public void SetStatus()
    {
        status = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
