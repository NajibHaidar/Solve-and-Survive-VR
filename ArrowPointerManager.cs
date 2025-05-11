using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MonsterArrowUIManager : MonoBehaviour
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

            // Get direction from center of screen
            Vector3 dirToMonster = (monster.transform.position - playerCamera.transform.position).normalized;
            Vector3 flatForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, Vector3.up).normalized;
            Vector3 flatRight = Vector3.Cross(Vector3.up, flatForward);

            float horizontal = Vector3.Dot(flatRight, dirToMonster);
            float vertical = Vector3.Dot(Vector3.up, dirToMonster);

            float angle = Mathf.Atan2(horizontal, Vector3.Dot(flatForward, dirToMonster)) * Mathf.Rad2Deg;
            RectTransform arrowRect = arrowUIs[arrowIndex].GetComponent<RectTransform>();

            // Position arrow at screen edge
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 dir = new Vector2(viewportPos.x - 0.5f, viewportPos.y - 0.5f);
            dir.Normalize();

            float edgeBuffer = 80f; // keep arrows inside screen edges
            Vector2 canvasSize = canvasRect.sizeDelta;
            Vector2 edgePos = screenCenter + dir * (Mathf.Min(canvasSize.x, canvasSize.y) / 2f - edgeBuffer);

            arrowRect.anchoredPosition = edgePos;
            arrowRect.rotation = Quaternion.Euler(0, 0, angle);

            arrowIndex++;
        }

        // Hide any unused arrows
        for (int i = arrowIndex; i < maxArrows; i++)
        {
            arrowUIs[i].SetActive(false);
        }
    }
}