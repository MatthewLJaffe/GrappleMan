using UnityEngine;

/// <summary>
/// displays launch arrow trajectory when player is in display and pull knob is selected
/// </summary>
public class KnobArrowEnable : MonoBehaviour
{
    [SerializeField] private float range = 1;
    [SerializeField] private Transform player;
    private GameObject arrow;
    private float _ropeLength;
    private void Awake() {
        arrow = transform.GetChild(0).gameObject;
        arrow.SetActive(false);
        _ropeLength = player.gameObject.GetComponent<PathDrawer>().ropeLength;

    }

    private void Update()
    {
        if (!arrow.activeSelf) {
            if (InRange())
                arrow.SetActive(true);
        }
        else if (!InRange())
            arrow.SetActive(false);
    }

    private bool InRange() 
    {
        return ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).magnitude < range &&
                ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)player.position).magnitude < _ropeLength - 1;
    }
}
