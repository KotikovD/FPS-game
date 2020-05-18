using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


namespace ObjectPlacer
{
    public class ObjectPlacer : EditorWindow
    {


        #region Fields

        const int MINVALUE = 1;
        const int MAXOBJECTS = 5;
        const int MAXLONGSIDEOFNAVMESH = 200;
        const int MAXCURRENTOBJECTS = 50;
        private int _totalObjectsCount = 1;
        private int _maxDistance = 50;

        private GameObject _parentGO;
        private bool _randomRotation = true;
        private Vector2 scrollPosition = Vector2.zero;

        private List<DataBlocks> _dataBlocks = new List<DataBlocks>();
        private List<GameObject> _objects = new List<GameObject>();

        #endregion


        #region DataBlocksClass

        [Serializable]
        public class DataBlocks
        {
            public GameObject GameObj;
            public int How;
        }

        #endregion


        #region Window

        [MenuItem("Tools/Object Placer")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ObjectPlacer), false, "Object Placer");
        }

        private void OnGUI()
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            GUILayout.Space(20);
            GUILayout.Label("Total objects", EditorStyles.boldLabel);
            _totalObjectsCount = EditorGUILayout.IntSlider(_totalObjectsCount, MINVALUE, MAXOBJECTS);
            GUILayout.Label("Longest side of NavMesh (approx)", EditorStyles.boldLabel);
            _maxDistance = EditorGUILayout.IntSlider(_maxDistance, MINVALUE, MAXLONGSIDEOFNAVMESH);
            _randomRotation = EditorGUILayout.Toggle($"Y random rotation", _randomRotation);
            GUILayout.Space(20);

            for (int i = 0; i < _totalObjectsCount; i++)
            {
                _dataBlocks.Add(new DataBlocks());
                GUILayout.BeginVertical("Box");
                _dataBlocks[i].GameObj = EditorGUILayout.ObjectField("Game object", _dataBlocks[i].GameObj, typeof(GameObject), true) as GameObject;
                _dataBlocks[i].How = EditorGUILayout.IntSlider("Current object count", _dataBlocks[i].How, 0, MAXCURRENTOBJECTS);
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Place"))
                CreateObjects();

            GUILayout.Space(10);
            if (GUILayout.Button("Remove"))
                RemoveAllObjects();

            EditorGUILayout.EndScrollView();
        }

        #endregion


        #region Metodths

        private void CreateObjects()
        {
            _parentGO = GameObject.Find("CreatedObjects");
            if (_parentGO == null)
                _parentGO = new GameObject { name = "CreatedObjects" };

            for (int i = 0; i < _totalObjectsCount; i++)
            {
                var block = _dataBlocks[i];
                BoxCollider pastGameObject = default;

                for (int b = 0; b < block.How;)
                {
                    Vector3 point = pastGameObject == null ? default : pastGameObject.transform.position;
                    var randomPoint = point + UnityEngine.Random.insideUnitSphere * _maxDistance;
                    
                    Quaternion rot = _randomRotation == false ? Quaternion.identity :
                        new Quaternion(0, UnityEngine.Random.Range(0, 360), 0, UnityEngine.Random.Range(0, 360));

                    if (!NavMesh.SamplePosition(randomPoint, out var hit, 1f, NavMesh.AllAreas))
                        continue;


                    //var buildSettings = NavMesh.GetSettingsCount();
                    //for (int s = 0; s <= buildSettings; ++s)
                    //{
                    //    // Set Agent Types Height
                    //    Debug.Log("tileSize " + NavMesh.GetSettingsByID(s).tileSize);
                    //    Debug.Log("voxelSize " + NavMesh.GetSettingsByID(s).voxelSize);
                    //    Debug.Log("voxelSize " + NavMesh.GetSettingsByID(s).a);
                    //    // Note there is a Name Field
                    //    // NavMesh.GetSettingsNameFromID(i);
                    //}
                    //NavMeshData.FindObjectOfType<PatchExtents>();
                    //CorrectPosition(block.GameObj, hit.position, out Vector3 newPosition, out float minDistance);

                    var bc = block.GameObj.GetComponent<BoxCollider>();
                    if (bc == null)
                        bc = block.GameObj.AddComponent<BoxCollider>();
                    var newPosition = hit.position - bc.size / 2;
                    newPosition.y = hit.position.y;

                    
                    var canInstantiate = false;
                    if (_objects.Count > 0)
                        foreach (var go in _objects)
                        {
                            var currentDistance = Vector3.Distance(go.transform.position, newPosition);
                            var minDistance = bc.size / 2 + pastGameObject.size / 2;

                            Debug.Log("minDistance " + minDistance);
                            Debug.Log("currentDistance " + currentDistance);

                           // var minDistance = 2.0f; //_maxDistance / 4;

                            if (minDistance.x < currentDistance && minDistance.y < currentDistance)
                            {
                                
                                pastGameObject = bc;
                                canInstantiate = true;
                            } 
                            else
                                continue;
                        }
                    else
                    {
                        pastGameObject = bc;
                        canInstantiate = true;
                    }

                    // Почему-то спавнится только первый блок объектов
                  //  DestroyImmediate(bc, true);

                    if (canInstantiate)
                    {
                        var obj = Instantiate(block.GameObj, newPosition, rot) as GameObject;
                        obj.name = block.GameObj.name;
                        obj.transform.parent = _parentGO.transform;
                        _objects.Add(obj);
                        b++;
                    }
                }
            }
        }

        

        /// <summary>
        /// Corrects the position of objects to the center of their Collider
        /// </summary>
        private void CorrectPosition(GameObject gameObj, Vector3 hit, out Vector3 newPosition, out float iqwe)
        {
            var bc = gameObj.GetComponent<BoxCollider>();
            if (bc == null)
                bc = gameObj.AddComponent<BoxCollider>();
            newPosition = hit - bc.size / 2;
            DestroyImmediate(bc, true);
            newPosition.y = hit.y;
            iqwe = newPosition.y;
        }

        //private float GetDistance(Vector3 position, Vector3 newPosition)
        //{
        //    return Mathf.Sqrt(Mathf.Pow(position.x - newPosition.x, 2) + Mathf.Pow(position.y - newPosition.y, 2));
        //}

        private void RemoveAllObjects()
        {
            DestroyImmediate(GameObject.Find("CreatedObjects"));
            _objects.Clear();
        }

        #endregion


    }


}