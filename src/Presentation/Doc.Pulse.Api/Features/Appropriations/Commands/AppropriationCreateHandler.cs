using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Contracts.Communications.V1.Appropriations.Commands;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Appropriations.Commands;

public class AppropriationCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required AppropriationCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<AppropriationCreateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(o => o.Facility).NotNull().Length(3, 255);
            RuleFor(o => o.FiscalYear).NotNull().GreaterThan(2020);
            RuleFor(o => o.ProgramId).NotNull().GreaterThan(0);
            RuleFor(o => o.ObjectCodeId).NotNull().GreaterThan(0);
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
                var entity = new Appropriation()
                {
                    Facility = cmd.Facility,
                    FiscalYear = cmd.FiscalYear,
                    ProgramId = cmd.ProgramId,
                    ObjectCodeId = cmd.ObjectCodeId,
                    CurrentModifiedAmount = cmd.CurrentModifiedAmount,
                    PreEncumberedAmount = cmd.PreEncumberedAmount,
                    EncumberedAmount = cmd.EncumberedAmount,
                    ExpendedAmount = cmd.ExpendedAmount,
                    ProjectedAmount = cmd.ProjectedAmount,
                    PriorYearActualAmount = cmd.PriorYearActualAmount,
                    TotalObligated = cmd.TotalObligated
                };

                await _dbContext.Appropriations.AddAsync(entity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                if (entity.Id == 0)
                    return response.WithError<Response>(new DatabaseSaveFailed());

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
