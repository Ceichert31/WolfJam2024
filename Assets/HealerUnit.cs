using UnityEngine;

public class HealerUnit : ShipUnit
{
    Health targetHealthScript;
    [SerializeField] int healingAmount;
    [SerializeField] float healingFrequency;
    bool canHeal = true;
    float timer;
    bool canFindNewHealthScript = false;
    [SerializeField] GameObject healParticles;

    void Start()
    {
        GetHealthScript();
    }

    // Update is called once per frame
    void Update()
    {
        if (canHeal)
        {
            timer += Time.deltaTime;
            if (timer > healingFrequency)
            {
                targetHealthScript.AddHealth(healingAmount);
                if (healParticles != null)
                {
                    Instantiate(healParticles, transform.position, Quaternion.identity);
                }
                timer = 0;
            }
        }

        if (targetHealthScript.CurrentHealth <= 0)
        {

            canHeal = false;
        }


        if (MyShipUnitState == ShipUnitState.Detatched)
        {
            canFindNewHealthScript = true;
        }

        if (MyShipUnitState == ShipUnitState.Attached && canFindNewHealthScript == true)
        {
            GetHealthScript();
            canHeal = true;
            canFindNewHealthScript = false;
        }
    }

    void GetHealthScript()
    {
        targetHealthScript = transform.parent.parent.GetComponent<Health>();
    }
}
