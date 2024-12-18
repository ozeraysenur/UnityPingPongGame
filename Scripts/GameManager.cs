using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float gravityModifier;
    public float timeScaleModifier;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScaleModifier;
        Physics.gravity *= gravityModifier;
    }


}
