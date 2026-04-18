using UnityEngine;

public class SonarController : MonoBehaviour
{
    [SerializeField] private Material sonarMat;
    [SerializeField] private float maxRadius = 50f;
    [SerializeField] private float speed = 15f;

    private float currentRadius;
    private bool isPinging;

    void Update()
    {
        if (sonarMat == null) return;

        if (Input.GetKeyDown(KeyCode.Z) && !isPinging)
        {
            isPinging = true;
            Debug.Log("dfsfsfew");
            currentRadius = 0f;
            sonarMat.SetVector("_PulsePos", transform.position);
        }

        if (isPinging)
        {
            currentRadius += Time.deltaTime * speed;
            sonarMat.SetFloat("_Radius", currentRadius);

            if (currentRadius > maxRadius)
            {
                isPinging = false;
                currentRadius = 0f;
                sonarMat.SetFloat("_Radius", 0f);
            }
        }
    }
}