using System;
using UnityEngine;
using System.Collections.Generic;

    public class CatmullRom
    {
        [Serializable]
        public struct CatmullRomPoint
        {
            public Vector3 position;
            public Vector3 tangent;
            public Vector3 normal;

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
            if(splinePoints == null)
            {
                throw new NullReferenceException("Spline not Initialized!");
            }

            return splinePoints;
        }

        public CatmullRom(Transform[] controlPoints, int resolution, bool closedLoop)
        {
            if(controlPoints == null || controlPoints.Length <= 2 || resolution < 2)
            {
                throw new ArgumentException("Catmull Rom Error: Too few control points or resolution too small");
            }

            this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++)
            {
                this.controlPoints[i] = controlPoints[i].position;             
            }

            this.resolution = resolution;
            this.closedLoop = closedLoop;

            GenerateSplinePoints();
        }
        
        public void Update(Transform[] controlPoints)
        {
            if(controlPoints.Length <= 0 || controlPoints == null)
            {
                throw new ArgumentException("Invalid control points");
            }

            this.controlPoints = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++)
            {
                this.controlPoints[i] = controlPoints[i].position;             
            }

            GenerateSplinePoints();
        }
        
        public void Update(int resolution, bool closedLoop)
        {
            if(resolution < 2)
            {
                throw new ArgumentException("Invalid Resolution. Make sure it's >= 1");
            }
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
                    {
                        Debug.DrawLine(splinePoints[i].position, splinePoints[0].position, color);
                    }                
                    else if(i < splinePoints.Length - 1)
                    {
                        Debug.DrawLine(splinePoints[i].position, splinePoints[i+1].position, color);
                    }
                }
            }
        }
        public List<Vector3> pos(List<Vector3> vec)
        {
            if(ValidatePoints())
            {
                for(int i = 0; i < splinePoints.Length; i++)
                {
                    if(i == splinePoints.Length - 1 && closedLoop)
                    {
                        Debug.Log("nope");
                    }                
                    else if(i < splinePoints.Length - 1)
                    {

                      vec[i] = splinePoints[i].position;
                    }
                }
            }

            return vec;
        }
        public void DrawNormals(float extrusion, Color color)
        {
            if(ValidatePoints())
            {
                for(int i = 0; i < splinePoints.Length; i++)
                {
                    Debug.DrawLine(splinePoints[i].position, splinePoints[i].position + splinePoints[i].normal * extrusion, color);
                }
            }
        }

        public void DrawTangents(float extrusion, Color color)
        {
            if (ValidatePoints())
            {
                for (int i = 0; i < splinePoints.Length; i++)
                {
                    Debug.DrawLine(splinePoints[i].position,
                        splinePoints[i].position + splinePoints[i].tangent * extrusion, color);
                }
            }
        }

        private bool ValidatePoints()
        {
            if(splinePoints == null)
            {
                throw new NullReferenceException("Spline not initialized!");
            }
            return splinePoints != null;
        }

        private void InitializeProperties()
        {
            int pointsToCreate;
            if (closedLoop)
            {
                pointsToCreate = resolution * controlPoints.Length; //Loops back to the beggining, so no need to adjust for arrays starting at 0
            }
            else
            {
                pointsToCreate = resolution * (controlPoints.Length - 1);
            }

            splinePoints = new CatmullRomPoint[pointsToCreate];       
        }
        
        private void GenerateSplinePoints()
        {
            InitializeProperties();

            Vector3 p0, p1; 
            Vector3 m0, m1;
            
            int closedAdjustment = closedLoop ? 0 : 1;
            for (int currentPoint = 0; currentPoint < controlPoints.Length - closedAdjustment; currentPoint++)
            {
                bool closedLoopFinalPoint = (closedLoop && currentPoint == controlPoints.Length - 1);

                p0 = controlPoints[currentPoint];
                
                if(closedLoopFinalPoint)
                {
                    p1 = controlPoints[0];
                }
                else
                {
                    p1 = controlPoints[currentPoint + 1];
                }

                // m0
                if (currentPoint == 0) // Tangent M[k] = (P[k+1] - P[k-1]) / 2
                {
                    if(closedLoop)
                    {
                        m0 = p1 - controlPoints[controlPoints.Length - 1];
                    }
                    else
                    {
                        m0 = p1 - p0;
                    }
                }
                else
                {
                    m0 = p1 - controlPoints[currentPoint - 1];
                }

                // m1
                if (closedLoop)
                {
                    if (currentPoint == controlPoints.Length - 1) //Last point case
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                    else if (currentPoint == 0) //First point case
                    {
                        m1 = controlPoints[currentPoint + 2] - p0;
                    }
                    else
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                }
                else
                {
                    if (currentPoint < controlPoints.Length - 2)
                    {
                        m1 = controlPoints[(currentPoint + 2) % controlPoints.Length] - p0;
                    }
                    else
                    {
                        m1 = p1 - p0;
                    }
                }

                m0 *= 0.5f; //Doing this here instead of  in every single above statement
                m1 *= 0.5f;

                float pointStep = 1.0f / resolution;

                if ((currentPoint == controlPoints.Length - 2 && !closedLoop) || closedLoopFinalPoint) //Final point
                {
                    pointStep = 1.0f / (resolution - 1);  // last point of last segment should reach p1
                }

                // Creates [resolution] points between this control point and the next
                for (int tesselatedPoint = 0; tesselatedPoint < resolution; tesselatedPoint++)
                {
                    float t = tesselatedPoint * pointStep;

                    CatmullRomPoint point = Evaluate(p0, p1, m0, m1, t);

                    splinePoints[currentPoint * resolution + tesselatedPoint] = point;
                }
            }
        }
        public static CatmullRomPoint Evaluate(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
        {
            Vector3 position = CalculatePosition(start, end, tanPoint1, tanPoint2, t);
            Vector3 tangent = CalculateTangent(start, end, tanPoint1, tanPoint2, t);            
            Vector3 normal = NormalFromTangent(tangent);

            return new CatmullRomPoint(position, tangent, normal);
        }
        
        public static Vector3 CalculatePosition(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
        {
            Vector3 position = (2.0f * t * t * t - 3.0f * t * t + 1.0f) * start
                               + (t * t * t - 2.0f * t * t + t) * tanPoint1
                               + (-2.0f * t * t * t + 3.0f * t * t) * end
                               + (t * t * t - t * t) * tanPoint2;

            return position;
        }
        
        public static Vector3 CalculateTangent(Vector3 start, Vector3 end, Vector3 tanPoint1, Vector3 tanPoint2, float t)
        {
            Vector3 tangent = (6 * t * t - 6 * t) * start
                              + (3 * t * t - 4 * t + 1) * tanPoint1
                              + (-6 * t * t + 6 * t) * end
                              + (3 * t * t - 2 * t) * tanPoint2;

            return tangent.normalized;
        }
        
        public static Vector3 NormalFromTangent(Vector3 tangent)
        {
            return Vector3.Cross(tangent, Vector3.up).normalized / 2;
        }        
    }
