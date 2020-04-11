using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    private static SpellBook instance;

    public static SpellBook MyInstance//Singelton Design Pattern
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpellBook>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Image castingBar;//A reference to the hero casting bar

    [SerializeField]
    private Text currentSpell;//A reference to the spell name on the casting bar

    [SerializeField]
    private Text castTime;//A reference to the casting time on the casting bar

    [SerializeField]
    private Image icon;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Spell[] spells;
    // Start is called before the first frame update

    private Coroutine spellRoutine;

    private Coroutine fadeRoutine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spell CastSpell(string spellName)//Cast a spell at an enemy
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        castingBar.fillAmount = 0;//Reset the fillamount

        castingBar.color = spell.MyBarColor;//Changes the bars color to fit the current spell
        
        currentSpell.text = spell.MyName;//Changes the text on the bar so that we can read what spell we are casting

        icon.sprite = spell.MyIcon;//Changes the icon on the casting bar

        spellRoutine = StartCoroutine(Progress(spell));//Starts casting the spells

        fadeRoutine = StartCoroutine(FadeBar());//Start fading the bar

        return spell;//Return the spell that we just cast
    }

    private IEnumerator Progress(Spell spell)//Updates the casting bar according to the current progress of the cast
    {
        float timePassed = Time.deltaTime;// How much time has passed since we started castig the spell

        float rate = 1.0f / spell.MyCastTime;// How fast are we gooing to move the bar

        float progress = 0.0f;//The current progress of the cast

        while(progress <= 1.0)//As long as we are not done casting
        {
            castingBar.fillAmount = Mathf.Lerp(0, 1,progress);//Updates the bar based on the progress

            progress += rate * Time.deltaTime;// Increases the progress

            timePassed += Time.deltaTime;//Updates the time passed

            castTime.text = (spell.MyCastTime - timePassed).ToString("F2");//Updates the cast time text

            if(spell.MyCastTime - timePassed < 0)//Makes sure that the time doesnt go below 0
            {
                castTime.text = "0.00";
            }

            yield return null;
        }

        StopCasting();//Stops our cast when we ae done
    }

    private IEnumerator FadeBar()//Fades the bar in on the screen when we start casting
    {

        float rate = 1.0f / 0.25f;//How fast are we going to fade the bar

        float progress = 0.0f;

        while (progress <= 1.0)//As long as we are not done fading
        {

            canvasGroup.alpha = Mathf.Lerp(0, 1, progress);//Update the alpha on the cnavasgroup

            progress += rate * Time.deltaTime;//Update the progress

            yield return null;
        }
    }


    public void StopCasting()//stops a cast
    {
       if(fadeRoutine != null)//stops the faderoutine
        {
            StopCoroutine(fadeRoutine);
            canvasGroup.alpha = 0;
            fadeRoutine = null;
        }

       if(spellRoutine != null)//Stops the spellroutine
        {
            StopCoroutine(spellRoutine);
            spellRoutine = null;
        }
    }

    public Spell GetSpell(string spellName)
    {
        Spell spell = Array.Find(spells, x => x.MyName == spellName);

        return spell;
    }
}
