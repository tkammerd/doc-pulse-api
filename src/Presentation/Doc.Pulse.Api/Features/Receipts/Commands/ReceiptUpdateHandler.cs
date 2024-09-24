using FluentValidation;
using AppDmDoc.SharedKernel.Core.Abstractions;
using AppDmDoc.SharedKernel.Core.Trouble.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Contracts.Communications.V1.Receipts.Commands;
using Doc.Pulse.Infrastructure.Abstractions;
using Doc.Pulse.Infrastructure.Data;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Doc.Pulse.Infrastructure.Extensions;
using Doc.Pulse.Api.Extensions;
using Doc.Pulse.Core.Entities;

namespace Doc.Pulse.Api.Features.Receipts.Commands;

public class ReceiptUpdateHandler
{
    public class Request : IRequest<Response>
    {
        public required ReceiptUpdateCmd Command { get; set; }
    }
    public class Response : MediatorResult<CommandResponse> { }

    public class DbContextValidator : AbstractValidator<ReceiptUpdateCmd>
    {
        private readonly AppDbContext _dbContext;
        public DbContextValidator(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            var keyFieldDescription = "FacilityAndFiscalYearAndReceiptNumber".SplitCamelCase();

            RuleFor(o => o.Facility).NotNull().Length(3, 255);
            RuleFor(p => p.FiscalYear).NotNull().GreaterThan(2020);
            RuleFor(p => p.ReceiptNumber).NotNull().InclusiveBetween(100, 99999);
            RuleFor(p => p.RfpId).GreaterThan(0).When(n => n != null);

            RuleFor(p => p)
                .Must(KeyFieldIsUnique)
                .WithErrorCode("UniqueFieldValidator")
                .WithMessage($"'{keyFieldDescription}' combined must be unique.");
        }

        private bool KeyFieldIsUnique(ReceiptUpdateCmd cmd)
        {
            return !_dbContext.Receipts.Any(r => r.Id != cmd.Id && 
                r.Facility == cmd.Facility &&
                r.FiscalYear == cmd.FiscalYear &&
                r.ReceiptNumber == cmd.ReceiptNumber);
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
                var entity = await _dbContext.Receipts
                    .AsTracking(QueryTrackingBehavior.TrackAll)
                    .SingleOrDefaultAsync(o => o.Id == cmd.Id, cancellationToken);
                if (entity == null)
                    return response.WithError<Response>(new EntityNotFound(cmd.Id.ToString()));

                entity.Facility = cmd?.Facility ?? "";
                entity.FiscalYear = cmd?.FiscalYear ?? 0;
                entity.ReceiptNumber = cmd?.ReceiptNumber ?? 0;
                entity.RfpId = cmd?.RfpId;
                entity.ReceiptDate = cmd?.ReceiptDate;
                entity.ReceivingReportAmount = cmd?.ReceivingReportAmount;
                entity.AmountInIsis = cmd?.AmountInIsis;
                entity.ReceiverNumber = cmd?.ReceiverNumber;
                entity.CheckNumber = cmd?.CheckNumber;
                entity.CheckDate = cmd?.CheckDate;

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
