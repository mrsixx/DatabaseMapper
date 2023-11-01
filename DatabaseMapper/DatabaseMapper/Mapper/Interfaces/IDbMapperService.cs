using DatabaseMapper.Core.Graph;

namespace DatabaseMapper.Core.Mapper.Interfaces
{
    public interface IDbMapperService
    {
        void IncrementModel(TableGraph model, string query);
    }
}
