using UnityEngine;

public class EditorModeCheck : MonoBehaviour
{
    public static bool isEditorMode = true;

    private void Awake()
    {
        isEditorMode = false;
        Debug.Log("������ ����");
    }

    private void OnApplicationQuit()
    {
        isEditorMode = true;
        Debug.Log("������ ����");
    }
}
