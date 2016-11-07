using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Crawler{


	List<int> sampleNodes = new List<int> ();

	public Crawler(){
		sampleNodes = new List<int> ();
	}

	public void Clear(){
		sampleNodes = new List<int> ();
	}


	public int RandomNode(Graph g){
		List<int> Nodes = new List<int> ();
		foreach (Node node in g.Nodes) {
			Nodes.Add (node.ID);
		}
		int now_node =  Nodes[Random.Range(0,Nodes.Count)];
		sampleNodes.Add (now_node);
		return now_node;

	}
	public int NextRW(Graph g, int now_node){
		if (sampleNodes.Count == 0) {
			return RandomNode (g);
		}
		List<int> Nodes = new List<int> ();
		foreach (int node in g.GetNode(now_node).neighbor) {
			Nodes.Add (node);
		}
		int next_node =  Nodes[Random.Range(0,Nodes.Count)];
		sampleNodes.Add (now_node);
		return next_node;
	}

	public List<int> RW(Graph g, float p){
		int num = (int)(g.Nodes.Count / p);
		List<int> sampleNodes = new List<int> ();
		List<int> Nodes = new List<int> ();
		foreach (Node node in g.Nodes) {
			Nodes.Add (node.ID);
		}
		int now_node = Nodes[Random.Range(0,Nodes.Count)];
		sampleNodes.Add (now_node);

		while (sampleNodes.Count < num) {
			Nodes.Clear ();
			foreach (int node in g.GetNode(now_node).neighbor) {
				Nodes.Add (node);
			}
			int next_node = Nodes[Random.Range(0,Nodes.Count)];

			now_node = next_node;
			sampleNodes.Add (now_node);
		}
		return sampleNodes;
	}



	public List<int> MHRW(Graph g, float p){
		int num = (int)(g.Nodes.Count / p);
		List<int> sampleNodes = new List<int> ();
		List<int> Nodes = new List<int> ();
		foreach (Node node in g.Nodes) {
			Nodes.Add (node.ID);
		}
		int now_node = Nodes[Random.Range(0,Nodes.Count)];
		sampleNodes.Add (now_node);

		while (sampleNodes.Count < num) {
			Nodes.Clear ();
			foreach (int node in g.GetNode(now_node).neighbor) {
				Nodes.Add (node);
			}
			int next_node = Nodes[Random.Range(0,Nodes.Count)];

			float ran = Random.Range (0f,1f);
			float q = (float)g.GetNode (now_node).neighbor.Count / g.GetNode (now_node).neighbor.Count;
			if (ran < q) {
				now_node = next_node;
			}
			sampleNodes.Add (now_node);
		}
		return sampleNodes;
	}

	public List<int> NBRW(Graph g, float p){
		int num = (int)(g.Nodes.Count / p);
		List<int> sampleNodes = new List<int> ();
		List<int> Nodes = new List<int> ();
		foreach (Node node in g.Nodes) {
			Nodes.Add (node.ID);
		}
		int now_node = Nodes[Random.Range(0,Nodes.Count)];
		sampleNodes.Add (now_node);

		while (sampleNodes.Count < num) {
			Nodes.Clear ();
			foreach (int node in g.GetNode(now_node).neighbor) {
				Nodes.Add (node);
			}
			int next_node = Nodes[Random.Range(0,Nodes.Count)];
			List<int> neighbor_list = new List<int>(g.GetNode (now_node).neighbor);
			int degree = neighbor_list.Count;
			while (true) {
				if (degree <= 1) {
					now_node = next_node;
					break;
				}
				if (next_node == sampleNodes [sampleNodes.Count - 1]) {
					// もう一度
				} else {
					now_node = next_node;
					break;
				}
			}
			//Debug.Log (now_node);
			sampleNodes.Add (now_node);
		}
		PrintArray (sampleNodes);
		return sampleNodes;
	}



	void PrintArray(List<int> lst){
		string s = "";
		for (int i = 0; i < lst.Count; i++) {
			s += lst[i].ToString()+",";
		}
		Debug.Log (s);
	}
}
