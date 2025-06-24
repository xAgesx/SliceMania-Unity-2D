using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class SwipeDetection : MonoBehaviour {

    [SerializeField]
    private float minimumdistance = 0.2f;
    [SerializeField]
    private float maximumTime = 1f;
    [SerializeField]
    private GameObject trail;
    private InputManager inputManager;
    private Vector2 startPos;
    private float startTime;
    private Vector2 endPos;
    private float endTime;
    private Coroutine coroutine;
    public List<GameObject> candyPrefabs;
    
    private void Awake() {
        inputManager = GetComponent<InputManager>();
    }

    private void OnEnable() {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;

    }
    private void OnDisable() {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;

    }

    private void SwipeStart(Vector2 position, float time) {
        
        startPos = position;
        startTime = time;
        trail.SetActive(true);
        trail.transform.position = position;
        coroutine = StartCoroutine(Trail());
    }
    private IEnumerator Trail() {
        while (true) {
            trail.transform.position = inputManager.primaryPosition();
            yield return null;
        }
    }
    private void SwipeEnd(Vector2 position, float time) {
        trail.GetComponent<CircleCollider2D>().isTrigger = true;
        DetectSwipe();
        trail.SetActive(false);
        endPos = position;
        endTime = time;
        /*var ItemToSlice = GetComponent<Pinch>().GetItemAtPosition(endPos);
        if(ItemToSlice != null)
            //sliceItem(ItemToSlice);
        */
        StopCoroutine(coroutine);
        
    }
    /*private void sliceItem(GameObject item) {
        if (item.gameObject.GetComponent<Item>().itemType != "Lollipop" ) {
            Destroy(item.gameObject);
            Vector2 pos = inputManager.primaryPosition();
            int index = candyPrefabs.IndexOf(item.gameObject);
            for (int i = 0; i < 2; i++) {
                //var spawnedItem = Instantiate(candyPrefabs[index], pos, Quaternion.Euler(0, 0, 0));
                
            }

        }
    }
    */
    private void DetectSwipe() {
        if (Vector3.Distance(startPos, endPos) > minimumdistance && (endTime - startTime) <= maximumTime) {
            Debug.DrawLine(startPos, endPos, Color.red, 5);

        }
    }
    

}
