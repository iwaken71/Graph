using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Test : MonoBehaviour {
	GameObject graphObject;
	//Graph g;
	GraphView graphView;
	Crawler crawler;
	Graph g;

	// Use this for initialization
	void Start () {
		graphObject = Instantiate(Resources.Load ("Graph") as GameObject);
		//g = graphObject.GetComponent<Graph> ();
		graphView = graphObject.GetComponent<GraphView> ();
		g = BAModel (250,3);//ReadGraph ("ABC");
		crawler = new Crawler();

		//StartCoroutine (BAModelView(10,3));
		//graphView.CircularLayout (10);
		graphView.SetGraph(g);
		graphView.PhysicsModelLayout();

	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			SamplingProcess ();
		}
	
	}

	void SamplingProcess(){
		int now_node = crawler.NextRW (g,0);
		graphView.SetColor (now_node, Color.yellow);


	}

	IEnumerator BAModelView(int n,int m){
		Graph g = new Graph ();
		graphView.SetGraph (g);
		List<int> nodeList = new List<int> ();
		for (int i = 0; i < m-1; i++) {
			for (int j = i + 1; j < m; j++){
				g.AddEdge (i, j);
				graphView.AddEdge (i, j);
				nodeList.Add (i);
				nodeList.Add (j);
			}
		}
		int index = m;
		while (g.Nodes.Count < n) {
			g.AddNode (index);
			yield return new WaitForSeconds (0.05f);  
			List<int> a = new List<int> (nodeList);
			for (int i = 0; i < m; i++) {
				int ran = UnityEngine.Random.Range (0, a.Count);
				int nextID = a [ran];
				g.AddEdge (index, nextID);
				graphView.AddEdge (index, nextID);
				a.RemoveAll (num => num == nextID);
				nodeList.Add (index);
				nodeList.Add (nextID);
			}
			index++;


		}
	}

	private Graph BAModel(int n,int m){
		Graph g = new Graph ();
		List<int> nodeList = new List<int> ();
		for (int i = 0; i < m-1; i++) {
			for (int j = i + 1; j < m; j++){
				g.AddEdge (i, j);
				nodeList.Add (i);
				nodeList.Add (j);
			}
		}
		int index = m;
		while (g.Nodes.Count < n) {
			g.AddNode (index);
			List<int> a = new List<int> (nodeList);
			for (int i = 0; i < m; i++) {
				int ran = UnityEngine.Random.Range (0, a.Count);
				int nextID = a [ran];
				g.AddEdge (index, nextID);
				a.RemoveAll (num => num == nextID);
				nodeList.Add (index);
				nodeList.Add (nextID);
			}
			index++;


		}
		return g;
	}

	Graph ReadGraph(string path)
	{
		//ストリームリーダーsrに読み込む
		//※Application.dataPathはプロジェクトデータのAssetフォルダまでのアクセスパスのこと,
		Graph g = new Graph();

		TextAsset csv = Resources.Load("data/"+path) as TextAsset;
		StringReader sr = new StringReader(csv.text);
		//StreamReader sr = new StreamReader(Application.dataPath +path);
		//ストリームリーダーをstringに変換
		string strStream = sr.ReadToEnd();

		//StringSplitOptionを設定(要はカンマとカンマに何もなかったら格納しないことにする)
		System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

		//行に分ける
		string []lines = strStream.Split(new char[]{'\r','\n'},option);

		//カンマ分けの準備(区分けする文字を設定する)
		char []spliter = new char[1]{'\t'};

		//行数設定
		int heightLength = lines.Length;

		//カンマ分けをしてデータを完全分割
		for (int i = 0; i < heightLength; i++)
		{
			string [] readData = lines[i].Split(spliter, option);
			int id1 = int.Parse (readData[0]);
			int id2 = int.Parse (readData[1]);
			g.AddEdge (id1,id2);
		}
		return g;
	}

	Graph CompleteGraph(int n){
		Graph g = new Graph();
		if (n <= 1) {
			g.AddNode (0);
			return g; 
		}

		for (int i = 0; i < n-1; i++) {
			for (int j = i + 1; j < n; j++) {

				g.AddEdge (i, j);

			}
		}
		return g;
	}
}
