using UnityEngine;
using UnityEditor;

public static class AutoUIAnchor
{
    [MenuItem("Simple UI/Auto UI Anchor _F1")]
    static void _AutoUIAnchor()
    {
        if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<RectTransform>())
        {
            GameObject targetObject = Selection.activeGameObject;
            RectTransform targetObjectRT = targetObject.GetComponent<RectTransform>();
            RectTransform parentTransform = targetObject.transform.parent.GetComponent<RectTransform>();

            Vector2 offsetMin = targetObjectRT.offsetMin;
            Vector2 offsetMax = targetObjectRT.offsetMax;
            Vector2 anchorMin = targetObjectRT.anchorMin;
            Vector2 anchorMax = targetObjectRT.anchorMax;
            Vector2 parent_scale = new Vector2(parentTransform.rect.width, parentTransform.rect.height);
            targetObjectRT.anchorMin = new Vector2(anchorMin.x + (offsetMin.x / parent_scale.x), anchorMin.y + (offsetMin.y / parent_scale.y));
            targetObjectRT.anchorMax = new Vector2(anchorMax.x + (offsetMax.x / parent_scale.x), anchorMax.y + (offsetMax.y / parent_scale.y));
            targetObjectRT.offsetMin = Vector2.zero;
            targetObjectRT.offsetMax = Vector2.zero;
            Debug.Log("It has been set");
        }
    }
}
