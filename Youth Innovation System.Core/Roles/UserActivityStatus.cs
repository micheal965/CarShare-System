using System.Runtime.Serialization;

namespace Youth_Innovation_System.Core.Roles
{
    public enum UserActivityStatus
    {
        [EnumMember(Value = "Online")]
        Online,
        [EnumMember(Value = "Offline")]
        Offline
    }
}
