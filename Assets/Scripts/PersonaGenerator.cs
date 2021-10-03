using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonaGenerator : MonoBehaviour
{

    public SpriteHuman femaleRig;
    public SpriteHuman maleRig;


    public List<Sprite> maleBodies = new List<Sprite>();
    public List<Sprite> femaleBodies = new List<Sprite>();


    public List<Sprite> maleClothes = new List<Sprite>();
    public List<Sprite> femaleClothes = new List<Sprite>();

    public List<Sprite> maleHair = new List<Sprite>();
    public List<Sprite> femaleHair = new List<Sprite>();

    public List<Sprite> femaleEyes = new List<Sprite>();
    public List<Sprite> femaleMouths = new List<Sprite>();
    public List<Sprite> femaleNoses = new List<Sprite>();

    public List<Sprite> maleEyes = new List<Sprite>();
    public List<Sprite> maleMouths = new List<Sprite>();
    public List<Sprite> maleNoses = new List<Sprite>();
    public GameObject GenerateHuman()
    {
        SpriteHuman rig;

        if (Random.value >= 0.5f)
        {
            rig = Instantiate(femaleRig);
            rig.body.sprite = femaleBodies[Random.Range(0, femaleBodies.Count)];
            rig.clothes.sprite = femaleClothes[Random.Range(0, femaleClothes.Count)];

            rig.hair.sprite = femaleHair[Random.Range(0, femaleHair.Count)];
            rig.eyes.sprite = femaleEyes[Random.Range(0, femaleEyes.Count)];
            rig.mouth.sprite = femaleMouths[Random.Range(0, femaleMouths.Count)];
            rig.nose.sprite = femaleNoses[Random.Range(0, femaleNoses.Count)];
        }
        else
        {
            rig = Instantiate(maleRig);
            rig.body.sprite = maleBodies[Random.Range(0, maleBodies.Count)];
            rig.clothes.sprite = maleClothes[Random.Range(0, maleClothes.Count)];

            rig.hair.sprite = maleHair[Random.Range(0, maleHair.Count)];
            rig.eyes.sprite = maleEyes[Random.Range(0, maleEyes.Count)];
            rig.mouth.sprite = maleMouths[Random.Range(0, maleMouths.Count)];
            rig.nose.sprite = maleNoses[Random.Range(0, maleNoses.Count)];
        }


        return rig.gameObject;
    }


}
