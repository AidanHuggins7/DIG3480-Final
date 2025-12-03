using UnityEngine;

public class GargoyleRotator : MonoBehaviour
{
    public float interval = 5f;      // Time between rotations
    public float rotationAmount = 45f; // Degrees per turn (clockwise)

    private void Start()
    {
        StartCoroutine(RotateRoutine());
    }

    private System.Collections.IEnumerator RotateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Rotate clockwise on Y axis
            transform.Rotate(0f, rotationAmount, 0f);
        }
    }
}
