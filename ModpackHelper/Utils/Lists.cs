using System.Collections;
using System.Linq;

namespace ModpackHelper.Utils
{
    public static class Lists
    {
        public static bool AreEqual(IList list1, IList list2)
        {
            if (list1 == null && list2 == null) return true;
            if (list1 == null || list2 == null) return false;
            if (ReferenceEquals(list1, list2)) return true;
            if (list1.Count == 0 && list2.Count == 0) return true;
            if (list1.Count != list2.Count) return false;
            if (list1[0].GetType() != list2[0].GetType()) return false;
            return !list1.Cast<object>().Where((t, i) => !t.Equals(list2[i])).Any();
        }
    }
}
