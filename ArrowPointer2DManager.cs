using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ArrowPointer2DManager : MonoBehaviour
{
    public Camera playerCamera;                    // Assign the VR headset camera
    public RectTransform canvasRect;              // Assign your UI Canvas (Screen Space - Overlay or Camera)
    public GameObject arrowUIPrefab;              // UI arrow prefab (Image with RectTransform)
    public int maxArrows = 3;                     // Number of max arrows

    private GameObject[] arrowUIs;

    void Start()
    {
        arrowUIs = new GameObject[maxArrows];
        for (int i = 0; i < maxArrows; i++)
        {
            arrowUIs[i] = Instantiate(arrowUIPrefab, canvasRect);
            arrowUIs[i].SetActive(false);
        }
    }

    void Update()
    {
        var monsters = GameObject.FindGameObjectsWithTag("EquationMonster");
        if (monsters.Length == 0) return;

        var closest = monsters
            .OrderBy(m => Vector3.Distance(playerCamera.transform.position, m.transform.position))
            .Take(maxArrows)
            .ToArray();

        int arrowIndex = 0;
        foreach (var monster in closest)
        {
            Vector3 viewportPos = playerCamera.WorldToViewportPoint(monster.transform.position);
            bool isVisible = viewportPos.z > 0 && viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1;

            if (isVisible)
            {
                arrowUIs[arrowIndex].SetActive(false);
                continue;
            }

            arrowUIs[arrowIndex].SetActive(true);

            // Calculate screen direction from center
            Vector3 screenPos = playerCamera.WorldToScreenPoint(monster.transform.position);
            Vector2 dirFromCenter = ((Vector2)screenPos - new Vector2(Screen.width / 2f, Screen.height / 2f)).normalized;

            // Place arrow near screen edge (with padding)
            float edgePadding = 80f;
            Vector2 canvasCenter = canvasRect.sizeDelta / 2f;
            Vector2 arrowPos = dirFromCenter * ((Mathf.Min(canvasRect.sizeDelta.x, canvasRect.sizeDelta.y) / 2f) - edgePadding);
            RectTransform arrowRect = arrowUIs[arrowIndex].GetComponent<RectTransform>();
            arrowRect.anchoredPosition = arrowPos;

            // Rotate arrow to point in direction (remember: image points left by default, so rotate +180)
            float angle = Mathf.Atan2(dirFromCenter.y, dirFromCenter.x) * Mathf.Rad2Deg + 180f;
            arrowRect.rotation = Quaternion.Euler(0, 0, angle);

            arrowIndex++;
        }

        // Hide extra arrows
        for (int i = arrowIndex; i < maxArrows; i++)
        {
            arrowUIs[i].SetActive(false);
        }
    }
}
