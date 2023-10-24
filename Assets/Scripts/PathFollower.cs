using UnityEngine;
using DG.Tweening;

namespace PathCreation.Examples
{

    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;
        public   bool isMoving = false;
      //public  Player player;
      // public GameManager gameManager;
      //  SoundManager soundManager;
        
      
        void Start()
        {
            //distanceTravelled += 0.2f;
            //transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction));
          

            //player = FindObjectOfType<Player>();
            //soundManager = FindObjectOfType<SoundManager>();
            //gameManager =GameManager.instance;
            if (pathCreator != null)
            {
                pathCreator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            //if(!player.iswin&& !player.islose && gameManager.isstart)
            //{
            //if (Input.GetMouseButtonDown(0))
            //{
            //        soundManager.MotorOn(true);
            //                isMoving = true;
            //}
            //if (Input.GetMouseButtonUp(0))
            //{
            //        player.stack.parent.DOLocalRotate(new Vector3(10f, 0f, 0f), 0.2f).SetLoops(2, LoopType.Yoyo);
                   
            //        soundManager.MotorOn(false);
            //        isMoving = false;
            //}
            if (pathCreator != null)
            {
                   distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled + 0.2f, endOfPathInstruction));
            }
            

        }
       public void Reset_Player_Pos_Rot()
        {
            distanceTravelled -= 10;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            transform.LookAt(pathCreator.path.GetPointAtDistance(distanceTravelled+0.2f, endOfPathInstruction));
        }
     
        void OnPathChanged()
        {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
  

    
}