using System.Collections.Generic;
using UnityEngine;

public class CountFormat 
{
    public const int COMBI_MAX = 10;
    public const int COUNTER_MAX = 14 + 2;
    public const int MENTSU_LENGTH_MAX = 4;


    public class CombiHelper 
    {
        // 上がりの組み合わせの配列
        public List<HaiCombi> combis = new List<HaiCombi>(COMBI_MAX);

        // remain jyun tehai count(contains the addHai)
        public int remain = 0;

        public HaiCombi current = new HaiCombi();


        public void initialize(int remain)
        {
            this.remain = remain;
            combis.Clear();
            current.Clear();
        }

        public void add()
        {
            HaiCombi combi = new HaiCombi();
            HaiCombi.copy(combi, current);
            combis.Add( combi );
        }
    }


    private List<HaiCounterInfo> _counterArr = new List<HaiCounterInfo>(COUNTER_MAX);
    private CombiHelper _combiHelper = new CombiHelper();


    public HaiCounterInfo[] getCounterArray()
    {
        return _counterArr.ToArray();
    }

    public HaiCombi[] getCombis()
    {
        return _combiHelper.combis.ToArray();
    }


    /// <summary>
    /// Calculate all hai's count in tehai. the 'tehai' should be sorted.
    /// </summary>

    public void setCounterFormat(Tehai tehai, Hai addHai)
    {
        _counterArr.Clear();


        int addHaiNumKind = 0;
        bool set = true;

        if( addHai != null ){
            addHaiNumKind = addHai.NumKind;
            set = false;
        }

        Hai[] jyunTehais = tehai.getJyunTehai();

        for(int i = 0; i < jyunTehais.Length; )
        {
            int jyunTehaiNumKind = jyunTehais[i].NumKind;

            // if the first one hai's NK is bigger than the addHai's, 
            // add the addHai to _counterArr firstly, then re-check the first one(as no i++).
            if( set == false && jyunTehaiNumKind > addHaiNumKind )
            {
                _counterArr.Add( new HaiCounterInfo(addHaiNumKind, 1) );
                set = true;

                continue;
            }

            // in middle range of tehai there exsits one as the same to addHai
            _counterArr.Add( new HaiCounterInfo(jyunTehaiNumKind, 1) );
            if( set == false && jyunTehaiNumKind == addHaiNumKind )
            {
                _counterArr[_counterArr.Count-1].count ++;
                set = true;
            }

            // check if the after one(at i+1) is the same to current one at 'i'
            while(++i < jyunTehais.Length) 
            {
                if( jyunTehaiNumKind == jyunTehais[i].NumKind ){
                    _counterArr[_counterArr.Count-1].count ++;
                }
                else{
                    break;
                }
            }
        }

        // the addHai is the biggest one.
        if( set == false )
            _counterArr.Add( new HaiCounterInfo(addHaiNumKind, 1) );

        for( int i = 0; i < _counterArr.Count; i++ )
        {
            if( _counterArr[i].count > MENTSU_LENGTH_MAX )
                _counterArr[i].count = MENTSU_LENGTH_MAX;
        }
    }


    //
    int getTotalCounterLength()
    {
        int totalCountLength = 0;

        for(int i = 0; i < _counterArr.Count; i++)
            totalCountLength += _counterArr[i].count;

        return totalCountLength;
    }


    public int calculateCombisCount( HaiCombi[] outCombis )
    {
        _combiHelper.initialize( getTotalCounterLength() );
        searchCombi(0);
		/*
        if( _combiHelper.combis.Count == 0 ) 
        {
            _chiitoitsu = checkChiitoitsu();

            if( _chiitoitsu ){
                return 1;
            }
            else 
            {
                _kokushi = checkKokushi();

                if( _kokushi )
                    return 1;
            }
        }
        */

        if( outCombis != null )
            outCombis = _combiHelper.combis.ToArray();

        return _combiHelper.combis.Count;
    }


    // 七对子.
    private bool _chiitoitsu;
    public bool isChiitoitsu()
    {
        return _chiitoitsu;
    }

    bool checkChiitoitsu()
    {
        int count = 0;

        for( int i = 0; i < _counterArr.Count; i++ ) 
        {
            if( _counterArr[i].count == 2 ){
                count++;
            }
            else{
                return false;
            }
        }

        return count == 8;
    }

	/*
    // 国士无双.
    private bool _kokushi;
    public bool isKokushi()
    {
        return _kokushi;
    }

    bool checkKokushi()
    {
        //牌の数を調べるための配列 (0番地は使用しない）
        int[] checkId = {
            Hai.ID_WAN_1, Hai.ID_WAN_9, Hai.ID_PIN_1, Hai.ID_PIN_9, Hai.ID_SOU_1, Hai.ID_SOU_9,
            Hai.ID_TON, Hai.ID_NAN, Hai.ID_SYA, Hai.ID_PE, Hai.ID_HAKU, Hai.ID_HATSU, Hai.ID_CHUN
        };
        int[] countHai = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //length = 13.

        //手牌のIDを検索する
        for( int i = 0; i < _counterArr.Count; i++ )
        {
            for( int j = 0; j < checkId.Length; j++ )
            {
                if( Hai.NumKindToID(_counterArr[i].numKind) == checkId[j] )
                    countHai[j] = _counterArr[i].count;
            }
        }

        //条件を満たしていれば不成立
        bool atama = false;

        //国士無双が成立しているか調べる(手牌がすべて1.9字牌 すべての１,９字牌を持っている）
        for( int i = 0; i < countHai.Length; i++ )
        {
            //0枚の牌があれば不成立
            if( countHai[i] == 0 )
                return false;

            if( countHai[i] > 2 )
                return false;

            if( countHai[i] == 2 )
                atama = true;
        }

        return atama;
    }
	*/


    void searchCombi(int startIndex)
    {
        // 検索位置を更新する
        for( ; startIndex < _counterArr.Count; startIndex++ )
        {
            if( _counterArr[startIndex].count > 0 )
                break;
        }

        if( startIndex >= _counterArr.Count )
            return;

        // 1. 頭をチェック(check)する
        if( _combiHelper.current.atamaNumKind == 0 ) 
        {
            if( _counterArr[startIndex].count >= 2 )
            {
                // 頭を確定する
                _counterArr[startIndex].count -= 2;
                _combiHelper.remain -= 2;
                _combiHelper.current.atamaNumKind = _counterArr[startIndex].numKind;

                // 上がりの組み合わせを見つけたら追加する
                if( _combiHelper.remain <= 0 ){
                    _combiHelper.add();
                } 
                else{
                    searchCombi( startIndex );
                }

                // 確定した頭を戻す
                _counterArr[startIndex].count += 2;
                _combiHelper.remain += 2;
                _combiHelper.current.atamaNumKind = 0;
            }
        }

        // 2. 順子をチェックする
        int left = startIndex;
        int center = startIndex + 1;
        int right = startIndex + 2;

        if( (left < _counterArr.Count) && !Hai.CheckIsTsuu(_counterArr[left].numKind) ) 
        {
            if( (center < _counterArr.Count) && (_counterArr[center].count > 0) && (_counterArr[left].numKind + 1 == _counterArr[center].numKind)) 
            {
                if( (right < _counterArr.Count) && (_counterArr[right].count > 0) && (_counterArr[left].numKind + 2 == _counterArr[right].numKind)) 
                {
                    // 順子を確定する
                    _counterArr[left].count --;
                    _counterArr[center].count --;
                    _counterArr[right].count --;

                    _combiHelper.remain -= 3;
					if (_combiHelper.current.shunNumKinds.Length > _combiHelper.current.shunCount && _counterArr.Count>left) {
						_combiHelper.current.shunNumKinds [_combiHelper.current.shunCount] = _counterArr [left].numKind;
						_combiHelper.current.shunCount++;
					}
					//Debug.Log ("shunNumKinds.Length="+_combiHelper.current.shunNumKinds.Length);
					//Debug.Log ("current.shunCount="+_combiHelper.current.shunCount);
					//Debug.Log ("_counterArr.Count="+_counterArr.Count);
					//Debug.Log ("left="+left);
                    //_combiHelper.current.shunCount++;

                    // 上がりの組み合わせを見つけたら追加する
                    if( _combiHelper.remain <= 0 ){
                        _combiHelper.add();
                    }
                    else{
                        searchCombi( startIndex );
                    }

                    // 確定した順子を戻す
                    _counterArr[left].count ++;
                    _counterArr[center].count ++;
                    _counterArr[right].count ++;

                    _combiHelper.remain += 3;
                    _combiHelper.current.shunCount --;
                }
            }
        }

        // 3. 刻子をチェックする
        if( _counterArr[startIndex].count >= 3 )
        {
            // 刻子を確定する
            _counterArr[startIndex].count -= 3;

            _combiHelper.remain -= 3;
            _combiHelper.current.kouNumKinds[_combiHelper.current.kouCount] = _counterArr[startIndex].numKind;
            _combiHelper.current.kouCount ++;

            // 上がりの組み合わせを見つけたら追加する
            if( _combiHelper.remain <= 0 ){
                _combiHelper.add();
            }
            else{
                searchCombi( startIndex );
            }

            // 確定した刻子を戻す
            _counterArr[startIndex].count += 3;

            _combiHelper.remain += 3;
            _combiHelper.current.kouCount --;
        }
    }

}
