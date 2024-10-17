using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Contracts.Communications.V1.Appropriations.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Appropriations.Commands;

public class AppropriationUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required AppropriationUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<AppropriationUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var tableDescription = "Appropriations";

            RuleFor(o => o.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
            RuleFor(o => o.Facility).NotNull().Length(3, 255);
            RuleFor(o => o.FiscalYear).NotNull().GreaterThan(2020);
            RuleFor(o => o.ProgramId).NotNull().GreaterThan(0);
            RuleFor(o => o.ObjectCodeId).NotNull().GreaterThan(0);
            RuleFor(p => p)
                .Must(command => {
                    var updatingEntity = _dbContext.Appropriations.FirstOrDefault(o => o.Id == command.Id);
                    return (updatingEntity?.RowVersion ?? []).SequenceEqual(command.RowVersion ?? []);
                })
                .WithErrorCode("RowVersionCheck")
                .WithMessage($"'{tableDescription}' record was changed by another user. Please refresh your browser.");
        }
    }

    public class Handler(AppDbContext dbContext, IMapperAdapter mapper) : HandlerBase<AppDbContext, Request, Response>(dbContext, mapper)
    {
        public async override Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var cmd = request.Command;
            Response response = new();

            var validator = new DbContextValidator(_dbContext);
            var validationResult = await validator.ValidateAsync(cmd, cancellationToken);

            if (validationResult.Errors.Count > 0)
            {
                foreach (var item in validationResult.Errors.Select(e => e.ErrorMessage))
                {
                    response.WithError(new FluentResults.Error(item));
                }
                return response;
            }

            try
            {
                var entity = await _dbContext.Appropriations
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.Facility = cmd?.Facility ?? "";
                entity.FiscalYear = cmd?.FiscalYear ?? 0;
                entity.ProgramId = cmd?.ProgramId ?? 0;
                entity.ObjectCodeId = cmd?.ObjectCodeId ?? 0;
                entity.CurrentModifiedAmount = cmd?.CurrentModifiedAmount;
                entity.PreEncumberedAmount = cmd?.PreEncumberedAmount;
                entity.EncumberedAmount = cmd?.EncumberedAmount;
                entity.ExpendedAmount = cmd?.ExpendedAmount;
                entity.ProjectedAmount = cmd?.ProjectedAmount;
                entity.PriorYearActualAmount = cmd?.PriorYearActualAmount;
                entity.TotalObligated = cmd?.TotalObligated;

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
