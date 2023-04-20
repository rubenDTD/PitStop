using UnityEngine;

namespace PathCreation.Examples
{
    public class PathFollowerSlow : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float slowdownDistance = 2; // La distancia a la que el objeto comenzará a disminuir su velocidad
        public float maxSpeed = 8;
        public float minSpeed = 2; // La velocidad mínima a la que se moverá el objeto
        float distanceTravelled;

        private bool isSlowingDown = false; // Indica si el objeto está disminuyendo gradualmente su velocidad

        void Start()
        {
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                // Si el objeto está disminuyendo gradualmente su velocidad, actualizamos la velocidad en función de la distancia a la zona de colisión
                if (isSlowingDown)
                {
                    GameObject slowdownZone = GameObject.FindGameObjectWithTag("SlowDownZone");
                    float distanceToSlowdown = Vector3.Distance(transform.position, slowdownZone.transform.position);
                    if (distanceToSlowdown < slowdownDistance)
                    {
                        float newSpeed = Mathf.Lerp(minSpeed, speed, distanceToSlowdown / slowdownDistance);
                        speed = newSpeed;
                    }
                }

                // Movemos el objeto a lo largo del camino a la velocidad actual
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            }
        }

        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        // Detectamos cuando el objeto entra en la zona de colisión
        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("SlowDownZone"))
            {
                isSlowingDown = true;
            }
            else if(other.gameObject.CompareTag("Semaforo"))
            {
                // Get the TrafficLight component from the collider's game object
                TrafficLight trafficLight = other.GetComponent<TrafficLight>();
                // Check if the traffic light is red or yellow
                if (trafficLight)
                {
                    if (trafficLight.redOn || trafficLight.yellowOn)
                    {
                        // Reduce the object's speed gradually
                        speed = Mathf.Lerp(speed, 0f, Time.deltaTime);
                    }
                    else if (trafficLight.greenOn)
                    {
                        speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime);
                    }
                }
            }
        }

        // Detectamos cuando el objeto sale de la zona de colisión
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("SlowDownZone"))
            {
                isSlowingDown = false;
                speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime);
            }
        }
    }
}
