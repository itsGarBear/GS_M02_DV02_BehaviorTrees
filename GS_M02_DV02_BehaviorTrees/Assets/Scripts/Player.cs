using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Door door;
    public GameObject treasure;

    bool isDoingBehavior = false;
    Task currTask;
    private void Start()
    {
        isDoingBehavior = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            if(!isDoingBehavior)
            {
                isDoingBehavior = true;
                currTask = CreateTasksAndGetTreasure();
                currTask.run();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    Task CreateTasksAndGetTreasure()
    {
        List<Task> taskList = new List<Task>();

        Task checkOpenDoor = new IsFalse(door.isLocked);
        Task openDoor = new OpenDoor(door);
        taskList.Add(checkOpenDoor);
        taskList.Add(openDoor);
        Sequence openUnlockedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorClosed = new IsTrue(door.isClosed);
        Task bargeDoor = new BargeDoor(door.transform.GetChild(0).GetComponent<Rigidbody>());
        taskList.Add(isDoorClosed);
        taskList.Add(bargeDoor);
        Sequence breakClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(openUnlockedDoor);
        taskList.Add(breakClosedDoor);
        Selector openTheDoor = new Selector(taskList);

        //
        taskList = new List<Task>();
        Task moveToDoor = new MoveToObject(this.GetComponent<Arriver>(), door.gameObject);
        Task moveToTreasure = new MoveToObject(this.GetComponent<Arriver>(), treasure.gameObject);
        taskList.Add(moveToDoor);
        taskList.Add(openTheDoor);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindClosedDoor = new Sequence(taskList);

        taskList = new List<Task>();
        Task isDoorOpen = new IsFalse(door.isClosed);
        taskList.Add(isDoorOpen);
        taskList.Add(moveToTreasure);
        Sequence getTreasureBehindOpenDoor = new Sequence(taskList);

        taskList = new List<Task>();
        taskList.Add(getTreasureBehindOpenDoor);
        taskList.Add(getTreasureBehindClosedDoor);
        Selector getTreasure = new Selector(taskList);

        return getTreasure;
    }
}
