using System;
using System.Collections.Generic;
using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Enums;

public partial class RoleTypeEnum
{
    public static RoleType AUTO = new RoleType(1, "Auto");
    public static RoleType USER_CREATED = new RoleType(2, "User Created");
    public static List<RoleType> RoleTypeEnumList = new List<RoleType>
    {
        AUTO, USER_CREATED
    };
}
