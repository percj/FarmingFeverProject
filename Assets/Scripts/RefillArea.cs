using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillArea : MonoBehaviour
{
    [SerializeField] GameObject carryPrafab;
    [SerializeField] AnimationCurve curve;
    [SerializeField] Transform trashPoss;
    [SerializeField] GameObject effect;
    [SerializeField] Animator trashAnim;
    float Close = 0;
    public void goToTrash(GameObject gameObject)
    {
        StartCoroutine(sendTrash(gameObject));
    }
    IEnumerator sendTrash(GameObject trashObject)
    {
        float time = 0;
        while (Vector3.Distance(trashObject.transform.position,trashPoss.position) > .1f && time <= 1)
        {
            Close += Time.deltaTime;
            time += Time.deltaTime ;
            Vector3 pos = Vector3.Lerp(new Vector3(trashObject.transform.position.x, 0, trashObject.transform.position.z), new Vector3(trashPoss.position.x, 0, trashPoss.position.z), time/10);
            pos.y += curve.Evaluate(time);
            trashObject.transform.position = pos;
            if (Vector3.Distance(trashObject.transform.position, trashPoss.position) < 1.3f)
                trashAnim.SetTrigger("Open");
            yield return null;
        }

        var effectRef = Instantiate(effect);
        effectRef.transform.position = trashPoss.position;
        Destroy(effectRef,2);
        Destroy(trashObject);
        GameSingleton.Instance.Sounds.PlayOneShot(GameSingleton.Instance.Sounds.TrashDrop);
        yield return null;
    }
}
