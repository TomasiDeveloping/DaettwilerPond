using Application.Interfaces;
using Application.Models.LSN50v2D20;

namespace Persistence.Logic;

public class Lsn50V2d20Logic : ILsn50V2d20Logic
{
    public Task HandleLifecycleAsync(Lifecycle lifecycle, string devEui)
    {
        throw new NotImplementedException();
    }

    public Task HandleDataPayloadAsync(Payload payload, string devEui)
    {
        throw new NotImplementedException();
    }
}