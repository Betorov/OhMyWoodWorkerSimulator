using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;



public class ScriptsMove : MonoBehaviour
{
    
    public Vector3 Size = new Vector3(60, 20, 200);
    public float DeltaSize = 1f;

    public GameObject _plane, _wood, _woodInternal, SpawnPoint;
    public int SubdivideLevel = 48;

    private MeshFilter meshFilterExternal, meshFilterInternal;
    private Vector3[] verticesExternal, verticesInternal;

    private float i = 0;
    private float step = 0.5f;

    private Vector3 startVector;

    public void Awake()
    {
        Size = _wood.transform.localScale;
        //_woodInternal.transform.localScale = Size * (1 - DeltaSize);
        

        //_woodInternal.transform.position = _wood.transform.position;

        meshFilterExternal = _wood.GetComponent<MeshFilter>();
       
        //meshFilterInternal = _woodInternal.GetComponent<MeshFilter>();
     
        //MeshHelper.Subdivide(meshFilterInternal.mesh, SubdivideLevel);

        verticesExternal = meshFilterExternal.mesh.vertices;
        //verticesInternal = meshFilterInternal.mesh.vertices;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("AARGGGH");

      

    }

    // Update is called once per frame
    public void Update ()
    {
        //TODO: this
        var auto = _plane.transform.localPosition;
        auto.y -= 0.1f;

      

        _plane.transform.localPosition = auto;     
           



        for (int i = 0; i < verticesExternal.Length; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(_wood.transform.TransformPoint(verticesExternal[i]) + Vector3.down, Vector3.up);
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
               verticesExternal[i] = _wood.transform.InverseTransformPoint(hit.point + Vector3.down * DeltaSize);
            }
        }
        meshFilterExternal.mesh.vertices = verticesExternal;
       /* for (int i = 0; i < verticesInternal.Length; i++)
        {
            RaycastHit hit;
            Ray ray = new Ray(_woodInternal.transform.TransformPoint(verticesInternal[i]) + Vector3.down, Vector3.up);
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                verticesInternal[i] = _woodInternal.transform.InverseTransformPoint(hit.point);
            }
        }
        meshFilterInternal.mesh.vertices = verticesInternal;*/
    }


}
