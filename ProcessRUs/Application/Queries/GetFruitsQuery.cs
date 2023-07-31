using MediatR;

namespace ProcessRUs.Application.Queries
{
    public class GetFruitsQuery : IRequest<string[]>
    {
    }

    public class GetFruitsQueryHandler : IRequestHandler<GetFruitsQuery, string[]>
    {
        public GetFruitsQueryHandler() { }

        public async Task<string[]> Handle(GetFruitsQuery request, CancellationToken cancellationToken)
        {
            string[] fruits = { "Apple", "Banana", "Orange", "Mango", "PawPaw" };
            return fruits;
        }
    }
}
