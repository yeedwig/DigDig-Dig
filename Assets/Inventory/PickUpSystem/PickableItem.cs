using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class PickableItem : MonoBehaviour
{
    [field: SerializeField]
    public Item item;
    public float lifeTime;

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioClip[] itemEarnedSound;

    [SerializeField]
    private float duration = 0.3f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.image;

        if(item.isResource)
        {
            Invoke("DestroyResource", lifeTime);
        }
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());

    }

    
    private void DestroyResource()
    {
        Destroy(gameObject);
    }
    private IEnumerator AnimateItemPickup()
    {
        SoundFXManager.instance.PlaySoundFXClip(itemEarnedSound, transform, 0.2f);
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
