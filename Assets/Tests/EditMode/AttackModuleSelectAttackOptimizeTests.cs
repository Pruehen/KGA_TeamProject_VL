using NUnit.Framework;
using UnityEngine;
using EnumTypes;
using System.Diagnostics;

[TestFixture]
public class AttackModuleSelectAttackOptimizeTests
{
    private EnemyAttack _enemyAttack;
    private const int TestIterations = 1000000;

    [SetUp]
    public void SetUp()
    {
        _enemyAttack = new EnemyAttack();
        InitializeEnemyAttack();
    }

    private void InitializeEnemyAttack()
    {
        _enemyAttack._modules = new AttackModule[10];
        for (int i = 0; i < _enemyAttack._modules.Length; i++)
        {
            _enemyAttack._modules[i] = new AttackModule();
            _enemyAttack._modules[i].AttackModuleData = CreateTestAttackModuleData(i);
        }

        _enemyAttack._defaultAttack = new AttackModule();
        _enemyAttack._defaultAttack.AttackModuleData = CreateTestAttackModuleData(0);
        _enemyAttack._attackRangeType = AttackRangeType.Close;
        _enemyAttack._phase = Phase.First;
    }

        private SO_AttackModule CreateTestAttackModuleData(int index)
    {
        // Create a test SO_AttackModule
        // You might need to create a simple version of this for testing purposes
        var moduleData = ScriptableObject.CreateInstance<SO_AttackModule>();
        moduleData.Priority = UnityEngine.Random.Range(1, 6);
        moduleData.AttackRangeType = AttackRangeType.Close;
        moduleData.Phase = Phase.First;
        // Set other necessary properties
        return moduleData;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void PerformanceTest_TryGetPriorityAttack()
    {
        Stopwatch stopwatch = new Stopwatch();

        // old method 테스트
        stopwatch.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            _enemyAttack.TryGetPriorityAttack_old();
        }
        stopwatch.Stop();
        long oldMethodTime = stopwatch.ElapsedMilliseconds;

        stopwatch.Reset();
        // new method 테스트
        stopwatch.Start();
        for (int i = 0; i < TestIterations; i++)
        {
            _enemyAttack.TryGetPriorityAttack();
        }
        stopwatch.Stop();
        long newMethodTime = stopwatch.ElapsedMilliseconds;

        UnityEngine.Debug.Log($"Old method time: {oldMethodTime}ms");
        UnityEngine.Debug.Log($"New method time: {newMethodTime}ms");
        UnityEngine.Debug.Log($"Performance improvement: {(oldMethodTime - newMethodTime) / (float)oldMethodTime * 100}%");
        Assert.Less(newMethodTime, oldMethodTime, "New method should be faster than the old method");
    }
}
