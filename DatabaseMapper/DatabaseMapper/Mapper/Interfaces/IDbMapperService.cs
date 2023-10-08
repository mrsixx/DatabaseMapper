using DatabaseMapper.Graph;

namespace DatabaseMapper.Mapper.Interfaces
{
    public interface IDbMapperService
    {
        void IncrementModel(TableGraph model, string query);
    }
}
