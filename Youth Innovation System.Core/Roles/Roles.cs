using System.Runtime.Serialization;

namespace Youth_Innovation_System.Core.Roles
{
    public enum UserRoles
    {
        [EnumMember(Value = "Admin")]
        Admin,
        [EnumMember(Value = "CarOwner")]
        CarOwner,
        [EnumMember(Value = "Renter")]
        Renter
    }
}
