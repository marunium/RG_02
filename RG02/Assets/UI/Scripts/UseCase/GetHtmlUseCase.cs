using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Networking;//UnityWebRequestを使うために必要
using resultHtml = System.Text.RegularExpressions.MatchCollection;
using System;
using System.Text;

public class GetHtmlUseCase : MonoBehaviour
{
	public IObservable<string> GetTextAsObservable(string url)
	{
		return Observable.FromCoroutine<string>(observer => GetTextCoroutine(observer, url));
	}

	private IEnumerator GetTextCoroutine(IObserver<string> observer, string url)
	{
		/*取得したいサイトURLを指定*/
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();

		if (www.isNetworkError || www.isHttpError)
		{
			observer.OnNext(www.error);
			observer.OnCompleted();
		}
		else
		{
			// 結果をテキストとして表示します
			//Debug.Log(www.downloadHandler.text);

			//  または、結果をバイナリデータとして取得します
			byte[] results = www.downloadHandler.data;

			//正規表現パターンとオプションを指定してRegexオブジェクトを作成
			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(
					@"<(h[1-6])\b[^>]*>(.*?)</\1>",
					System.Text.RegularExpressions.RegexOptions.IgnoreCase
					| System.Text.RegularExpressions.RegexOptions.Singleline);

			//TextBox1.Text内で正規表現と一致する対象をすべて検索
			resultHtml mc = r.Matches(www.downloadHandler.text);

			//foreach (System.Text.RegularExpressions.Match m in mc)
			//{
			//	//正規表現に一致したグループと位置を表示
			//	Debug.Log("タグ:" + m.Groups[1].Value +
			//		"\nタグ内の文字列:" + m.Groups[2].Value +
			//		"\nタグの位置:" + m.Groups[1].Index);
			//}

			var stringBuilder = new StringBuilder();
			foreach (System.Text.RegularExpressions.Match m in mc)
			{
				var text = $"[{ m.Groups[1].Value}]:{m.Groups[2].Value}\n";
				stringBuilder.Append(text);
			}

			observer.OnNext(stringBuilder.ToString());
			observer.OnCompleted();
		}
	}
}
