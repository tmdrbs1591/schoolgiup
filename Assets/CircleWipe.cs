using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleWipe : MonoBehaviour
{
    public Shader shader;

    public Material material;

    public bool closing;
    public bool disable;
    public float radius;
    public string scene;

    public static CircleWipe instance;

    void Start()
    {
        material = new Material(shader);
        material.SetFloat("_Radius", radius);
    }

    public void Close()
    {
        closing = true;
    }

    private void Update()
    {
        material.SetFloat("_Radius", radius);
        if (closing)
        {
            radius -= Time.unscaledDeltaTime * 15;
            if (radius <= -1) SceneManager.LoadScene(scene);
        }
        else
        {
            radius += Time.unscaledDeltaTime * 12;
            //if (radius >= 10 && disable) Destroy(this);
        }
        radius = Mathf.Clamp(radius, -1, 10);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
