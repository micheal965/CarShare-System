using System.Runtime.Serialization;

namespace Youth_Innovation_System.Core.Roles
{
    public enum RentalAppStatus
    {

        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Approved")]
        Approved,
        [EnumMember(Value = "Rejected")]
        Rejected,
    }
}
