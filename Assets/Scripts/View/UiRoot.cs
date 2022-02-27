using UnityEngine;

public sealed class UiRoot : MonoBehaviour
{
    [SerializeField] RectTransform rf;
    Rect safeArea;
    Resolution resolution;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
#else
        safeArea = Screen.safeArea;
        resolution = Screen.currentResolution;
        rf.anchorMin = new Vector2(safeArea.xMin / resolution.width, safeArea.yMin / resolution.height);
        rf.anchorMax = new Vector2(safeArea.xMax / resolution.width, safeArea.yMax / resolution.height);
#endif
    }
}
