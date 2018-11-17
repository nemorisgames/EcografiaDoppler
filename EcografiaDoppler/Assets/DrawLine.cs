using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawLine : MonoBehaviour {
	private static DrawLine _instance;
	public static DrawLine Instance { get{ return _instance;}}
	public enum Mode{
		PS,
		DV,
		Vm
	}

	public Mode mode;
	public UIPanel panel;
	public float PS, PD, DV, Vm, SD, RI, PI;
	public UILabel PSLabel, DVLabel, VMLabel, SDLabel,RILabel,PILabel;
	public GameObject target;
	private LineRenderer currentLine;
	public bool drawingLine;
	private float nextPointTime;
	public float sampleTime = 0.05f;
	private List<Vector3> lrPoints;
	public List<Vector2> PSLine, DVLine, VmLine;
	private GameObject PSLineObject, DVLineObject, VmLineObject;
	public GameObject lrPrefab;
	private GraphController graph;
	public UIButton PSBtn, DVBtn, VmBtn;

	void Awake(){
		if(_instance == null)
			_instance = this;

		graph = GameObject.FindObjectOfType<GraphController>();
	}

	void Start () {
		lrPoints = new List<Vector3>();
		VmLine = new List<Vector2>();
		PSLine = new List<Vector2>();
		DVLine = new List<Vector2>();
		ActivarPanelLineas(false);
	}

	void UpdateButtonColors(){
		DVBtn.defaultColor = Color.gray;
		PSBtn.defaultColor = Color.gray;
		VmBtn.defaultColor = Color.gray;
		switch(mode){
			case Mode.DV:
			DVBtn.defaultColor = Color.white;
			break;

			case Mode.PS:
			PSBtn.defaultColor = Color.white;
			break;

			case Mode.Vm:
			VmBtn.defaultColor = Color.white;
			break;
		}
		DVBtn.UpdateColor(true);
		VmBtn.UpdateColor(true);
		PSBtn.UpdateColor(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(panel.alpha == 0)
			return;

		UpdateButtonColors();

		if(Input.GetMouseButtonDown(0) && !drawingLine && IsTarget()){
			StartLine();
		}

		if(drawingLine && Time.time > nextPointTime){
			DrawPoint();
		}
		
		if(Input.GetMouseButtonUp(0) && drawingLine)
			StopLine();

		/* if(Input.GetMouseButtonDown(1))
			EraseLastLine();*/

		if(Input.GetKeyDown(KeyCode.Space))
			EraseAllLines();
	}

	bool IsTarget(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray,out hit,100f)){
			if(hit.collider.gameObject == target){
				return true;
			}
		}
		return false;
	}

	void StartLine(){
		switch(mode){
			case Mode.DV:
			DVLine.Clear();
			DVLabel.text = "";
			Destroy(DVLineObject);
			break;
			case Mode.PS:
			PSLine.Clear();
			PSLabel.text = "";
			Destroy(PSLineObject);
			break;
			case Mode.Vm:
			VmLine.Clear();
			VMLabel.text = "";
			Destroy(VmLineObject);
			break;
		}
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		point.z = -8;
		/*if(IsTarget() && !GraphController.Instance.IsInWaveRange(point)){
			return;
		}*/
		drawingLine = true;
		GameObject go = Instantiate(lrPrefab,transform.position,transform.rotation,transform);
		currentLine = go.GetComponent<LineRenderer>();
		currentLine.positionCount = 0;
		switch(mode){
			case Mode.DV:
			case Mode.PS:
			currentLine.material.color = Color.red;
			break;
			case Mode.Vm:
			currentLine.material.color = Color.yellow;
			break;
		}
	}

	void DrawPoint(){
		nextPointTime = Time.time + sampleTime;
		Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		point.z = -8;
		if(IsTarget()){
			if(mode != Mode.Vm && lrPoints.Count != 0){
				point.x = lrPoints[0].x;
			}
			/*if(!GraphController.Instance.IsInWaveRange(point)){
				StopLine();
				return;
			}*/
			lrPoints.Add(point);
			currentLine.positionCount = lrPoints.Count;
			currentLine.SetPositions(lrPoints.ToArray());
			switch(mode){
				case Mode.DV:
				DVLine.Add(graph.WorldSpaceToQuadSpace(point));
				break;
				case Mode.PS:
				PSLine.Add(graph.WorldSpaceToQuadSpace(point));
				break;
				case Mode.Vm:
				VmLine.Add(graph.WorldSpaceToQuadSpace(point));
				break;
			}
		}
	}

	void StopLine(){
		drawingLine = false;
		lrPoints.Clear();
		switch(mode){
			case Mode.DV:
			if(CheckBounds(Max(DVLine))){
				DVLineObject = currentLine.gameObject;
				DV = Max(DVLine);
				DVLabel.text = Round(Max(DVLine)) + " cm/sec";
			}
			else{
				Destroy(currentLine.gameObject);
				DVLine.Clear();
				DV = 0;
			}
			break;
			case Mode.PS:
			if(CheckBounds(Max(PSLine))){
				PSLineObject = currentLine.gameObject;
				PS = Max(PSLine);
				PSLabel.text = Round(Max(PSLine)) + " cm/sec";
			}
			else{
				Destroy(currentLine.gameObject);
				PSLine.Clear();
				PS = 0;
			}
			break;
			case Mode.Vm:
			Vm = Avg(VmLine);
			VmLineObject = currentLine.gameObject;
			VMLabel.text = Round(Avg(VmLine)) + " cm/sec";
			break;
		}
	}

	bool CheckBounds(float value){
		//Debug.Log(Mathf.Abs(value));
		if(Mathf.Abs(value) > 100){
			return false;
		}
		else return true;
	}

	string Round(float num, int decimals = 2){
		float aux = (Mathf.Round(num * Mathf.Pow(10,decimals)))/Mathf.Pow(10,decimals);
		return aux.ToString();
	}

	void EraseAllLines(){
		transform.DestroyChildren();
		DVLineObject = PSLineObject = VmLineObject = null;
		PSLine.Clear();
		DVLine.Clear();
		VmLine.Clear();
		PSLabel.text = "";
		DVLabel.text = "";
		VMLabel.text = "";
		SDLabel.text = "";
		PILabel.text = "";
		RILabel.text = "";
	}

	void EraseLastLine(){
		int erase = transform.childCount - 1;
		if(erase >= 0){
			Destroy(transform.GetChild(erase).gameObject);
		}
	}

	public void PSClickBtn(){
		mode = Mode.PS;
	}

	public void DVClickBtn(){
		mode = Mode.DV;
	}

	public void VmClickBtn(){
		mode = Mode.Vm;
	}

	public void Calculate(){
		if(PSLine.Count != 0 && DVLine.Count != 0){
			SD = Normalize(PS/DV);
			RI = Normalize((PS-PD)/PS);
			SDLabel.text = Round(SD);
			RILabel.text = Round(RI);

			if(VmLine.Count != 0){
				PI = Normalize((PS-PD)/Vm);
				PILabel.text = Round(PI);
			}
		}
	}

	public float Max(List<Vector2> list){
		Normalize(list);
		if(GraphController.Instance.inverseFunction)
			return Mathf.Min(list[0].y,list[list.Count-1].y);
		else
			return Mathf.Max(list[0].y,list[list.Count-1].y);
	}

	public float Avg(List<Vector2> list){
		Normalize(list);
		float sum = 0;
		foreach(Vector2 v in list)
			sum += v.y;
		/*for(int i = 0; i < list.Count; i++){
			if(i + 1 < list.Count){
				float velocity = (list[i + 1].y - list[i].y);
				sum += velocity;
				Debug.Log(velocity);
			}
		}
		return sum;*/
		return sum/list.Count;
	}

	public void ActivarPanelLineas(bool b){
		EraseAllLines();
		panel.alpha = b ? 1 : 0;
	}

	public void SetPD(float f){
		PD = Normalize(f);
	}

	float Normalize(float f){
        if(SceneManager.GetActiveScene().name == "EcografiaUmbilical")
            return f * 0.92f;
		else if(SceneManager.GetActiveScene().name == "EcografiaCerebral")
            return f * 0.88f;
		else if(SceneManager.GetActiveScene().name == "EcografiaDuctus")
            return f * 0.82f;
        else if(SceneManager.GetActiveScene().name == "EcografiaUtero")
            return f * 0.80f;
        else
            return f * 1f;
	}

	void Normalize(List<Vector2> list){
		Vector2 aux;
		for(int i = 0; i < list.Count; i++){
			aux = list[i];
			aux.y = Normalize(aux.y);
			list[i] = aux;
		}
	}
}
