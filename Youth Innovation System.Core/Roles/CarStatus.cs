using System.Runtime.Serialization;

namespace Youth_Innovation_System.Core.Roles
{
    public enum CarStatus
    {
        [EnumMember(Value = "Rented")]
        Rented,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Accepted")]
        Accepted,
        [EnumMember(Value = "Rejected")]
        Rejected,
    }
}
