using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedSprite : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Image image;

    [SerializeField]
    private bool isAutoplay = true;

    [SerializeField]
    private Sprite[] spriteList;

    private float delayBetweenFrames = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        // Set to first sprite
        if (spriteRenderer != null) {
            spriteRenderer.sprite = spriteList[0];
        }

        if (image != null) {
            image.sprite = spriteList[0];
        }

        if (isAutoplay) {
            Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play() {
        if (!gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
        }

        StartCoroutine(PlaySpriteAnimationCoroutine());
    }

    public void Reverse() {
        if (!gameObject.activeInHierarchy) {
            gameObject.SetActive(true);
        }

        StartCoroutine(ReverseSpriteAnimationCoroutine());
    }

    private IEnumerator ReverseSpriteAnimationCoroutine() {
        if (spriteRenderer != null) {

            for (int i = 1; i < spriteList.Length; i++) {
                spriteRenderer.sprite = spriteList[spriteList.Length - i];

                yield return new WaitForSeconds(delayBetweenFrames);
            }
        }

        if (image != null) {

            for (int i = 1; i < spriteList.Length; i++) {
                image.sprite = spriteList[spriteList.Length - i];

                yield return new WaitForSeconds(delayBetweenFrames);
            }
        }
    }

    private IEnumerator PlaySpriteAnimationCoroutine() {
        if (spriteRenderer != null) {

            for (int i = 0; i < spriteList.Length; i++) {
                spriteRenderer.sprite = spriteList[i];

                yield return new WaitForSeconds(delayBetweenFrames);
            }
        }

        if (image != null) {

            for (int i = 0; i < spriteList.Length; i++) {
                image.sprite = spriteList[i];

                yield return new WaitForSeconds(delayBetweenFrames);
            }
        }
    }
}
