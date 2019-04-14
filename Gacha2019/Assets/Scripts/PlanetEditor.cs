using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[ExecuteInEditMode]
public class PlanetEditor : MonoBehaviour
{
    [HideInInspector]
    public GameObject prefab;


    [HideInInspector]
    public Transform m_Parent;
    [HideInInspector]
    public bool EditModeActive;
    [HideInInspector]
    public bool RotationActivated;

    public float speed;


    Vector2 mouseDelta;

    private void OnEnable()
    {
        if (!Application.isEditor)
        {
            Destroy(this);
        }

        SceneView.onSceneGUIDelegate += OnScene;

        m_Parent = SceneView.FindObjectOfType<Planet>().transform.GetChild(0);
    }


    private void Awake()
    {
        m_Parent = SceneView.FindObjectOfType<Planet>().transform.GetChild(0);

    }

    

    void OnScene(SceneView scene)
    {



        if (!m_Parent) m_Parent = SceneView.FindObjectOfType<Planet>().transform.GetChild(0);

        if (!EditModeActive) return;

        if (RotationActivated)
        { 
           
            m_Parent.Rotate(new Vector3(0, -mouseDelta.x, -mouseDelta.y) * Time.deltaTime * speed, Space.World);

            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        else
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));

        Event e = Event.current;

        mouseDelta = e.delta;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            

            Vector3 mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            

            RaycastHit hit;
            Ray ray = scene.camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                //GameObject go = Instantiate(prefab, m_Parent);

                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, m_Parent);
                Undo.RegisterCreatedObjectUndo(go, "Created go");

                go.transform.localScale = Vector3.one;
                go.transform.position = hit.point;
                go.transform.up = go.transform.position.normalized;
            }
            else
            {

                RotationActivated = true;
            }

            e.Use();
        }
        else if(e.type == EventType.MouseUp && e.button == 0)
        {
            RotationActivated = false;

        }

        if (e.type == EventType.MouseDown && e.button == 1)
        {


            Vector3 mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;



            RaycastHit hit;
            Ray ray = scene.camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("HIT" + hit.transform.name);

                //GameObject go = Instantiate(prefab, m_Parent);
                /*
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, m_Parent);
                Undo.RegisterCreatedObjectUndo(go, "Created go");

                go.transform.localScale = Vector3.one;
                go.transform.position = hit.point;
                go.transform.up = go.transform.position.normalized;*/
            }
           

            e.Use();
        }

    }
}

#endif