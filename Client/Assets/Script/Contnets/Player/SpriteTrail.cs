using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTrail : MonoBehaviour
{
    public SpriteRenderer m_Renderer;
    public Gradient m_gradientColor;
    public float m_lifeTime = 1;
    private float m_time = 0;

    private SpriteRenderer renderers;
    private MaterialPropertyBlock mpb;

    public void SetSpriteTrail(SpriteRenderer spriteRenderer)
    {
        m_Renderer.sprite = spriteRenderer.sprite;
        m_Renderer.flipX = spriteRenderer.flipX;
        m_Renderer.flipY = spriteRenderer.flipY;
        transform.position = spriteRenderer.transform.position;
    }

    private void Start()
    {
        renderers = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if(0 == m_lifeTime|| m_lifeTime <= m_time)
            Destroy(gameObject);
        m_Renderer.GetPropertyBlock(mpb);
        mpb.SetColor("_ColorIns", m_gradientColor.Evaluate(m_time / m_lifeTime));
        renderers.SetPropertyBlock(mpb);
        
    }
}
