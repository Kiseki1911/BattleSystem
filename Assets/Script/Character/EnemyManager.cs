using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //private Fatigue fatigue;
    public Vector3 targetPos;
    public float speed = 0.02f;
    public GameObject weapon;
    public float weaponRotation_z;
    public float rotation_z;
    public int curHealth;
    public int maxHealth = 100;

    public int attackDamange=20;
    
    public bool isRoundAttack;
    public Material [] drop;
    public GameObject materialCube;
    public ParticleSystem deathEffect;

    public Collider2D[] unitSees = new Collider2D[5];
    public bool isSeeingPlayer=false;
    public int seeRange=5;
    [SerializeField]private bool moving=true;
    public Vector3 curSpeed;
    private Vector3 oldPos;
    
    private int fireTimer=0;
    private int waterTimer=0;
    private int windTimer=0;
    private int poisonTimer=0;
    
    RaycastHit2D[] rayResults;
    // Start is called before the first frame update
    void Start()
    {
        oldPos=transform.position;
        CharacterReset();
        targetPos = transform.position;
        //fatigue = Fatigue.Instance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(moving)
            transform.position = Vector3.Lerp(transform.position,targetPos,speed);
        //fatigue.DecreaseFat(1);
        curSpeed=transform.position-oldPos;
        oldPos=transform.position;
    }
    public void Movement(Vector3 directionUnit){
        rayResults = Physics2D.RaycastAll(transform.position,directionUnit,1f);
        for(int i=0; i <rayResults.Length;i++){
            if(rayResults[i].collider.tag=="Wall"){
                //Debug.Log("wall ahead");
                return;
            }
        }
        Debug.DrawRay(transform.position, directionUnit, Color.green);
        if((targetPos-transform.position).magnitude<0.05){
            targetPos =Vector3Int.RoundToInt(transform.position)+directionUnit;
            //fatigue.IncreaseFat(30);
            isSeeingPlayer=SeePlayer(seeRange);
            
        }
    }
    public void RoundAttack(){
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float mSpeed;
        //weapon.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset);
        if(rotation_z<0){
            rotation_z+=360f;
        }
        weaponRotation_z=weapon.transform.eulerAngles.z;
        mSpeed = weaponRotation_z-rotation_z;
        //Debug.Log(mSpeed);
        JointMotor2D motorRef = weapon.GetComponent<HingeJoint2D>().motor;
        motorRef.motorSpeed = mSpeed;
        weapon.GetComponent<HingeJoint2D>().motor = motorRef;
    }
    public void WeaponReset(){
        Debug.Log("in reset");
        weapon.transform.localEulerAngles =Vector3.zero;
        weapon.transform.localPosition =new Vector3(1f,0.01f,0);
        //Debug.Log(weapon.transform.localPosition);
        //weapon.transform.eulerAngles =new Vector3(0f,0f,30f);
        JointMotor2D motorRef = weapon.GetComponent<HingeJoint2D>().motor;
        motorRef.motorSpeed = 0f;
        weapon.GetComponent<HingeJoint2D>().motor = motorRef;
        //Debug.Log(weapon.transform.localPosition);
    }

    public void CharacterReset(){
        curHealth=maxHealth;
    }
    public void TakeDamage(int dmg,List<SpecialEffect> effect=null){
        curHealth -= ((int)(dmg*(1+0.1*waterTimer)));
        if(curHealth<=0){
            OnDeath();
        }
        foreach (var item in effect)
        {
            switch (item.type)
            {
                case DmgType.FIRE:
                    if(fireTimer<=0){
                        fireTimer=item.strength;
                        StartCoroutine(onfire());
                    }
                    else{
                        fireTimer+=item.strength;
                    }
                    break;
                case DmgType.WATER:
                    if(waterTimer<=0){
                        waterTimer=item.strength;
                        StartCoroutine(onWind());
                    }
                    else{
                        windTimer+=item.strength;
                    }
                    break;
                case DmgType.WIND:
                    if(windTimer<=0){
                        windTimer=item.strength;
                        StartCoroutine(onWind());
                    }
                    else{
                        windTimer+=item.strength;
                    }
                    break;
                case DmgType.POISON:if(poisonTimer<=0){
                        poisonTimer=item.strength;
                        StartCoroutine(onPoison());
                    }
                    else{
                        poisonTimer+=item.strength;
                    }
                    break;
            }
        }
    }
    private void OnDeath(){
        Instantiate(deathEffect,transform.position, Quaternion.identity);
        DropItem();
        if(transform.GetComponentInChildren<Projectile>()!=null){
            transform.GetComponentInChildren<Projectile>().gameObject.transform.SetParent(null);
        }
        transform.parent.GetComponent<roomGen>().onEnemyDeath();
        gameObject.SetActive(false);
        foreach (var item in drop)
        {
            Debug.Log(item.name);
            var cube=Instantiate(materialCube,transform.position+(Vector3)Random.insideUnitCircle,Quaternion.identity);
            cube.GetComponent<materialObj>().setMaterial(item);
        }
    }

    void DropItem(){

    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("coli something");
        if(other.gameObject.GetComponentInParent<CharacterActions>()!=null && other.CompareTag("Player")){
            Debug.Log("hit player");
            var collisionPoint = other.ClosestPoint(transform.position);
            Debug.Log(collisionPoint);
            WeaponManager.Instance.onHit=true;
            StartCoroutine(WeaponManager.Instance.OnHit(collisionPoint));
            other.gameObject.GetComponentInParent<CharacterActions>().TakeDamage(attackDamange);
            Debug.Log(other.gameObject.GetComponentInParent<CharacterActions>().curHealth);
        }
    }

    private bool SeePlayer(int viewRange){
        int units = Physics2D.OverlapCircleNonAlloc(transform.position, viewRange,unitSees);
        //Debug.Log(units);
        if(units>0){
            for(int i =0;i<units;i++){
                //Debug.Log(unitSees[i]);
                if(unitSees[i].CompareTag("Player")){
                    //Debug.Log("sees player");
                    return true;
                }
            }
        }
        return false;
    }
    IEnumerator  onfire(){
        while (fireTimer>0)
        {
            curHealth -= ((int)((float)maxHealth*0.05f));
            if(curHealth<=0){
                OnDeath();
                break;
            }
            fireTimer--;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator  onPoison(){
        while (poisonTimer>0)
        {
            curHealth -= 3*poisonTimer;
            if(curHealth<=0){
                OnDeath();
                break;
            }
            poisonTimer--;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerable onWater(){
        while (waterTimer>0)
        {
            waterTimer--;
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator  onWind(){
        moving=false;
        while (windTimer>0)
        {
            windTimer--;
            yield return new WaitForSeconds(1);
        }
        moving=true;
    }
}
