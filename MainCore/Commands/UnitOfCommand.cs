﻿using MainCore.Commands.Base;
using MainCore.Commands.General;
using MainCore.Commands.Navigate;
using MainCore.Commands.Update;
using MainCore.Commands.Validate;
using MainCore.Infrasturecture.AutoRegisterDi;

namespace MainCore.Commands
{
    [RegisterAsTransient(withoutInterface: true)]
    public record UnitOfCommand(ICommandHandler<OpenBrowserCommand> OpenBrowserCommand,
                                ICommandHandler<CloseBrowserCommand> CloseBrowserCommand,
                                ICommandHandler<SleepBrowserCommand> SleepBrowserCommand,
                                ICommandHandler<DelayClickCommand> DelayClickCommand,
                                ICommandHandler<DelayTaskCommand> DelayTaskCommand,
                                ICommandHandler<SwitchTabCommand> SwitchTabCommand,
                                ICommandHandler<SwitchVillageCommand> SwitchVillageCommand,
                                ICommandHandler<ToBuildingCommand> ToBuildingCommand,
                                ICommandHandler<ToDorfCommand> ToDorfCommand,
                                ICommandHandler<ToHeroInventoryCommand> ToHeroInventoryCommand,
                                ICommandHandler<UpdateAccountInfoCommand> UpdateAccountInfoCommand,
                                ICommandHandler<UpdateDorfCommand> UpdateDorfCommand,
                                ICommandHandler<UpdateFarmListCommand> UpdateFarmListCommand,
                                ICommandHandler<UpdateHeroItemsCommand> UpdateHeroItemsCommand,
                                ICommandHandler<UpdateVillageListCommand> UpdateVillageListCommand,
                                ICommandHandler<ValidateIngameCommand, bool> ValidateIngameCommand,
                                ICommandHandler<ValidateLoginCommand, bool> ValidateLoginCommand,
                                ICommandHandler<ValidateProxyCommand, bool> ValidateProxyCommand);
}