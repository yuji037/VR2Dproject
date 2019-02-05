using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DossunMover : GimmickBase {

    [SerializeField]
    float upSpeed;

    [SerializeField]
    float fallSpeed;

    [SerializeField]
    float maxFallLength=10.0f;

    bool isUp;

    Vector3 origin;

    bool wasHit;

    [SerializeField]
    LayerMask hitLayerMask;

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!isServer) return;
        origin = transform.position;
        StartCoroutine(MoveCorotuine());
    }
    // Update is called once per frame
    IEnumerator MoveCorotuine()
    {
        yield return new WaitForFixedUpdate();

        while (true)
        {
            float sumFall = 0f;
            while (!Physics.CheckBox(transform.position,transform.lossyScale*0.5f,transform.rotation, hitLayerMask) && sumFall < 10.0f)
            {
                var fallValue = 1.0f * fallSpeed * Time.fixedDeltaTime;
                transform.Translate(new Vector3(0, -fallValue, 0));
                sumFall += fallValue;
                yield return new WaitForFixedUpdate();
            }

            float sumUp = 0f;
            var dis = Mathf.Abs(origin.y - transform.position.y);
            while (dis>=sumUp)
            {
                var upvalue = 1.0f * upSpeed* Time.fixedDeltaTime;
                transform.Translate(new Vector3(0, upvalue , 0));
                sumUp += upvalue;
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
