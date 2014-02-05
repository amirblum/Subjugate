using UnityEngine;
using System.Collections;
//using System.Linq;

public class CollisionDetector : MonoBehaviour {

	public GameObject heroObject;
	private ArrayList activeEntities;
	//private SortedList activeEntities = new SortedList(new CompareByAngle());
	//private static double collisionAngleThresh = 0.1;

	// Use this for initialization
	void Start () {
		activeEntities = GetComponent<EntitiesManager>().activeEntities;
	}

	// Update is called once per frame
	void Update () {
		ArrayList toRemove = new ArrayList();
		Hashtable collideWith = new Hashtable ();
		GameObject currObj, secObj;
		for (int i=0; i<activeEntities.Count; i++) {
			currObj = (GameObject)activeEntities[i];
			for(int j=i+1; j < activeEntities.Count ; ++j){
				secObj = (GameObject)activeEntities[j];
				if (Colliding(secObj, currObj)){
					if (! toRemove.Contains(currObj))
					{
						toRemove.Add (currObj);
						collideWith.Add(currObj, new ArrayList());
					}
					((ArrayList)collideWith[currObj]).Add(secObj);
					if (! toRemove.Contains(secObj))
					{
						toRemove.Add (secObj);
						collideWith.Add(secObj, new ArrayList());
					}
					((ArrayList)collideWith[secObj]).Add(currObj);
				}
			}
		}
		foreach (Object obj in toRemove)
		{
			((GameObject)obj).GetComponent<Collidable>().Collide((ArrayList)collideWith[obj]);
		}

	}

	//returns true iff the bounds of boj1 and obj2 are intersecting
	private bool Colliding(GameObject obj1, GameObject obj2){
		GameObject colObj1 = (GameObject)Instantiate (obj1);
		colObj1.transform.localScale = colObj1.GetComponent<Collidable> ().ScaleToCollision;
		GameObject colObj2 = (GameObject)Instantiate (obj2);
		colObj2.transform.localScale = colObj2.GetComponent<Collidable> ().ScaleToCollision;
		if (colObj1.renderer.bounds.Intersects (colObj2.renderer.bounds)){
			DestroyObject(colObj1, 0);
			DestroyObject (colObj2, 0);
			return true;
		}
		DestroyObject(colObj1, 0);
		DestroyObject (colObj2, 0);
		return false;
	}
}
