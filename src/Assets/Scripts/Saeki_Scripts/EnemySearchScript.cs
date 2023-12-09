using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySearchScript : MonoBehaviour
{
    public enum Move
    {
        None,
        Light,
        Escape,
        Chase,
        Search,
        Stop,
        Heard,
    };

    [Header("�v���C���[�̃I�u�W�F�N�g�Ԃ�����")]
    [SerializeField] GameObject player;

    [Space]
    [Header("�s�����x (0.1f�`)")]
    [SerializeField] private float Speed = 0.9f;

    [Space]
    [Header("����ꏊ�̐��y�э��W")]
    [SerializeField] private Transform[] Waypoints;

    [Space]
    [Header("�Œ�G�̏������� (-180�`180)")]
    [SerializeField] private float StayRotation_y;

    [Space]
    [Header("���͊m�F�̐U�ꕝ�@(0.1f�`)")]
    [SerializeField] private float StopRotation = 0.0f;

    [Space]
    [Header("�ǐՂ̌p������ (0.1f�`)")]
    [SerializeField] private float ChaseInterval = 2.0f;

    [Space]
    [Header("�s����̑ҋ@���� (0.1f�`)")]
    [SerializeField] private float StopInterval = 3.0f;

    [Space]
    [Header("�����A�C�e��")]
    [SerializeField] private GameObject DropItem;

    [Space]
    [Header("�U��ނ��m��(n * 10 %)"),Range(0,10)]
    [SerializeField] private int Random_turn = 0;

    
    [Space]
    [Header("���̃��f��")]
    [SerializeField] private GameObject Moush;

    
    [Space]
    [Header("�A�j���[�^�[")]
    [SerializeField] private Animator anim;

    [Space]
    [Space]
    [Header("�s���@(��{�������Ȃ�)")]
    public Move EnemyMove; 

    private Move BeforeMove;

    [Space]
    [Header("�g���K�[�̖��̂͂��ׂĕ����Ăق���")]
    //[SerializeField] private GameObject Light;
    //EnemyLight _LIghtTrigger;
    [SerializeField] private GameObject Visbility;
    EnemyVisibility _VisbilityTrigger;
    [SerializeField] private GameObject Behind;
    BehindArea _BehindTrigger;

    HideScript _Hide;
    private NavMeshAgent agent;
    private SkinnedMeshRenderer blendshape_SMR;

    private bool MadeItem;
    private int destPoint;
    private float Interval;
    private bool RandamBack;
    private int BackRote_y = 0;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // autoBraking �𖳌��ɂ���ƁA�ڕW�n�_�̊Ԃ��p���I�Ɉړ�

        agent.autoBraking = false;

        //_LIghtTrigger = Light.GetComponent<EnemyLight>();

        _VisbilityTrigger = Visbility.GetComponent<EnemyVisibility>();

        _BehindTrigger = Behind.GetComponent<BehindArea>();

        _Hide = player.GetComponent<HideScript>();

        EnemyMove = Move.Search;
        BeforeMove = Move.None;

        destPoint = 0;
        agent.destination = Waypoints[destPoint].position;
        agent.speed = Speed;
        MadeItem = true;

        anim.SetBool("Surp", false);
        anim.SetBool("Stoping", false);
        blendshape_SMR = Moush.GetComponent<SkinnedMeshRenderer>();

        RandamBack = false;
    }
    void ChangeMove()
    {
        Debug.Log(_VisbilityTrigger.TimeOvercontroll);
        if (!_VisbilityTrigger.TimeOvercontroll && EnemyMove != Move.Escape && EnemyMove != Move.None)
        {
            EnemyMove = Move.Light;
            return;
        }

        if (!_Hide.HideMode &&_VisbilityTrigger.visbilityEnter && EnemyMove != Move.Escape && EnemyMove != Move.None)
        {
            if (BeforeMove != Move.Light)
                EnemyMove = Move.Chase;
            return;
        }
        else
            _VisbilityTrigger.visbilityEnter = false;

        if (_BehindTrigger.behindEnter && EnemyMove != Move.None)
        {
            EnemyMove = Move.Escape;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChangeMove();

        if (EnemyMove == Move.None)
        {
            anim.SetBool("Stoping", false);
            NoneCommand();
        }

        else if (EnemyMove == Move.Escape)
        {
            anim.SetBool("Surp", true);
            blendshape_SMR.SetBlendShapeWeight(0, 0);
            anim.SetBool("Stoping", false);
            EscapeCommand();
        }

        else if (EnemyMove == Move.Stop)
        {
            anim.SetBool("Stoping", true);
            StopCommand();
        }

        else if (EnemyMove == Move.Light)
        {
            anim.SetBool("Stoping", true);
            LightCommand();
        }

        else if (EnemyMove == Move.Heard)
        {
            anim.SetBool("Stoping", false);
            HeardCommand();
        }

        else if (EnemyMove == Move.Chase)
        {
            anim.SetBool("Stoping", false);
            ChaseCommand();
        }

        else if (EnemyMove == Move.Search)
        {
            anim.SetBool("Stoping", false);
            anim.SetBool("Surp", false);
            blendshape_SMR.SetBlendShapeWeight(0, 100);
            SearchCommand();
        }

        Debug.Log(EnemyMove, gameObject);

        BeforeMove = EnemyMove;
    }

    void NoneCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            RandamBack = false;
            BackRote_y = 0;
            Interval = 0.0f;
        }

        Interval += Time.deltaTime;

        if (Interval > 3f)
            EnemyMove = Move.Search;
    }
    void SearchCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            Interval = 0.0f;
        }

        if (agent.speed != Speed)
            agent.speed = Speed;

        if (BeforeMove != EnemyMove)
            agent.destination = Waypoints[destPoint].position;
        // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă����玟�̖ڕW�n�_��I��
        if (!agent.pathPending && agent.remainingDistance < 0.8f)
            GotoNextPoint();
    }

    void ChaseCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            RandamBack = false;
            BackRote_y = 0;
            Interval = 0.0f;
        }

        if (agent.speed != Speed)
            agent.speed = Speed;

        if (!_VisbilityTrigger.visbilityEnter)
            Interval += Time.deltaTime;
        else
            Interval = 0.0f;

        agent.destination = player.transform.position;
        //Debug.Log(player.transform.position);
        if (!_VisbilityTrigger.visbilityEnter && Interval > ChaseInterval)
        {
            Interval = 0.0f;
            EnemyMove = Move.Stop;
        }
    }

    void LightCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            Interval = 0.0f;
        }

        agent.speed = 0.0f;

        // �^�[�Q�b�g�ւ̌����x�N�g���v�Z
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0.0f;
        // �^�[�Q�b�g�̕����ւ̉�]
        
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.3f);

        //agent.destination = transform.position;

        if (!_VisbilityTrigger.visbilityEnter)
        {
            EnemyMove = Move.Stop;
        }
    }

    void HeardCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            RandamBack = false;
            BackRote_y = 0;
            Interval = 0.0f;

            Vector3 temp_pos = new Vector3();
            GameObject[] Binns = GameObject.FindGameObjectsWithTag("Fall");
            HaerdSoundsScript[] Script = new HaerdSoundsScript[Binns.Length];

            for (int i = 0; i < Binns.Length; i++)
            {
                temp_pos = Binns[i].transform.position;
                Script[i] = Binns[i].GetComponent<HaerdSoundsScript>();

                if (Script[i].Chack == 1)
                    break;
            }
            agent.destination = temp_pos;
        }

        if (!agent.pathPending && agent.remainingDistance < 1f)
            EnemyMove = Move.Stop;
    }

    void StopCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            Interval = 0.0f;
        }

        agent.destination = transform.position;
        Interval += Time.deltaTime;

        if (RandamBack)
        {
            BackRote_y++;

            if(BackRote_y < 250)
                transform.Rotate(new Vector3(0, 0.8f, 0));
            else
            {
                RandamBack = false;
                BackRote_y = 0;
            }   
        }
        else
        {
            if (Interval - StopInterval < 0)
            {
                if (StopInterval - Interval >= StopInterval * 3.0f / 4.0f)
                    transform.Rotate(new Vector3(0, StopRotation, 0));

                else if (StopInterval - Interval >= StopInterval / 4.0f)
                    transform.Rotate(new Vector3(0, -StopRotation, 0));

                else
                    transform.Rotate(new Vector3(0, StopRotation, 0));
            }
            else
            {
                Interval = 0.0f;
                agent.destination = Waypoints[destPoint].position;
                EnemyMove = Move.Search;
            }
        }
    }

    void EscapeCommand()
    {
        if (BeforeMove != EnemyMove)
        {
            RandamBack = false;
            BackRote_y = 0;
            Interval = 0.0f;

            agent.speed = 2.0f;

            agent.destination = transform.position - (player.transform.position - transform.position) * 2;
            Invoke("ActiveChange", 3.0f);
            EnemyMove = Move.None;
        }

        if (DropItem && BeforeMove != EnemyMove && MadeItem)
                Invoke("CreateItem", 2.0f);
    }

    void ActiveChange()
    {
        this.gameObject.SetActive(false);
    }

    void CreateItem()
    {
        MadeItem = false;
        GameObject obj = Instantiate(DropItem, transform.position, Quaternion.identity);
    }

    void GotoNextPoint()
    {
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (Waypoints.Length == 0)
            return;

        if (Waypoints.Length == 1)
        {
            transform.rotation = Quaternion.Euler(0, StayRotation_y, 0);
            EnemyMove = Move.Stop;
            return;
        }

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ�
        destPoint++;

        // �ꏄ������ŏ��̒n�_�Ɉړ�
        if (Waypoints.Length == destPoint)
        {
            destPoint = 0;
        }

        // �G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ�
        agent.destination = Waypoints[destPoint].position;

        if (Random.Range(0, 10) < Random_turn)//0�`10���烉���_��{
        {
            RandamBack = true;
            EnemyMove = Move.Stop;
        }
            
    }
}
