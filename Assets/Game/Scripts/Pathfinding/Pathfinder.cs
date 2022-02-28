using System;
using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.PlayerLoop;
using System.Linq;
using Cinemachine;
public class Pathfinder : MonoBehaviour
{
	private CatmullRom spline;
	private CatmullRom.CatmullRomPoint[] waypointsForPlayer;
	private BoxCollider[] m_Collider;
	private float Tvalue = 50f, toFirstPointLerp = 0.5f;
	private bool pathfindingAllowed = true, startPathfindingDisable, noCam, noManager;
	[ReorderableList] public Transform[] controlPoints;
	[SerializeField] private CinemachineVirtualCamera cam = default;
	[SerializeField] private 	GameManager manager;
	[SerializeField] [Tooltip("Funktioniert nur wenn gamemanager zugewiesen")]private float disableShakeSec = 1.1f;
	[BoxGroup("Einstellungen")] private bool ClosedLoop = false;
	[InfoBox("Resolution steuert am meisten die Geschwindkeit;  Kann nicht ingame verändert werden, dass muss man in der Scene view machen (wegen performance)", EInfoBoxType.Normal)]
	[BoxGroup("Einstellungen")] [Range(2, 100)] [SerializeField] private int Resolution = 30;
	[BoxGroup("Einstellungen")] [Range(1, 3)][SerializeField] private int speedLevel = 1;
	[BoxGroup("Einstellungen")] [Range(2, 15)] [SerializeField] private float secPathDisabled = 5;
	[BoxGroup("Einstellungen")] [Range(5, 200)] [SerializeField] private float forceRedExit = 80;
	[BoxGroup("Einstellungen")] [Range(5, 200)] [SerializeField] private float forceGreenExit = 80;

	[BoxGroup("Debug")][SerializeField]private bool drawNormal;
	[BoxGroup("Debug")][SerializeField]private bool drawTangent;
	[BoxGroup("Debug")][Range(0, 20)] public float normalLength;
	[BoxGroup("Debug")][Range(0, 20)] public float tangentLength;
	[BoxGroup("Debug")][Range(1, 20)] [SerializeField] private float colliderSize = 3;
	private void Awake()
	{
		if (manager == null) noManager = true;
		if (cam != null) noCam = false;
		if (cam == null)
		{
			cam = GetComponent<CinemachineVirtualCamera>();
			noCam = true;
		}
	}
	void Start()
	{
		if(cam != null) cam.gameObject.SetActive(false);
		if (spline == null && controlPoints.Length > 2) spline = new CatmullRom(controlPoints, Resolution, ClosedLoop);
		if (spline != null) waypointsForPlayer = spline.GetPoints();
		else if (controlPoints.Length > 2) spline = new CatmullRom(controlPoints, Resolution, ClosedLoop);
		playerLayerInt = LayerMask.NameToLayer("Player");
		playerNoCollisionLayerInt = LayerMask.NameToLayer("PlayerNoCollision");
	}
	[Button("Fill List with Controlpoints")]
	public void AllChildsToList()
	{
		Array.Clear(controlPoints, 0, controlPoints.Length);
		controlPoints = GetComponentsInChildren<Transform>();
			controlPoints = controlPoints.Skip(1).ToArray();
	}
	[Button("Empty everything from list")]
	public void RemoveFromList()
	{
		Array.Clear(controlPoints, 0, controlPoints.Length);
		Array.Resize(ref controlPoints, 0);
	}
	[Button("SetCollider")]
	void setCollider()
	{
		if (m_Collider != null)
		{
			Array.Clear(m_Collider,0,m_Collider.Length);
			Array.Resize(ref m_Collider, 0);
		}
		m_Collider = GetComponents<BoxCollider>();
		if (transform.childCount < 1) return;
		m_Collider[0].size = new Vector3(colliderSize, colliderSize, colliderSize);
		m_Collider[0].center = gameObject.transform.GetChild(0).transform.localPosition;
		if (transform.childCount < 2) return;
		m_Collider[1].size = new Vector3(colliderSize, colliderSize, colliderSize);
		m_Collider[1].center = gameObject.transform.GetChild(transform.childCount-1).transform.localPosition;
	}
	[Button] void resetRotationControlPoints()
	{
		int children = transform.childCount;
		for (int i = 0; i < children; ++i) 
			gameObject.transform.GetChild(i).transform.rotation = new Quaternion(0, 0, 0, 0);
	}
	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		foreach(Transform elem in controlPoints)
			if(elem == null) return;
		if (transform.childCount < 2 && controlPoints.Length < 2) return;
		if (spline != null && transform.childCount >2 && controlPoints.Length > 2)
		{  
			spline.Update(controlPoints); 
			spline.Update(Resolution, ClosedLoop); 
			spline.DrawSpline(Color.white);
			if (drawNormal) spline.DrawNormals(normalLength, Color.yellow);
			if (drawTangent) spline.DrawTangents(tangentLength, Color.green); 
			waypointsForPlayer = spline.GetPoints();
		}
		else if(this.transform.childCount >2 && controlPoints.Length > 2)
			spline = new CatmullRom(controlPoints, Resolution, ClosedLoop);
	}
#endif
	IEnumerator CamShakeDisableAterPush_Coroutine(float sec)
	{
		GameManager.bridgePause = true;
		yield return new WaitForSeconds(sec);
		GameManager.bridgePause = false;
	}
	IEnumerator movePath(Collider other)
	{
		for (int i = 0 ; i <waypointsForPlayer.Length-1 ; i++)
		{
			if (i == waypointsForPlayer.Length-2)
			{
				if(!noCam)cam.gameObject.SetActive(false);
				if(!noManager)manager.AllowMovement = true;
				StopCoroutine(waitUntilNextTrigger());
				StartCoroutine(waitUntilNextTrigger());
				MathLibary.boostDirection(waypointsForPlayer[waypointsForPlayer.Length - 2].position,
					waypointsForPlayer[waypointsForPlayer.Length - 1].position, forceRedExit, other.attachedRigidbody);
				if (manager != null)
				{ 
					StopCoroutine(CamShakeDisableAterPush_Coroutine(disableShakeSec));
					StartCoroutine(CamShakeDisableAterPush_Coroutine(disableShakeSec));
				}
			}
			if (i < waypointsForPlayer.Length - 1 && i >1)
			{
				other.transform.position = new Vector3(
					Mathf.Lerp(other.transform.position.x, waypointsForPlayer[i].position.x,
						Tvalue*Time.deltaTime),
					Mathf.Lerp(other.transform.position.y, waypointsForPlayer[i].position.y,
						Tvalue*Time.deltaTime),
					Mathf.Lerp(other.transform.position.z, waypointsForPlayer[i].position.z,
						Tvalue*Time.deltaTime));
				if(speedLevel==1) yield return new TimeUpdate.WaitForLastPresentationAndUpdateTime();
				if(speedLevel==2) yield return new WaitForEndOfFrame();
				if (speedLevel==3) yield return new WaitForFixedUpdate();
			}
		}
		ReferenceLibary.Player.layer = playerLayerInt;
	}
	IEnumerator movePathReverse(Collider other)
	{
		for (int i = waypointsForPlayer.Length; i >1 ; i--)
		{
			if (i == 2)
			{
				if(!noCam)cam.gameObject.SetActive(false);
				if(!noManager)manager.AllowMovement = true;
				StopCoroutine(waitUntilNextTrigger());
				StartCoroutine(waitUntilNextTrigger());
				MathLibary.boostDirection(waypointsForPlayer[2].position,
					waypointsForPlayer[1].position, forceGreenExit, other.attachedRigidbody);
				if (manager != null)
				{
					StopCoroutine(CamShakeDisableAterPush_Coroutine(disableShakeSec));
					StartCoroutine(CamShakeDisableAterPush_Coroutine(disableShakeSec));
				}
			}
			if (i < waypointsForPlayer.Length - 1 && i >2)
			{
				other.transform.position = new Vector3(
					Mathf.Lerp(other.transform.position.x, waypointsForPlayer[i - 1].position.x,
						Tvalue*Time.deltaTime),
					Mathf.Lerp(other.transform.position.y, waypointsForPlayer[i - 1].position.y,
						Tvalue*Time.deltaTime),
					Mathf.Lerp(other.transform.position.z, waypointsForPlayer[i - 1].position.z,
						Tvalue*Time.deltaTime));
					
				if(speedLevel==1) yield return new TimeUpdate.WaitForLastPresentationAndUpdateTime();
				if(speedLevel==2) yield return new WaitForEndOfFrame();
				if (speedLevel==3) yield return new WaitForFixedUpdate();
			}
		}
			ReferenceLibary.Player.layer = playerLayerInt;
	}
	IEnumerator waitUntilNextTrigger()
	{
		GameManager.StartMovingGhostLayer = false;
		ReferenceLibary.PlayerMov.DisableGravity = false;
		GameManager.LerpCameraBack = true;
		yield return new WaitForSeconds(secPathDisabled);
	    pathfindingAllowed = true;
	}
	private void OnTriggerEnter(Collider other)
	{
		spline = new CatmullRom(controlPoints, Resolution, ClosedLoop);
		if (other.CompareTag("Player"))
		{
			float distanceStart, distanceEnd;
			waypointsForPlayer = spline.GetPoints();
			distanceStart = MathLibary.CalculateDistancePos(other.transform.position,
				waypointsForPlayer[0].position);
			distanceEnd = MathLibary.CalculateDistancePos(other.transform.position,
				waypointsForPlayer[waypointsForPlayer.Length-1].position);
			if (pathfindingAllowed)
			{
				GameManager.StartMovingGhostLayer = true;
				pathfindingAllowed = false;
				if(!noCam) cam.gameObject.SetActive(true);
				if(!noManager)manager.AllowMovement = false;
				ReferenceLibary.PlayerMov.DisableGravity = true;
				ReferenceLibary.Player.layer = playerNoCollisionLayerInt;
				if (distanceStart > distanceEnd)
				{
					StopCoroutine(movePathReverse(other)); 
					StartCoroutine(movePathReverse(other));
				}
				else if (distanceEnd > distanceStart)
				{
					StopCoroutine(movePath(other));
					StartCoroutine(movePath(other));
				}
			}
		}
	}
	int playerLayerInt;
	int playerNoCollisionLayerInt;
}

	
	

