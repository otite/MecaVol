using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public static KeyboardInput Instance;
    public float leftAmount, rightAmount;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            leftAmount += 0.0001f;
        }
        else
        {
            leftAmount -= 0.0005f;
            if (leftAmount <= 0f) leftAmount = 0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rightAmount += 0.0001f;
        }
        else
        {
            rightAmount -= 0.0005f;
            if (rightAmount <= 0f) rightAmount = 0f;

        }

    }
}
