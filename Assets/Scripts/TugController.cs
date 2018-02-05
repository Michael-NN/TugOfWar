using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TugController : MonoBehaviour {
	/*----------------------------------------------------------------------------------------------------------
	 * Land of Records on AI parameters
	 * Bot
	 * -0.065,10,1
	 * -0.1,10,0.5
	 * Rand
	 * -
	 * VS Data
	 * -B:0.05,10,0.5 v R:0.05,0.1625
	 * --49:51 avgtim 28s
	*///----------------------------------------------------------------------------------------------------------
	public float strength;
	public float radius;
	public GameObject redText;
	public GameObject bluText;
	public GameObject midText;
	public GameObject lowText;
	public GameObject redFlag;
	public GameObject bluFlag;
	public bool simulating;

	public bool redBotOn;
	public float redRate;
	public int redClip;
	public float redReload;

	public bool redRandOn;
	public float redRandMin;
	public float redRandMax;

	public bool redTimeOn;
	public float[] redTimeParam;

	public bool redSpaceOn;
	public float[] redSpaceParam;


	public bool bluBotOn;
	public float bluRate;
	public int bluClip;
	public float bluReload;

	public bool bluRandOn;
	public float bluRandMin;
	public float bluRandMax;

	public bool bluTimeOn;
	public float[] bluTimeParam;

	public bool bluSpaceOn;
	public float[] bluSpaceParam;


	private Rigidbody2D myBody;
	private int redScore;
	private int bluScore;
	private bool onGoing;
	private Text redTextT;
	private Text bluTextT;
	private Text midTextT;
	private Text lowTextT;

	private bool redBotTugging;
	private bool redRandTugging;
	private bool redTimeTugging;
	private bool redSpaceTugging;

	private bool bluBotTugging;
	private bool bluRandTugging;
	private bool bluTimeTugging;
	private bool bluSpaceTugging;


	// Use this for initialization
	void Start () {
		myBody = GetComponent<Rigidbody2D> ();
		onGoing = false;
		redTextT = redText.GetComponent<Text> ();
		bluTextT = bluText.GetComponent<Text> ();
		midTextT = midText.GetComponent<Text> ();
		lowTextT = lowText.GetComponent<Text> ();
		redTextT.text = "0";
		bluTextT.text = "0";
		midTextT.text = "";
		lowTextT.text = "";
		redTextT.color = Color.red;
		bluTextT.color = Color.blue;
		midTextT.color = Color.black;
		lowTextT.color = Color.black;
		redFlag.transform.position = (new Vector3(-radius,-0.75f,1.0f));
		bluFlag.transform.position = (new Vector3(radius,-0.75f,1.0f));
		redBotTugging = false;
		redRandTugging = false;
		redTimeTugging = false;
		redSpaceTugging = false;
		bluBotTugging = false;
		bluRandTugging = false;
		bluTimeTugging = false;
		bluSpaceTugging = false;

		midTextT.text = "Press 'R' to start";
	}
	
	// Update is called once per frame
	void Update () {
		if (onGoing) {
			if (Input.GetKeyDown ("space")) {
				myBody.velocity += new Vector2 (-1.0f * strength, 0.0f);
			}
			if (Input.GetKeyDown ("return")) {
				myBody.velocity += new Vector2 (1.0f * strength, 0.0f);
			}
			if (transform.position.x <= -radius) {
				victory (0);
			}
			if (transform.position.x >= radius) {
				victory (1);
			}
			if(redBotOn&&!redBotTugging){
				StartCoroutine (redBotTug ());
			}
			if(redRandOn&&!redRandTugging){
				StartCoroutine (redRandTug ());
			}
			if(redTimeOn&&!redTimeTugging){
				StartCoroutine (redTimeTug ());
			}
			if(redSpaceOn&&!redSpaceTugging){
				StartCoroutine (redSpaceTug ());
			}
			if(bluBotOn&&!bluBotTugging){
				StartCoroutine (bluBotTug ());
			}
			if(bluRandOn&&!bluRandTugging){
				StartCoroutine (bluRandTug ());
			}
			if(bluTimeOn&&!bluTimeTugging){
				StartCoroutine (bluTimeTug ());
			}
			if(bluSpaceOn&&!bluSpaceTugging){
				StartCoroutine (bluSpaceTug ());
			}
		} else {
			if(Input.GetKeyDown("r")){
				transform.position = new Vector2 (0.0f,0.0f);
				midTextT.text = "";
				lowTextT.text = "";
				StartCoroutine (countDown());
			}
			myBody.velocity = new Vector2 (0.0f, 0.0f);
		}
	}

	private void victory(int winner){
		onGoing = false;
		if(winner==0){
			redScore++;
			redTextT.text = "" + redScore;
			midTextT.text = "Left Wins!";
			lowTextT.text = "Press 'R' for rematch!";
		}
		else{
			bluScore++;
			bluTextT.text = "" + bluScore;
			midTextT.text = "Right Wins!";
			lowTextT.text = "Press 'R' for rematch!";
		}
		/**/
		if(simulating){
			transform.position = new Vector2 (0.0f,0.0f);
			midTextT.text = "";
			lowTextT.text = "";
			StartCoroutine (countDown());
		}
		/**/
	}

	private IEnumerator countDown(){
		midTextT.text = "3";
		yield return new WaitForSecondsRealtime (1.0f);
		midTextT.text = "2";
		yield return new WaitForSecondsRealtime (1.0f);
		midTextT.text = "1";
		yield return new WaitForSecondsRealtime (1.0f);
		midTextT.text = "Go!";
		onGoing = true;
		yield return new WaitForSecondsRealtime (1.0f);
		midTextT.text = "";
	}

	private IEnumerator redBotTug(){
		redBotTugging = true;
		while(onGoing){
			for(int i=redClip; i>0; i--){
				myBody.velocity += new Vector2 (-1.0f * strength, 0.0f);
				yield return new WaitForSecondsRealtime (redRate);
			}
			yield return new WaitForSecondsRealtime (redReload);
		}
		redBotTugging = false;
	}

	private IEnumerator redRandTug(){
		redRandTugging = true;
		while(onGoing){
			myBody.velocity += new Vector2 (-1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime(Random.Range (redRandMin, redRandMax));
		}
		redRandTugging = false;
	}

	private IEnumerator redTimeTug(){
		redTimeTugging = true;
		float startTime = Time.time;
		while(onGoing){
			myBody.velocity += new Vector2 (-1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime (redTimeFunc (Time.time - startTime));
		}
		redTimeTugging = false;
	}
	private float redTimeFunc(float t){
		float result = t;
		return Mathf.Clamp (result, 0.0f, 10.0f);
	}

	private IEnumerator redSpaceTug(){
		redSpaceTugging = true;
		while(onGoing){
			myBody.velocity += new Vector2 (-1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime (redSpaceFunc (-transform.position.x));
		}
		redSpaceTugging = false;
	}
	private float redSpaceFunc(float t){
		float result = (1 / (-t + redSpaceParam[0]))-(1/(radius+redSpaceParam[0]));
		return Mathf.Clamp (result, 0.0f, 10.0f);
	}

	private IEnumerator bluBotTug(){
		bluBotTugging = true;
		while(onGoing){
			for(int i=bluClip; i>0; i--){
				myBody.velocity += new Vector2 (1.0f * strength, 0.0f);
				yield return new WaitForSecondsRealtime (bluRate);
			}
			yield return new WaitForSecondsRealtime (bluReload);
		}
		bluBotTugging = false;
	}

	private IEnumerator bluRandTug(){
		bluRandTugging = true;
		while(onGoing){
			myBody.velocity += new Vector2 (1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime(Random.Range (bluRandMin, bluRandMax));
		}
		bluRandTugging = false;
	}

	private IEnumerator bluTimeTug(){
		bluTimeTugging = true;
		float startTime = Time.time;
		while(onGoing){
			myBody.velocity += new Vector2 (1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime (bluTimeFunc (Time.time - startTime));
		}
		bluTimeTugging = false;
	}
	private float bluTimeFunc(float t){
		float result = bluTimeParam[0]+bluTimeParam[1]*Mathf.Sin(bluTimeParam[2]+bluTimeParam[3]*t);
		return Mathf.Clamp (result, 0.0f, 10.0f);
	}

	private IEnumerator bluSpaceTug(){
		bluSpaceTugging = true;
		while(onGoing){
			myBody.velocity += new Vector2 (1.0f * strength, 0.0f);
			yield return new WaitForSecondsRealtime (bluSpaceFunc (transform.position.x));
		}
		bluSpaceTugging = false;
	}
	private float bluSpaceFunc(float t){
		float result = (1 / (-t + bluSpaceParam[0]))-(1/(radius+bluSpaceParam[0]));
		return Mathf.Clamp (result, 0.0f, 10.0f);
	}

}
