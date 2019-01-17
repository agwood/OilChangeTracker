using AutoMapper;
using OilChangeTracker.ViewModels;

namespace OilChangeTracker.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Vehicle, VehicleViewModel>();

            CreateMap<VehicleFormViewModel, Vehicle>();
        }
    }
}