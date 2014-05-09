using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace HandHistories.Statistics.Core
{
    /// <summary>
    /// This class creates all conditions and makes sure there is only one instance per tree
    /// and chains all stats in the correct order.
    /// 
    /// The condition tree only accepts Types that inherits IStatisticCondition that is to prevent modification
    /// of the conditions by using constructor parameters however inhereitance is allowed
    /// </summary>
    public class ConditionTree
    {
        enum ConditionTreeState
        {
            AddConditions,
            Ready
        }

        public event Action<ConditionTree> InitializationFinnished;

        ConditionTreeState CurrentState = ConditionTreeState.AddConditions;

        List<Type> AddedConditions = new List<Type>();

        List<IStatisticCondition> AllConditions = new List<IStatisticCondition>();

        List<IStatisticCondition> CoreConditions = new List<IStatisticCondition>();

        public void AddStatistic(IStatistic statistic)
        {
            foreach (var condition in statistic.Conditions)
            {
                AddCondition(condition);
            }
        }

        public void AddCondition(Type ConditionType)
        {
            if (CurrentState != ConditionTreeState.AddConditions)
            {
                throw new InvalidOperationException("Can't add Conditions when we have prepared the tree");
            }
            if (!AddedConditions.Contains(ConditionType))
            {
                AddedConditions.Add(ConditionType);
            } 
        }

        /// <summary>
        /// Instaniates all conditions and creates the tree
        /// </summary>
        public void InitializeTree()
        {
            if (CurrentState != ConditionTreeState.AddConditions)
            {
                throw new InvalidOperationException("Can't initialize the tree multiple times");
            }
            
            //Instansiates all conditions
            foreach (var conditionType in AddedConditions)
            {
                CreateConditionFromType(conditionType);
            }

            //Instansiating all prequisite conditions
            List<IStatisticCondition> allConditions = new List<IStatisticCondition>(AllConditions);
            foreach (var stat in allConditions)
            {
                AddPrequisites(stat);
            }

            foreach (var stat in AllConditions)
            {
                if (stat.PrequisiteConditions == null)
                {
                    CoreConditions.Add(stat);
                }
                else
                {
                    foreach (var prequisite in stat.PrequisiteConditions)
                    {
                        IStatisticCondition chain = AllConditions.FirstOrDefault(p => p.GetType() == prequisite);
                        chain.ConditionTrigger += stat.EvaluateHand;
                    }
                }
            }

            if (InitializationFinnished != null)
            {
                InitializationFinnished(this);
            }
            AddedConditions = null;
            CurrentState = ConditionTreeState.Ready;
        }

        private IStatisticCondition CreateConditionFromType(Type item)
        {
            object statistic = item.GetConstructor(new Type[] { }).Invoke(new object[] { });
            IStatisticCondition newCondition = statistic as IStatisticCondition;
            if (newCondition == null)
            {
                throw new Exception("Not a StatisticCondition: " + statistic.GetType().FullName);
            }
            AllConditions.Add(newCondition);
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
                var Condition = AllConditions.FirstOrDefault(p => p.GetType() == prequisite);
                if (Condition == null)
                {
                    IStatisticCondition newCondition = CreateConditionFromType(prequisite);
                    AddPrequisites(newCondition);
                }
            }
        }

        public void EvaluateHand(GeneralHandData generalHand)
        {
            if (CurrentState != ConditionTreeState.Ready)
            {
                throw new Exception("Must initialize tree before evaluating hands");
            }
            foreach (var player in generalHand.PlayerList)
            {
                foreach (var stat in CoreConditions)
                {
                    stat.EvaluateHand(generalHand, player.Value, null);
                }
            }
        }

        public IStatisticCondition GetHandCondition(Type statisticType)
        {
            if (CurrentState != ConditionTreeState.Ready)
            {
                throw new Exception("Must initialize tree before GetHandCondition() can be used");
            }
            return AllConditions.Find(p => p.GetType() == statisticType);
        }
    }
}
