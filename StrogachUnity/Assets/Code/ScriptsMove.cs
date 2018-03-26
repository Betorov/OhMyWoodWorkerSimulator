using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ScriptsMove : MonoBehaviour
{
    //Logic to remove this

    public GameObject _plane;
    public GameObject _wood;

    private float i = 0;

    private Vector3 startVector;
	// Use this for initialization
	void Start ()
    {
        startVector = _plane.transform.position;

        
	}
	
	// Update is called once per frame
	void Update ()
    {
        _plane.transform.position = new Vector3(startVector.x, startVector.y - i, startVector.z + i);

        Mesh mesh = new Mesh();



        i += 1f;
    }
}
