
using Bz.Fott.Registration.Application.CompetitorRegistration;
using System;
using System.Threading.Tasks;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public interface ICompetitorService
{
    Task<(Guid id, string number)> RegisterCompetitorAndReturnNumber(RegisterCompetitor competitor);
}
