using System.Collections.Generic;
using BehaviorTree;

public class GuardBT : Tree
{
    public UnityEngine.Transform[] waypoints;

    public static float speed = 2f;
    public static float fovRange = 2f;
    public static float attackRange = 1f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
    {
        new Sequence(new List<Node>
        {
            new CheckEnemyInAttackRange(transform),
            new TaskAttack(transform),
        }),

        new Sequence(new List<Node>
        {
            new CheckEnemyInFOVRange(transform),
            new TaskGoToTarget(transform),
        }),

        new Sequence(new List<Node>
        {
            new CheckIfBlockade(transform),
            new TaskDestroyBlockade(transform),
        }),

        new Sequence(new List<Node>
        {
            new CheckNoEnemiesInScene(),
            new TaskDance(transform),
        }),

        new TaskInspectBox(transform),
        new TaskPatrol(transform, waypoints),
    });

        return root;
    }
}