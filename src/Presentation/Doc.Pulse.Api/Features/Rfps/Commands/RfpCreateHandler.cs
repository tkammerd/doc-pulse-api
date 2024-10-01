using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Contracts.Communications.V1.Rfps.Commands;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using FluentValidation;
using MediatR;
using Ots.AppDmDoc.Abstractions.AutoMapper;

namespace Doc.Pulse.Api.Features.Rfps.Commands;

public class RfpCreateHandler
{
    public class Request : IRequest<Response>
    {
        public required RfpCreateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<RfpCreateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "FacilityAndFiscalYearAndRFPNumber".SplitCamelCase();

            RuleFor(p => p.Facility).NotNull().Length(3, 255);
            RuleFor(p => p.FiscalYear).NotNull().GreaterThan(2020);
            RuleFor(p => p.RfpNumber).NotNull().Length(3, 255);
            RuleFor(p => p.Description).Length(3, 255).When(n => n != null);
            RuleFor(p => p.ObjectCodeId).NotNull().GreaterThan(0);
            RuleFor(p => p.VendorId).NotNull().GreaterThan(0);
            RuleFor(p => p.AgencyId).NotNull().GreaterThan(0);
            RuleFor(p => p.AccountOrganizationId).NotNull().GreaterThan(0);
            RuleFor(p => p.ProgramId).NotNull().GreaterThan(0);
            RuleFor(p => p.PurchaseOrderNumber).Length(3, 255).When(n => n != null);
            RuleFor(p => p.Completed).Length(1, 255).When(n => n != null);
            RuleFor(p => p.CheckOrDocumentNumber).Length(3, 255).When(n => n != null);
            RuleFor(p => p.Comments).Length(3, 255).When(n => n != null);
            RuleFor(p => p.ReportingCategory).Length(3, 255).When(n => n != null);
            RuleFor(p => p.VerifiedOnIsis).Length(1, 255).When(n => n != null);
            RuleFor(p => p.RequestedBy).Length(3, 255).When(n => n != null);

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' combined must be unique.");
        }

        private bool KeyFieldIsUnique(RfpCreateCmd cmd)
        {
            return !_dbContext.Rfps.Any(r => 
                r.Facility == cmd.Facility &&
                r.FiscalYear == cmd.FiscalYear &&
                r.RfpNumber == cmd.RfpNumber);
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
                var entity = new Rfp()
                {
                    Facility = cmd.Facility,
                    FiscalYear = cmd.FiscalYear,
                    RfpNumber = cmd.RfpNumber,
                    RfpDate = cmd.RfpDate,
                    Description = cmd.Description,
                    ObjectCodeId = cmd.ObjectCodeId,
                    VendorId = cmd.VendorId,
                    AgencyId = cmd.AgencyId,
                    AccountOrganizationId = cmd.AccountOrganizationId,
                    ProgramId = cmd.ProgramId,
                    PurchaseOrderNumber = cmd.PurchaseOrderNumber,
                    AmountObligated = cmd.AmountObligated,
                    Completed = cmd.Completed,
                    CheckOrDocumentNumber = cmd.CheckOrDocumentNumber,
                    Comments = cmd.Comments,
                    ReportingCategory = cmd.ReportingCategory,
                    VerifiedOnIsis = cmd.VerifiedOnIsis,
                    RequestedBy = cmd.RequestedBy
                };

                await _dbContext.Rfps.AddAsync(entity, cancellationToken);
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
