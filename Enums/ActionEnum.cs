using System;
using System.Collections.Generic;
using CodeBE_LEM.Entities;
using Action = CodeBE_LEM.Entities.Action;

namespace CodeBE_LEM.Enums;

public partial class ActionEnum
{
    public static Action UPLOAD_FILE = new Action(1, "Upload File");
    public static Action DOWLOAD_FILE = new Action(2, "Dowload File");
    public static Action CREATE_BOARD = new Action(3, "Create group");
    public static Action UPDATE_BOARD = new Action(4, "Update group");
    public static Action DELETE_BOARD = new Action(5, "Delete group");
    public static Action KICK_MEMBER = new Action(7, "Kick member");
    public static Action CREATE_QUESTION = new Action(8, "Create question");
    public static Action UPDATE_QUESTION = new Action(9, "Update question");
    public static Action DELETE_QUESTION = new Action(10, "Delete question");
    public static Action CREATE_JOB = new Action(11, "Create job in board");
    public static Action UPDATE_JOB = new Action(12, "Update job in board");
    public static Action DELETE_JOB = new Action(13, "Delete job in board");
    public static Action LIST_PERMISSON = new Action(14, "View list permission");
    public static Action LIST_ROLE = new Action(15, "View list role");
    public static Action CREATE_ROLE = new Action(16, "Create role in class");
    public static Action UPDATE_ROLE = new Action(17, "Update role in class");
    public static Action DELETE_ROLE = new Action(18, "Delete role in class");
    public static Action MARK_OR_FEEDBACK = new Action(19, "Create feedback");
    public static Action CREATE_CLASSWORK = new Action(20, "Create classwork");
    public static Action DELETE_CLASS = new Action(21, "Delete class");

    public static List<Action> ActionEnumList = new List<Action>
    {
        UPLOAD_FILE, DOWLOAD_FILE, 
        CREATE_BOARD, UPDATE_BOARD, DELETE_BOARD, 
        KICK_MEMBER, 
        CREATE_QUESTION, UPDATE_QUESTION, DELETE_QUESTION,
        CREATE_JOB, UPDATE_JOB, DELETE_JOB,
        LIST_PERMISSON, LIST_ROLE, CREATE_ROLE, UPDATE_ROLE, DELETE_ROLE, MARK_OR_FEEDBACK, CREATE_CLASSWORK, DELETE_CLASS
    };

    public static List<Action> ActionEnumListForStudent = new List<Action>
    {
        UPLOAD_FILE, DOWLOAD_FILE,
        CREATE_JOB, UPDATE_JOB, DELETE_JOB,
    };
}
