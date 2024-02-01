using HikeBE.Entities;
using System;
using System.Collections.Generic;

namespace HikeBE.Enums;

public partial class TypeEnum
{
    public static HikeType RECOVERY = new HikeType(1, "Recovery run", "RECOVERY");
    public static HikeType BASE = new HikeType(2, "Base run", "BASE");
    public static HikeType LONG = new HikeType(3, "Long run", "LONG");
    public static HikeType CYCLING = new HikeType(4, "Cycling", "CYCLING");
    public static List<HikeType> HikeTypeEnumList = new List<HikeType>
    {
        RECOVERY, BASE, LONG, CYCLING
    };
}
