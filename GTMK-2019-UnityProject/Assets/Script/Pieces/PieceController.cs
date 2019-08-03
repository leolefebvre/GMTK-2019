using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class PieceController : MonoBehaviour
{
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

    [Header ("For debugging, don't touch")]
    public bool selected = false;
    public bool isInWinColor { get { return (currentColor.r == winColor.r) && (currentColor.g == winColor.g) && (currentColor.b == winColor.b); } }

    public bool leSuperBool = false;

    private SpriteRenderer _spRenderer;
    public SpriteRenderer spRender
    {
        get
        {
            if (_spRenderer == null)
            {
                _spRenderer = GetComponent<SpriteRenderer>();
            }
            return _spRenderer;
        }
    }

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        leSuperBool = isInWinColor;

    }

    public void UpdateColor(Color newColor)
    {
        currentColor = newColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "DetectionCollider")
        {
            SelectionManager.Instance.AddToSelectedList(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "DetectionCollider")
        {
            SelectionManager.Instance.RemoveToSelectedList(this);
        }
    }
}
