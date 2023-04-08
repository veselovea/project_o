using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    public Transform spawner;
    public Vector2 range;
    public GameObject Enemies;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2);
        Vector2 pos = spawner.position + new Vector3(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
        Instantiate(Enemies, pos, Quaternion.identity);
        Repeat();
    }

    void Repeat()
    {
        StartCoroutine(Spawn());
    }
}
