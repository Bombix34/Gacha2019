using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[ExecuteInEditMode]
public class PlanetEditor : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    private Transform m_Parent;

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
        Event e = Event.current;

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

            e.Use();
        }
    }
}

#endif