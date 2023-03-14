
using Bz.Fott.Registration.Application.CompetitorRegistration;
using System.Threading.Tasks;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public interface ICompetitorService
{
    Task<string> RegisterCompetitorAndReturnNumber(RegisterCompetitor competitor);
}
