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

    public PlayerController PCZ;
    public PlayerController PCRZ;
    public PlayerController PCT;
    public PlayerController PC7;
    public PlayerController PCR7;
    public PlayerController PCSquare;
    public PlayerController PCBar;

    public List<GameObject> LstPlayerController;

    private Rigidbody2D m_rigidBody;
    private bool m_bNoSiulate;
    private float m_fCurrentTimeNoToSimulateRigid;

    private SpawnPosition m_spawnPosition;
    
	// Use this for initialization
	void Start () {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spawnPosition = FindObjectOfType<SpawnPosition>();
        if(m_spawnPosition == null)
        {
            Debug.Log("SpawnPostion Not Found");
        }
        


    }

    // Update is called once per frame
    void Update () {
		if( Input.GetKey(LeftKeycode) == true )
        {
            //transform.position.Set(transform.position.x + MoveDiff, transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x - MoveDiff, transform.position.y, transform.position.z);
        }
        if( Input.GetKey(RightKeycode) == true )
        {
            //transform.position.Set(transform.position.x - MoveDiff, transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x + MoveDiff, transform.position.y, transform.position.z);
        }
        if( Input.GetKeyUp(RotationKeycode) == true)
        {
            transform.Rotate(0, 0, 90);
        }
        if( Input.GetKeyDown(JumpKeycode) == true )
        {
            transform.position = new Vector3(transform.position.x , transform.position.y + JumpDiff, transform.position.z);
            m_bNoSiulate = true;
            m_rigidBody.simulated = false;
            m_fCurrentTimeNoToSimulateRigid = 0.0f;
        }
        if( Input.GetKeyDown(FixedKeycode) == true)
        {
            m_bNoSiulate = false;
            m_rigidBody.simulated = true;
            enabled = false;

            int nRanResult = Random.Range(0, LstPlayerController.Count - 1);

            if(nRanResult < 0 || nRanResult >6 )
            {
                int a = 0;
            }
            else
            {
                GameObject dummyObj = Instantiate(LstPlayerController[nRanResult], m_spawnPosition.transform.position, Quaternion.identity);
                dummyObj.transform.SetParent(transform.parent);
                dummyObj.GetComponent<PlayerController>().enabled = true;
            }

        }
        if(m_bNoSiulate == true)
        {
            m_fCurrentTimeNoToSimulateRigid += Time.deltaTime;
            if( m_fCurrentTimeNoToSimulateRigid >= TimeNoToSimulateRigid)
            {
                m_bNoSiulate = false;
                m_rigidBody.simulated = true;
            }
        }
	}
}
