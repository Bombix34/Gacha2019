using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[ExecuteInEditMode]
public class PlanetEditor : MonoBehaviour
{
    public GameObject prefab;

    private Transform m_Parent;
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

        m_Parent = GameObject.Find("AutoRotation").transform;
    }

    void OnScene(SceneView scene)
    {
        if (!EditModeActive) return;


        if(!m_Parent) m_Parent = GameObject.Find("AutoRotation").transform;

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
                GameObject go = Instantiate(prefab, m_Parent);
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
        
    }
}

#endif