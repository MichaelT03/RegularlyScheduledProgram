using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteraction : MonoBehaviour
{
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAnimation()
    {
        if (myAnimator.GetBool("isOn") == false)
            myAnimator.SetBool("isOn", true);
        else
            myAnimator.SetBool("isOn", false);
    }
}
