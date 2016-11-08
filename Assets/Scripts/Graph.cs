using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Graph{

	// Nodeの集合
	private HashSet<Node> Nodes;

	public Graph(){
		Nodes = new HashSet<Node> ();
	}
		
	public Node GetNode(int id){
		// 引数idのNodeが存在するか
		bool existNode = Nodes.Any (node => node.ID == id);
		if (existNode) {
			Node node1 = 
				Nodes.Where (node => node.ID == id)
					.First ();
			return node1;
		} else {
			Debug.Log ("存在しません");
			return null;
		}
	}
		
	// 一応Nodeを返り値に持つが、返り値を使わないことが多い。
	public Node AddNode(int id){
		bool existNode = Nodes.Any (node => node.ID == id);
		if (existNode) {
			Node node1 = 
				Nodes.Where (node => node.ID == id)
					.First ();
			return node1;

		} else {
			Node newNode = new Node (id);
			Nodes.Add (newNode);
			return newNode;
		}

	}
	public void AddEdge(int id1,int id2){
		Node node1 = AddNode (id1);
		node1.neighbor.Add (id2);
		Node node2 = AddNode (id2);
		node2.neighbor.Add (id1);
	}

	public int RandomSeed(){
		int number = Nodes.Count;
		int random_number = Random.Range (0,number);
		int count = 0;
		foreach (Node node in Nodes) {
			if (random_number == count) {
				return  node.ID;
			}
			count++;
		}
		return -1;
	}

	public HashSet<Node> GetNodes(){
		return new HashSet<Node>(Nodes);
	}
}
