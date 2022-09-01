using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentManager : MonoBehaviour
{
    Shader opaque, transparent;
    public GameObject fragmentOriginal;
    public GameObject[] fragments;
    public Vector3 originalPos, randomPos;
    public Quaternion originalRot;

    void Awake()
    {
        opaque = Shader.Find("Standard");
        transparent = Shader.Find("Transparent/Diffuse");
    }
    public void SpawnFragments()
    {
        for (int i = 0; i < fragments.Length; i++)
        {
            GameObject newFragment = Instantiate(fragmentOriginal, this.transform);

            randomPos.x = Random.Range(-4, 6) * 2;
            randomPos.y = Random.Range(-4, 6) * 2;
            randomPos.z = Random.Range(-4, 6) * 2 + 10;

            newFragment.transform.SetPositionAndRotation(randomPos, Random.rotation);

            // Changing shader
            newFragment.GetComponentInChildren<MeshRenderer>().material.shader = transparent;

            Color randomColor = Random.ColorHSV() * new Vector4(1, 1, 1, 0.5f);
            newFragment.GetComponentInChildren<MeshRenderer>().material.color = randomColor;

            fragments[i] = newFragment;
        }
    }
    public void ReturnFragment (int frag)
    {
        fragments[frag].transform.SetPositionAndRotation(originalPos, originalRot);
        fragments[frag].GetComponentInChildren<MeshRenderer>().material.shader = transparent;
    }

    public void PresentFragment (int frag)
    {
        originalPos = fragments[frag].transform.position;
        originalRot = fragments[frag].transform.rotation;

        fragments[frag].transform.SetPositionAndRotation(new Vector3(1, 1, 4), Quaternion.identity);

        fragments[frag].GetComponentInChildren<MeshRenderer>().material.shader = opaque;
    }
}
