using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crawler{

	List<int> sampleNodes = new List<int> ();
	Dictionary<int,float> ClusterDic = new Dictionary<int,float>();
	Dictionary<int,List<int>> NeighborDic = new Dictionary<int, List<int>>();

	public Crawler(){
		sampleNodes = new List<int> ();
	}

	public void Clear(){
		sampleNodes = new List<int> ();
		ClusterDic.Clear ();
		NeighborDic.Clear ();
	}

	List<int> Neighbor(Graph g,int v){
		if (NeighborDic.ContainsKey (v)) {
			return NeighborDic [v];
		} else {
			return new List<int>(g.GetNode (v).neighbor);
		}
	}

	public int RandomNode(Graph g){
		int now_node = g.RandomSeed ();
		sampleNodes.Add (now_node);
		return now_node;
	}
	public int NextRW(Graph g, int now_node){
		if (sampleNodes.Count == 0) {
			return RandomNode (g);
		}
		List<int> neighbor_list = Neighbor (g, now_node);
		int next_node =  neighbor_list[Random.Range(0,neighbor_list.Count)];
		sampleNodes.Add (now_node);
		return next_node;
	}

	public List<int> RW(Graph g, int budget){
		List<int> Nodes = new List<int> ();
		int now_node = g.RandomSeed ();
		sampleNodes.Add (now_node);

		while (sampleNodes.Count < budget) {
			List<int> neighbor_list = Neighbor (g, now_node);
			int next_node = neighbor_list[Random.Range(0,neighbor_list.Count)];
			now_node = next_node;
			sampleNodes.Add (now_node);
		}
		PrintArray (sampleNodes);
		return sampleNodes;
	}



	public List<int> MHRW(Graph g, int budget){
		int num = budget;
		int now_node = g.RandomSeed ();
		sampleNodes.Add (now_node);

		while (sampleNodes.Count < num) {
			List<int> neighbor_list = Neighbor (g, now_node);
			int next_node = neighbor_list[Random.Range(0,neighbor_list.Count)];
			int degree = neighbor_list.Count;

			float ran = Random.Range (0f,1f);
			float q = (float)degree / g.GetNode (next_node).neighbor.Count;
			if (ran < q) {
				now_node = next_node;
			}
			sampleNodes.Add (now_node);
		}
		return sampleNodes;
	}

	public List<int> NBRW(Graph g,int budget){
	//	List<int> sampleNodes = new List<int> ();

		int now_node = g.RandomSeed ();
		sampleNodes.Add (now_node);

		while (sampleNodes.Count <budget) {
			List<int> neighbor_list = Neighbor (g, now_node);
			int next_node = neighbor_list[Random.Range(0,neighbor_list.Count)];
			int degree = neighbor_list.Count;
			if (degree <= 1) {
				now_node = next_node;
			} else {
				while (true) {
					if (next_node == sampleNodes [sampleNodes.Count - 1]) {
						// もう一度
					} else {
						now_node = next_node;
						break;
					}
				}
			}
			//Debug.Log (now_node);
			sampleNodes.Add (now_node);
		}
		return sampleNodes;
	}

	void PrintArray(List<int> lst){
		string s = "";
		for (int i = 0; i < lst.Count; i++) {
			s += lst[i].ToString()+",";
		}
		Debug.Log (s);
	}

	// グラフと頂点IDからクラスタ係数を求める
	public float Clustering(Graph g,int v){
		List<int> neighbor= Neighbor (g, v);
		int degree = neighbor.Count;

		if (degree <= 1)
			return 0;

		int count = 0;
		int triangle = 0;
		
		for (int i = 0; i < degree - 1; i++) {
			List<int> neighbor2 = new List<int> (Neighbor(g,neighbor[i]));
			for (int j = i + 1; j < degree; j++) {
				if (neighbor.Contains (neighbor2 [j]))
					triangle++;
			}
		}
		return (float)triangle / count;
	}
}
