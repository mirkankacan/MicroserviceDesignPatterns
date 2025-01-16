using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMap : SagaClassMap<OrderStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
        {
            entity.Property(x => x.BuyerId).HasMaxLength(255);
            entity.Property(x => x.Cvv).HasMaxLength(4);
            entity.Property(x => x.CardName).HasMaxLength(100);
            entity.Property(x => x.CardNumber).HasMaxLength(20);
            entity.Property(x => x.Expiration).HasMaxLength(7);
        }
    }
}