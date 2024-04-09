using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CodeBE_LEM.Services
{

    public class BaseService<T> where T : class, IFilterable
    {
        public List<T> FilterData(List<T> entities, FilterDTO filter)
        {

            if (filter.Id != null || filter.Id?.Count > 0)
            {
                entities = entities.Where(q => filter.Id.Contains(q.Id)).ToList(); 
            }
            if (filter.Code != null)
            {
                entities = entities.Where(q => filter.Code.Contains(q.Code)).ToList(); 
            }
            if (filter.Name != null)
            {
                entities = entities.Where(q => filter.Name.Contains(q.Name)).ToList(); 
            }

            if (filter.OrderType != null && filter.OrderBy != null)
            {
                switch (filter.OrderType)
                {
                    case FilterOrderType.ASC:
                        switch (filter.OrderBy)
                        {
                            case FilterOrderBy.Id:
                                entities = entities.OrderBy(q => q.Id).ToList();
                                break;
                            case FilterOrderBy.Code:
                                entities = entities.OrderBy(q => q.Code).ToList();
                                break;
                            case FilterOrderBy.Name:
                                entities = entities.OrderBy(q => q.Name).ToList();
                                break;
                        }
                        break;
                    case FilterOrderType.DESC:
                        switch (filter.OrderBy)
                        {
                            case FilterOrderBy.Id:
                                entities = entities.OrderByDescending(q => q.Id).ToList();
                                break;
                            case FilterOrderBy.Code:
                                entities = entities.OrderByDescending(q => q.Code).ToList();
                                break;
                            case FilterOrderBy.Name:
                                entities = entities.OrderByDescending(q => q.Name).ToList();
                                break;
                        }
                        break;
                } 
            }

            if (filter.Skip != null && filter.Take != null)
            {
                entities = entities.Skip(filter.Skip.Value).Take(filter.Take.Value).ToList(); 
            }

            return entities;
        }
    }
}
