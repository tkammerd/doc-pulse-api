using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Rfps.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Core.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Xml.Linq;

namespace Doc.Pulse.Api.Features.Rfps.Commands;

public class RfpUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required RfpUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<RfpUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "FacilityAndFiscalYearAndRFPNumber".SplitCamelCase();

            RuleFor(o => o.Id).NotNull().NotEqual(0)
                .WithMessage($"Id not valid: Please indicate a valid Identifier.");
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

        private bool KeyFieldIsUnique(RfpUpdateCmd cmd)
        {
            return !_dbContext.Rfps.Any(r => r.Id != cmd.Id &&
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
                var entity = await _dbContext.Rfps
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.Facility = cmd?.Facility ?? "";
                entity.FiscalYear = cmd?.FiscalYear ?? 0;
                entity.RfpNumber = cmd?.RfpNumber ?? "";
                entity.RfpDate = cmd?.RfpDate;
                entity.Description = cmd?.Description;
                entity.ObjectCodeId = cmd?.ObjectCodeId ?? 0;
                entity.VendorId = cmd?.VendorId ?? 0;
                entity.AgencyId = cmd?.AgencyId ?? 0;
                entity.AccountOrganizationId = cmd?.AccountOrganizationId ?? 0;
                entity.ProgramId = cmd?.ProgramId ?? 0;
                entity.PurchaseOrderNumber = cmd?.PurchaseOrderNumber;
                entity.AmountObligated = cmd?.AmountObligated;
                entity.Completed = cmd?.Completed;
                entity.CheckOrDocumentNumber = cmd?.CheckOrDocumentNumber;
                entity.Comments = cmd?.Comments;
                entity.ReportingCategory = cmd?.ReportingCategory;
                entity.VerifiedOnIsis = cmd?.VerifiedOnIsis;
                entity.RequestedBy = cmd?.RequestedBy;

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
