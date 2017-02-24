using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Res manager.
/// </summary>
public class ResManager 
{
    private static Dictionary<string, string> StringTable;

    public static void LoadStringTable(bool useJapanese = false)
    {
        if( StringTable == null ){
            StringTable = new Dictionary<string, string>();

            string fileName = "string_cn";
            if(useJapanese) fileName = "string_jp";

            TextAsset ta = Resources.Load<TextAsset>("Table/" + fileName);

            List<string> valueList = new List<string>();

            List<object> stringList = (List<object>)MiniJSON.Deserialize(ta.text);
            for( int i = 0; i < stringList.Count; i++ )
            {
                Dictionary<string, object> kv = (Dictionary<string, object>)stringList[i];

                foreach(var kvs in kv)
                    valueList.Add(kvs.Value.ToString());
            }

            for( int i = 0; i < valueList.Count-1; i += 2 )
                StringTable.Add( valueList[i], valueList[i+1] );

            //PrintStringTable();
        }
    }

    static void PrintStringTable()
    {
        Debug.Log("---------------- String Table -------------");
        foreach( var kvs in StringTable )
        {
            Debug.Log( kvs.Value );
        }
    }

    public static string getString(string key)
    {
        LoadStringTable();

        string value;
        if( StringTable.TryGetValue(key, out value) ){
            return value;
        }
        return key;
    }


    private static List<GameObject> _mahjongPaiPool = new List<GameObject>();
    private static Transform poolRoot = null;
    private static GameObject mahjongPaiPrefab = null;


    public static void SetPoolRoot(Transform root)
    {
        poolRoot = root;
    }

    public static void ClearMahjongPaiPool()
    {
        for(int i = 0; i < _mahjongPaiPool.Count; i++)
        {
            if(_mahjongPaiPool[i] != null)
                GameObject.Destroy( _mahjongPaiPool[i] );
        }
        _mahjongPaiPool.Clear();
    }

    public static GameObject CreateMahjongObject()
    {
        if(_mahjongPaiPool.Count > 0)
        {
            GameObject pai = _mahjongPaiPool[0].gameObject;
            pai.SetActive(true);

            _mahjongPaiPool.RemoveAt(0);

            return pai;
        } else {
            if( mahjongPaiPrefab == null )
                mahjongPaiPrefab = Resources.Load<GameObject>("Prefabs/mj");
            return Object.Instantiate(mahjongPaiPrefab) as GameObject;
        }
    }

    public static bool CollectMahjongPai(MahjongPai pai)
    {
        if( pai == null )
            return false;

        pai.Clear();

        if(poolRoot == null)
            poolRoot = new GameObject("MahjongPoolRoot").transform;

        pai.transform.parent = poolRoot;
		Utils.SetLayerRecursively (pai.gameObject, LayerMask.NameToLayer ("Default"));
        pai.gameObject.SetActive(false);
        _mahjongPaiPool.Add( pai.gameObject );

        return true;
    }


    public static GameObject CreatePlayerUIObject()
    {
		//GameObject go = Object.Instantiate (Resources.Load<GameObject> ("Prefabs/PlayerUI")) as GameObject;
        GameObject go = Object.Instantiate(Resources.Load<GameObject>("Prefabs/PlayerUI_New")) as GameObject;
        go.name = "PlayerUI";
        return go;
    }

	public static string getMagjongName(int kind, int num) {
		string head = "w";
		if(kind == Hai.KIND_WAN){
			head = "萬";
		}
		else if( kind == Hai.KIND_PIN) {
			head = "筒";
		}
		else if( kind == Hai.KIND_SOU ) {
			head = "條";
		}
		else if( kind == Hai.KIND_FON ) {
			head = "";
			if (num == 1) {
				head = "東";
			} else if (num == 2) {
				head = "南";
			} else if (num == 3) {
				head = "西";
			} else if (num == 4) {
				head = "北";
			}
		}
		else if( kind == Hai.KIND_SANGEN ) {
			head = "";
			if (num == 1) {
				head = "中";
			} else if (num == 2) {
				head = "發";
			} else if (num == 3) {
				head = "白";
			}
		}
		else {
			Debug.LogError("Unknown mahjong kind of " + kind);
		}
		string name = string.Empty;
		//if (kind == Hai.KIND_SHUU)
		name = num + head;
		//else
		//	name = head;
		return name;
	}

	public static string getMahjongTextureName(int kind, int num) {
		string head = "w";
		if(kind == Hai.KIND_WAN){
			head = "w";
		}
		else if( kind == Hai.KIND_PIN) {
			head = "tong";
		}
		else if( kind == Hai.KIND_SOU ) {
			head = "tiao";
		}
		else if( kind == Hai.KIND_FON ) {
			head = "c";
		}
		else if( kind == Hai.KIND_SANGEN ) {
			head = "c";
			num += 4;
		}
		else {
			Debug.LogError("Unknown mahjong kind of " + kind);
		}
		string name = head + "_" + num;
		return name;
	}

	public static Texture getMahjongTexture(int kind, int num) {
        string head = "w";
        if(kind == Hai.KIND_WAN){//萬
            head = "w";
        }
        else if( kind == Hai.KIND_PIN) {//筒
            head = "tong";
        }
        else if( kind == Hai.KIND_SOU ) {//條
            head = "tiao";
        }
        else if( kind == Hai.KIND_FON ) {//風
            head = "c";
        }
		else if( kind == Hai.KIND_SANGEN ) {//三元牌
            head = "c";
            num += 4;
        }
        else {
            Debug.LogError("Unknown mahjong kind of " + kind);
        }
		string name = head + "_" + num;
		//Debug.Log ("name="+name);
		string path = "Textures/Mahjong/q1/" + name;
		Texture t = Resources.Load (path) as Texture;
		if (t == null)
			Debug.LogError (path +" not found!!");
        return t;
    }

	public static Sprite getMahjongSprite(int kind, int num) {
		string head = "w";
		if(kind == Hai.KIND_WAN){//萬
			head = "w";
		}
		else if( kind == Hai.KIND_PIN) {//筒
			head = "tong";
		}
		else if( kind == Hai.KIND_SOU ) {//條
			head = "tiao";
		}
		else if( kind == Hai.KIND_FON ) {//風
			head = "c";
		}
		else if( kind == Hai.KIND_SANGEN ) {//三元牌
			head = "c";
			num += 4;
		}
		else {
			Debug.LogError("Unknown mahjong kind of " + kind);
		}
		string name = head + "_" + num;
		//Debug.Log ("name="+name);
		string path = "Sprites/q2/" + name;
		Sprite s = Resources.Load<Sprite> (path) as Sprite;
		if (s == null)
			Debug.LogError (path +" not found!!");
		//Sprite s = Sprite.Create (t, new Rect (0, 0, 38, 53), Vector2.zero);
		return s;
	}

	public static Sprite getSprite(string name) {
		string path = "Sprites/q2/" + name;
		Sprite s = Resources.Load<Sprite> (path) as Sprite;
		if (s == null)
			Debug.LogError (path +" not found!!");
		return s;
	}

    public static Sprite getChiiPonGanSprite(int _index)
    {
        string path = null;

        if (_index == 0)
            path = "image/playing-04";
        else
            path = "image/playing-09";

        Sprite s = Resources.Load<Sprite>(path) as Sprite;
        if (s == null)
            Debug.LogError(path + " not found!!");
        return s;
    }

    public static Sprite getSelectCCSprite(int num)
    {
        string path = "image/playing-c" + num;
        Sprite s = Resources.Load<Sprite>(path) as Sprite;
        if (s == null)
            Debug.LogError(path + " not found!!");
        return s;
    }
}
