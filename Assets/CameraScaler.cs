using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Camera _cam;
    [SerializeField]
    private Vector2 targetAspectRatio = new Vector2(16f, 9f);
    
    private void Awake()
    {
        _cam = Camera.main;

        // Рассчитываем целевое соотношение сторон
        float targetAspect = targetAspectRatio.x / targetAspectRatio.y;

        // Текущее соотношение сторон экрана
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Вычисляем масштаб по вертикали
        float scaleHeight = windowAspect / targetAspect;

        // Если текущая ширина меньше желаемой, корректируем размер камеры
        if (scaleHeight < 1.0f)
        {  
            _cam.orthographicSize = _cam.orthographicSize / scaleHeight;
        }
    }
}