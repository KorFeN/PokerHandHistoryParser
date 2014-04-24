using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HandHistories.Statistics.Core
{
    /// <summary>
    /// This class creates all conditions and makes sure there is only one instance per tree
    /// and chains all stats in the correct order
    /// </summary>
    public class ConditionTree
    {
        enum ConditionTreeState
        {
            AddConditions,
            Ready
        }

        ConditionTreeState CurrentState = ConditionTreeState.AddConditions;

        List<Type> AllConditionTypes = new List<Type>();

        List<IStatisticCondition> AddConditions = new List<IStatisticCondition>();

        List<IStatisticCondition> CoreConditions = new List<IStatisticCondition>();

        public void AddCondition(Type ConditionType)
        {
            if (CurrentState != ConditionTreeState.AddConditions)
            {
                throw new InvalidOperationException("Can't add Conditions when we have prepared the tree");
            }
            if (AllConditionTypes.Contains(ConditionType))
            {
                throw new Exception("Condition already exists: " + ConditionType.FullName);
            }
            AllConditionTypes.Add(ConditionType);
        }

        public void InitializeTree()
        {
            if (CurrentState != ConditionTreeState.AddConditions)
            {
                throw new InvalidOperationException("Can't prepare the tree multiple times");
            }
            CurrentState = ConditionTreeState.Ready;

            foreach (var conditionType in AllConditionTypes)
            {
                CreateConditionFromType(conditionType);
            }

            foreach (var stat in AddConditions)
            {
                AddPrequisites(stat);
            }

            foreach (var stat in AddConditions)
            {
                if (stat.PrequisiteConditions == null)
                {
                    CoreConditions.Add(stat);
                }
                else
                {
                    foreach (var prequisite in stat.PrequisiteConditions)
                    {
                        IStatisticCondition chain = AddConditions.FirstOrDefault(p => p.GetType() == prequisite);
                        chain.ConditionTrigger += stat.EvaluateHand;
                    }
                }
            }
        }

        private IStatisticCondition CreateConditionFromType(Type item)
        {
            object statistic = item.GetConstructor(new Type[] { }).Invoke(new object[] { });
            IStatisticCondition newCondition = statistic as IStatisticCondition;
            if (newCondition == null)
            {
                throw new Exception("Not a StatisticCondition: " + statistic.GetType().FullName);
            }
            AddConditions.Add(newCondition);
            return newCondition;
        }

        private void AddPrequisites(IStatisticCondition stat)
        {
            if (stat.PrequisiteConditions == null)
            {
                return;
            }
            if (stat.PrequisiteConditions != null && stat.PrequisiteConditions.Count() > 1)
            {
                throw new Exception("Dont support multiPrequisites yet");
            }

            foreach (var prequisite in stat.PrequisiteConditions)
            {
                var Condition = AddConditions.FirstOrDefault(p => p.GetType() == prequisite);
                if (Condition == null)
                {
                    IStatisticCondition newCondition = CreateConditionFromType(prequisite);
                    AddPrequisites(newCondition);
                }
            }
        }

        public void EvaluateHand(GeneralHandData generalHand, PlayerHandData playerHand)
        {
            if (CurrentState != ConditionTreeState.Ready)
            {
                throw new Exception("Must initialize tree before evaluating hands");
            }
            foreach (var stat in CoreConditions)
            {
                stat.EvaluateHand(generalHand, playerHand);
            }
        }

        public IStatisticCondition GetHandCondition(Type statisticType)
        {
            if (CurrentState != ConditionTreeState.Ready)
            {
                throw new Exception("Must initialize tree before GetHandCondition() can be used");
            }
            return AddConditions.Find(p => p.GetType() == statisticType);
        }
    }
}
