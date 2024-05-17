using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace Terrain
{
    public enum Status
    {
        OK,
        FAILED,
        FAILED_TO_LOAD,
        ERR_UNAVAILABEL,
        ERR_UNCOFIGURED,
        ERR_UNATHORIZED
    }
    public class StN
    {
        public static readonly Dictionary<Status, string> StatusNames = new Dictionary<Status, string>()
        {
            {Status.OK, "OK"},
            {Status.FAILED, "Generic error"},
            {Status.ERR_UNAVAILABEL, "Unavailable error"}
        };
        
        public static string GetMessage(Status status)
        {
            if (StatusNames.ContainsKey(status)) return StatusNames[status];
            return $"{status}";
        }
    }
}
