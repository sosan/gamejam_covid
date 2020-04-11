using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SA
{
    public class GameManager : MonoBehaviour
    {
        

         

        public Color[] diamondsColor;
        public Sprite[] diamondSpArray;

        public GameObject tempDiamond;


        //Mapa
        public int maxHeight = 15;
        public int maxWidth = 17;

        public Color color1;
        public Color color2;

        public Sprite mapSprite;
       
        //Player
        public Color cocheObj = Color.black;

        //Player Coche.
        GameObject cocheobj;
        SpriteRenderer cocheSRend;
        public Sprite playerPrefabSprite;

        //Apple
        public Color appleColor = Color.red;
        GameObject appleObj;
        public Sprite diamondSprite;

        // Vagoneta
        public Sprite vagonetaSprite;

        public Transform camaraHolder;

        GameObject tailParent;

        GameObject mapObject;
        GameObject playerObj;

        SpriteRenderer mapRenderer;

        Sprite playerSprite;

        Node[,] grid;

        [SerializeField]
        List<Node> avaliableNodes = new List<Node>();

        [SerializeField]
        List<SpecialNode> tail = new List<SpecialNode>();

        Node playerNode;
        Node prevPlayerNode;

        [SerializeField]
        public List<AppleNodeCl> appleNode = new List<AppleNodeCl>();

        bool up, down, right, left;

        public float moveRate = .5f;
        float timer;

        Direction targetDirection;
        Direction curDirection;

        public enum Direction
        {
            up, down, left, right
        }

        public UnityEvent onStart;
        public UnityEvent onGameOver;
        public UnityEvent firstinput;

        private Node node;

        public bool isGameOver;
        public bool isTheFirstInput;

        private Vector2Int gridMoveDirection;
        private Vector2Int gridPosition;

        
        #region Init

        private void Start()
        {
            onStart.Invoke();
        }

        public void StartNewGame()
        {
            ClearReferences();
            CreateMap();
            PlacePlayer();
            MovePlayer();
            PlaceCamara();
            CreateApple();
            //targetDirection = Direction.right;
            isGameOver = false;
        }

        public void ClearReferences() 
        {
            if(mapObject != null)
               Destroy(mapObject);
           
            if (playerObj != null)
                Destroy(playerObj);
           
            if (appleObj != null)
                Destroy(appleObj);
           
            foreach(var t in tail)
            {
                if (t.obj != null)
                    Destroy(t.obj);
            }
            tail.Clear();
            for (int i = 0; i < appleNode.Count; i++)
            {
                Destroy(appleNode[i].nodeGo);
            }
            appleNode.Clear();
            avaliableNodes.Clear();
            grid = null;
            
        }

        void CreateApple()

        {             
            

            for (ushort i = 0; i < 6; i++)
            {
                 RandomlyPlaceApple();
                
            }
        }

        void PlaceCamara()
        {
            Node n = GetNode(maxWidth / 2, maxHeight / 2);
            Vector3 p = n.worldPosition;
            p += Vector3.one * .5f;
            camaraHolder.position = p;
        }

        void CreateMap()
        {
            mapObject = new GameObject("map");
            mapRenderer = mapObject.AddComponent<SpriteRenderer>();
            

            grid = new Node[maxWidth, maxHeight];

            Texture2D txt = new Texture2D(maxWidth, maxHeight);

            for (int x = 0; x < maxWidth; x++)
            {
                for (int y = 0; y < maxHeight; y++)
                {
                    Vector3 tp = Vector3.zero;
                    tp.x = x;
                    tp.y = y;

                    Node n = new Node()
                    {
                        x = x,
                        y = y,
                        worldPosition = tp
                    };
                    grid[x, y] = n;

                    avaliableNodes.Add(n);

                    #region Visual
                    if (x % 2 != 0)
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color1);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color2);
                        }
                    }
                    else
                    {
                        if (y % 2 != 0)
                        {
                            txt.SetPixel(x, y, color2);
                        }
                        else
                        {
                            txt.SetPixel(x, y, color1);
                        }
                    }   
                    #endregion
                }
            }

            txt.filterMode = FilterMode.Point;

            txt.Apply();
            Rect rect = new Rect(0, 0, maxWidth, maxHeight);
            Sprite sprite = Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
           // Sprite sprite = mapSprite;
            mapRenderer.sprite = sprite;

        }

        void PlacePlayer()
        {
            playerObj = new GameObject("Player");
            playerSprite = CreateSprite(cocheObj);
            SpriteRenderer playerRender = playerObj.AddComponent<SpriteRenderer>();
            playerRender.sprite = playerPrefabSprite;
            playerRender.sortingOrder = 1;
            playerNode = GetNode(3, 3);
            PlacePlayerObject(playerObj, playerNode.worldPosition);
            playerObj.transform.localScale = Vector3.one * 10f;

            tailParent = new GameObject("TailParent");
        }
        #endregion

        #region Update

        private void Update()
        {

            if(isGameOver)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    onStart.Invoke();
                }
                return;
            }
                
           
            GetInput();
            SetPlayerDirection();

            if (isTheFirstInput)
            {
                

                timer += Time.deltaTime;
                if (timer > moveRate)
                {
                    curDirection = targetDirection;
                    MovePlayer();
                    timer = 0;
                }
            }
            else
            {
                if (up || down || left || right) 
                {
                    isTheFirstInput = true;
                    firstinput.Invoke();
                }

            }

        } 

        void GetInput()
        {
            up = Input.GetButtonDown("Up");
            down = Input.GetButtonDown("Down");
            left = Input.GetButtonDown("Left");
            right = Input.GetButtonDown("Right");

        }

        void SetPlayerDirection()
        {
            if (up)
            {
                SetDirection(Direction.up);
            }
            else if (down)
            {
                SetDirection(Direction.down);
            }
            else if (right)
            {
                SetDirection(Direction.right);
            }
            else if (left)
            {
                SetDirection(Direction.left);
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) +90);

        }

        void SetDirection(Direction d) 
        {
            if (!IsOpposite(d)) 
            { 
                targetDirection = d;
                timer = moveRate + 1; 
            }
        }

        void MovePlayer()
        {

            int x = 0;
            int y = 0;

            Vector3 targetDirection = Vector3.zero;

            switch (curDirection)
            {
                case Direction.up:
                    y = 1;
                    break;
                    
                case Direction.down:
                    y = -1;
                    break;

                case Direction.left:
                    x = -1;
                    break;

                case Direction.right:
                    x = 1;
                    break;
            }
            Node targetNode = GetNode(playerNode.x + x, playerNode.y + y);

            if (targetNode == null)
            {
                //Game Over
                onGameOver.Invoke(); 
            }
            else
            {
                if (IsTailNode(targetNode))
                {
                    //GameOver
                    onGameOver.Invoke();
                }
                else
                {
                    bool isScore = false;

                    if (IsAppleNode(targetNode))
                    {
                        isScore = true;                     
                    }

                    Node previousNode = playerNode;
                    avaliableNodes.Add(previousNode);

                    if (isScore)
                    {
                        for (int i = 0; i < appleNode.Count; i++)
                        {
                            if (appleNode[i].node == targetNode)
                            {
                                Destroy(appleNode[i].nodeGo);
                                appleNode.RemoveAt(i);
                            }
                        }
                        tail.Add(CreateTailNode(previousNode.x, previousNode.y));
                        avaliableNodes.Remove(previousNode);

                       
                    }
                    Movetail();
                    PlacePlayerObject(playerObj, targetNode.worldPosition);
                    playerObj.transform.position = targetNode.worldPosition;
                    playerNode = targetNode;
                    avaliableNodes.Remove(playerNode);

                    if (isScore)
                    {
                        if (avaliableNodes.Count > 0)
                        {
                            RandomlyPlaceApple();
                        }
                        else
                        {
                            //You won
                        }
                    }
                }
            }
        }

        void Movetail()
        {
            Node prevNode = null;

            for (int i = 0; i < tail.Count; i++)
            {
                SpecialNode p = tail[i];
                avaliableNodes.Add(p.node);

                if (i == 0)
                {
                    prevNode = p.node;
                    p.node = playerNode;
                }
                else
                {
                    Node prev = p.node;
                    p.node = prevNode;
                    prevNode = prev;
                }

                avaliableNodes.Remove(p.node);
                PlacePlayerObject(p.obj, p.node.worldPosition);
                p.obj.transform.position = p.node.worldPosition;
            }
        }


        #endregion

        #region Utilities

        bool IsOpposite(Direction d) 
        { 
         switch(d)
         {
                default:

                case Direction.up:
                    if (curDirection == Direction.down)
                        return true;
                    else
                        return false;  
               
               case Direction.down:
                    if (curDirection == Direction.up)
                        return true;
                    else
                        return false;
               
                case Direction.left:
                    if (curDirection == Direction.right)
                        return true;
                    else
                        return false;

                case Direction.right:
                    if (curDirection == Direction.left)
                        return true;
                    else
                        return false;


            }
        }

        bool IsTailNode(Node n)
        {
            for (int i = 0;  i < tail.Count;  i++)
            { 
              if(tail[i].node == n)
                {
                    return true;
                }
            }

            return false;
        }

        bool IsAppleNode(Node n)
        {
            for (int i = 0; i < appleNode.Count; i++)
            {
                if (appleNode[i].node == n)
                {
                    return true;
                }
            }

            return false;
        }
        void PlacePlayerObject(GameObject obj, Vector3 pos)
        {
            pos += Vector3.zero;
            obj.transform.position = pos;
        }

        bool IsValidDirecction(Node targetNode) 
        {
            return (targetNode == prevPlayerNode);
        }
        Node GetNode(int x, int y) 
        {
            if (x < 0 || x > maxWidth-1 || y < 0 || y > maxHeight-1)
                return null;
            return grid[x, y];
        }

        Sprite CreateSprite(Color targetColor)
        {
            Texture2D txt = new Texture2D(1, 1);
            txt.SetPixel(0, 0, targetColor);
            txt.Apply();
            txt.filterMode = FilterMode.Point;
            Rect rect = new Rect(0, 0, 1, 1);
            return Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect); //Bug
        }

        

       void RandomlyPlaceApple()
        {
            appleObj = new GameObject("Apple");
            SpriteRenderer appleRenderer = appleObj.AddComponent<SpriteRenderer>();
            appleRenderer.sortingOrder = 1;

            int ran = Random.Range(0, avaliableNodes.Count);
            AppleNodeCl appleNodeTemp = new AppleNodeCl();
            appleNodeTemp.node = avaliableNodes[ran];
                  
            int rndPrefabs = UnityEngine.Random.Range(0, diamondSpArray.Length);
           appleRenderer.sprite = diamondSpArray[rndPrefabs];

            int rndColor = UnityEngine.Random.Range(0, diamondsColor.Length);
            appleRenderer.color = diamondsColor[rndColor];

            PlacePlayerObject(appleObj, appleNodeTemp.node.worldPosition);
            appleNodeTemp.nodeGo = appleObj;
            appleNode.Add(appleNodeTemp);

        }

        SpecialNode CreateTailNode(int x, int y)
        {
            SpecialNode s = new SpecialNode();
            s.node = GetNode(x, y);
            s.obj = new GameObject();
            s.obj.transform.parent = tailParent.transform;
            s.obj.transform.position = s.node.worldPosition;
            s.obj.transform.localScale = Vector3.one * 0.95f;
            SpriteRenderer r = s.obj.AddComponent<SpriteRenderer>();
            r.sprite = vagonetaSprite;
            r.sortingOrder = 1;

            return s; 
        }

        public void GameOver()
        {
            isGameOver = true;
            isTheFirstInput = false;
        }

        private float GetAngleFromVector(Vector2Int dir)
        {
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }
        #endregion
    }

    [System.Serializable]
    public class AppleNodeCl 
    {
        public GameObject nodeGo;
        public Node node;
    }
}
