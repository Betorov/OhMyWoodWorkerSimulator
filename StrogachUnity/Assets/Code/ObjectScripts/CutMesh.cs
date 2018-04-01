using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code
{
    class CutMesh : MonoBehaviour
    {
        public GameObject Terrain;

        public void Start()
        {
           
        }

        public void deleteTri(int index)
        {
            Destroy(Terrain.GetComponent<MeshCollider>());

            Mesh mesh = transform.GetComponent<MeshFilter>().mesh;
            int[] oldTriangles = mesh.triangles;
            int[] newTriangles = new int[mesh.triangles.Length - 3];

            int i = 0;
            int j = 0;
            while(j < mesh.triangles.Length)
            {
                if (j != index * 3)
                {
                    newTriangles[i++] = oldTriangles[j++];
                    newTriangles[i++] = oldTriangles[j++];
                    newTriangles[i++] = oldTriangles[j++];
                }
                else
                    j += 3;
            }

            transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;

            Terrain.AddComponent<MeshCollider>();
        }

        public void Update()
        {
            
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Enter cut");
                RaycastHit hit;

                var vector3 = Input.mousePosition;

                var cameras = Camera.allCameras;

                Debug.Log(vector3.ToString());

                Ray ray = new Ray(Terrain.transform.TransformPoint(new Vector3(170, 10, 180)), Vector3.up);
                if (Physics.Raycast(ray, out hit, 1.0f))
                {
                    //Destroy(hit.collider);
                    deleteTri(hit.triangleIndex);
                }
            }
        }
    }
}
