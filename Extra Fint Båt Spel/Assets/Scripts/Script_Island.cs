using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Island : MonoBehaviour
{
    Script_GenerateIsland generate;
    // Start is called before the first frame update
    void Start()
    {
        generate = FindObjectOfType<Script_GenerateIsland>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Island")
        {
            generate.islandCurrent--;
            Destroy(gameObject); Debug.Log("died");
        }
    }

}
