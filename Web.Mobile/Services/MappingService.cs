using AutoMapper;
using global::Services.Model;
using Web.Mobile.Models.ViewModels;
using Web.Mobile.Services.Resolvers;

namespace Web.Mobile.Services
{
    public static class MappingService
    {
        public static void Init()
        {
            Mapper.CreateMap<Item, ItemCompactView>()
                .ForMember(x => x.SampleContent, x => x.ResolveUsing<SampleContentResolver>());

            Mapper.CreateMap<Item, ItemView>();
        }
    }
}