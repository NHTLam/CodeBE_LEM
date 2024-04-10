using System;
using System.Collections.Generic;
using CodeBE_LEM.Entities;
using AppUserType = CodeBE_LEM.Entities.AppUserType;

namespace CodeBE_LEM.Enums;

public partial class AppUserTypeEnum
{
    public static AppUserType OWN = new AppUserType(1, "Own");
    public static AppUserType CREATOR = new AppUserType(2, "Creator");
    public static AppUserType COOPERATOR = new AppUserType(3, "Cooperator");
    public static AppUserType GUEST = new AppUserType(4, "Guest");
    public static AppUserType SUPERVISOR = new AppUserType(5, "Supervisor");
    public static List<AppUserType> AppUserTypeEnumList = new List<AppUserType>
    {
        OWN, CREATOR, COOPERATOR, GUEST,
        SUPERVISOR
    };
}
