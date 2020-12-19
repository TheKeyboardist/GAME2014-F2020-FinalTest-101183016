/***;
*Project            : GAME2014 Final Test
*
*Program name       : "ShrinkingPlatformController.cs"
*
* Author            : Ivan Kravchenko
* 
* Student Number    : 101183016
*
*Date created       : 19/12/20
*
*Description        : controls shrinking platform
*
*Last modified      : 19/12/20
*
* Revision History  :
*
*Date        Author Ref    Revision (Date in YYYYMMDD format) 
*19/12/20    Ivan Kravchenko        Created script. 
*
|**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatformController : MonoBehaviour
{
    //audio for the platfrom
    [SerializeField]
    public AudioSource shrinkSound;
    public AudioSource resetSound;
    bool playingShrinkSound;


    Vector3 deltaShrink;
    Vector3 deltaDistance;
    float amplitude;



    Vector3 tempPlayerPos;

    Vector3 startScale;
    Vector3 startLocation;
    bool moveUp;
    bool moveDown;
    bool keepResetting;

    // Start is called before the first frame update
    void Start()
    {
        playingShrinkSound = false;
        keepResetting = false;
        amplitude = 0.2f;
        deltaDistance = new Vector3(0.0f, 0.001f, 0.0f);
        deltaShrink = new Vector3(0.1f, 0.01f, 0.0f);

        startScale = transform.localScale;
        startLocation = transform.position;

        RandomizeStartDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
        Float();

        ResetItself();
    }


    //moves the platfrom every frame
    void Float()
    {
        if (moveDown)
        {
            if (transform.position.y > (startLocation.y - amplitude))
            {
                transform.position -= deltaDistance;
            }
            else
            {
                moveDown = false;
                moveUp = true;
            }
        }
        if (moveUp)
        {
            if (transform.position.y < (startLocation.y + amplitude))
            {
                transform.position += deltaDistance;
            }
            else
            {
                moveUp = false;
                moveDown = true;
            }
        }
    }


    //chooses which direction the platfrom will go first
    void RandomizeStartDirection()
    {
        if (Random.Range(1, 10) < 6)
        {
            moveDown = true;
            moveUp = false;
        }
        else
        {
            moveDown = false;
            moveUp = true;
        }
    }


    //shrinks the platfrom every frame
    void DestroyItself()
    {   
        if(transform.localScale.x > 0.05f)
        {
            keepResetting = false;
            transform.localScale -= deltaShrink;
        }
        else
        {
            shrinkSound.Stop();
        }
    }


    //resets the platfrom every frame
    void ResetItself()
    {


        if(keepResetting)
        {
            shrinkSound.Stop();
            if (transform.localScale.x < startScale.x)
            {
                transform.localScale += deltaShrink;
            }
            else
            {
                keepResetting = false;
                resetSound.Stop();
            }    
        }
    }
    //fixes platfrom shrink bug
    private void OnTriggerEnter2D(Collider2D collision)
    {
        resetSound.Stop();
     StartCoroutine(delayReset(3.0f));    
    }
    //part of the shrink fix
    IEnumerator delayReset(float timer)
    {
        yield return new WaitForSeconds(timer);
        if(transform.localScale.x < 0.5f)
        {
            keepResetting = true;
        }

        
    }

    //tells the platfrom to shrink when player on top
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!playingShrinkSound && !keepResetting || (playingShrinkSound && keepResetting))
            StartCoroutine(DelayEndOfShrinkSound(3.0f));

        DestroyItself();
    }
    //tells the platform to reset when player has left
    private void OnTriggerExit2D(Collider2D collision)
    {
        resetSound.Stop();
        shrinkSound.Stop();
        keepResetting = true;
        resetSound.Play();
    }

    //allows the sounds to play properly
    IEnumerator DelayEndOfShrinkSound(float timer)
    {
        playingShrinkSound = true;
        shrinkSound.Play();
        yield return new WaitForSeconds(timer);
        playingShrinkSound = false;
    }

}
