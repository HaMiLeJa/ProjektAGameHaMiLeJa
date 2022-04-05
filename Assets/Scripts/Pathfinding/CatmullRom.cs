using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

public class CatmullRom
    {
        [Serializable]
        public struct CatmullRomPoint
        {
            public Vector3 position,tangent ,normal;
            public CatmullRomPoint(Vector3 position, Vector3 tangent, Vector3 normal)
            {
                this.position = position;
                this.tangent = tangent;
                this.normal = normal;
            }
        }
        private int resolution; 
        private bool closedLoop;
        private CatmullRomPoint[] splinePoints;
        private Vector3[] controlPoints;
        
   
        public CatmullRomPoint[] GetPoints()
        {
            if(splinePoints == null) throw new NullReferenceException("Spline ist noch null");
                return splinePoints;
        }
        public CatmullRom(Transform[] controlPoints, int resolution, bool closedLoop)
        {
            if(controlPoints == null || controlPoints.Length <= 2 || resolution < 2)
                throw new ArgumentException("Mehr Controlpoints oder höhere Resolution");
            this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++)
                this.controlPoints[i] = controlPoints[i].position;
            this.resolution = resolution; this.closedLoop = closedLoop;
            GenerateSplinePoints();
        }
        public CatmullRom(TransformAccessArray controlPoints, int resolution, bool closedLoop)
        {
          
            this.controlPoints = new Vector3[controlPoints.length];
            for(int i = 0; i < controlPoints.length; i++)
                this.controlPoints[i] = controlPoints[i].position;
            this.resolution = resolution; this.closedLoop = closedLoop;
            GenerateSplinePoints();
        }
        public void Update(Transform[] controlPoints)
        {
            if(controlPoints.Length <= 0 || controlPoints == null)
                throw new ArgumentException("Illegale Aktion, Controlpoints stimmen nicht");
                this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++) 
                this.controlPoints[i] = controlPoints[i].position;
            GenerateSplinePoints();
        }
        public void Update(int resolution, bool closedLoop)
        {
            if(resolution < 2) throw new ArgumentException("Invalid Resolution. Make sure it's >= 1");
            this.resolution = resolution;
            this.closedLoop = closedLoop;
            GenerateSplinePoints();
        }
        public void DrawSpline(Color color)
        {
            if(ValidatePoints())
            {
                for(int i = 0; i < splinePoints.Length; i++)
                {
                    if(i == splinePoints.Length - 1 && closedLoop)
                        Debug.DrawLine(splinePoints[i].position, splinePoints[0].position, color);
                    else if(i < splinePoints.Length - 1)
                        Debug.DrawLine(splinePoints[i].position, splinePoints[i+1].position, color);
                }
            }
        }
        public void DrawNormals(float extrusion, Color color)
        {
            if(ValidatePoints())
            {
                for(int i = 0; i < splinePoints.Length; i++)
                    Debug.DrawLine(splinePoints[i].position, splinePoints[i].position + splinePoints[i].normal * extrusion, color);
            }
        }
        public void DrawTangents(float extrusion, Color color)
        {
            if (ValidatePoints())
            {
                for (int i = 0; i < splinePoints.Length; i++)
                    Debug.DrawLine(splinePoints[i].position,splinePoints[i].position + splinePoints[i].tangent * extrusion, color);
            }
        }
        private bool ValidatePoints()
        {
            if(splinePoints == null) throw new NullReferenceException("Spline not initialized!");
            return splinePoints != null;
        }
        private void InitializeProperties()
        {
            int pointsToCreate;
            if (closedLoop) pointsToCreate = resolution * controlPoints.Length;
            else pointsToCreate = resolution * (controlPoints.Length - 1);
            splinePoints = new CatmullRomPoint[pointsToCreate];       
        }
        [BurstCompile]
        private void GenerateSplinePoints()
        {
            InitializeProperties();
            Vector3 p0, p1, m0, m1;
            int closedAdjustment = closedLoop ? 0 : 1;
            for (int currentPoint = 0; currentPoint < controlPoints.Length - closedAdjustment; currentPoint++)
            {
                bool closedLoopFinalPoint = (closedLoop && currentPoint == controlPoints.Length - 1);
                p0 = controlPoints[currentPoint];
                if(closedLoopFinalPoint) p1 = controlPoints[0];
                else p1 = controlPoints[currentPoint + 1];
                if (currentPoint == 0)
                {
                    if(closedLoop) m0 = p1 - controlPoints[controlPoints.Length - 1];
                    else m0 = p1 - p0;
                }
                else m0 = p1 - controlPoints[currentPoint - 1];
                if (closedLoop)
                {
                    if (currentPoint == controlPoints.Length - 1) m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    else if (currentPoint == 0) m1 = controlPoints[currentPoint + 2] - p0;
                    else m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                }
                else
                {
                    if (currentPoint < controlPoints.Length - 2) m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    else m1 = p1 - p0;
                }
                m0 *= 0.5f; m1 *= 0.5f; float pointStep = 1.0f / resolution;
                if ((currentPoint == controlPoints.Length - 2 && !closedLoop) || closedLoopFinalPoint) pointStep = 1.0f / (resolution - 1);
                
                NativeArray<float> tValues = new NativeArray<float>(resolution, Allocator.Persistent);
                NativeQueue<CatMullRomNativeContainer> tempJobsQueue =
                new NativeQueue<CatMullRomNativeContainer>(Allocator.Persistent);
                for (int tesselatedPoint = 0; tesselatedPoint < resolution; tesselatedPoint++)
                    tValues[tesselatedPoint] = tesselatedPoint * pointStep;
                
                EvaluateJob evaluateJob = new EvaluateJob
                 {
                     start = p0 , end = p1, tanPoint1 = m0, tanPoint2 = m1, pointStepJob = pointStep, resolution = resolution, currentPoint = currentPoint,
                     CatmullRomPointsWithIndex = tempJobsQueue.AsParallelWriter()
                 };
                 evaluateJob.Schedule(resolution, 2).Complete();
                 tValues.Dispose();
                 while (!tempJobsQueue.IsEmpty())
                 {
                   var f =   tempJobsQueue.Dequeue();
                   splinePoints[f.listIndex].normal = f.points.norm;
                   splinePoints[f.listIndex].tangent= f.points.tan;
                   splinePoints[f.listIndex].position= f.points.pos;
                 }
                 tempJobsQueue.Dispose();
            }
        }
    }

public struct CatMullRomNativeContainer
{
    public readonly int  listIndex;
    public CatMullRomStruct points;

    public CatMullRomNativeContainer(int listIndex, CatMullRomStruct points)
    {
     this.listIndex = listIndex;
     this.points = points;
    }
}
public struct CatMullRomStruct
{
    public Vector3 pos;
    public Vector3 tan;
    public Vector3 norm;
}
[BurstCompile(FloatPrecision.Low,FloatMode.Fast)] 
struct EvaluateJob : IJobParallelFor
{
    [Unity.Collections.ReadOnly]  public Vector3 start, end, tanPoint1, tanPoint2;
    [Unity.Collections.ReadOnly] public float pointStepJob;
    public NativeQueue<CatMullRomNativeContainer>.ParallelWriter CatmullRomPointsWithIndex;
    private CatMullRomStruct point;
    private float t;
    [Unity.Collections.ReadOnly]public int currentPoint, resolution;
    public void Execute(int index)
    {
        t = index * pointStepJob;
        
        point.pos =(2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
                   + (t * t * t - 2.0f * t * t + t) * tanPoint1
                   + (-2.0f * t * t * t + 3.0f * t * t) * end
                   + (t * t * t - t * t) * tanPoint2;
        
        point.tan =((6 * t * t - 6 * t) * start
                    + (3 * t * t - 4 * t + 1) * tanPoint1
                    + (-6 * t * t + 6 * t) * end
                    + (3 * t * t - 2 * t) * tanPoint2).normalized;

        point.norm = Vector3.Cross(point.tan, Vector3.up).normalized / 2;
        CatmullRomPointsWithIndex.Enqueue(new CatMullRomNativeContainer(currentPoint * resolution + index, point));
    }
}