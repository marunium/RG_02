using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

public class SearchTagCanvasView : MonoBehaviour
{
	[SerializeField] private InputField inputField;
	[SerializeField] private Button searchButton;
	[SerializeField] private Text resultText;

	private GetHtmlUseCase getHtmlUseCase = new GetHtmlUseCase();

	private void Start()
	{
		OnClickSearchButtonAsObservable()
			.Do(_ => SetText(string.Empty))
			//.SelectMany(x => getHtmlUseCase.GetTextAsObservable("https://sirohood.exp.jp/20190208-1792/"))
			.SelectMany(x => getHtmlUseCase.GetTextAsObservable(inputField.text))
			.Subscribe(SetText)
			.AddTo(this);
	}

	public IObservable<string> OnEndEditAsObservable()
	{
		return inputField.OnEndEditAsObservable();
	}

	public IObservable<Unit> OnClickSearchButtonAsObservable()
	{
		return searchButton.OnClickAsObservable();
	}

	public void SetText(string text)
	{
		resultText.text = text;
	}
}
