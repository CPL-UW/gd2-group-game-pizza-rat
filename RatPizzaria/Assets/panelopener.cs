using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject Tutorial;

    public void OpenTutorial()
    {
        if (Tutorial != null)
        {
            Tutorial.SetActive(true);

        }

    }
}