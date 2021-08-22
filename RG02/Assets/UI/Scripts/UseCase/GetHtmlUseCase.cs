using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;//UnityWebRequestを使うために必要

public class GetHtmlUseCase : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		StartCoroutine(GetText());
	}

	IEnumerator GetText()
	{
		/*取得したいサイトURLを指定*/
		UnityWebRequest www = UnityWebRequest.Get("https://sirohood.exp.jp/20190208-1792/");
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
		}
		else
		{
			// 結果をテキストとして表示します
			Debug.Log(www.downloadHandler.text);

			//  または、結果をバイナリデータとして取得します
			byte[] results = www.downloadHandler.data;
		}
	}
}
