using System.Collections.Generic;
using System.Threading.Tasks;
using TAINATechTest.Services.ViewModels;

namespace TAINATechTest.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<ListPersonViewModel>> GetAllPeople();
        Task<DetailsPersonViewModel> GetPersonById(long id);

        Task<long> AddPersonAsync(CreatePersonViewModel person);
        Task<long> UpdatePersonAsync(UpdatePersonViewModel person);
    }
}
