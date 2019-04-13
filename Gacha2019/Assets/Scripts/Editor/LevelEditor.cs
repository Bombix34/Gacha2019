using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow
{
 


    [MenuItem("Window/LEVEL EDITOR")]
    public static void ShowWindow()
    {

        GetWindow<LevelEditor>();


    }

    bool showInfo;

    GameObject[] AllPrefabs;

    GameObject selectedObject;
    int selectedObjectIndex;
    bool _editMode;
    Texture[] Previews;

    GameObject LevelSphere;


    Vector2 scrollPos;

    PlanetEditor plEdit;


    bool RotatingLevel;


    float RotateSpeed;

    public void OnGUI()
    {

        if (!plEdit)
        {

            plEdit = FindObjectOfType<PlanetEditor>();
            if(!plEdit)
            {
                new GameObject("Planet Editor").AddComponent<PlanetEditor>();
                plEdit = FindObjectOfType<PlanetEditor>();
                //plEdit.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
                



        }
            


       

        scrollPos = GUILayout.BeginScrollView(scrollPos);

        Instructions();

        

        if(GUILayout.Button("REFRESH ASSETS"))
        {
            GetAssets();
        }


        if (!_editMode)
            GUI.color = Color.gray;
        else
            GUI.color = Color.green;

       

        if(GUILayout.Button("EDIT MODE",GUILayout.Height(50)))
        {
            _editMode = !_editMode;
            plEdit.EditModeActive = _editMode;
        }

        GUI.color = Color.white;

        DrawButtons();


        GUILayout.EndScrollView();
        
    }



    void GetAssets()
    {
        AllPrefabs =
        Resources.LoadAll<GameObject>("LevelComponents");

        Previews = new Texture[AllPrefabs.Length];

        for (int i = 0; i < AllPrefabs.Length; i++)
        {
            Previews[i] = AssetPreview.GetAssetPreview(AllPrefabs[i]);
        }

    }

    void Instructions()
    {
        showInfo = EditorGUILayout.Foldout(showInfo, "Instructions");
        if(showInfo)
        {
            GUILayout.Label("Refresh recharge les assets");
            GUILayout.Label("Edit Mode :");
            GUILayout.Label("Vert Quand Actif");
            GUILayout.Label("Quand Actif, permet de placer objets");
            GUILayout.Label("et de drag dans le vide pour rotate");


            GUILayout.Space(25);


        }



    }

    void DrawButtons()
    {
        
        selectedObjectIndex = GUILayout.SelectionGrid(selectedObjectIndex,Previews,3);
        selectedObject = AllPrefabs[selectedObjectIndex];

        if (plEdit)
            plEdit.prefab = selectedObject;
    }

    // Window has been selected
    void OnFocus()
    {
        // Remove delegate listener if it has previously
        // been assigned.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
        // Add (or re-add) the delegate.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    void OnSceneGUI(SceneView sceneView)
    {
        // Do your drawing here using Handles.
        Handles.BeginGUI();
        if (plEdit.EditModeActive)
            GUILayout.Box("EDIT ACTIVE");
            

        // Do your drawing here using GUI.
        Handles.EndGUI();
    }


}
