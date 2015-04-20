using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	private AsyncOperation ao;
	private Transform status;
	// Use this for initialization
	void Start () {
		status = transform.FindChild ("Progress");
		StartCoroutine (Load());
	}
	
	// Update is called once per frame
	void Update () {
		if(ao != null)
		{
			if(status != null)
				status.localScale = new Vector3(ao.progress, 1, 1);
			if (ao.progress >= .9f)
				//ao.allowSceneActivation = true;
			Debug.Log (ao.progress);
		}
	}

	public IEnumerator Load()
	{
		ao = Application.LoadLevelAsync ("Scene");
		ao.allowSceneActivation = false;
		yield return ao;
	}
}
