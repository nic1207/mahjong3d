using UnityEngine;
using System.Collections;
/// <summary>
/// Static coroutine.
/// </summary>
public class StaticCoroutine : MonoBehaviour {
	
	private static StaticCoroutine _instance = null;

	public static StaticCoroutine Instance()
	{
		if (_instance == null) {
			_instance = GameObject.FindObjectOfType (typeof(StaticCoroutine)) as StaticCoroutine;

			if (_instance == null) {
				GameObject ob = new GameObject ("StaticCoroutine");
				_instance = ob.AddComponent<StaticCoroutine> () as StaticCoroutine;
			}
		}
		return _instance;

	}

	IEnumerator Perform(IEnumerator coroutine)
	{
		yield return StartCoroutine(coroutine);
		Die();
	}

	/// <summary>
	/// Place your lovely static IEnumerator in here and witness magic!
	/// </summary>
	/// <param name="coroutine">Static IEnumerator</param>
	public static void DoCoroutine(IEnumerator coroutine)
	{
		_instance.StartCoroutine(_instance.Perform(coroutine)); //this will launch the coroutine on our instance
	}

	void Die()
	{
		_instance = null;
		Destroy(gameObject);
	}

	void OnApplicationQuit()
	{
		_instance = null;
	}
}
