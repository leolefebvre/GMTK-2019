using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class PieceController : MonoBehaviour
{
    public SpriteRenderer spRender;
    public Color winColor;
    public Color currentColor
    {
        get
        {
            return spRender.color;
        }
        private set
        {
            spRender.color = value;
        }
    }

    private Color originColor;

    [Header("For debugging, don't touch")]
    public bool selected = false;
    public bool isInWinColor { get { return (currentColor.r == winColor.r) && (currentColor.g == winColor.g) && (currentColor.b == winColor.b); } }

    public bool leSuperBool = false;

    public float timeOnSelect = 0f;

    public int selectionOrder = int.MaxValue;
    public int newSelectionOrder = int.MaxValue;

    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    private Collider2D _collider;
    public Collider2D pieceCollider
    {
        get
        {
            if (_collider == null)
            {
                // grab the first collider we can find
                foreach (Collider2D coll in GetComponents<Collider2D>())
                {
                    if (coll.enabled)
                    {
                        _collider = coll;
                        break;
                    }
                }

            }
            return _collider;
        }
    }

    public float distanceFromStartSelection
    {
        get
        {
            Vector3 startPosWorld = Camera.main.ScreenToWorldPoint(SelectionManager.Instance.selectionStartPos);
            return Vector3.Distance(pieceCollider.bounds.ClosestPoint(startPosWorld), startPosWorld);
        }
    }

    // Use this for initialization
    void Start()
    {
        originColor = currentColor;
    }

    // Update is called once per frame
    void Update() {
        leSuperBool = isInWinColor;

    }

    public void UpdateColor(Color newColor)
    {
        currentColor = newColor;
    }

    public void RestartColor()
    {
        currentColor = originColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DetectionCollider")
        {
            timeOnSelect = Time.time;
            SelectionManager.Instance.AddToSelectedList(this);
            PlaySelectAnimation();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "DetectionCollider")
        {
            SelectionManager.Instance.RemoveToSelectedList(this);
        }
    }

    #region animations triggers

    public void PlaySelectAnimation()
    {
        animator.SetTrigger("SelectTrigger");
    }

    public void PlayReleaseAnimation()
    {
        animator.SetTrigger("ReleaseTrigger");
    }

    public void PlayRestartAnimation()
    {
        animator.SetTrigger("RestartTrigger");
    }

    #endregion
    
}
