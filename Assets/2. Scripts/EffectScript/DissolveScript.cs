using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{

    public Shader dissolveShader;

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    private bool dissolving = false;
    private ParticleSystem ps;
    private float timer;
    private Renderer rendererComponent;
    private int shaderProperty;

    void Start() {
        timer = spawnEffectTime * 0.25f;  // Hacky tips, make it start from 0.2, to decrease white colour effect
        shaderProperty = Shader.PropertyToID("_cutoff");
        rendererComponent = GetComponent<Renderer>();
    }

    void Update()
    {
        if (dissolving)
        {
            if (timer < spawnEffectTime + pause)
            {
                timer += Time.deltaTime;
            }
            else
            {
                dissolving = false;
            }

            rendererComponent.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));

        }
    }

    public IEnumerator Dissolve()
    {   
        rendererComponent.material.shader = dissolveShader;
        dissolving = true;
        yield return new WaitForSeconds(spawnEffectTime + pause);
    }
}
