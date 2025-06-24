using DG.Tweening;
using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    public float floatStrength = 0.5f; // How high it floats
    public float duration = 1f;        // Time to reach up and down

    void Start()
    {
        Util.WaitForSeconds(this, () =>
        {
            // Loop the Y movement up and down
            transform.DOMoveY(transform.position.y + floatStrength, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }, Random.Range(0f, 1.0f));
       
    }
}
