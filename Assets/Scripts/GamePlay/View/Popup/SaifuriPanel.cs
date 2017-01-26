using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class SaifuriPanel : MonoBehaviour
{
    public Text lab_tip;
    public Text lab_num;

    private float EachAnimTime = 0.3f;


    private int num1;
    private int num2;
	private int num3;

    private bool AnimEnd = true;

    bool startAnim1 = false;
    bool startAnim2 = false;
    float saifuriTime = 0f;
    int updateTick = 0;


    void Start(){
        
    }


    public void Hide()
    {
        AnimEnd = true;

        startAnim1 = false;
        startAnim2 = false;
        saifuriTime = 0f;
        updateTick = 0;

        gameObject.SetActive(false);
    }

    public void Show(int num1, int num2, int num3 )
    {
		this.num1 = num1;
		this.num2 = num2;
		this.num3 = num3;
		if(lab_num)
        	lab_num.text = "";
		//Debug.Log ("total="+(num1+num2+num3));
        gameObject.SetActive(true);

        AnimEnd = false;
        startAnim1 = true;

    }


    void Update()
    {
        if( AnimEnd == true ) return;

        if( startAnim1 == true ){
            saifuriTime += Time.deltaTime;
            updateTick++;

			//if(lab_num && lab_num.alpha < 1f )
            //    lab_num.alpha += Time.deltaTime * 3f;

            if( saifuriTime < EachAnimTime ){
                if( updateTick % 2 == 0 )
					SetSaiString( GetRandomNum(), GetRandomNum(), GetRandomNum() );                
            }
            else{
                startAnim1 = false;
                startAnim2 = true;
                saifuriTime = 0f;
            }
        }
        else if( startAnim2 == true ){
            saifuriTime += Time.deltaTime;
            updateTick++;

            if( saifuriTime < EachAnimTime ){
                if( updateTick % 2 == 0 )
					SetSaiString( num1, GetRandomNum(), GetRandomNum() );
            }
            else{
                startAnim2 = false;
                saifuriTime = 0f;

                SetSaiString( num1, num2, num3 );
            }
        }
        else{
            saifuriTime += Time.deltaTime;

            if( saifuriTime >= 0.5f )
                OnEnd();
        }
    }

	void SetSaiString( int n1, int n2, int n3 )
    {
		if(lab_num)
			lab_num.text = n1.ToString() + " , " + n2.ToString()+" , "+n3.ToString();
    }

    int GetRandomNum()
    {
        return Random.Range(1, 7);
    }

    void OnEnd()
    {
        AnimEnd = true;

        Hide();

        EventManager.Get().SendEvent(UIEventType.On_Select_Wareme_End);
    }
}
