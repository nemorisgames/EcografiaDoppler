using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public float PS, DV, Vm, SD, RI, PI;
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
			DVLineObject = currentLine.gameObject;
			DVLabel.text = Round(Max(DVLine)) + " mts/sec";
			break;
			case Mode.PS:
			PSLineObject = currentLine.gameObject;
			PSLabel.text = Round(Max(PSLine)) + " mts/sec";
			break;
			case Mode.Vm:
			VmLineObject = currentLine.gameObject;
			VMLabel.text = Round(Avg(VmLine)) + " mts/sec";
			break;
		}
	}

	string Round(float num, int decimals = 3){
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
		if(PSLine.Count != 0){
			PS = Max(PSLine);
		}

		if(DVLine.Count != 0){
			DV = Max(DVLine);
		}

		if(VmLine.Count != 0){
			Vm = Avg(VmLine);
		}

		if(PSLine.Count != 0 && DVLine.Count != 0){
			SD = PS/DV;
			RI = (PS-DV)/PS;
			SDLabel.text = Round(SD) + " mts/sec";
			RILabel.text = Round(RI) + " mts/sec";

			if(VmLine.Count != 0){
				PI = (PS-DV)/Vm;
				PILabel.text = Round(PI) + " mts/sec";
			}
		}
	}

	public float Max(List<Vector2> list){
		if(GraphController.Instance.inverseFunction)
			return Mathf.Min(list[0].y,list[list.Count-1].y);
		else
			return Mathf.Max(list[0].y,list[list.Count-1].y);
	}

	public float Avg(List<Vector2> list){
		float sum = 0;
		foreach(Vector2 v2 in list)
			sum += v2.y;
		return sum/list.Count;
	}

	public void ActivarPanelLineas(bool b){
		EraseAllLines();
		panel.alpha = b ? 1 : 0;
	}
}
