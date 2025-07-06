using Entities;
using Mapster;
using Shared.DataTransferObjects;
namespace CompanyEmployees.Mapping
{
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<Company, CompanyDto>
                .NewConfig().Map(dest => dest.FullAddress, src => string.Join("", src.Address, src.Country));
        }
    }
}
