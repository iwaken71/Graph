using System.Collections.Generic;

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