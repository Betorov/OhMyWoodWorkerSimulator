using Assets.Code.MoveLogic;
using Strogach.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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

        public Text coordinatText;

        public int SubdivideLevel = 64;// 50
    
        // массивы вершин
        private Vector3[] verticesOrigin, verticesOutside, verticesInside;
        private float groundLevelOutside = 1;

        //
        //
        //
        public GameObject plane;
        public GameObject wood;
        

        public void Awake()
        {
            
        }

        public void Start()
        {
            BlockSize = wood.transform.localScale;

            meshOutside.transform.localScale = BlockSize;
            meshInside.transform.localScale = BlockSize * (1.0f - DeltaSize);

            MeshHelper.Subdivide(meshOutside.mesh, SubdivideLevel);
            MeshHelper.Subdivide(meshInside.mesh, SubdivideLevel);

            verticesOrigin = meshOutside.mesh.vertices;
            verticesOutside = verticesOrigin;
            verticesInside = meshInside.mesh.vertices;
            

            ExchangeContext.Speed = 35.0f;
            // Create moving logic
            _planeLogic = new PlaneLogic(
                plane.transform.position,
                wood.transform.localScale.x,
                wood.transform.localScale.z);
        }   

        public void Update()
        {

            var vector = plane.transform.position;

            vector.x += 1f;
            vector.y -= 1f;

            coordinatText.text = 
                "Coordinate plane Now X: " + 
                plane.transform.position.x + 
                " And Y: " + 
                plane.transform.position.z;

            // _planeLogic.nextPointPlaneFor(plane);
            //plane.transform.position = _planeLogic.nextPointPlane(plane.transform.position);
            //Двигаем нож
           
            plane.transform.position = Vector3.MoveTowards(
                transform.position,
                //vector,
                _planeLogic.nextPointPlane(transform.position, plane, wood), 
                ExchangeContext.Speed * Time.deltaTime);

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
