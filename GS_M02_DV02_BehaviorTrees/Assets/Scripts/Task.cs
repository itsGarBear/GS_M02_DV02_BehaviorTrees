using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Task
{
    public abstract bool run();
}

public class IsTrue : Task
{
    bool varToTest;

    public IsTrue(bool myBool)
    {
        varToTest = myBool;
    }

    public override bool run()
    {
        return varToTest;
    }
}

public class IsFalse : Task
{
    bool varToTest;

    public IsFalse(bool myBool)
    {
        varToTest = myBool;
    }

    public override bool run()
    {
        return !varToTest;
    }
}

public class OpenDoor : Task
{
    Door myDoor;

    public OpenDoor(Door door)
    {
        myDoor = door;
    }

    public override bool run()
    {
        return !myDoor.Open();
    }
}

public class BargeDoor : Task
{
    Rigidbody myDoorRB;

    public BargeDoor(Rigidbody door)
    {
        myDoorRB = door;
    }

    public override bool run()
    {
        myDoorRB.AddForce(-10f, 0, 0, ForceMode.VelocityChange);
        return true;
    }
}

public class MoveToObject : Task
{
    Arriver myMover;
    GameObject myTarget;

    public MoveToObject(Kinematic mover, GameObject target)
    {
        myMover = mover as Arriver;
        myTarget = target;
    }

    public override bool run()
    {
        myMover.myTarget = myTarget;
        return true;
    }
}

public class Sequence : Task
{
    List<Task> childen;

    public Sequence(List<Task> taskList)
    {
        childen = taskList;
    }
    public override bool run()
    {
        foreach(Task child in childen)
        {
            if (child.run() == false)
                return false;
        }
        return true;
    }
}

public class Selector : Task
{
    List<Task> childen;

    public Selector(List<Task> taskList)
    {
        childen = taskList;
    }
    public override bool run()
    {
        foreach (Task child in childen)
        {
            if (child.run())
                return true;
        }
        return false;
    }
}
