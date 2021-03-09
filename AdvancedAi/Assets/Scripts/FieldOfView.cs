/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///Name: FieldOfView.cs 
///Created by: Charlie Bullock based upon field of view visualisation series by Sebastian Lague
///Description: Creates an adjustable view cone which can detect specified objects with correct tagged layer and reacts
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //Variables
    #region Variables
    [SerializeField]
    private float viewRadius;
    //Maximum and minimum sie of the view angle (0 nothing, 360 all around)
    [Range(0,360)]
    [SerializeField]
    private float viewAngle;
    //Masks for objects and targets
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;
    //List for currently visible targets
    private List<Transform> visibleTargets = new List<Transform>();
    [SerializeField]
    private float meshResolution;
    [SerializeField]
    private int edgeResolveIteration;
    [SerializeField]
    private float edgeDistanceThreshold;
    [SerializeField]
    private MeshFilter viewMeshFilter;
    Mesh viewMesh;
    private AudioManager aM;
    private bool enumActive = false;
    private GameObject targetObject;
    #endregion Variables

    private void Start()
    {
        aM = FindObjectOfType<AudioManager>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    private void OnEnable()
    {
        aM = FindObjectOfType<AudioManager>();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", 0.2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    //Function finds all visible targets when called
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < targetsInViewRadius.Length;i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.forward,directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast (transform.position,directionToTarget, distanceToTarget,obstacleMask))
                {
                    visibleTargets.Add(target);
                    if (target.gameObject.layer == 9)
                    {
                        aM.AlertSound(true);
                        if (TryGetComponent(out Agent agent))
                        {
                            if (target.GetComponent<Assasin>())
                            {
                                target.GetComponent<Spy>().enemyPosition = transform.transform;
                            }
                            targetObject = target.gameObject;
                            agent.spyDetected = true;
                            agent.SetTargetSpeed(10);
                            agent.SetTargetPosition(targetObject.transform.position);
                        }
                    }
                    else if (GetComponent<Assasin>() == null)
                    {
                        aM.AlertSound(false);
                        GetComponent<Spy>().enemyPosition = target.transform.transform;
                    }
                }
            }
        }
        if (GetComponent<Agent>() != null && targetObject != null)
        {
            GetComponent<Agent>().SetTargetPosition(targetObject.transform.position);
            if (enumActive == false)
            {
                enumActive = true;
                Collider[] guardsInRadius = Physics.OverlapSphere(transform.position, viewRadius);
                for (int i = 0; i < guardsInRadius.Length; i++)
                {
                    if (guardsInRadius[i].tag == "Guard")
                    {
                        if (guardsInRadius[i].TryGetComponent(out Agent guardAgent))
                        {
                            guardAgent.spyDetected = true;
                            guardAgent.SetTargetPosition(targetObject.transform.position);
                            guardAgent.SetTargetSpeed(10);
                        }
                    }
                }
                StartCoroutine(WaitTillReset());
            }
        }
    }

    IEnumerator WaitTillReset()
    {
        yield return new WaitForSeconds(10f);
        if (TryGetComponent(out Agent agent))
        {
            agent.spyDetected = false;
            agent.SetTargetSpeed(5);
        }
    }

    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount;i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistanceThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1;i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2 ;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minimumViewCast, ViewCastInfo maximumViewCast)
    {
        float minimumAngle = minimumViewCast.angle;
        float maximumAngle = maximumViewCast.angle;
        Vector3 minimumPoint = Vector3.zero;
        Vector3 maximumPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration;i++)
        {
            float angle = (minimumAngle + maximumAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDistanceThresholdExceeded = Mathf.Abs(minimumViewCast.distance - newViewCast.distance) > edgeDistanceThreshold;

            if (newViewCast.hit == minimumViewCast.hit && !edgeDistanceThresholdExceeded)
            {
                minimumAngle = angle;
                minimumPoint = newViewCast.point;
            }
            else
            {
                maximumAngle = angle;
                maximumPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minimumPoint, maximumPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast (transform.position, direction, out hit,viewRadius,obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    //When using an angle of an object in Unity swap around Sin and Cos
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA,Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
