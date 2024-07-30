using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shovel : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler //ָ��ĵ����������˳�
{
    private Transform shovelImg;
    //�Ƿ���ʹ��ȡ������
    private bool isShovel;

    public bool IsShovel {
        get { return isShovel; }
        set
        {
            isShovel = value;
            //��Ҫȡ������
            if (isShovel)
            {
                
            }
            //�Ѳ��ӷŻ�ȥ
            else
            {
                shovelImg.transform.position = transform.position; //������ȡ������ͼ��ͱ���transformһ�£����ԷŻ�ȥ��ʱ��ֱ�Ӷ���Ϊ����λ��
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        if (IsShovel == false)
        {
            IsShovel = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //������ȥ�Ĳ���
    {
        shovelImg.transform.localScale = new Vector2(0.6f, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData) //������ߵĲ���
    {
        shovelImg.transform.localScale = new Vector2(0.8f, 0.8f);
    }

    // Start is called before the first frame update
    void Start()
    {
        shovelImg = transform.Find("Image");
    }


    void Update()
    {
        //�����Ҫȡ������
        if (IsShovel)
        {
            shovelImg.position = Input.mousePosition;
            //����ڲ���ʹ�ù����а������������ж��Ƿ�Ҫȡ������
            if (Input.GetMouseButtonDown(0))
            {
                Grid grid = GridManager.Instance.GetGridByMouse();
                //���������û�и�Ա����ôֱ�����������߼�
                if (grid.CurrOfficialBase == null) return;
                //���������������ڸ�ԱС��1.5�ף�����ȡ������
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),grid.CurrOfficialBase.transform.position)<1.5f) //���������ת��Ϊ�������꣬���������ڸ�Աλ�ñȽ�
                {
                    grid.CurrOfficialBase.DestroyGameobject();
                    grid.CurrOfficialBase.currGrid.CurrOfficialBase = null; //�ǰ����Ϊ��
                    IsShovel = false;
                }

            }
            //����ڲ���ʹ�ù����а�������Ҽ�����ô�Ͱ�ȡ������ͼ��Ż�ȥ
            if (Input.GetMouseButtonDown(1))
            {
                IsShovel = false;
            }
        }
    }
}
