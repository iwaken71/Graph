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
}
