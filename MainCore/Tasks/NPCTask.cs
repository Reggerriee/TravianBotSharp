﻿using FluentResults;
using MainCore.Commands;
using MainCore.Commands.Special;
using MainCore.Common.Errors;
using MainCore.Infrasturecture.AutoRegisterDi;
using MainCore.Repositories;
using MainCore.Tasks.Base;
using MediatR;

namespace MainCore.Tasks
{
    [RegisterAsTransient(withoutInterface: true)]
    public class NPCTask : VillageTask
    {
        public NPCTask(IUnitOfCommand unitOfCommand, IUnitOfRepository unitOfRepository, IMediator mediator) : base(unitOfCommand, unitOfRepository, mediator)
        {
        }

        protected override async Task<Result> Execute()
        {
            if (CancellationToken.IsCancellationRequested) return new Cancel();
            Result result;

            result = _unitOfCommand.SwitchVillageCommand.Execute(AccountId, VillageId);
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));

            result = _unitOfCommand.ToDorfCommand.Execute(AccountId, 1);
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));

            result = await _unitOfCommand.UpdateDorfCommand.Execute(AccountId, VillageId);
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));

            result = await _mediator.Send(new ToNPCPageCommand(AccountId, VillageId));
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));

            result = await _mediator.Send(new NPCCommand(AccountId, VillageId));
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));

            result = await _unitOfCommand.UpdateDorfCommand.Execute(AccountId, VillageId);
            if (result.IsFailed) return result.WithError(new TraceMessage(TraceMessage.Line()));
            return Result.Ok();
        }

        protected override void SetName()
        {
            var village = _unitOfRepository.VillageRepository.GetVillageName(VillageId);
            _name = $"NPC in {village}";
        }
    }
}