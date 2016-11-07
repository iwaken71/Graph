using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

public class GraphView : MonoBehaviour {

	public bool view2D = false;
	[RangeReactiveProperty(0f,1f)]
	public FloatReactiveProperty alpha,nodeBaseSize,nodeSizeUp,edgeAlpha;
	[RangeReactiveProperty(0f,0.2f)]
	public FloatReactiveProperty edgeWidth;



	Graph graph;

	Dictionary<int,Vector3> nodePosition = new Dictionary<int,Vector3> ();
	Dictionary<int,GameObject> nodeObj = new Dictionary<int,GameObject> ();

	GameObject nodeParent,edgeParent;
	GameObject nodePrefab,edgePrefab;

	void Awake(){
		nodeParent = new GameObject ("Node");
		edgeParent = new GameObject ("Edge");
		nodeParent.transform.parent = this.transform;
		edgeParent.transform.parent = this.transform;
		nodePrefab = Resources.Load ("Node")as GameObject;
		edgePrefab = Resources.Load ("Edge")as GameObject;
	}

	void Start(){
		nodeBaseSize.Value = 0.2f;
		nodeSizeUp.Value = 0.1f;
		alpha.Value = 1.0f;
		edgeWidth.Value = 0.05f;
		edgeAlpha.Value = 0.2f;
		nodeBaseSize.Subscribe(x => {
			NodeSizeUpdate(x,nodeSizeUp.Value);
		});
		nodeSizeUp.Subscribe(x => {
			NodeSizeUpdate(nodeBaseSize.Value,x);
		});

		alpha.Subscribe (x => {
			NodeColorAlphaUpdate (x);
		});
		edgeWidth.Subscribe (x => {
			DrawAllEdge();
		});

		edgeAlpha.Subscribe (x=>{
			DrawAllEdge();
		});

	}

	public void SetGraph(Graph input){
		this.graph = input;

		foreach (Node node in graph.Nodes) {
			GameObject obj = Instantiate (nodePrefab)as GameObject;
			obj.name = node.ID.ToString ();
			int degree = node.neighbor.Count;
			obj.transform.localScale = Vector3.one * (nodeBaseSize.Value+ degree*nodeSizeUp.Value);
			obj.transform.SetParent (nodeParent.transform);
			obj.SetActive (false);
			nodeObj.Add (node.ID,obj);
			Vector3 pos = randomPos ();
			nodePosition.Add (node.ID,pos);
		}
	}

	public void SetColor(int id,Color col){
		nodeObj [id].GetComponent<Renderer> ().material.color = col;
	}



	void NodeSizeUpdate(float nodeBaseSize,float nodeSizeUp){
		if (graph == null)
			return;
		foreach (Node node in graph.Nodes) {
			GameObject obj = nodeObj [node.ID];
			int degree = node.neighbor.Count;
			obj.transform.localScale = Vector3.one * (nodeBaseSize+ degree*nodeSizeUp);
		}
	}

	void NodeColorAlphaUpdate(float a){
		if (graph == null)
			return;
		foreach (Node node in graph.Nodes) {
			GameObject obj = nodeObj [node.ID];
			Color c = obj.GetComponent<Renderer> ().material.color;
			obj.GetComponent<Renderer> ().material.color = new Color (c.r,c.g,c.b,a);

		}
	}





	/*
	public void AddEdge(int id1,int id2){
		if (graph == null) {
			Debug.LogError ("graphがセットされていません");
			return;
		}
			
		bool existNode1 = graph.Nodes.Any (node => node.ID == id1);
		bool existNode2 = graph.Nodes.Any (node => node.ID == id2);
		graph.AddEdge (id1,id2);
		if (!existNode1) {
			GameObject obj = Instantiate (nodePrefab)as GameObject;
			obj.name = id1.ToString ();
			obj.transform.SetParent (nodeParent.transform);
			obj.SetActive (false);
			nodeObj.Add (id1,obj);
			Vector3 pos = randomPos ();
			nodePosition.Add (id1, pos);
		}
		if (!existNode2) {
			GameObject obj = Instantiate (nodePrefab)as GameObject;
			obj.name = id2.ToString ();
			obj.transform.SetParent (nodeParent.transform);
			obj.SetActive (false);
			nodeObj.Add (id2,obj);
			Vector3 pos = randomPos ();
			nodePosition.Add (id2, pos);
		}
	}
	*/



	void Update(){
		if (Input.GetKeyDown (KeyCode.P)) {
			PhysicsModelLayout ();
		}
	}
	// 力学モデルの描画を行なう
	public void PhysicsModelLayout(){
		StartCoroutine (PhysicsModel());
	}

	// Nodeを見えるようにする
	void OnVisualNode(){
		foreach (GameObject node in nodeObj.Values) {
			node.SetActive (true);
		}
	}

	// エッジの描画を消去する
	private void ClearEdge(){
		for(int i = 0; i < edgeParent.transform.childCount;i++){
			Destroy (edgeParent.transform.GetChild(i).gameObject);
		}
	}

	// 円配置のレイアウト
	public void CircularLayout(float radius){
		if (graph == null)
			return;
		nodePosition.Clear ();
		int nodeNum = graph.Nodes.Count;
		int count = 0;
		OnVisualNode ();

		foreach (Node node in graph.Nodes) {
			var angle = (360.0f / nodeNum) * count * (Mathf.PI / 180);
			Vector3 pos = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle))*radius;
			nodePosition.Add (node.ID, pos);
			count++;
			nodeObj [node.ID].transform.position = pos;
		}

		DrawAllEdge ();
	}
	private void DrawAllNode(){
		if (graph == null)
			return;
		OnVisualNode ();
		foreach (Node node in graph.Nodes) {
			nodeObj [node.ID].transform.position = nodePosition [node.ID];
		}
	}
	private void DrawAllEdge(){
		if (graph == null)
			return;
		ClearEdge ();
		OnVisualNode ();
		foreach (Node node in graph.Nodes) {
			foreach (int id in node.neighbor) {
				DrawEdge (node.ID,id);
			}
		}
	}

	// 辺を描画する
	private void DrawEdge(int id1,int id2){
		if (graph == null)
			return;
		
		if (nodePosition.ContainsKey (id1) && nodePosition.ContainsKey (id2)) {
			string name = id1.ToString () + "," + id2.ToString ();
			GameObject newLine = Instantiate (edgePrefab);
			newLine.name = name;
			LineRenderer lr = newLine.GetComponent<LineRenderer> ();
			Vector3[] vec = new Vector3[2];
			vec [0] = nodePosition [id1];
			vec [1] = nodePosition [id2];
			lr.SetWidth (edgeWidth.Value, edgeWidth.Value);
			Color c  = lr.material.color;
			lr.material.color = new Color (c.r, c.g, c.b, edgeAlpha.Value);
			lr.SetPositions (vec);
			newLine.transform.SetParent (edgeParent.transform);
		} else {
			Debug.LogError ("edgeがかけません");
		}
	}

	// 力学モデルによるグラフ描画アルゴリズム
	// k:バネ定数 f:クーロン力の定数 n:バネの自然長 delta: 更新時間 limit: 運動エネルギーによる停止条件
	IEnumerator PhysicsModel(float k = 1,float f = 0.001f,float n = 5,float delta = 0.25f,float limit = 1f){
		if (graph == null)
			yield break;
		Dictionary<int,Vector3> vel = new Dictionary<int,Vector3> ();
		foreach (Node node in graph.Nodes) {
			vel [node.ID] = Vector3.zero;
			if(!nodePosition.ContainsKey(node.ID)){
				Vector3 pos = randomPos ();
				nodePosition[node.ID] = pos;
			}
		}
		float E = 0;
		do {
			E = 0;
			foreach(Node node1 in graph.Nodes){
				Vector3 power = new Vector3(0,0,0); 

				foreach(Node node2 in graph.Nodes){
					if(node1.ID!=node2.ID){
						Vector3 toNode = (nodePosition[node2.ID]-nodePosition[node1.ID]).normalized;
						float sqrDistance = SqrDistance(nodePosition[node1.ID],nodePosition[node2.ID]);
						power += toNode*f/(sqrDistance); 
					}
				}
				foreach(int neighbor in node1.neighbor){
					Vector3 toNode = (nodePosition[neighbor]-nodePosition[node1.ID]).normalized;
					float distance = Vector3.Distance(nodePosition[node1.ID],nodePosition[neighbor]);
					power += toNode*k*(distance-n);
				}

				vel[node1.ID] = (vel[node1.ID]+delta*power)*0.5f;
				nodePosition[node1.ID] = nodePosition[node1.ID] + delta * vel[node1.ID];
				float v = vel[node1.ID].sqrMagnitude;
				E += v;


			}
			Debug.Log(E);
			DrawAllNode();
			DrawAllEdge();
			if(E>1000000){
				yield break;
			}
			yield return new WaitForSeconds (delta);  
		} while(E > limit);
		yield break;

	}

	float SqrDistance(Vector3 a,Vector3 b){
		Vector3 c = a - b;
		return c.sqrMagnitude;
	}

	Vector3 randomPos(){
		if (view2D) {
			return new Vector3 (UnityEngine.Random.Range (0f, 10f),0, UnityEngine.Random.Range (0f, 10f));
		} else {
			return  new Vector3 (UnityEngine.Random.Range (0f, 10f), UnityEngine.Random.Range (0f, 10f), UnityEngine.Random.Range (0f, 10f));
		}

	}
}
