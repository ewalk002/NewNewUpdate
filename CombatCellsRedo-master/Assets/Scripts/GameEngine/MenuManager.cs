﻿using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	private GameObject wbcTurret;
	private GameObject mucusTurret;
	private GameObject syringeTurret;
	private GameObject mortarTurret;
	private GameObject teslaTurret;
	private GameObject towerMenu;
	private GameObject towerRadiusPrefab;
	
	private GameObject lastHoverObj = null;
	private GameObject currHoverObj = null;
	
	private GameObject lastMenuedObj = null;
	private GameObject currMenuedObj = null;
	
	private GameObject menu = null;
	private GameObject menuTowerIcon = null;
	private GameObject menuTowerIconHover = null;
	
	//towers
	private GameObject wbc = null;
	private GameObject previewTower = null;
	
	public bool hoverMade;
	public bool menuMade;
	
	public Material originalMat;
	public Material hoverMat;
	public Material menuedMat;
	public Material towerRangeMat;
	
	private int placementLayerMask;
	private int wbcLayerMask;
	private int mucusLayerMask;
	private int mortarLayerMask;
	private int syringLayerMask;
	private int teslaLayerMask;
	private int towerMenuLayerMask;
	
	private GameObject towerRadius = null;
	
	
	/* Bernie Added
	private GameObject costLabel = null;*/
	public Texture WBC_icon_enabled;
	public Texture WBC_icon_disabled;
	public Texture Mucus_icon_enabled;
	public Texture Mucus_icon_disabled;
	public Texture Mortar_icon_enabled;
	public Texture Mortar_icon_disabled;
	public Texture Syringe_icon_enabled;
	public Texture Syringe_icon_disabled;
	public Texture Tesla_icon_enabled;
	public Texture Tesla_icon_disabled;
	//To get atom count!
	private GameObject GameEngine;
	
	// Use this for initialization
	void Start () {
		menuMade = false;
		hoverMade = false;
		
		// Layer masks
		wbcLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_WBC));
		mucusLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_MUCUS));
		mortarLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_MORTAR));
		syringLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_SYRINGE));
		teslaLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_TESLA));
		placementLayerMask = 1 << (LayerMask.NameToLayer (ConstantsLib.RAY_CAST_TILES));
		towerMenuLayerMask = wbcLayerMask | mucusLayerMask | mortarLayerMask | 
			syringLayerMask | teslaLayerMask;
		
		//originalMat = gameObject.renderer.material;
		towerMenu = (GameObject)Resources.Load ( ConstantsLib.TOWER_MENU_PATH, typeof(GameObject));
		wbcTurret = (GameObject)Resources.Load ( ConstantsLib.WBC_PATH, typeof(GameObject));
		mucusTurret = (GameObject)Resources.Load ( ConstantsLib.MUCUS_PATH, typeof(GameObject));
		syringeTurret = (GameObject)Resources.Load ( ConstantsLib.SYRINGE_PATH, typeof(GameObject));
		mortarTurret = (GameObject)Resources.Load ( ConstantsLib.MORTAR_PATH, typeof(GameObject));
		teslaTurret = (GameObject)Resources.Load ( ConstantsLib.TESLA_PATH, typeof(GameObject));
		towerRadiusPrefab = (GameObject)Resources.Load (ConstantsLib.TOWER_RAD_PATH, typeof(GameObject));
		
		/* Bernie Added
		costLabel = (GameObject)Resources.Load (ConstantsLib.COST_LABEL_PATH, typeof(GameObject));*/
		
		GameEngine = GameObject.Find (ConstantsLib.GAME_ENGINE_NAME);
		
	}
	
	void newMenu( GameObject tile ) // makes new menu facing camera, sets menuMad  to true
	{
		
		
		tile.renderer.material = menuedMat; // set clicked tile to green color
		Vector3 hitPoint = tile.transform.position; // position of object we hit
		Vector3 atCamera = Camera.main.transform.position - tile.transform.position;
		menu = (GameObject)Instantiate (towerMenu, hitPoint, Quaternion.identity);
		//Quaternion.LookRotation( atCamera ) );
		menu.transform.localRotation = Camera.main.transform.localRotation;
		menu.transform.Rotate(new Vector3(0,180,0));
		
		/*
		//Bernia Added This works but if I clicked on Menu and i get enough $$ the texture will not change!
		int AtomCount = GameEngine.GetComponent<InGameMenuManager>().AtomCount;
		Transform wbc_icon_object = menu.transform.FindChild("pCylinder2");
		if(AtomCount >= ConstantsLib.WBC_COST)
		{
			wbc_icon_object.renderer.material.mainTexture = WBC_icon_enabled;
		}
		else
		{
			wbc_icon_object.renderer.material.mainTexture = WBC_icon_disabled;
		}
		*/
		/* Bernie Added
		GameObject temp = (GameObject)Instantiate (costLabel);
		temp.transform.localScale = GameObject.Find ("Panel").transform.localScale;

		temp.transform.parent = GameObject.Find ("Panel").transform;
		temp.transform.position = hitPoint;//GameObject.Find ("Panel").transform.position;

		Vector3 templocal = temp.transform.localPosition;
		templocal.y += 100;
		temp.transform.localPosition = templocal;

		temp.transform.rotation = GameObject.Find ("Panel").transform.rotation;
		temp.transform.localScale = new Vector3 (700,700,0);

	*/
		float camDelta = Camera.main.orthographicSize / 2;
		
		menu.transform.Translate (Vector3.forward*(Camera.main.transform.position.y-camDelta));
		menuMade = true;
	}
	
	void deleteMenu( GameObject m, GameObject tile ) // destroys menu and sets menuMade to false
	{
		Destroy( m );
		tile.renderer.material = originalMat;
		menuMade = false;
	}
	
	void orangeRadius( GameObject g, Vector3 hoverPoint )
	{	
		towerRadius = (GameObject)Instantiate( towerRadiusPrefab, hoverPoint, Quaternion.identity );
		hoverPoint.y += ConstantsLib.Y_OFFSET;
		towerRadius.transform.position = hoverPoint;
		
		towerRadius.transform.localScale = g.transform.localScale;
		
		float initColliderRad = towerRadius.GetComponent<SphereCollider>().radius;
		float finalColliderRad = g.GetComponent<SphereCollider>().radius;		
		
		float scalar = finalColliderRad/initColliderRad;
		
		towerRadius.transform.localScale*=scalar;
		towerRadius.renderer.material = towerRangeMat;
	}
	
	void towerPreview( GameObject g, Vector3 hoverPoint )
	{
		hoverPoint.y += ConstantsLib.Y_OFFSET;
		//Debug.Log ("previed");
		previewTower = (GameObject)Instantiate (g, hoverPoint, Quaternion.identity);
		previewTower.GetComponent<tower_wbc> ().enabled = false;
	}
	
	void towerWhenYouCLick( GameObject g, GameObject plane, Vector3 hoverPoint )
	{
		
		if( previewTower != null )
		{
			Destroy( previewTower );
		}
		
		if( towerRadius != null )
		{
			Destroy( towerRadius );
		}
		
		wbc = (GameObject)Instantiate (g, hoverPoint, Quaternion.identity);
		//wbc.GetComponent<tower_wbc> ().enabled = true;
		
		if( g.gameObject.tag == ConstantsLib.WBC_TAG )
		{
			plane.gameObject.tag = ConstantsLib.PLACEMENT_WBC;
		}
		else if( g.gameObject.tag == ConstantsLib.MUCUS_TAG )
		{
			plane.gameObject.tag = ConstantsLib.PLACEMENT_MUCUS;
		}
		else if( g.gameObject.tag == ConstantsLib.MORTAR_TAG )
		{
			plane.gameObject.tag = ConstantsLib.PLACEMENT_MORTAR;
		}
		else if( g.gameObject.tag == ConstantsLib.SYRINGE_TAG )
		{
			plane.gameObject.tag = ConstantsLib.PLACEMENT_SYRINGE;
		}	
		else if( g.gameObject.tag == ConstantsLib.TESLA_TAG )
		{
			plane.gameObject.tag = ConstantsLib.PLACEMENT_TESLA;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Bernie Added
		GameObject openMenu = GameObject.Find ("towerMenu(Clone)");
		if(openMenu != null)
		{

			int AtomCount = GameEngine.GetComponent<InGameMenuManager>().AtomCount;
			Debug.Log (AtomCount);
			Transform wbc_icon_object = openMenu.transform.FindChild("pCylinder2");
			Transform mucus_icon_object = openMenu.transform.FindChild("pCylinder3");
			Transform mortar_icon_object = openMenu.transform.FindChild("pCylinder4");
			Transform syringe_icon_object = openMenu.transform.FindChild("pCylinder5");
			Transform tesla_icon_object = openMenu.transform.FindChild("pCylinder6");
			if(AtomCount >= ConstantsLib.WBC_COST)
			{
				wbc_icon_object.renderer.material.mainTexture = WBC_icon_enabled;
			}
			else
			{
				wbc_icon_object.renderer.material.mainTexture = WBC_icon_disabled;
			}
			if(AtomCount >= ConstantsLib.MUCUS_COST)
			{
				mucus_icon_object.renderer.material.mainTexture = Mucus_icon_enabled;
			}
			else
			{
				mucus_icon_object.renderer.material.mainTexture = Mucus_icon_disabled;
			}
			if(AtomCount >= ConstantsLib.MORTAR_COST)
			{
				mortar_icon_object.renderer.material.mainTexture = Mortar_icon_enabled;
			}
			else
			{
				mortar_icon_object.renderer.material.mainTexture = Mortar_icon_disabled;
			}
			if(AtomCount >= ConstantsLib.SYRINGE_COST)
			{
				syringe_icon_object.renderer.material.mainTexture = Syringe_icon_enabled;
			}
			else
			{
				syringe_icon_object.renderer.material.mainTexture = Syringe_icon_disabled;
			}
			if(AtomCount >= ConstantsLib.TESLA_COST)
			{
				tesla_icon_object.renderer.material.mainTexture = Tesla_icon_enabled;
			}
			else
			{

				tesla_icon_object.renderer.material.mainTexture = Tesla_icon_disabled;
			}
			
		}
		
		
		
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		var hit = new RaycastHit ();
		
		
		if( Physics.Raycast( ray, out hit, Mathf.Infinity, towerMenuLayerMask ) ) //Hover over the tower menu
		{
			int AtomCount = GameEngine.GetComponent<InGameMenuManager>().AtomCount;
			menuTowerIconHover = hit.collider.gameObject;
			Vector3 hoverPoint = currMenuedObj.transform.position;
			hoverPoint.y += ConstantsLib.Y_OFFSET;
			
			//WBC HoverOver
			if( menuTowerIconHover.tag == ConstantsLib.WBC_ICON_TAG )
			{
				if( (previewTower == null && towerRadius == null) && (AtomCount >= ConstantsLib.WBC_COST) )
				{
					orangeRadius( wbcTurret, hoverPoint );
					towerPreview( wbcTurret, hoverPoint );
				}
			}
			else if( menuTowerIconHover.tag == ConstantsLib.MUCUS_ICON_TAG )
			{
				if( (previewTower == null && towerRadius == null) && (AtomCount >= ConstantsLib.MUCUS_COST) )
				{
					orangeRadius( mucusTurret, hoverPoint );
					towerPreview( mucusTurret, hoverPoint );
					
				}
			}
			else if( menuTowerIconHover.tag == ConstantsLib.SYRINGE_ICON_TAG )
			{
				if( (previewTower == null && towerRadius == null) && (AtomCount >= ConstantsLib.SYRINGE_COST) )
				{
					orangeRadius( syringeTurret, hoverPoint );
					towerPreview( syringeTurret, hoverPoint );
					
				}
			}
			else if( menuTowerIconHover.tag == ConstantsLib.TESLAS_ICON_TAG )
			{
				if( (previewTower == null && towerRadius == null) && (AtomCount >= ConstantsLib.TESLA_COST) )
				{
					orangeRadius( teslaTurret, hoverPoint );
					towerPreview( teslaTurret, hoverPoint );
					
				}
			}
			else if( menuTowerIconHover.tag == ConstantsLib.MORTAR_ICON_TAG )
			{
				if( (previewTower == null && towerRadius == null ) && (AtomCount >= ConstantsLib.MORTAR_COST))
				{
					orangeRadius( mortarTurret, hoverPoint );
					towerPreview( mortarTurret, hoverPoint );
					
				}
			}
			else
			{
				if( previewTower != null )
				{
					Destroy( previewTower );
				}
				
				if( towerRadius != null )
				{
					Destroy( towerRadius );
				}
			}
			
			if( Input.GetMouseButtonDown( 0 ) )
			{
				
				//Bernie Added:
				//Debug.Log ( "Hit the menu" );
				
				menuTowerIcon = hit.collider.gameObject; // replace last object with what we just hit
				Vector3 hitPoint = currMenuedObj.transform.position;
				hitPoint.y += ConstantsLib.Y_OFFSET;
				
				deleteMenu( menu, currMenuedObj );
				
				
				if( (menuTowerIcon.tag == ConstantsLib.WBC_ICON_TAG) && (AtomCount >= ConstantsLib.WBC_COST) ) //wbc
				{
					GameEngine.GetComponent<InGameMenuManager>().AtomCount -= ConstantsLib.WBC_COST;
					towerWhenYouCLick( wbcTurret, currMenuedObj, hoverPoint );
				}
				else if( (menuTowerIcon.tag == ConstantsLib.MUCUS_ICON_TAG) && (AtomCount >= ConstantsLib.MUCUS_COST)) //mucus
				{
					GameEngine.GetComponent<InGameMenuManager>().AtomCount -= ConstantsLib.MUCUS_COST;
					towerWhenYouCLick( mucusTurret, currMenuedObj, hoverPoint );
				}
				else if( (menuTowerIcon.tag == ConstantsLib.MORTAR_ICON_TAG) && (AtomCount >= ConstantsLib.MORTAR_COST)) //mortar
				{
					GameEngine.GetComponent<InGameMenuManager>().AtomCount -= ConstantsLib.MORTAR_COST;
					towerWhenYouCLick( mortarTurret, currMenuedObj, hoverPoint );
				}
				else if( menuTowerIcon.tag == ConstantsLib.SYRINGE_ICON_TAG && (AtomCount >= ConstantsLib.SYRINGE_COST)) //syringe
				{
					GameEngine.GetComponent<InGameMenuManager>().AtomCount -= ConstantsLib.SYRINGE_COST;
					towerWhenYouCLick( syringeTurret, currMenuedObj, hoverPoint );
				}
				else if( menuTowerIcon.tag == ConstantsLib.TESLAS_ICON_TAG && (AtomCount >= ConstantsLib.TESLA_COST)) //tesla
				{
					GameEngine.GetComponent<InGameMenuManager>().AtomCount -= ConstantsLib.TESLA_COST;
					towerWhenYouCLick( teslaTurret, currMenuedObj, hoverPoint );
				}
			}
		}
		else if( Physics.Raycast( ray, out hit, Mathf.Infinity, placementLayerMask ) ) // Hovering over Placement Tiles
		{
			if( previewTower != null )
			{
				Destroy( previewTower );
			}
			
			if( towerRadius != null )
			{
				Destroy( towerRadius );
			}
			
			currHoverObj = hit.collider.gameObject;
			
			if( currHoverObj.tag == ConstantsLib.PLACEMENT_OPEN )
			{
				if( !hoverMade )
				{
					currHoverObj.renderer.material = hoverMat;
					lastHoverObj = currHoverObj;
					hoverMade = true;
				}
				else if( hoverMade && ( currHoverObj != lastHoverObj ) )
				{
					lastHoverObj.renderer.material = originalMat;
					currHoverObj.renderer.material = hoverMat;
					lastHoverObj = currHoverObj;
				}
				
				
				if( Input.GetMouseButtonDown( 0 ) )
				{		 
					currMenuedObj = hit.collider.gameObject;
					if( !menuMade )
					{
						//Debug.Log( "a" );
						newMenu( currMenuedObj );
						lastMenuedObj = currMenuedObj;
					}
					
					else if( menuMade && ( currMenuedObj == lastMenuedObj ) )
					{
						//Debug.Log( "b" );
						deleteMenu( menu, lastMenuedObj );
						currMenuedObj = null;
					}
					
					else if( menuMade && ( currMenuedObj != lastMenuedObj ) )
					{
						//Debug.Log( "c" );
						deleteMenu( menu, lastMenuedObj );
						newMenu( currMenuedObj );
						lastMenuedObj = currMenuedObj;
					}
				}
			}
			else if( currHoverObj.tag == ConstantsLib.PLACEMENT_WBC )
			{
				orangeRadius( wbcTurret, currHoverObj.transform.position );
			}
			else if( currHoverObj.tag == ConstantsLib.PLACEMENT_MUCUS )
			{
				orangeRadius( mucusTurret, currHoverObj.transform.position );
				
			}
			else if( currHoverObj.tag == ConstantsLib.PLACEMENT_MORTAR )
			{
				orangeRadius( mortarTurret, currHoverObj.transform.position );
				
			}
			else if( currHoverObj.tag == ConstantsLib.PLACEMENT_SYRINGE )
			{
				orangeRadius( syringeTurret, currHoverObj.transform.position );
				
			}
			else if( currHoverObj.tag == ConstantsLib.PLACEMENT_TESLA )
			{
				orangeRadius( teslaTurret, currHoverObj.transform.position );
			}
			else
			{
				currHoverObj.renderer.material = originalMat;
				lastHoverObj.renderer.material = originalMat;
				currHoverObj = null;
				
				if( Input.GetMouseButtonDown( 0 ) )
				{	
					if( menu != null )
					{
						deleteMenu( menu, currMenuedObj );
					}
				}
			}
			
		}
		else // if didnt click anything
		{
			
			if( previewTower != null )
			{
				Destroy( previewTower );
			}
			
			if( towerRadius != null )
			{
				Destroy( towerRadius );
			}
			if( currHoverObj != null )
			{
				currHoverObj.renderer.material = originalMat;
			}
			
			if( Input.GetMouseButtonDown( 0 ) )
			{
				if( menu != null )
				{
					deleteMenu( menu, lastMenuedObj );
				}
			}
		}
		
		
		
	}
}
