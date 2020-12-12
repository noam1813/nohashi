using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageEffectManager : MonoBehaviour
{
    public static StageEffectManager instance;
    
    [SerializeField] private Image NightShadow;

    [SerializeField] private RectTransform MizukusaGroup;

    [SerializeField] private List<SpriteRenderer> MizukusaMaterial;

    [SerializeField] private Vector3 MizukusaShowPos;
    [SerializeField] private Vector3 MizukusaHidePos;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    /// <summary>
    /// 夜の陰の表示非表示を切り替える。アニメーション付き
    /// </summary>
    /// <param name="mode"></param>
    public void SetShadow(bool mode)
    {
        if (mode)
        {
            // SequenceはDoTween関係の変数。融通の利くアニメーションと思ってもいいかも。
            //陰を表示する時のシークエンス
            Sequence EnterShadowSequence = DOTween.Sequence();
            
            //AppendでSequenceにアニメーションを追加。Appendの中のアニメーションを一気に実行する。
            EnterShadowSequence.Append(
                
                //NightShadowの色を1秒間かけて不透明にしている
                DOTween.ToAlpha(
                    () => NightShadow.color,
                    color => NightShadow.color = color,
                    1f,
                    1f
                )
            );
            
            //再生する
            EnterShadowSequence.Play();
        }
        else
        {
            //陰を消すときのシークエンス
            Sequence ExitShadowSequence = DOTween.Sequence();
            
            ExitShadowSequence.Append(
                //NightShadowの色を1秒間かけて透明にしている
                DOTween.ToAlpha(
                    () => NightShadow.color,
                    color => NightShadow.color = color,
                    0f,
                    1f
                )
            );
            
            ExitShadowSequence.Play();
        }
    }

    public void SetMizukusa(bool mode)
    {
        if (mode)
        {
            for (int i = 0; i < MizukusaMaterial.Count; i++)
            {
                MizukusaMaterial[i].DOColor(
                    new Color32(0,99,26,200), 
                    1f
                    );
            }
        }
        else
        {
            for (int i = 0; i < MizukusaMaterial.Count; i++)
            {
                MizukusaMaterial[i].DOColor(
                    new Color32(0,99,26,0), 
                    1f
                );
            }
        }
        
    }
}
