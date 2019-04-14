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
    //[HideInInspector]
    public bool PaintModeActive;
    [HideInInspector]
    public bool RotationActivated;

    public float speed;
    [HideInInspector]
    public bool MakeLine;

    Vector2 mouseDelta;

    Vector2 lineStart;

    Vector2 lineEnd;

    public float Step;

    EditorCurve Curve;

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

                if(!PaintModeActive)
                {
                    //GameObject go = Instantiate(prefab, m_Parent);


                    Debug.Log("INSTANCIATE");
                    GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, m_Parent);
                    Undo.RegisterCreatedObjectUndo(go, "Created go");

                    go.transform.localScale = Vector3.one;
                    go.transform.position = hit.point;
                    go.transform.up = go.transform.position.normalized;
                }
                else
                {
                    MakeLine = true;
                    lineStart = e.mousePosition;

                    Curve = new EditorCurve(hit.point, hit.normal);

                    //gameObject.AddComponent<BezierSpline>().AddCurve();
                    
                }
               
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

            if (PaintModeActive)
            {
                Vector3 mousePos = e.mousePosition;
                float ppp = EditorGUIUtility.pixelsPerPoint;
                mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
                mousePos.x *= ppp;



                RaycastHit hit;
                Ray ray = scene.camera.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out hit))
                {
                    Curve.SetEnd(hit.point, hit.normal);
                    Curve.Step = Step;
                    InstanciateLine(Curve.positionsSteped());
                    Curve = null;
                }
                else
                    Curve = null;

                //DestroyImmediate(gameObject.GetComponent<BezierSpline>());
                MakeLine = false;
            }

        }

        

    }

    private void OnDrawGizmos()
    {
        if(Curve != null)
        {
            if(Curve.pinStart != null)
            Gizmos.DrawSphere(Curve.pinStart, 0.5f);
                if(Curve.pinEnd != null)
            Gizmos.DrawSphere(Curve.pinEnd, 0.5f);

            if (Curve.Complete)
            {
                Handles.DrawLine(Curve.pinStart, Curve.pinEnd);

                Gizmos.color = Color.red;

                foreach (Vector3 v in Curve.positionsSteped())
                {
                    Gizmos.DrawSphere(v, 0.5f);
                }
            }
               

        }
       





    }

    public void InstanciateLine(Vector3[] linePositions)
    {
        for (int i = 0; i < linePositions.Length; i++)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, m_Parent);
            Undo.RegisterCreatedObjectUndo(go, "Created go");

            RaycastHit hit;

            if (Physics.Linecast(linePositions[i] * 2, linePositions[i], out hit))
            {
                go.transform.position = hit.point;
            }
            else
                go.transform.position = linePositions[i];



            go.transform.localScale = Vector3.one;
            
            go.transform.up = go.transform.position.normalized;

            //go.transform.LookAt(linePositions[0]);



        }


    }

    


   

}

#endif