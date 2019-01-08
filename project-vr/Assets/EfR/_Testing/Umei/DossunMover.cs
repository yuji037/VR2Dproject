using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossunMover : MonoBehaviour {

    [SerializeField]
    float upSpeed;

    [SerializeField]
    float fallSpeed;

    [SerializeField]
    float maxFallLength=10.0f;

    bool isUp;

    Vector3 origin;

    private void Start()
    {
        origin = transform.position;
        StartCoroutine(MoveCorotuine());
    }

    // Update is called once per frame
    IEnumerator MoveCorotuine()
    {
        while (true)
        {
            float sumFall = 0f;
            while (!Physics.BoxCast(transform.position, transform.lossyScale * 0.49f, Vector3.down, transform.rotation, 0.1f) && sumFall < 10.0f)
            {
                var fallValue = 1.0f * fallSpeed * Time.deltaTime;
                transform.Translate(0, -fallValue, 0);
                sumFall += fallValue;
                yield return null;
            }

            float sumUp = 0f;
            var dis = Mathf.Abs(origin.y - transform.position.y);
            while (dis>=sumUp)
            {
                var upvalue = 1.0f * upSpeed* Time.deltaTime;
                transform.Translate(0, upvalue , 0);
                sumUp += upvalue;
                yield return null;
            }
            yield return null;
        }
    }
}
