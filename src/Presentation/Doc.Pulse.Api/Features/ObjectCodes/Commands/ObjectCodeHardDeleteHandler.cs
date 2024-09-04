using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Commands;
using Doc.Pulse.Contracts.Communications.V1.ObjectCodes.Queries;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api.Features.ObjectCodes.Commands;

public class ObjectCodeHardDeleteHandler
{
    public class Request : IRequest<Response>
    {
        public required ObjectCodeHardDeleteCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class Validator : AbstractValidator<ObjectCodeHardDeleteCmd>
    {
        public Validator()
        {
            RuleFor(o => o.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper, TimeProvider dateTimeProvider) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        private readonly TimeProvider _clock = dateTimeProvider;

        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var cmd = request.Command;
            Response response = new();

            try
            {
                var entity = await _dbContext.ObjectCodes
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == request.Command.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(request.Command.Id.ToString()));

                _dbContext.Remove(entity);
                await _dbContext.SaveChangesAsync(cancellationToken);

                response.WithValue<Response>(new CommandResponse()
                {
                    Id = entity.Id
                });
            }
            catch (Exception exception)
            {
                response.WithException(exception);
            }

            return response;
        }
    }
}
