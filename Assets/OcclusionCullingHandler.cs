using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OcclusionCullingHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] cullingObjects = new GameObject[0];

    [System.Serializable]
    internal class CullingObjectSettings
    {
        [SerializeField, HideInInspector] internal string objectName;
        [SerializeField, HideInInspector] internal GameObject targetObject;
        [SerializeField, HideInInspector] internal Renderer objectRenderer;

        [SerializeField, HideInInspector] internal Vector2 dimensions, objectCenter, topRightCorner, topLeftCorner, bottomLeftCorner, bottomRightCorner;
        [SerializeField, HideInInspector] internal float rightEdge, leftEdge, topEdge, bottomEdge;

        [SerializeField] internal Vector2 additionalSize = Vector2.zero;
        [SerializeField] internal bool displayBorders = true;
        [SerializeField] internal Color borderColor = Color.white;
        [Header("'Is Static' Prevents Real-Time Updates to Improve Performance")]
        [SerializeField] internal bool isStatic = true;
    }

    [SerializeField] private CullingObjectSettings[] cullingObjectSettings = new CullingObjectSettings[1];

    [SerializeField] float updateInterval = 0.1f;

    [SerializeField] private Vector2 globalAdditionalSize = Vector2.zero;

    [Space, SerializeField] private bool applyGlobalSettings = true;
    [SerializeField, HideInInspector] private bool globalDisplayBorders = true;
    [SerializeField, HideInInspector] private Color globalBorderColor = Color.white;
    [Header("'Is Static' Prevents Real-Time Updates to Improve Performance")]
    [SerializeField, HideInInspector] private bool globalIsStatic = true;

    private float elapsedTime;

    private new Camera mainCamera;

    [SerializeField, HideInInspector] private bool settingsInitialized;
    private bool settingsApplied = true;

    private Bounds CalculateCombinedBounds(GameObject parent)
    {

        Vector3 absScale = new Vector3(Mathf.Abs(parent.transform.localScale.x), Mathf.Abs(parent.transform.localScale.y), 0);
        Bounds combinedBounds = new Bounds(parent.transform.position, absScale);

        Renderer[] renderers = parent.GetComponentsInChildren<Renderer>(GetComponent<Renderer>());

        foreach (Renderer rendererChild in renderers)
        {

            if (combinedBounds.size == absScale)
            {
                combinedBounds = rendererChild.bounds;
            }

            combinedBounds.Encapsulate(rendererChild.bounds);
        }

        return combinedBounds;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(OcclusionCullingHandler))]
    private class OcclusionCullingHandlerEditor : Editor
    {
        private OcclusionCullingHandler scriptReference;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(scriptReference == null){
                scriptReference = (OcclusionCullingHandler)target;
                return;
            }

            if(scriptReference.applyGlobalSettings){
                EditorGUILayout.PropertyField(serializedObject.FindProperty("globalDisplayBorders"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("globalBorderColor"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("globalIsStatic"));
                if(GUILayout.Button("Apply Global Settings")){
                    scriptReference.settingsApplied = false;
                    if(!scriptReference.settingsApplied){
                        foreach(CullingObjectSettings setting in scriptReference.cullingObjectSettings){
                            setting.displayBorders = scriptReference.globalDisplayBorders;
                            setting.borderColor = scriptReference.globalBorderColor;
                            setting.isStatic = scriptReference.globalIsStatic;

                            if(setting.targetObject == scriptReference.cullingObjectSettings[scriptReference.cullingObjectSettings.Length - 1].targetObject){
                                scriptReference.settingsApplied = true;
                            }
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            if(!Application.isPlaying){
                
                if(scriptReference.cullingObjectSettings.Length != scriptReference.cullingObjects.Length){
                    scriptReference.settingsInitialized = false;
                    scriptReference.cullingObjectSettings = new CullingObjectSettings[scriptReference.cullingObjects.Length];
                    return;
                }

                if(!scriptReference.settingsInitialized){

                    for(int i = 0; i < scriptReference.cullingObjectSettings.Length; i++){
                        scriptReference.cullingObjectSettings[i].targetObject = scriptReference.cullingObjects[i];
                        scriptReference.cullingObjectSettings[i].objectRenderer = scriptReference.cullingObjects[i].GetComponent<Renderer>();

                        if(i == scriptReference.cullingObjectSettings.Length - 1)
                            scriptReference.settingsInitialized = true;
                    }
                }
            }
        }

        public void OnSceneGUI(){

            if(scriptReference == null){
                scriptReference = (OcclusionCullingHandler)target;
                return;
            }
            
            if(scriptReference.settingsInitialized){

                foreach(CullingObjectSettings setting in scriptReference.cullingObjectSettings)
                {
                    if(setting.targetObject)
                    {
                        setting.objectName = setting.targetObject.name;

                        if(Selection.activeGameObject == scriptReference.gameObject | !Application.isPlaying){

                            Vector2 extraSize = scriptReference.globalAdditionalSize + setting.additionalSize;
                            Bounds bounds = scriptReference.CalculateCombinedBounds(setting.targetObject);
                            setting.dimensions = (Vector2)bounds.extents + extraSize;
                            setting.objectCenter = bounds.center;

                            setting.topRightCorner = new Vector2(setting.objectCenter.x + setting.dimensions.x, setting.objectCenter.y + setting.dimensions.y);
                            setting.topLeftCorner = new Vector2(setting.objectCenter.x - setting.dimensions.x, setting.objectCenter.y + setting.dimensions.y);
                            setting.bottomLeftCorner = new Vector2(setting.objectCenter.x - setting.dimensions.x, setting.objectCenter.y - setting.dimensions.y);
                            setting.bottomRightCorner = new Vector2(setting.objectCenter.x + setting.dimensions.x, setting.objectCenter.y - setting.dimensions.y);

                            setting.rightEdge = setting.objectCenter.x + setting.dimensions.x;
                            setting.leftEdge = setting.objectCenter.x - setting.dimensions.x;
                            setting.topEdge = setting.objectCenter.y + setting.dimensions.y;
                            setting.bottomEdge = setting.objectCenter.y - setting.dimensions.y;

                            bool shouldDisplayBorders = scriptReference.applyGlobalSettings ? scriptReference.globalDisplayBorders : setting.displayBorders;

                            if(shouldDisplayBorders)
                            {
                                setting.topRightCorner = new Vector2(setting.objectCenter.x + setting.dimensions.x, setting.objectCenter.y + setting.dimensions.y);
                                setting.topLeftCorner = new Vector2(setting.objectCenter.x - setting.dimensions.x, setting.objectCenter.y + setting.dimensions.y);
                                setting.bottomLeftCorner = new Vector2(setting.objectCenter.x - setting.dimensions.x, setting.objectCenter.y - setting.dimensions.y);
                                setting.bottomRightCorner = new Vector2(setting.objectCenter.x + setting.dimensions.x, setting.objectCenter.y - setting.dimensions.y);
                                Handles.color = scriptReference.applyGlobalSettings ? scriptReference.globalBorderColor : setting.borderColor;
                                Handles.DrawLine(setting.topRightCorner, setting.topLeftCorner);
                                Handles.DrawLine(setting.topLeftCorner, setting.bottomLeftCorner);
                                Handles.DrawLine(setting.bottomLeftCorner, setting.bottomRightCorner);
                                Handles.DrawLine(setting.bottomRightCorner, setting.topRightCorner);
                            }

                            bool isStaticObject = scriptReference.applyGlobalSettings ? scriptReference.globalIsStatic : setting.isStatic;
                            setting.targetObject.isStatic = isStaticObject;
                        }
                    }
                }
            }
        }

        
    }
#endif

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > updateInterval) elapsedTime = 0;
        else return;

        float cameraWidth = mainCamera.orthographicSize * ((float)Screen.width / (float)Screen.height);
        float cameraRightEdge = mainCamera.transform.position.x + cameraWidth;
        float cameraLeftEdge = mainCamera.transform.position.x - cameraWidth;
        float cameraTopEdge = mainCamera.transform.position.y + mainCamera.orthographicSize;
        float cameraBottomEdge = mainCamera.transform.position.y - mainCamera.orthographicSize;

        foreach (CullingObjectSettings setting in cullingObjectSettings)
        {
            if (setting.targetObject)
            {
                Rigidbody2D rb2d = setting.targetObject.GetComponent<Rigidbody2D>();
                if (rb2d == null) continue;

                bool isStaticObject = applyGlobalSettings ? globalIsStatic : setting.isStatic;

                if (!isStaticObject)
                {
                    setting.objectCenter = CalculateCombinedBounds(setting.targetObject).center;
                    setting.rightEdge = setting.objectCenter.x + setting.dimensions.x;
                    setting.leftEdge = setting.objectCenter.x - setting.dimensions.x;
                    setting.topEdge = setting.objectCenter.y + setting.dimensions.y;
                    setting.bottomEdge = setting.objectCenter.y - setting.dimensions.y;
                }

                bool isVisibleInCamera = setting.rightEdge >= cameraLeftEdge && setting.leftEdge <= cameraRightEdge &&
                                         setting.topEdge >= cameraBottomEdge && setting.bottomEdge <= cameraTopEdge;

                if (isVisibleInCamera)
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic; // Make Rigidbody2D dynamic
                }
                else
                {
                    rb2d.bodyType = RigidbodyType2D.Static; // Make Rigidbody2D static
                }
            }
        }
    }


}
