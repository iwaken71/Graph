using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Graph{

	// Nodeの集合
	public HashSet<Node> Nodes;

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
			return new Node(-1);
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
}


public class Node{
	int id;
	public HashSet<int> neighbor; //隣接ノードの集合

	public int ID{
		get{ return id;}
	}

	public Node(int id){
		this.id = id;
		neighbor = new HashSet<int>();
	}

	public override bool Equals(object obj)
	{
		//objがnullか、型が違うときは、等価でない
		if (obj == null || this.GetType() != obj.GetType())
		{
			return false;
		}
		//idで比較する
		Node c = (Node)obj;
		return (this.id == c.id);
	}
	//Equalsがtrueを返すときに同じ値を返す
	public override int GetHashCode()
	{
		return this.id;
	}
}
