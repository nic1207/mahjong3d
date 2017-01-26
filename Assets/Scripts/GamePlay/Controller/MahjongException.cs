using UnityEngine;
using System.Collections;


public class MahjongException : System.Exception
{
    public MahjongException() : base(){

    }
    public MahjongException(string msg) : base(msg){

    }
    public MahjongException(string format, params object[] args) 
        : base( string.Format(format, args) ){

    }
}
