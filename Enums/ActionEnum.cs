using System;
using System.Collections.Generic;
using CodeBE_LEM.Entities;
using Action = CodeBE_LEM.Entities.Action;

namespace CodeBE_LEM.Enums;

public partial class ActionEnum
{
    public static Action CREATE = new Action(1, "Create");
    public static Action UPDATE = new Action(2, "Update");
    public static Action DELETE = new Action(3, "Delete");
    public static Action READ = new Action(4, "Read");
    public static Action APPROVAL = new Action(5, "Approval");
    public static List<Action> ActionEnumList = new List<Action>
    {
        CREATE, UPDATE, DELETE, READ,
        APPROVAL
    };
}
