using CodeBE_LEM.Enums;
using System.ComponentModel;
using System.Reflection;

namespace CodeBE_LEM.Controllers.BoardController
{
    [DisplayName("Board")]
    public class BoardRoute
    {
        public const string Module = "/lem/board";
        public const string Create = Module +"/create";
        public const string Get = Module + "/get";
        public const string GetOwn = Module + "/get-own";
        public const string List = Module + "/list";
        public const string ListByClassroom = Module + "/list-by-classroom";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
        public const string ListCardByUserId = Module + "/list-card-by-userId";
        public const string CreateBoardsForClass = Module + "/create-boards-for-class";
        public const string DuplicateCard = Module + "/duplicate-card";
        public const string DeleteCard = Module + "/delete-card";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>>
        {
            { 
                ActionEnum.CREATE_BOARD.Name, new List<string>()
                {
                    Create, CreateBoardsForClass
                }
            },
            { 
                ActionEnum.UPDATE_BOARD.Name, new List<string>()
                {
                    Update, DuplicateCard, 
                }
            },
            {
                ActionEnum.DELETE_BOARD.Name, new List<string>()
                {
                    Delete, DeleteCard
                }
            }
        };
    }
}
