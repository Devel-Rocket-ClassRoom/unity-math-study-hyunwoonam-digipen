using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class offscreenindicator : MonoBehaviour
{
    private Camera mainCamera;
    public Canvas canvas;
    public RectTransform indicatorPrefab; 
    public Transform[] targets; 

    public float edgeBuffer = 30f;

    private List<RectTransform> indicators = new List<RectTransform>();
    private Vector2 screenCenter;

    void Start()
    {
        mainCamera = Camera.main;

        foreach (var target in targets)
        {
            RectTransform indicator = Instantiate(indicatorPrefab, canvas.transform);
            Image indicatorImage = indicator.GetComponent<Image>();
            indicatorImage.color = target.GetComponent<Renderer>().sharedMaterial.color;
            indicator.gameObject.SetActive(false);
            indicators.Add(indicator);
        }
    }

    void LateUpdate()
    {
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        for (int i = 0; i < targets.Length; i++)
        {
            UpdateIndicator(targets[i], indicators[i]);
        }
    }

    private void UpdateIndicator(Transform target, RectTransform indicator)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        bool isBehindCamera = screenPos.z < 0;

        bool isOnScreen = !isBehindCamera &&
                          screenPos.x >= 0 && screenPos.x <= Screen.width &&
                          screenPos.y >= 0 && screenPos.y <= Screen.height;

        if (isOnScreen)
        {
            indicator.gameObject.SetActive(false);
        }
        else
        {
            indicator.gameObject.SetActive(true);

            Vector2 targetPos2D = new Vector2(screenPos.x, screenPos.y);
            Vector2 direction = targetPos2D - screenCenter;

            if (isBehindCamera)
            {
                direction = -direction;
            }

            direction.Normalize();

            float halfWidth = (Screen.width / 2f) - edgeBuffer;
            float halfHeight = (Screen.height / 2f) - edgeBuffer;

            float tx = Mathf.Abs(direction.x) / halfWidth;
            float ty = Mathf.Abs(direction.y) / halfHeight;

            float maxComponent = Mathf.Max(tx, ty);

            Vector2 clampedPos = direction / maxComponent;

            clampedPos.x = Mathf.Clamp(clampedPos.x, -halfWidth, halfWidth);
            clampedPos.y = Mathf.Clamp(clampedPos.y, -halfHeight, halfHeight);

            indicator.position = screenCenter + clampedPos;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            indicator.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        
    }
}
