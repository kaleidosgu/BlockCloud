using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public KeyCode LeftKeycode;
    public KeyCode RightKeycode;
    public KeyCode RotationKeycode;
    public KeyCode JumpKeycode;
    public KeyCode FixedKeycode;

    public float MoveDiff;
    public float JumpDiff;
    public float TimeNoToSimulateRigid;
    
    public bool NotUpdateExecution;

    public List<GameObject> LstPlayerController;

    public float JumpSpeed;

    private Rigidbody2D m_rigidBody;
    private bool m_bNoSiulate;
    private float m_fCurrentTimeNoToSimulateRigid;

    private SpawnPosition m_spawnPosition;

    private bool m_bBeFixed;

    private bool m_bGenerated;

    private bool m_bCanJump;
    
	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spawnPosition = FindObjectOfType<SpawnPosition>();
        if(m_spawnPosition == null)
        {
            Debug.Log("SpawnPostion Not Found");
        }
        NotUpdateExecution = false;
        m_bCanJump = true;

    }

    // Update is called once per frame
    void Update ()
    {
        if (m_bNoSiulate == true)
        {
            m_fCurrentTimeNoToSimulateRigid += Time.deltaTime;
            if (m_fCurrentTimeNoToSimulateRigid >= TimeNoToSimulateRigid)
            {
                m_bNoSiulate = false;
                if (m_rigidBody != null)
                {
                    m_rigidBody.simulated = true;
                }
            }
        }
        if(m_bGenerated == false)
        {
            if (Input.GetKeyDown(FixedKeycode) == true)
            {
                _generateBlock();
                m_bGenerated = true;
            }
        }
        if (NotUpdateExecution == true)
        {
            return;
        }
		if( Input.GetKey(LeftKeycode) == true )
        {
            transform.position = new Vector3(transform.position.x - MoveDiff, transform.position.y, transform.position.z);
        }
        if( Input.GetKey(RightKeycode) == true )
        {
            transform.position = new Vector3(transform.position.x + MoveDiff, transform.position.y, transform.position.z);
        }
        if( Input.GetKeyUp(RotationKeycode) == true)
        {
            transform.Rotate(0, 0, 90);
        }
        if( Input.GetKeyDown(JumpKeycode) == true )
        {
            _jumpProcess();
            _controlEulerAngle();
        }
	}
    private void _controlEulerAngle()
    {
        float fCurrentAngle = transform.localEulerAngles.z;
        float fChangeAngle = 0;
        if( fCurrentAngle >= 0 && fCurrentAngle < 45 )
        {
            fChangeAngle = 0;
        }
        else if (fCurrentAngle > 45 && fCurrentAngle <= 135)
        {
            fChangeAngle = 90;
        }
        else if (fCurrentAngle > 135 && fCurrentAngle <= 225)
        {
            fChangeAngle = 180;
        }
        else if (fCurrentAngle > 225 && fCurrentAngle <= 315)
        {
            fChangeAngle = 270;
        }
        else
        {
            fChangeAngle = 0;
        }
        transform.localEulerAngles = new Vector3(0, 0, fChangeAngle);
    }

    private void _jumpBlock()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y + JumpDiff, transform.position.z);
        //m_bNoSiulate = true;
        //if (m_rigidBody != null)
        //{
        //    m_rigidBody.simulated = false;
        //}
        //m_fCurrentTimeNoToSimulateRigid = 0.0f;

        //m_bJump = true;
        _jumpProcess();
    }

    private void _jumpProcess()
    {
        if(m_bCanJump == true)
        {
            //float fAddForce = JumpSpeed * Time.deltaTime;
            float fAddForce = JumpSpeed ;
            m_rigidBody.AddForce(Vector2.up * fAddForce);
            m_bCanJump = false;
        }
    }
    private void _destroyRigid()
    {
        m_rigidBody = null;
        Destroy(GetComponent<Rigidbody2D>());
    }
    private void _generateBlock()
    {
        m_bNoSiulate = false;

        NotUpdateExecution = true;
        m_bBeFixed = true;

        int nRanResult = Random.Range(0, LstPlayerController.Count - 1);

        if (nRanResult < 0 || nRanResult >= LstPlayerController.Count)
        {
            Debug.Log(string.Format("nRanResult[{0}]", nRanResult));
        }
        else
        {
            if (m_spawnPosition != null)
            {
                if (LstPlayerController[nRanResult] != null)
                {
                    GameObject dummyObj = Instantiate(LstPlayerController[nRanResult], m_spawnPosition.transform.position, Quaternion.identity);
                    dummyObj.transform.SetParent(transform.parent);
                    dummyObj.GetComponent<PlayerController>().NotUpdateExecution = false;
                }
                else
                {
                    Debug.Log(string.Format("LstPlayerController cout {0} nRanResult = {1}", LstPlayerController.Count, nRanResult));
                }
            }
            else
            {
                Debug.Log("m_spawnPosition is null");
            }
        }
        NotUpdateExecution = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (BlockGlobalDefine.CurrentCountsCreated <= BlockGlobalDefine.MaxCountsCreated)
        //{
        //    BlockGlobalDefine.CurrentCountsCreated++;
        //    //if (collision.gameObject.tag == BlockGlobalDefine.TagBlock || collision.gameObject.tag == BlockGlobalDefine.TagGround)
        //    if (collision.gameObject.tag == BlockGlobalDefine.TagBlock )
        //    {
        //        m_rigidBody = null;
        //        Destroy(GetComponent<Rigidbody2D>());
        //        NotUpdateExecution = true;
        //    }
        //}
        m_bCanJump = true;

        if(m_bBeFixed == true)
        {
            if (m_rigidBody != null)
            {
                m_rigidBody.simulated = true;
            }
            _destroyRigid();
        }
    }
}
