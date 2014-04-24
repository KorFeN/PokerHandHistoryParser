using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HandHistories.Statistics.HandData
{
    /// <summary>
    /// This class provides a way to store data that is local to the current hand
    /// and that is accessible between diffrent conditions
    /// </summary>
    public class CustomHandData
    {
        Dictionary<Type, object> HandDataDictionary = new Dictionary<Type, object>();

        public void StoreData(Type ThisType, object Data)
        {
            if (!HandDataDictionary.ContainsKey(ThisType))
            {
                HandDataDictionary.Add(ThisType, Data);
            }
            else
            {
                HandDataDictionary[ThisType] = Data;
            }
        }

        public object FetchData(Type ThisType)
        {
            if (HandDataDictionary.ContainsKey(ThisType))
            {
                return HandDataDictionary[ThisType];
            }
            return null;
        }
    }
}
