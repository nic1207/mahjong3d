using System.Collections.Generic;

/// <summary>
/// 副露を管理する。
/// 副露包括吃牌(チー),碰牌(ポン)和杠，也就是放桌角的那些牌.
/// </summary>

public class Fuuro 
{
    // 種別
    private EFuuroType _type = EFuuroType.MinShun;

    // 構成牌
    private Hai[] _hais = null;

    // 他家との関係(和其他人的关系, 新牌从谁那里得来)
    private int _fromRelation = -1;

    // index of the new hai in m_hais that is newly picked by player or from others(AI). 
    // 新牌的位置.
    private int _newPickIndex = -1;


    public Fuuro(Fuuro other)
    {
        copy( this, other );
    }

    public Fuuro(EFuuroType newType, Hai[] newHais, int newRelation, int newPickIndex)
    {
        this._type = newType;
        this._hais = newHais;
        this._fromRelation = newRelation;
        this._newPickIndex = newPickIndex;
    }


    public EFuuroType Type
    {
        get{ return _type; }
        set{ _type = value; }
    }

    public Hai[] Hais
    {
        get{ return _hais; }
        set{ _hais = value; }
    }

    public int FromRelation
    {
        get{ return _fromRelation; }
        set{ _fromRelation = value; }
    }

    public int NewPickIndex
    {
        get{ return _newPickIndex; }
        set{ _newPickIndex = value; }
    }


    public void Update(EFuuroType newType, Hai[] newHais, int newRelation, int newPick)
    {
        Type = newType;
        Hais = newHais;
        FromRelation = newRelation;
        NewPickIndex = newPick;
    }

    /// <summary>
    /// Copy the specified src furro to dest.
    /// 副露をコピーする
    /// </summary>

    public static void copy(Fuuro dest, Fuuro src)
    {
        dest._type = src._type;
        dest._fromRelation = src._fromRelation;
        dest._newPickIndex = src._newPickIndex;

        if( src._hais != null )
        {
            List<Hai> hai_copy = new List<Hai>();

            for(int i = 0; i < src._hais.Length; i++)
                hai_copy.Add( new Hai(src._hais[i]) );

            dest._hais = hai_copy.ToArray();
        }

    }

}
