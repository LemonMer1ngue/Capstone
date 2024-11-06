using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject boxPrefab; // Prefab kotak
    private Queue<GameObject> boxPool = new Queue<GameObject>();

    // Mengambil kotak dari pool
    public GameObject GetBox()
    {
        if (boxPool.Count > 0)
        {
            GameObject box = boxPool.Dequeue();
            box.SetActive(true);
            return box;
        }
        else
        {
            // Jika pool kosong, buat kotak baru
            return Instantiate(boxPrefab);
        }
    }

    // Mengembalikan kotak ke pool
    public void ReturnBox(GameObject box)
    {
        box.SetActive(false);
        boxPool.Enqueue(box);
    }
}
