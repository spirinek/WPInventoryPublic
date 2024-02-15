using AutoMapper;
using WPInventory.Data.Models.Entities;

namespace WPInventory.BL.Computers
{
   public class ComputerMapping : Profile
    {
        public ComputerMapping()
        {
            CreateMap<CPU, CPUDto>();
            CreateMap<HDD, HDDDto>();
            CreateMap<MotherBoard, MotherBoardDto>();
            CreateMap<NWAdapter, NWAdapterDto>();
            CreateMap<RAM, RAMDto>();
            CreateMap<VideoCard, VideoCardDto>();

            CreateMap<Monitor, MonitorDto>();

            CreateMap<Computer, ConcreteComputerDto>()
                .ForMember(x => x.LastSeen, x => x.MapFrom(y => y.ScanDates.LastSeen))
                .ForMember(x => x.Added, x => x.MapFrom(y => y.ScanDates.Added))
                .ForMember(x => x.Changed, x => x.MapFrom(y => y.ScanDates.Changed));


        }
    }
}
