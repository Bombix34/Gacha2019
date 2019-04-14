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

    private void Awake()
    {
        GetAssets();
    }

    bool showInfo;


    GameObject[] AllLevels;
    GUIContent[] LevelsPreviews;
    int SelectedLevelIndex;
    int OldSelectedLevelIndex;

    GameObject[] AllPrefabs;
    Texture[] Previews;
    int selectedObjectIndex;

    GameObject selectedObject;
    
    bool _editMode;

    string MainPath = "Assets/Resources/Levels/";

    Vector2 scrollPos;

    PlanetEditor plEdit;



    public void OnGUI()
    {

        PlanetEditorFinder();

          scrollPos = GUILayout.BeginScrollView(scrollPos);

        Instructions();

        

        if(GUILayout.Button("REFRESH ASSETS"))
        {
            GetAssets();
        }


        LevelsManaging();


        if (!_editMode)
            GUI.color = Color.gray;
        else
            GUI.color = Color.green;

       

        if(GUILayout.Button("EDIT MODE",GUILayout.Height(50)))
        {
            _editMode = !_editMode;
            plEdit.EditModeActive = _editMode;
        }
        SaveLevel();

        GUI.color = Color.white;

        DrawButtons();


        GUILayout.EndScrollView();
        
    }


    void PlanetEditorFinder()
    {
        if (!plEdit)
        {

            plEdit = FindObjectOfType<PlanetEditor>();
            /*
            if (!plEdit)
            {
                new GameObject("Planet Editor").AddComponent<PlanetEditor>();
                plEdit = FindObjectOfType<PlanetEditor>();
                plEdit.speed = 4.5f;
                //plEdit.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
            */
            plEdit.speed = 4.5f;


        }


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

        AllLevels = Resources.LoadAll<GameObject>("Levels");

        LevelsPreviews = new GUIContent[AllLevels.Length];

        for (int i = 0; i < AllLevels.Length; i++)
        {
            LevelsPreviews[i] = new GUIContent(AllLevels[i].name,AssetPreview.GetAssetPreview(AllLevels[i]));
        }

    }


    void LevelsManaging()
    {
       if(GUILayout.Button("Add level"))
        {
            GameObject o = new GameObject();
            CreateNew(o, MainPath + "Level" + (AllLevels.Length + 1) + ".prefab");
            DestroyImmediate(o);
        }




        SelectedLevelIndex = GUILayout.SelectionGrid(SelectedLevelIndex, LevelsPreviews, 3);

        if(SelectedLevelIndex != OldSelectedLevelIndex)
        {
            // Save le level actuel
            CreateNew(plEdit.m_Parent.gameObject, MainPath + AllLevels[OldSelectedLevelIndex].name + ".prefab");

            // Récup le parent
            Transform tempParent = plEdit.m_Parent.parent;


            // Remove le niveau actuel
            DestroyImmediate(plEdit.m_Parent.gameObject);

            //Instancie le nouveau niveau

            GameObject g = (GameObject)PrefabUtility.InstantiatePrefab(AllLevels[SelectedLevelIndex], tempParent);
            plEdit.m_Parent = g.transform;
            g.transform.localPosition = Vector3.zero;

            GetAssets();
        }

       


        OldSelectedLevelIndex = SelectedLevelIndex;

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
            GUILayout.Label("Save sauvegarde l'objet en prefab");

            GUILayout.Space(5);

            GUILayout.Label("Si jamais ca bug, relancer la fenetre");

            GUILayout.Space(20);


        }



    }

    void DrawButtons()
    {
        
        selectedObjectIndex = GUILayout.SelectionGrid(selectedObjectIndex,Previews,3);
        selectedObject = AllPrefabs[selectedObjectIndex];

        if (plEdit)
            plEdit.prefab = selectedObject;
    }


    // Sauvegarde le niveau actuel
    void SaveLevel()
    {
        GUI.color = Color.cyan;
        if(GUILayout.Button("SAVE",GUILayout.Height(38)))
        {
            CreateNew(plEdit.m_Parent.gameObject, MainPath + AllLevels[SelectedLevelIndex].name + ".prefab");

        }



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

    static void CreateNew(GameObject obj, string localPath)
    {
        //Create a new Prefab at the path given

        
        //GameObject prefab = PrefabUtility.SaveAsPrefabAsset(obj,localPath);
        PrefabUtility.SaveAsPrefabAssetAndConnect(obj, localPath,InteractionMode.UserAction);
    }

    private void Update()
    {
        plEdit.m_Parent = FindObjectOfType<Planet>().transform.GetChild(0);
    }


}
