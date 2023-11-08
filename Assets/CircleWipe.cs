using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircleWipe : MonoBehaviour
{
    Material material;

    public bool closing;
    public bool disable;
    public float radius;
    public string scene;

    public static CircleWipe instance;

    void Start()
    {
        material = gameObject.GetComponent<Image>().material;
        material.SetFloat("_Radius", radius);
    }

    public void Close()
    {
        closing = true;
    }
    public void SceneTransition(string _scene)
    {
        scene = _scene;
        closing = true;
    }

    private void Update()
    {
        if (!GameManager.instance.ready) return;
        material.SetFloat("_Radius", radius);
        if (closing)
        {
            radius -= Time.unscaledDeltaTime * 1.5f;
            if (radius <= 0) SceneManager.LoadScene(scene);
        }
        else
        {
            radius += Time.unscaledDeltaTime * 1.2f;
            //if (radius >= 10 && disable) Destroy(this);
        }
        radius = Mathf.Clamp(radius, 0, 1);
    }
}
