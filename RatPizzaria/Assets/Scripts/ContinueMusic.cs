using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueMusic : MonoBehaviour
{
    private void Awake() {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("menumusic");
        if(musicObj.Length > 1) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
