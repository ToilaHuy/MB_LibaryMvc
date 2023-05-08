using WebApplication8.Data;
using AutoMapper;
using WebApplication8.Models;

namespace WebApplication8.Helper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Item, ItemViewModel>().ReverseMap();
            CreateMap<Item, ItemVMCreate>().ReverseMap();

            CreateMap<Borrower, BorrowerVMCreate>().ReverseMap();
            CreateMap<Borrower, BorrowerVM>().ReverseMap();

            CreateMap<BorrowingHistory, BorrowingHistoryCreate>().ReverseMap();
            CreateMap<BorrowingHistory, BorrowingHistoryVM>().ReverseMap();


            CreateMap<BorrowingHistory, BorrowingHistoryVMIndex>()
                //.ForMember(x => x.BorrowingHistoryId,y => y.MapFrom(z => z.BorrowingHistoryId))
                .ForMember(x => x.Borrower, y => y.MapFrom(z => z.Borrower.BorrowerName));

            
             

        }
    }
}
