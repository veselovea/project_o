using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TestTODeO : MonoBehaviour
{
    public static event Action<Eblock[]> OnSaveFortress;

    void Awake()
    {
        NetworkDataReceive.OnLoadFortress += LoadFortress;
    }

    void FixedUpdate()
    {
        if (_isCan && Input.GetKey(KeyCode.S))
        {
            _isCan = false;
            Eblock[] eblocks = new Eblock[]
            {
                new Eblock("bloack_1", Vector3.zero),
                new Eblock("bloack_1", Vector3.zero),
                new Eblock("bloack_4", Vector3.zero),
                new Eblock("bloack_3", Vector3.zero),
                new Eblock("bloack_2", Vector3.zero),
                new Eblock("bloack_2", Vector3.zero)
            };
            SaveFortress(eblocks);
        }
    }

    private bool _isCan = true;

    private IEnumerator Corot()
    {
        yield return new WaitForSecondsRealtime(1f);
        _isCan = true;
    }

    public void LoadFortress(Eblock[] blocks)
    {
        Debug.Log(string.Join(';', blocks.Select(x => x.BlockName)));
    }

    public void SaveFortress(Eblock[] blocks) 
        => OnSaveFortress?.Invoke(blocks);
}
