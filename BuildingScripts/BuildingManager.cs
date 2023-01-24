using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Camera cameraMain;
    public GameObject blockPrefab;
    // Start is called before the first frame update
    void Start()
    {
        cameraMain = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 click = cameraMain.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(click, -Vector3.forward);

            if (hit.transform == null || hit.transform.name != "HitBox")
            {
                Vector3 position;

                position.x = Mathf.Round(click.x);
                position.y = Mathf.Round(click.y);

                position.z = 0;

                Instantiate(blockPrefab, position, Quaternion.identity);
            }
        }
    }
}
