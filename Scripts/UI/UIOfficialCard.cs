using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //����UI���
using UnityEngine.EventSystems;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting; //��������ϵͳ

public enum CardState //��Ƭ������״̬
{
    //��cost����CD
    CanPlace,
    //��cost��CD
    InCD,
    //û��cost����CD
    NotCost,
    //û��cost��CD
    NotAll
}

public class UIOfficialCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler //����������ͼ���ϵĽӿ������ӿ�
{
    //����ͼƬ��Img��ȡ
    private Image maskImg;

    //�����Img���
    private Image image;

    //��ֲ��Ҫ��������
    public int needCostNum;

    //��ȴʱ��
    public float CDTime;

    //��ǰʱ�䣬������ȴʱ��ļ���
    private float currTimeForCd;

    //�Ƿ���Է��ø�Ա(ֻ����CD)
    private bool canPlace;

    //�Ƿ���Ҫ���ø�Ա
    private bool wantPlace;

    //����������Ա
    private OfficialBase official;

    //����������������������ʱ��͸����Ա
    private OfficialBase officialInGrid;

    //��ǰ��Ƭ��Ӧ�ĸ�Ա����
    public OfficialType CardOfficialType;

    //��Ƭ״̬
    private CardState cardState;

    //��Ա��Ԥ����
    private GameObject prefeb;
    public CardState CardState { get => cardState;
        set
        {

        }
    
    }

    public bool CanPlace { get => canPlace; set
        {
            canPlace = value;
            CheckState();
        }
    }

    public bool WantPlace
    {
        get => wantPlace;
        set
        {
            wantPlace = value;
        }
    }



    void Start()
    {
        maskImg = transform.Find("Mask").GetComponent<Image>();
        image = GetComponent<Image>();
        CanPlace = true;
        PlayerManager.Instance.AddCostNumUpdateActionListener(CheckState);
    }

    private void Update()
    {
        //�����Ҫ���ø�Ա�ҷ��õĸ�Ա��Ϊ��
        CheckState();
        if(WantPlace==true && official != null)
        {
            //��ֲ��������ǵ����
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition); //������λ��
            Grid grid = GridManager.Instance.GetGridByWorldPos(mousePoint); //ת��Ϊ����λ��
            official.transform.position = new Vector3 (mousePoint.x,mousePoint.y,0);
            //�����������ȽϽ�����û��ֲ���������У���Ҫ�������ϳ���һ��͸���ĸ�Ա
            if (grid.HaveOfficial == false && Vector2.Distance(mousePoint, grid.Position) < 1)
            {
                if(officialInGrid == null)
                {
                    //ʵ����official��λ�������λ�ã�����ת
                    officialInGrid = GameObject.Instantiate<GameObject>(official.gameObject, grid.Position, Quaternion.identity, PlayerManager.Instance.poolObj.transform).GetComponent<OfficialBase>();
                    officialInGrid.InitForCreate(true);
                }
                else
                {
                    officialInGrid.transform.position = grid.Position;
                }
                //���ʱ����ֲ��
                if (Input.GetMouseButtonDown(0))
                {
                    official.TransitionToEnable(grid);
                    official = null;
                    Destroy(officialInGrid.gameObject);
                    officialInGrid = null;
                    wantPlace = false;
                    canPlace = false;
                    CanPlaceCheck();

                    UIManager.Instance.CurrCard = null;

                    //��ֲ�ɹ�������cost
                    PlayerManager.Instance.CostNum -= needCostNum;
                }
            }
            //�������̫Զ������
            else
            {
                if (officialInGrid != null)
                {
                    Destroy(officialInGrid.gameObject);
                    officialInGrid = null;
                }

            }
        }
        //����Ҽ���ȡ������״̬
        if (Input.GetMouseButtonDown(1))
        {
            if (official != null) Destroy(official.gameObject);
            if (official != null) Destroy(officialInGrid.gameObject);
            official = null;
            officialInGrid = null;
            WantPlace = false;

            UIManager.Instance.CurrCard = null;
        }
    }

    private void CheckState() //�˺�������״̬���
    {
        //��cost�Ҳ���CD
        if (canPlace && PlayerManager.Instance.CostNum >= needCostNum)
        {
            CardState = CardState.CanPlace;
            maskImg.fillAmount = 0;
            image.color = Color.white;
        }
        //��cost������CD
        else if (!canPlace && PlayerManager.Instance.CostNum >= needCostNum)
        {
            CardState = CardState.InCD;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
        //û��cost���ǲ���CD
        else if (canPlace && PlayerManager.Instance.CostNum < needCostNum)
        {
            CardState = CardState.NotCost;
            maskImg.fillAmount = 0;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
        //û��cost������CD
        else if (!canPlace && PlayerManager.Instance.CostNum < needCostNum)
        {
            CardState = CardState.NotAll;
            image.color = new Color(0.75f, 0.75f, 0.75f);
        }
    }

    private void CDEnter() //�˺����������cd����
    {
        //����fillamount
        maskImg.fillAmount = 1;
        //���ֺ�ʼ������ȴ
        StartCoroutine(CalCD());
    }

    

    IEnumerator CalCD() //Я�̺���������ȴʱ��
    {
        float calCD = (1 / CDTime) * 0.01f; //Ҫ�����·���0.1f
        currTimeForCd = CDTime;
        while (currTimeForCd>=0)
        {
            yield return new WaitForSeconds(0.01f); //���ﶨ����ú�����ʱ����
            maskImg.fillAmount -= calCD;
            currTimeForCd -= 0.01f;
        }
        //��������ζ����ȴ���������Է���
        CanPlace = true;

    }
    public void OnPointerEnter(PointerEventData eventData) //��ʼ��IPEnter������������붯��
    {
        if (!canPlace) return;
        Vector2 newScale = transform.localScale;
        newScale.x *= 1.05f;
        newScale.y *= 1.05f;
        transform.localScale = newScale;
    }

    public void OnPointerExit(PointerEventData eventData) //��ʼ��IPExit����������Ƴ�����
    {
        if (!canPlace) return;
        Vector2 newScale = transform.localScale;
        newScale.x /= 1.05f;
        newScale.y /= 1.05f;
        transform.localScale = newScale;
    }

    //�����ʱ��Ч��
    public void OnPointerClick(PointerEventData eventData) //��ʼ��IPClick���������������
    {
        if ((!CanPlace) || (PlayerManager.Instance.CostNum < needCostNum)) return;
        AudioManager.Instance.PlayEFAudio(GameManager.Instance.GameConf.ButtonClick);
        if (!WantPlace)
        {
            wantPlace = true;
            prefeb = OfficialManager.Instance.GetOfficialForType(CardOfficialType);
            official = GameObject.Instantiate<GameObject>(prefeb, Vector3.zero, Quaternion.identity, PlayerManager.Instance.poolObj.transform).GetComponent<OfficialBase>();//����Ա���ڵ�����
            official.InitForCreate(false);
            UIManager.Instance.CurrCard = this;
        }
    }
    public void CanPlaceCheck() //�˺������ڼ��canplace��״̬,canplaceÿ�ı�һ�ξ͵���һ��
    {
        //������ܷ��ã���ʼ��ȴ
        if (!canPlace)
        {
            //��ȫ���֣���ʾ���ɵ��
            maskImg.fillAmount = 1;
            //��ʼ��ȴ
            CDEnter();
        }
        else
        {
            maskImg.fillAmount = 0;
        }
    }
}
