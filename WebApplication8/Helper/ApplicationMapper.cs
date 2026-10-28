using AutoMapper;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Item, ItemViewModel>().ReverseMap();
            CreateMap<Item, ItemVmCreate>().ReverseMap();
            CreateMap<Item, ItemViewUpdateModel>().ReverseMap();

            CreateMap<Borrower, BorrowerVmCreate>().ReverseMap();
            CreateMap<Borrower, BorrowerVm>().ReverseMap();
            CreateMap<Borrower, BorrowerUpdate>().ReverseMap();

            CreateMap<HistoryDetailViewModel, HistoryDetail>().ReverseMap();

            
            CreateMap<BorrowingHistory, BorrowingHistoryCreate>().ReverseMap();
            CreateMap<BorrowingHistory, BorrowingHistoryVm>().ReverseMap();


            CreateMap<BorrowingHistory, BorrowingHistoryVmIndex>()
                //.ForMember(x => x.BorrowingHistoryId,y => y.MapFrom(z => z.BorrowingHistoryId))
                .ForMember(x => x.Borrower, y => y.MapFrom(z => z.Borrower.BorrowerName));




        }
    }
}
