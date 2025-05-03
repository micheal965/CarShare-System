using System.Runtime.Serialization;

namespace Youth_Innovation_System.Core.Roles
{
    public enum UserStatus
    {
        [EnumMember(Value = "Accepted")]
        accepted,
        [EnumMember(Value = "Rejected")]
        rejected,
        [EnumMember(Value = "Pending")]
        pending
    }
}
