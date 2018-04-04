using Assets.Code.MoveLogic;
using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace Assets.Code
{

    public class PlaneMove : MonoBehaviour
    {
        private PlaneLogic _planeLogic;

        //
        //
        //

        // Список точек краев бруска
        private Vector3[] points;
        // Now point
        private int currentPointIndex = 0;

        // максимальная глубина резки
        private  float MaxCutDepth = 20.02f;

        // размеры бруска
        private Vector3 BlockSize;

        // разница в резмерах внешнего и внутреннего меша
        private float DeltaSize = 0.001f;
        
        // Нож
        public Collider knifeCollider;

        // Внешние и внутреннии меши бруска
        public MeshFilter meshOutside, meshInside;
        public Material material;


        public int SubdivideLevel = 50;
    
        // массивы вершин
        private Vector3[] verticesOrigin, verticesOutside, verticesInside;
        private float groundLevelOutside = 1;

        //
        //
        //
        public GameObject plane;
        public GameObject wood;

        Exchanger _exchanger;
        

        public void Awake()
        {
            BlockSize = wood.transform.localScale;

            meshOutside.transform.localScale = BlockSize;
            meshInside.transform.localScale = BlockSize * (1.0f - DeltaSize);


            MeshHelper.Subdivide(meshOutside.mesh, SubdivideLevel);
            MeshHelper.Subdivide(meshInside.mesh, SubdivideLevel);

            verticesOrigin = meshOutside.mesh.vertices;
            verticesOutside = verticesOrigin;
            verticesInside = meshInside.mesh.vertices;

            // Create moving logic
            _planeLogic = new PlaneLogic(
                plane.transform.position,
                wood.transform.localScale.x,
                wood.transform.localScale.z);
        }

        public void Start()
        {          
            
        }   

        public void Update()
        {

            //Двигаем нож
            plane.transform.position = Vector3.MoveTowards(
                transform.position,
                _planeLogic.nextPointPlane(transform.position), 
                _planeLogic.Speed);

            updateInteraction();
        }

        public void updateInteraction()
        {
            var knifeBounds = Extensions.InverseTransformBounds(meshInside.transform, knifeCollider.bounds);
            var knifeBottom = meshInside.
                transform.
                InverseTransformPoint(knifeCollider.transform.position - knifeCollider.bounds.extents).
                y;
            var maxCut = MaxCutDepth / meshInside.transform.localScale.y;

            var cores = SystemInfo.processorCount;
            var verticesPerCore = (verticesOutside.Length - 1) / cores + 1;

            // параллельно рассчитываем пересечения ножа с вершинами и корректируем меш
            bool changed = false;

            Parallel.For(cores, (core) =>
            {
                var start = core * verticesPerCore;
                var end = Mathf.Min((core + 1) * verticesPerCore, verticesOutside.Length);
                for (int i = start; i < end; i++)
                    // Если размер вертиксов ножа совпадает с вершинами куба то вырезаем
                    if (knifeBounds.Contains(verticesOutside[i]))
                    {
                        // проверяем не превышается ли максимальная глубина резки
                        if (verticesInside[i].y - knifeBottom <= maxCut)
                        {
                            verticesInside[i].y = knifeBottom;
                            verticesOutside[i].y -= groundLevelOutside;

                            changed = true;
                        }
                        else
                        {
                            Debug.Log("Error " + (verticesInside[i].y - knifeBottom) + " " + maxCut);
                            break;
                        }
                    }
            });

            // Обновляем картину если что-то поменялось
            if (changed)
            {
                meshInside.mesh.vertices = verticesInside;
                meshOutside.mesh.vertices = verticesOutside;
            }
        }
        

    }
}
