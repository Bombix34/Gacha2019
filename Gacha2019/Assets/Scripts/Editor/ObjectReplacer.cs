using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

public class ObjectReplacer : EditorWindow
{
    string name = null;
    private GameObject prefab = null;

    [MenuItem("Window/ObjectReplacer")]
    public static void ShowWindow()
    {
        GetWindow<ObjectReplacer>();
    }

    public void OnGUI()
    {
        name = GUILayout.TextField(name);
        prefab = EditorGUILayout.ObjectField(prefab, typeof(GameObject), true) as GameObject;

        if (prefab != null && GUILayout.Button("Apply"))
        {
            Apply(name);
        }
    }

    private void Apply(string _name)
    {
        List<Scene> scenes = new List<Scene>();
        GameObject[] gameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null
                && gameObject.name.ToLower().Contains(_name.ToLower())
                && gameObject.activeSelf)
            {
                if (!scenes.Contains(gameObject.scene))
                {
                    scenes.Add(gameObject.scene);
                }

                GameObject.Instantiate(
                    prefab,
                    gameObject.transform.position,
                    gameObject.transform.rotation,
                    gameObject.transform.parent);

                DestroyImmediate(gameObject);
            }
        }

        foreach (Scene scene in scenes)
        {
            EditorSceneManager.MarkSceneDirty(scene);
        }
    }
}