using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagerScript : MonoBehaviour
{
    public int tens;
    public int twenties;
    public int fifties;
    public int ones;
    public int twos;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);

        // Singleton 
        // if there is more than one of this type of object, destroy it
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
