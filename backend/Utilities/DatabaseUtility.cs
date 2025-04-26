using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduConnect.DTOs;

namespace EduConnect.Utilities
{
    public class DatabaseUtility
    {
        public static string ApplyDeltas(string content, List<DocumentDelta> deltas)
        {
            var sb = new StringBuilder(content);
            foreach (var delta in deltas)
            {
                if (delta.Delete.HasValue)
                {
                    sb.Remove(delta.Position, delta.Delete.Value);
                }
                if (!string.IsNullOrEmpty(delta.Insert))
                {
                    sb.Insert(delta.Position, delta.Insert);
                }
            }

            return sb.ToString();
        }
    }
}