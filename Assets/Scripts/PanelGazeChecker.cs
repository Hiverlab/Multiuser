using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelGazeChecker : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPanelAnimation() {
        animator.enabled = true;
    }
}
