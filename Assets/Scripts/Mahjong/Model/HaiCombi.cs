using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 上がりの組み合わせのクラスです
/// 
/// 胡牌的组合: 包括刻子(槓、碰)、顺子,
/// 頭(atama)是剩余那个牌
/// </summary>

public class HaiCombi 
{
    // 頭のNK
    public int atamaNumKind = 0;

    // 順子のNKの配列 (store the left hai's NK of a shun mentsu)
    public int[] shunNumKinds = new int[5];

    // 順子のNKの配列の有効な個数
    public int shunCount = 0;

    // 刻子のNKの配列
    public int[] kouNumKinds = new int[5];

    // 刻子のNKの配列の有効な個数
    public int kouCount = 0;

    //public List<int> shunNumKinds = new List<int>();
    //public List<int> kouNumKinds = new List<int>();


    public void Clear()
    {
        this.atamaNumKind = 0;

        for(int i = 0; i < shunNumKinds.Length; i++){
            shunNumKinds[i] = 0;
        }
        this.shunCount = 0;

        for(int i = 0; i < kouNumKinds.Length; i++){
            kouNumKinds[i] = 0;
        }
        this.kouCount = 0;

        //shunNumKinds.Clear();
        //kouNumKinds.Clear();
    }

    public static void copy(HaiCombi dest, HaiCombi src)
    {
        dest.Clear();

        dest.atamaNumKind = src.atamaNumKind;

        dest.shunCount = src.shunCount;
        for( int i = 0; i < dest.shunCount; i++ )
            dest.shunNumKinds[i] = src.shunNumKinds[i];

        dest.kouCount = src.kouCount;
        for( int i = 0; i < dest.kouCount; i++ )
            dest.kouNumKinds[i] = src.kouNumKinds[i];

        /*
        for( int i = 0; i < src.shunNumKinds.Count; i++ )
            dest.shunNumKinds.Add( src.shunNumKinds[i] );
        for( int i = 0; i < src.kouNumKinds.Count; i++ )
            dest.kouNumKinds.Add( src.kouNumKinds[i] );
        */
    }
}
