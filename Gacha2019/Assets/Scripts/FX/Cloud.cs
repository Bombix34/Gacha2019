using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    float rotationSpeed;

    float minSize;
    float maxSize;

    float randChrono;

    float fadeSpeed;

    float radius;

    void Start()
    {
        radius = 12f;
        rotationSpeed = Random.Range(8f, 20f);
        minSize = Random.Range(0.5f, 0.6f);
        maxSize = Random.Range(1.2f, 1.5f);

        fadeSpeed = Random.Range(0.2f, 0.6f);

        randChrono = Random.Range(0.5f, 2f);

        InitPosition();
        InitRotation();
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.forward, rotationSpeed * Time.deltaTime);
        if (randChrono>0)
            randChrono -= Time.deltaTime;
        else if(randChrono<0)
        {
            StartCoroutine(ModifySize());
        }
    }

    public void InitPosition()
    {
        transform.position = RandomPointOnUnitCircle(11f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 20f);
    }

    public void InitRotation()
    {
        Vector3 vectorToTarget = Vector3.zero - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion qt = Quaternion.AngleAxis(angle+90, Vector3.forward);
        transform.rotation = qt;
    }

    public static Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        //un peu de random
        float randTweak = Random.Range(-0.05f, 0.05f);

        return new Vector2(x+randTweak, y+randTweak);
    }

    IEnumerator ModifySize()
    {
        if(transform.localScale.x > minSize)
        {
            while (transform.localScale.x > minSize)
            {
                randChrono = 0;
                transform.localScale = new Vector3(transform.localScale.x - (Time.deltaTime*fadeSpeed), transform.localScale.y - (Time.deltaTime * fadeSpeed), transform.localScale.z - (Time.deltaTime * fadeSpeed));
                yield return new WaitForSeconds(0.001f);
               
            }
            randChrono = Random.Range(1f, 3f);
        }
        else
        {
            while (transform.localScale.x < maxSize)
            {
                randChrono = 0;
                transform.localScale = new Vector3(transform.localScale.x + (Time.deltaTime * fadeSpeed), transform.localScale.y + (Time.deltaTime * fadeSpeed), transform.localScale.z + (Time.deltaTime * fadeSpeed));
                yield return new WaitForSeconds(0.001f);

            }
            randChrono = Random.Range(1f, 3f);
        }
    }
}
