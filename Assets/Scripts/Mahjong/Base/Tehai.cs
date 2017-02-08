using System.Collections.Generic;

/// <summary>
/// 手牌を管理する。
/// 手牌:拿到手上的牌
/// </summary>

public class Tehai
{
    public static int Compare( Hai x, Hai y )
    {
        if( x.ID == y.ID ){
            if( x.IsRed && !y.IsRed )
                return -1;
            else if( !x.IsRed && y.IsRed )
                return 1;
            else
                return 0;
        }
        else{
            return x.ID - y.ID;
        }
    }


    // 手牌的最大長度
    public readonly static int JYUN_TEHAI_LENGTH_MAX = 17;
    // 副露の最大値
    public readonly static int FUURO_MAX = 4;

    // 面子の長さ(3)
    public readonly static int MENTSU_LENGTH_3 = 3;
    // 面子の長さ(4)
    public readonly static int MENTSU_LENGTH_4 = 4;


    // 純手牌
    private List<Hai> _jyunTehais = new List<Hai>(JYUN_TEHAI_LENGTH_MAX);

    // 副露の配列
    private List<Fuuro> _fuuros = new List<Fuuro>(FUURO_MAX);


    public Tehai()
    {
        initialize();
    }

    public Tehai( Tehai src )
    {
        initialize();
        copy( this, src, true );

        Sort();
    }

    public void initialize()
    {
        _jyunTehais.Clear();
        _fuuros.Clear();
    }


    // 手牌をコピーする
    public static void copy(Tehai dest, Tehai src, bool jyunTehaiCopy)
    {
        if( jyunTehaiCopy == true )
        {
            dest._jyunTehais.Clear();

            for(int i = 0; i < src._jyunTehais.Count; i++)
                dest._jyunTehais.Add( new Hai( src._jyunTehais[i] ) );
        }

        dest._fuuros.Clear();

        for(int i = 0; i < src._fuuros.Count; i++)
            dest._fuuros.Add( new Fuuro( src._fuuros[i] ) );
    }


    // 副露の配列を取得する
    public Fuuro[] getFuuros()
    {
        return _fuuros.ToArray();
    }

    // 鸣听Flag
    public bool isNaki()
    {
        for(int i = 0; i < _fuuros.Count; i++)
        {
            if(_fuuros[i].Type != EFuuroType.AnKan)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 副露の配列をコピーする
    /// </summary>

    public static bool copyFuuros(Fuuro[] dest, Fuuro[] src, int length)
    {
        if(length > FUURO_MAX)
            return false;

        for(int i = 0; i < length; i++)
            Fuuro.copy(dest[i], src[i]);

        return true;
    }

    public void Sort()
    {
        _jyunTehais.Sort( Tehai.Compare );
        return;
    }

    // 純手牌を取得する
    public Hai[] getJyunTehai()
    {
        return _jyunTehais.ToArray();
    }

    public int getJyunTehaiCount()
    {
        return _jyunTehais.Count;
    }

    public int getHaiIndex(int haiID)
    {
        return _jyunTehais.FindIndex( h=> h.ID == haiID );
    }

    // 純手牌に牌を追加する
    public bool addJyunTehai(Hai hai)
    {
        if( _jyunTehais.Count >= JYUN_TEHAI_LENGTH_MAX )
            return false;

        _jyunTehais.Add( new Hai(hai) );

        return true;
    }

    public bool insertJyunTehai(int index, Hai hai)
    {
        if( _jyunTehais.Count >= JYUN_TEHAI_LENGTH_MAX )
            return false;

        if( index < 0 || index > _jyunTehais.Count )
            return false;

        _jyunTehais.Insert(index, hai);

        return true;
    }

    // 純手牌から指定位置の牌を削除する
    public Hai removeJyunTehaiAt(int index)
    {
        if( index >= _jyunTehais.Count )
            return null;

        Hai hai = _jyunTehais[index];
        _jyunTehais.RemoveAt(index);

        return hai;
    }

    // 純手牌から指定の牌を削除する
    public bool removeJyunTehai(Hai hai)
    {
        for( int i = 0; i < _jyunTehais.Count; i++ )
        {
            if( _jyunTehais[i].ID == hai.ID ){
                _jyunTehais.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    // 純手牌をコピーする
    public static bool copyJyunTehai(Hai[] dest, Hai[] src, int length = -1)
    {
        if(length <= 0)
            length = src.Length;

        if( length > JYUN_TEHAI_LENGTH_MAX )
            return false;

        for(int i = 0; i < length; i++)
        {
            Hai.copy(dest[i], src[i]);
        }

        return true;
    }

    // 純手牌の指定位置の牌をコピーする
    public bool copyJyunTehaiIndex(Hai hai, int index)
    {
        if( index >= _jyunTehais.Count )
            return false;

        Hai.copy(hai, _jyunTehais[index]);

        return true;
    }


    // チー(左)の可否をチェックする
    public bool validChiiLeft(Hai suteHai, List<Hai> sarashiHais)
    {
        if (_fuuros.Count >= FUURO_MAX)
            return false;
        
        if (suteHai.IsTsuu)
            return false;

        if (suteHai.Num == Hai.NUM_8 || suteHai.Num == Hai.NUM_9)
            return false;

        int noKindLeft = suteHai.NumKind;
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if (_jyunTehais[i].NumKind == noKindCenter) 
            {
                for (int j = i + 1; j < _jyunTehais.Count; j++) 
                {
                    if (_jyunTehais[j].NumKind == noKindRight) 
                    {
                        sarashiHais.Add( _jyunTehais[i] );
                        sarashiHais.Add( _jyunTehais[j] );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(左)を設定する
    public bool setChiiLeft(Hai suteHai, int relation)
    {
        List<Hai> sarashiHais = new List<Hai>();

        if( !validChiiLeft(suteHai, sarashiHais) )
            return false;

        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_3];

        hais[0] = new Hai(suteHai);
        int newPickIndex = 0;

        int noKindLeft = suteHai.NumKind;
        int noKindCenter = noKindLeft + 1;
        int noKindRight = noKindLeft + 2;

        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if (_jyunTehais[i].NumKind == noKindCenter)
            {
                hais[1] = new Hai(_jyunTehais[i]);

                removeJyunTehaiAt(i);

                for (int j = i; j < _jyunTehais.Count; j++)
                {
                    if (_jyunTehais[j].NumKind == noKindRight)
                    {
                        hais[2] = new Hai(_jyunTehais[j]);

                        removeJyunTehaiAt(j);

                        _fuuros.Add( new Fuuro(EFuuroType.MinShun, hais, relation, newPickIndex) );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(中央)の可否をチェックする
    public bool validChiiCenter(Hai suteHai, List<Hai> sarashiHais)
    {
        if (_fuuros.Count >= FUURO_MAX)
            return false;

        if (suteHai.IsTsuu)
            return false;

        if (suteHai.Num == Hai.NUM_1 || suteHai.Num == Hai.NUM_9)
            return false;

        int noKindCenter = suteHai.NumKind;
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < _jyunTehais.Count; i++) 
        {
            if (_jyunTehais[i].NumKind == noKindLeft)
            {
                for (int j = i + 1; j < _jyunTehais.Count; j++) 
                {
                    if (_jyunTehais[j].NumKind == noKindRight)
                    {
                        sarashiHais.Add( _jyunTehais[i] );
                        sarashiHais.Add( _jyunTehais[j] );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(中央)を設定する
    public bool setChiiCenter(Hai suteHai, int relation)
    {
        List<Hai> sarashiHais = new List<Hai>();

        if( !validChiiCenter(suteHai, sarashiHais) )
            return false;

        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_3];

        hais[1] = new Hai(suteHai);
        int newPickIndex = 1;

        int noKindCenter = suteHai.NumKind;
        int noKindLeft = noKindCenter - 1;
        int noKindRight = noKindCenter + 1;

        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if (_jyunTehais[i].NumKind == noKindLeft)
            {
                hais[0] = new Hai(_jyunTehais[i]);

                removeJyunTehaiAt(i);

                for (int j = i; j < _jyunTehais.Count; j++)
                {
                    if (_jyunTehais[j].NumKind == noKindRight)
                    {
                        hais[2] = new Hai(_jyunTehais[j]);

                        removeJyunTehaiAt(j);

                        _fuuros.Add( new Fuuro(EFuuroType.MinShun, hais, relation, newPickIndex) );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(右)の可否をチェックする
    public bool validChiiRight(Hai suteHai, List<Hai> sarashiHais)
    {
        //if (_fuuros.Count >= FUURO_MAX)
        //    return false;

        if (suteHai.IsTsuu)
            return false;

        if (suteHai.Num == Hai.NUM_1 || suteHai.Num == Hai.NUM_2)
            return false;

        int noKindRight = suteHai.NumKind;
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < _jyunTehais.Count; i++) 
        {
            if (_jyunTehais[i].NumKind == noKindLeft)
            {
                for (int j = i + 1; j < _jyunTehais.Count; j++) 
                {
                    if (_jyunTehais[j].NumKind == noKindCenter)
                    {
                        sarashiHais.Add( _jyunTehais[i] );
                        sarashiHais.Add( _jyunTehais[j] );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // チー(右)を設定する
    public bool setChiiRight(Hai suteHai, int relation)
    {
        List<Hai> sarashiHais = new List<Hai>();

        if( !validChiiRight(suteHai, sarashiHais) )
            return false;

        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_3];

        hais[2] = new Hai(suteHai);
        int newPickIndex = 2;

        int noKindRight = suteHai.NumKind;
        int noKindLeft = noKindRight - 2;
        int noKindCenter = noKindRight - 1;

        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if (_jyunTehais[i].NumKind == noKindLeft)
            {
                hais[0] = new Hai(_jyunTehais[i]);

                removeJyunTehaiAt(i);

                for (int j = i; j < _jyunTehais.Count; j++)
                {
                    if (_jyunTehais[j].NumKind == noKindCenter)
                    {
                        hais[1] = new Hai(_jyunTehais[j]);

                        removeJyunTehaiAt(j);

                        _fuuros.Add( new Fuuro(EFuuroType.MinShun, hais, relation, newPickIndex) );

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // ポン(碰)の可否をチェックする
    public bool validPon(Hai suteHai)
    {
        if(_fuuros.Count >= FUURO_MAX)
            return false;

        int count = 1; // include the suteHai.
        for(int i = 0; i < _jyunTehais.Count; i++)
        {
            if(_jyunTehais[i].ID == suteHai.ID)
            {
                count++;

                if(count >= Tehai.MENTSU_LENGTH_3)
                    return true;
            }
        }

        return false;
    }

    // ポンを設定する
    public bool setPon(Hai suteHai, int relation)
    {
        if( !validPon(suteHai) )
            return false;

        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_3];

        hais[0] = new Hai(suteHai);
        int newPickIndex = 0;

        int count = 1;
        for(int i = 0; i < _jyunTehais.Count; i++) 
        {
            if( _jyunTehais[i].ID == suteHai.ID)
            {
                hais[count] = new Hai(_jyunTehais[i]);
                count++;

                removeJyunTehaiAt(i);
                i--;

                if(count >= Tehai.MENTSU_LENGTH_3)
                    break;
            }
        }

        _fuuros.Add( new Fuuro(EFuuroType.MinKou, hais, relation, newPickIndex) );

        return true;
    }


    // Any tsumo 槓の可否をチェックする
    public bool validAnyTsumoKan(Hai addHai, List<Hai> kanHais)
    {
        if( _fuuros.Count > FUURO_MAX ) // the 4th can kakan.
            return false;

        addJyunTehai(addHai);
        Sort();

        Hai checkHai = null;

        // 加槓のチェック
        for(int i = 0; i < _fuuros.Count; i++) 
        {
            if(_fuuros[i].Type == EFuuroType.MinKou)
            {
                checkHai = _fuuros[i].Hais[0];
                for(int j = 0; j < _jyunTehais.Count; j++) 
                {
                    if( _jyunTehais[j].ID == checkHai.ID ) 
                        kanHais.Add( new Hai(checkHai) );
                }
            }
        }

        if( _fuuros.Count >= FUURO_MAX ){
            removeJyunTehai(addHai);
            Sort();
            return kanHais.Count > 0;
        }

        // 暗槓のチェック
        checkHai = _jyunTehais[0];
        int count = 1;

        for(int i = 1; i < _jyunTehais.Count; i++) 
        {
            if( _jyunTehais[i].ID == checkHai.ID)
            {
                count++;
                if( count >= Tehai.MENTSU_LENGTH_4 )
                    kanHais.Add( new Hai( checkHai ) );
            }
            else{
                checkHai = _jyunTehais[i];
                count = 1;
            }
        }

        removeJyunTehai(addHai);
        Sort();

        return kanHais.Count > 0;
    }



    public bool validDaiMinKan(Hai suteHai)
    {
        if( _fuuros.Count >= FUURO_MAX )
            return false;

        int count = 1;
        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if(_jyunTehais[i].ID == suteHai.ID)
            {
                count++;
                if (count >= Tehai.MENTSU_LENGTH_4)
                    return true;
            }
        }

        return false;
    }

    // 大明槓を設定する
    public bool setDaiMinKan(Hai suteHai, int relation)
    {
        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_4];

        hais[0] = new Hai(suteHai);
        int newPickIndex = 0;

        int count = 1;

        for(int i = 0; i < _jyunTehais.Count; i++)
        {
            if( _jyunTehais[i].ID == suteHai.ID)
            {
                hais[count] = new Hai(_jyunTehais[i]);
                count++;

                removeJyunTehaiAt(i);
                i--;

                if( count >= Tehai.MENTSU_LENGTH_4 )
                    break;
            }
        }

        _fuuros.Add( new Fuuro(EFuuroType.DaiMinKan, hais, relation, newPickIndex) );

        return true;
    }


    // 加槓の可否をチェックする
    public bool validKaKan(Hai tsumoHai)
    {
        if( _fuuros.Count > FUURO_MAX )
            return false;

        for (int i = 0; i < _fuuros.Count; i++)
        {
            if (_fuuros[i].Type == EFuuroType.MinKou)
            {
                if( _fuuros[i].Hais[0].ID == tsumoHai.ID)
                    return true;
            }
        }

        return false;
    }

    // 加槓を設定する
    public bool setKaKan(Hai tsumoHai)
    {
        if( !validKaKan(tsumoHai) )
            return false;

        int relation = (int)ERelation.JiBun; //0;
        int newPickIndex = 3;

        for (int i = 0; i < _fuuros.Count; i++) 
        {
            if( _fuuros[i].Type == EFuuroType.MinKou ) 
            {
                if(_fuuros[i].Hais[0].ID == tsumoHai.ID)
                {
                    List<Hai> fuuroHais = new List<Hai>( _fuuros[i].Hais );
                    fuuroHais.Add( new Hai(tsumoHai) );

                    _fuuros[i].Update(EFuuroType.KaKan, fuuroHais.ToArray(), relation, newPickIndex);
                }
            }
        }

        return true;
    }


    // 暗槓の可否をチェックする
    public bool validAnKan(Hai tsumoHai)
    {
        if( _fuuros.Count >= FUURO_MAX )
            return false;

        int count = 1;
        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if(_jyunTehais[i].ID == tsumoHai.ID)
            {
                count++;
                if( count >= Tehai.MENTSU_LENGTH_4 )
                    return true;
            }
        }

        return false;
    }

    // 暗槓を設定する
    public bool setAnKan(Hai tsumoHai)
    {
        if( !validAnKan(tsumoHai) )
            return false;

        int relation = (int)ERelation.JiBun; //0;

        Hai[] hais = new Hai[Tehai.MENTSU_LENGTH_4];

        hais[0] = new Hai(tsumoHai);
        int newPickIndex = 3;

        int count = 1;
        for (int i = 0; i < _jyunTehais.Count; i++)
        {
            if(_jyunTehais[i].ID == tsumoHai.ID)
            {
                hais[count] = new Hai(_jyunTehais[i]);
                count++;

                removeJyunTehaiAt(i);
                i--;

                if( count >= Tehai.MENTSU_LENGTH_4 )
                    break;
            }
        }

        _fuuros.Add( new Fuuro(EFuuroType.AnKan, hais, relation, newPickIndex) );

        return true;
    }

}
